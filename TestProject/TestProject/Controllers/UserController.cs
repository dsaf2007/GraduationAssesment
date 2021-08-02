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
        List<Rule> rules = new List<Rule>();
        List<UserSubject> userSubjects = new List<UserSubject>();
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
        public IActionResult Index()
        {
            // List<Rule> rules = new List<Rule>(); // rule list
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
                    int currentSheetNum = 1;
                    List<int> multiInputRuleNumber = new List<int>();
                    //  List sheet
                    // int sheetNum = 1;
                    // string entireRule = "";
                    string entireUserModel = "";
                    string ruleType = "";
                    // will be passed to View

                    reader.Read();
                    while(reader.Read())
                    {
                        string singleInput = "";
                        // string[] multiInput;
                        int ruleFlag = -1;

                        string[] value_arr = new string[6]; // 모두 string임에 주의
                        
                        for(int i = 0; i < 6; i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = reader.GetValue(i).ToString();
                        }
                        if(value_arr[0] == "" || value_arr[0] == null)
                          value_arr[0] = ruleType;
                        else
                          ruleType = value_arr[0];
                        
                        UserModel newUserModel = new UserModel{
                            type = ruleType, // 구분
                            number = value_arr[1], // 일련번호
                            question = value_arr[2], // 질문
                            input = ((value_arr[5] == "목록")? "[List]" : value_arr[3]), // 입력
                            flag = ruleFlag.ToString(), // 응답유형
                            reference = value_arr[5] // 비고
                        };
                        // -----------------------------------------------
                        // Rule Generator
                        Rule newRule = new Rule{
                            type = ruleType, // 구분
                            number = value_arr[1], // 일련번호
                            question = value_arr[2], // 질문
                            singleInput = null,
                            multiInput = null,
                            flag = ruleFlag, // 응답유형
                            reference = value_arr[5] // 비고
                        };
                        // flag setting
                        // 0: 대소비교
                        // 1: OX
                        // 2: 목록 중 선택
                        // 3: 목록 전체 필수
                        if(value_arr[5] != "" || value_arr[5] != null) // 기본정보: 비고 란 비어있음
                        {
                          if(value_arr[5] == "단수" || value_arr[5] == "OX") 
                          {
                            singleInput = value_arr[3];
                            newRule.singleInput = singleInput.ToUpper();
                            ruleFlag = (value_arr[5] == "단수" 
                                          && !("OX".Contains(singleInput.ToUpper()))) 
                                        ? 0 : 1;
                          }
                          if(value_arr[5] == "목록") 
                          {
                            ruleFlag = (value_arr[2].Contains("필수") 
                              || value_arr[2].Contains("기초설계") 
                              || value_arr[2].Contains("종합설계")) 
                              ? 3 : 2;
                            multiInputRuleNumber.Add(Convert.ToInt32(newRule.number));
                          }
                        }
                        
                        newUserModel.flag = ruleFlag.ToString();
                        newRule.flag = ruleFlag;
                        // Web에 전체 출력
                        userModels.Add(newUserModel);
                        entireUserModel += newUserModel.ToString();
                        // 실제 Rule 저장
                        rules.Add(newRule);
                    }

                    while(reader.NextResult()) // next sheet
                    {
                      string classTable = "";
                      List<Class> newClasses = new List<Class>();
                      currentSheetNum++;
                      reader.Read();reader.Read();
                      while(reader.Read())
                      {
                        // 전공 or 설계과목 : cols = 5
                        int cols = reader.FieldCount;
                        string[] value_arr = new string[cols];
                        for(int i = 0 ; i < cols ; i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = Regex.Replace(reader.GetValue(i).ToString(), @"\s", ""); // 과목명 내 띄어쓰기 제거
                        }
                        if (String.IsNullOrEmpty(value_arr[1])) break;
                        
                        if(!(value_arr[0].Contains("예시"))) // 대체인정 시트가 아닌경우만
                        {
                            Class newClass = new Class{
                              classCode = value_arr[1],
                              className = value_arr[2],
                              credit = Convert.ToInt32(value_arr[3].Trim()),
                              design = -1,
                              year = Convert.ToInt32(value_arr[4].Trim())
                            };
                            if(cols == 6) // 설계과목일 경우
                            {
                              newClass.design = Convert.ToInt32(value_arr[cols-2]);
                              newClass.year = Convert.ToInt32(value_arr[cols-1]);
                            }
                            newClasses.Add(newClass);
                            classTable += newClass.ToString();
                        }
                      }
                      if(String.IsNullOrEmpty(classTable))
                        classTable = "empty";
                        
                      resultList.Add(newClasses);
                      // 응답유형이 목록인 룰의 input : Sheet2에 저장
                      // sheet rule: 1부터 시작하므로 -1

                      // todo: 과목 List간 대입으로 변경할것
                      this.rules[multiInputRuleNumber[currentSheetNum-2]-1].multiInput = classTable.Trim().Split("\n");
                      this.rules[multiInputRuleNumber[currentSheetNum-2]-1].requiredClasses = newClasses;
                    }
                }
            }
            List<Rule> resultRules = this.rules;
            var t = new Tuple<IEnumerable<UserModel>, List<List<Class>>, List<Rule>> (userModels, resultList, resultRules) {};
            return View(t);
        }
        // User Data Read
        [HttpGet]
        public IActionResult userview()
        {
            var filename = "./wwwroot/upload/input.xls";
            var grade_file = "./wwwroot/upload/user_score.xlsx";

            List<UserSubject> userSubjects = new List<UserSubject>();
            // List<Class> userClasess = new List<Class>;
            
            UserCredit user_credit = new UserCredit();
            ClassList class_list = new ClassList();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // 이수과목확인표
            using (var stream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    for(int i = 0; i < 8; i++)
                    {
                        reader.Read();
                    }
                    while (reader.Read()) 
                    { }
                }
            }
            // 전체성적조회파일
            using (var grade_stream = System.IO.File.Open(grade_file,System.IO.FileMode.Open,System.IO.FileAccess.Read))
            {
                using (var grade_reader = ExcelReaderFactory.CreateReader(grade_stream))
                {
                    grade_reader.Read();
                    string temp_year = "";
                    string temp_sem = "";
                    while(grade_reader.Read())
                    {
                        string[] value_arr = new string[19];
                        for (int i = 0; i < 19; i++)
                        {
                            if (grade_reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = Regex.Replace(grade_reader.GetValue(i).ToString(), @"\s", "");
                        }
                        if(value_arr[2] != "")
                        {
                            temp_year = value_arr[2];
                        }
                        if(value_arr[3] != "")
                        {
                            temp_sem = value_arr[3];
                        }
                        userSubjects.Add(new UserSubject
                        {
                            year = temp_year, // 연도
                            semester = temp_sem, // 학기
                            completionDiv = value_arr[4], // 이수구분 : 전공, 전필, 학기, 공교 등
                            completionDivField =value_arr[5], // 이수구분영역 : 기초, 전문, 자연과학 등
                            classCode = value_arr[6], // 학수번호
                            className = value_arr[8], // 과목명
                            credit = value_arr[10], // 학점
                            engineeringFactor = value_arr[16], // 공학요소 : 전공, MSC, 전문교양
                            engineeringFactorDetail = value_arr[17], // 공학세부요소 : 전공설계, 수학, 과학 등
                            english = value_arr[18] // 원어강의 종류
                        });

                    }
                }
                int public_lib = 0; int basic_lib = 0; int majorCredit= 0; int majorDesignCredit = 0; int msc = 0;int english = 0;
                
                foreach (UserSubject userSubject in userSubjects)
                {
                    if(userSubject.engineeringFactorDetail == "기초교양(교필)")
                    {
                        public_lib += Convert.ToInt32(userSubject.credit);
                        class_list.publicClasses.Add(userSubject.classCode);
                    }
                    if(userSubject.engineeringFactorDetail == "기본소양")
                    {
                        basic_lib += Convert.ToInt32(userSubject.credit);
                        class_list.basicClasses.Add(userSubject.classCode);
                    }
                    if(userSubject.engineeringFactor == "MSC/BSM")
                    {
                        msc += Convert.ToInt32(userSubject.credit);
                        class_list.mscClasses.Add(userSubject.classCode);
                    }
                    if(userSubject.engineeringFactor == "전공")
                    {
                        majorCredit+= Convert.ToInt32(userSubject.credit);
                        if(userSubject.completionDiv == "전필")
                        {
                            class_list.majorEssentialList.Add(userSubject.classCode);
                        }
                        if(userSubject.engineeringFactorDetail == "전공설계")
                        {
                            majorDesignCredit += Convert.ToInt32(userSubject.credit);
                            class_list.majorClasses.Add(userSubject.classCode);
                            continue;
                        }
                        class_list.majorClasses.Add(userSubject.classCode);
                    }
                    if(userSubject.english == "영어")
                    {
                        english += Convert.ToInt32(userSubject.credit);
                        class_list.englishList.Add(userSubject.classCode);
                    }
                }
                user_credit.englishCredit = english; // 영어
                user_credit.basicLibCredit = basic_lib;
                user_credit.majorCredit= majorCredit;
                user_credit.majorDesignCredit = majorDesignCredit; // 전공설계
                user_credit.mscCredit = msc;
                user_credit.publicLibCredit = public_lib;
            }
            this.userSubjects = userSubjects;
            var t = new Tuple<IEnumerable<UserSubject>, UserCredit, ClassList>(userSubjects, user_credit, class_list) { };
            return View(t);
        }
    }
}
