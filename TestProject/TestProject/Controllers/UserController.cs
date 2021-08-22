using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadExcel.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace ReadExcel.Controllers
{
    public class UserController : Controller
    {
        public static List<Rule> _rules = new List<Rule>();
        public static List<UserSubject> userSubjects = new List<UserSubject>();
        public static List<string> fileNames = new List<string>();
        public IActionResult start()
        {
            return View();
        }
        private IWebHostEnvironment environment;
        public UserController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        //Default GET method
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(ICollection<IFormFile> fileCollection)//파일 업로드
        {
            var uploadDirectoryPath = Path.Combine(this.environment.WebRootPath, "upload"+Path.DirectorySeparatorChar);
            fileNames.Clear();
            foreach(IFormFile formFile in fileCollection)
            {
                if(formFile.Length > 0)
                {
                    string fileName = Path.GetFileName
                    (
                        ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Value
                    );
                    fileNames.Add(fileName);// 업로드 파일리스트 추가.
                    using(FileStream stream = new FileStream(Path.Combine(uploadDirectoryPath, fileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            // List<Rule> _rules = new List<Rule>(); // rule list
            List<List<Class>> resultList = new List<List<Class>>();
            List<UserModel> userModels = new List<UserModel>();
            // TODO 0: 사용자 이수교과목 데이터 파싱 및 저장(파일?)해서 룰에 대입할수있게
            // TODO 1: 실제 Sheet 에 맞게 모델 추가, 수정(컬럼 개수; 설계학점 등) 디비 정보 반영 및 순서 변경 
            // TODO 2: 하드코딩 되어있는것 반복문&클래스설계로 바꿔야할듯..?
            // TODO 3: 예외처리용 동일,대체교과목 설계

            const string filename = "./wwwroot/upload/template_test.xlsx";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // sheet 
                    int currentRuleNum = 0;
                    int currentSheetNum = 1;
                    List<int> multiInputRuleNumber = new List<int>();
                    string ruleType = "";
                    // will be passed to View
                    reader.Read();
                    while(reader.Read())
                    {
                        string singleInput = "";
                        // string[] multiInput;
                        int ruleFlag = -1;

                        string[] valueArray = new string[6]; // 모두 string임에 주의
                        
                        for(int i = 0; i < 6; i++)
                        {
                            if (reader.GetValue(i) == null)
                                valueArray[i] = "";
                            else
                                valueArray[i] = reader.GetValue(i).ToString();
                        }
                        if(valueArray[0] == "" || valueArray[0] == null)
                          valueArray[0] = ruleType;
                        else
                          ruleType = valueArray[0];

                        // 필요없음
                        UserModel newUserModel = new UserModel{
                            type = ruleType, // 구분
                            number = valueArray[1], // 일련번호
                            question = valueArray[2], // 질문
                            input = ((valueArray[5] == "목록")? "[List]" : valueArray[3]), // 입력
                            flag = ruleFlag.ToString(), // 응답유형
                            reference = valueArray[5] // 비고
                        };
                        // -----------------------------------------------
                        // Rule Generator
                        Rule newRule = new Rule{
                            type = ruleType, // 구분
                            number = valueArray[1], // 일련번호
                            question = valueArray[2], // 질문
                            singleInput = null,
                            multiInput = null,
                            flag = ruleFlag, // 응답유형
                            reference = valueArray[5] // 비고
                        };
                        // flag setting
                        // 0: 대소비교
                        // 1: OX
                        // 2: 목록 중 선택
                        // 3: 목록 전체 필수
                        if(valueArray[5] != "" || valueArray[5] != null) // 기본정보: 비고 란 비어있음
                        {
                          if(valueArray[5] == "단수" || valueArray[5] == "OX") 
                          {
                            singleInput = valueArray[3];
                            ruleFlag = 0;
                            if(valueArray[5] == "OX")
                            {
                              singleInput = singleInput.ToUpper();
                              ruleFlag = 1;
                            }
                            newRule.singleInput = singleInput;
                          }
                          if(valueArray[5] == "목록") 
                          {
                            ruleFlag = (valueArray[2].Contains("필수") 
                              || valueArray[2].Contains("설계"))
                              ? 3 : 2;
                            multiInputRuleNumber.Add(currentRuleNum);
                          }
                        }
                        
                        newUserModel.flag = ruleFlag.ToString();
                        // rule flag : int
                        newRule.flag = ruleFlag;
                        // Web에 전체 출력
                        userModels.Add(newUserModel);
                        // 실제 Rule 저장
                        _rules.Add(newRule);
                        currentRuleNum++;
                    }

                    while(reader.NextResult()) // next sheet
                    {
                      List<Class> newClasses = new List<Class>();
                      currentSheetNum++;
                      reader.Read();reader.Read();
                      while(reader.Read())
                      {
                        // 전공 or 설계과목 : cols = 5
                        int cols = reader.FieldCount;
                        string[] valueArray = new string[cols];
                        for(int i = 0 ; i < cols ; i++)
                        {
                            if (reader.GetValue(i) == null)
                                valueArray[i] = "";
                            else
                                valueArray[i] = Regex.Replace(reader.GetValue(i).ToString(), @"\s", ""); // 과목명 내 띄어쓰기 제거
                        }
                        if (String.IsNullOrEmpty(valueArray[1])) break;
                        
                        if(!(valueArray[0].Contains("예시"))) // 대체인정 시트가 아닌경우만
                        {
                            Class newClass = new Class{
                              classCode = valueArray[1],
                              className = valueArray[2],
                              credit = Convert.ToInt32(valueArray[3].Trim()),
                              design = -1,
                              year = Convert.ToInt32(valueArray[4].Trim())
                            };
                            if(cols == 6) // 설계과목일 경우
                            {
                              newClass.design = Convert.ToInt32(valueArray[cols-2]);
                              newClass.year = Convert.ToInt32(valueArray[cols-1]);
                            }
                            newClasses.Add(newClass);
                        }
                      }
                        
                      resultList.Add(newClasses);
                      // 응답유형이 목록인 룰의 input : Sheet2에 저장

                      // todo: 과목 List간 대입으로 변경할것
                      int ruleIdx = multiInputRuleNumber[currentSheetNum-2];
                      _rules[ruleIdx].requiredClasses = newClasses;
                    }
                }
            }
            // var filename = "./wwwroot/upload/input.xls";
            //var gradeFile = "./wwwroot/upload/Sheet1.xlsx";
            string filePath = this.environment.WebRootPath;

            string inputFile = Path.Combine(filePath,"upload",fileNames[0]);
            string gradeFile = Path.Combine(filePath,"upload",fileNames[1]);

            UserInfo userInfo = new UserInfo();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            userSubjects = ReadUserSubject(gradeFile);//전체성적 파일 조회
            userInfo.GetUserSubjects(userSubjects);//수강 과목 리스트 및 이수 학점
            userInfo.GetUserInfo(inputFile);
            for(int i = 0 ; i < _rules.Count; i++)
            {
              _rules[i].userInfo = userInfo;
              _rules[i].userClasses = userSubjects;
            }
            RuleManager ruleManager = new RuleManager(_rules);
            ruleManager.CheckAllRules();
            
            List<Rule> resultRules = _rules;
            var t = new Tuple<IEnumerable<UserSubject>, UserInfo, List<Rule>>(userSubjects, userInfo, _rules) {};
            return View(t);
        }
        // User Data Read
        [HttpGet]
         public IActionResult userview()
        {
            string filePath = this.environment.WebRootPath;

            string inputFile = Path.Combine(filePath,"upload",fileNames[0]);
            string gradeFile = Path.Combine(filePath,"upload",fileNames[1]);

            UserInfo userInfo = new UserInfo();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            userSubjects = ReadUserSubject(gradeFile);//전체성적 파일 조회
            userInfo.GetUserSubjects(userSubjects);//수강 과목 리스트 및 이수 학점
            userInfo.GetUserInfo(inputFile);
            

            for(int i = 0 ; i < _rules.Count; i++)
            {
              _rules[i].userInfo = userInfo;
              _rules[i].userClasses = userSubjects;
            }

            var t = new Tuple<IEnumerable<UserSubject>, UserInfo, List<Rule>>(userSubjects, userInfo, _rules) { };
            return View(t);
        }

        public List<UserSubject> ReadUserSubject(string filename_)
        {
            List<UserSubject> temp = new List<UserSubject>();

            // 전체성적조회파일
            using (var gradeStream = System.IO.File.Open(filename_, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var gradeReader = ExcelReaderFactory.CreateReader(gradeStream))
                {
                    gradeReader.Read();
                    string tempYear = "";
                    string tempSemester = "";
                    while (gradeReader.Read())
                    {
                        string[] valueArray = new string[19];
                        for (int i = 0; i < 19; i++)
                        {
                            if (gradeReader.GetValue(i) == null)
                                valueArray[i] = "";
                            else
                                valueArray[i] = Regex.Replace(gradeReader.GetValue(i).ToString(), @"\s", "");
                        }
                        if (valueArray[2] != "")
                        {
                            tempYear = valueArray[2];
                        }
                        if (valueArray[3] != "")
                        {
                            tempSemester = valueArray[3];
                        }
                        temp.Add(new UserSubject
                        {
                            year = tempYear, // 연도
                            semester = tempSemester, // 학기
                            completionDiv = valueArray[4], // 이수구분 : 전공, 전필, 학기, 공교 등
                            completionDivField = valueArray[5], // 이수구분영역 : 기초, 전문, 자연과학 등
                            classCode = valueArray[6], // 학수번호
                            className = valueArray[8], // 과목명
                            credit = valueArray[10], // 학점
                            engineeringFactor = valueArray[16], // 공학요소 : 전공, MSC, 전문교양
                            engineeringFactorDetail = valueArray[17], // 공학세부요소 : 전공설계, 수학, 과학 등
                            english = valueArray[18], // 원어강의 종류
                            retake = valueArray[13] //재수강 여부
                        }); ;

                    }
                }

            }
            return temp;
        }
    }
}



