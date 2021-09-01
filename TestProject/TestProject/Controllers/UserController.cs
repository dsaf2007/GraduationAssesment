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
            const string filename = "./wwwroot/upload/template_test_graduate.xlsx";

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

                        // -- Rule Generator --
                        RuleBuilder ruleBuilder = new RuleBuilder();
                        Rule newRule = ruleBuilder.SetType(ruleType)
                                                  .SetSequenceNumber(valueArray[1])
                                                  .SetQuestion(valueArray[2])
                                                  .SetSingleInput(valueArray[3])
                                                  .SetFlag(ruleFlag)
                                                  .SetReference(valueArray[5])
                                                  .Build();
                                            
                        if(valueArray[5] == "목록")
                        {
                            multiInputRuleNumber.Add(currentRuleNum);
                        }
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
            userInfo.GetUserInfo(inputFile);
            userInfo.GetUserSubjects(userSubjects);//수강 과목 리스트 및 이수 학점
           
            
            // 전체 rule 체크
            RuleManager ruleManager = new RuleManager(_rules, userInfo, userSubjects);
            ruleManager.CheckAllRules();

            // db 저장. 이거 나중에 함수로 빼면될듯
            string ruleName = "2016-1-CSE";
            foreach(Rule rule in _rules)
            {
              string ruleNumber = rule.sequenceNumber;
              string ruleAlias = rule.question;
              string ruleAttribute = (rule.flag > 1) ? ParseSubjectList(rule.requiredClasses) : rule.singleInput;
              string ruleReference = rule.reference;
              // todo: db 저장할 부분
            }
            //RuleData ruleData = new RuleData();

            
            var t = new Tuple<IEnumerable<UserSubject>, UserInfo, List<Rule>,List<string>>(userSubjects, userInfo, _rules, userInfo.exceptionList) {};
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
                        }); 
                    }
                }
            }
            return temp;
        }
        // 과목 목록 문자열로 파싱
        public string ParseSubjectList(List<Class> subjects)
        {
          List<string> subjectArray = new List<string>();
          char columnSeparator = '_';
          char subjectSeperator = ',';

          foreach(Class subject in subjects)
          {
            // UserSubject.ToString으로 할까
            List<string> subjectMembers = new List<string>()
            {
              subject.classCode,
              subject.className,
              subject.credit.ToString(),
              subject.year.ToString()
            };
            string temp = string.Join(columnSeparator, subjectMembers);

            subjectArray.Add(temp);
          }
          return string.Join(subjectSeperator, subjectArray);
        }
    }
}



