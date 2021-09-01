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
using MySql.Data.MySqlClient;

namespace ReadExcel.Controllers
{
    public class UserController : Controller
    {
        public static List<Rule> _rules = new List<Rule>();
        public static List<Rule> tempRules = new List<Rule>();
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
          System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
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
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // --- Read DB ---
            using (MySqlConnection connection = new MySqlConnection("Server=118.67.128.31;Port=5555;Database=test;Uid=CSDC;Pwd=1q2w3e4r"))
            {
              string selectQuery = "SELECT * FROM RULE_TB";
              connection.Open();
              MySqlCommand command = new MySqlCommand(selectQuery, connection);

              using (var reader = command.ExecuteReader())
              {
                while (reader.Read())
                {
                  List<Class> tempClasses = new List<Class>();
                  Rule tempRule = new Rule();
                  RuleBuilder ruleBuilder = new RuleBuilder();

                  tempRule = ruleBuilder.SetSequenceNumber(reader["RULE_NUM"].ToString())
                                        .SetQuestion(reader["RULE_ALIAS"].ToString())
                                        .SetSingleInput(reader["RULE_ATTRIBUTE"].ToString())
                                        .SetReference(reader["RULE_REFERENCE"].ToString())
                                        .Build();
                                        
                  if(tempRule.reference == "목록")
                    tempRule.requiredClasses = ParseSubjectString(reader["RULE_ATTRIBUTE"].ToString());
                  tempRules.Add(tempRule);
                }
              }
            }
            // // ----------
            // using (var stream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            // {
            //     using(var reader = ExcelReaderFactory.CreateReader(stream))
            //     {
            //         // sheet 
            //         int currentRuleNum = 0;
            //         int currentSheetNum = 1;
            //         List<int> multiInputRuleNumber = new List<int>();
            //         string ruleType = "";
            //         // will be passed to View
            //         reader.Read();
            //         while(reader.Read())
            //         {
            //             int ruleFlag = -1;
            //             string[] valueArray = new string[6]; // 모두 string임에 주의
                        
            //             for(int i = 0; i < 6; i++)
            //             {
            //                 if (reader.GetValue(i) == null)
            //                     valueArray[i] = "";
            //                 else
            //                     valueArray[i] = reader.GetValue(i).ToString();
            //             }
            //             if(valueArray[0] == "" || valueArray[0] == null)
            //               valueArray[0] = ruleType;
            //             else
            //               ruleType = valueArray[0];

            //             // -- Rule Generator --
            //             RuleBuilder ruleBuilder = new RuleBuilder();
            //             Rule newRule = ruleBuilder.SetType(ruleType)
            //                                       .SetSequenceNumber(valueArray[1])
            //                                       .SetQuestion(valueArray[2])
            //                                       .SetSingleInput(valueArray[3])
            //                                       .SetFlag(ruleFlag)
            //                                       .SetReference(valueArray[5])
            //                                       .Build();
                                            
            //             if(valueArray[5] == "목록")
            //             {
            //                 multiInputRuleNumber.Add(currentRuleNum);
            //             }
            //             // 실제 Rule 저장
            //             _rules.Add(newRule);
            //             currentRuleNum++;
            //         }

            //         while(reader.NextResult()) // next sheet
            //         {
            //           List<Class> newClasses = new List<Class>();
            //           currentSheetNum++;
            //           reader.Read();reader.Read();
            //           while(reader.Read())
            //           {
            //             // 전공 or 설계과목 : cols = 5
            //             int cols = reader.FieldCount;
            //             string[] valueArray = new string[cols];
            //             for(int i = 0 ; i < cols ; i++)
            //             {
            //                 if (reader.GetValue(i) == null)
            //                     valueArray[i] = "";
            //                 else
            //                     valueArray[i] = Regex.Replace(reader.GetValue(i).ToString(), @"\s", ""); // 과목명 내 띄어쓰기 제거
            //             }
            //             if (String.IsNullOrEmpty(valueArray[1])) break;
                        
            //             if(!(valueArray[0].Contains("예시"))) // 대체인정 시트가 아닌경우만
            //             {
            //                 Class newClass = new Class{
            //                   classCode = valueArray[1],
            //                   className = valueArray[2],
            //                   credit = Convert.ToInt32(valueArray[3].Trim()),
            //                   design = -1,
            //                   year = Convert.ToInt32(valueArray[4].Trim())
            //                 };
            //                 if(cols == 6) // 설계과목일 경우
            //                 {
            //                   newClass.design = Convert.ToInt32(valueArray[cols-2]);
            //                   newClass.year = Convert.ToInt32(valueArray[cols-1]);
            //                 }
            //                 newClasses.Add(newClass);
            //             }
            //           }
                  
            //           int ruleIdx = multiInputRuleNumber[currentSheetNum-2];
            //           _rules[ruleIdx].requiredClasses = newClasses;
            //         }
            //     }
            // }
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
           
            // string ruleName = userInfo.applicationYear + "-" + userInfo.major;
            // major: 컴퓨터공학전공->컴퓨터, 정보통신공학 -> 정보통신 등으로 template 기초정보와
            // 호환 되도록 자르면
            // template파일 통해 만든 ruleName 과
            // userInfo 통해 만든 ruleName 호환 가능
            // 그러면 
            // select * from RULE_TB where template.ruleName = userInfo.ruleName 같은 방식으로 가능

            // 전체 rule 체크
            // RuleManager ruleManager = new RuleManager(_rules, userInfo, userSubjects);
            RuleManager ruleManager = new RuleManager(tempRules, userInfo, userSubjects);

            ruleManager.CheckAllRules();
            //RuleData ruleData = new RuleData();

            // var t = new Tuple<IEnumerable<UserSubject>, UserInfo, List<Rule>,List<string>>(userSubjects, userInfo, _rules, userInfo.exceptionList) {};
            var t = new Tuple<IEnumerable<UserSubject>, UserInfo, List<Rule>,List<string>>(userSubjects, userInfo, tempRules, userInfo.exceptionList) {};

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
        // 학수번호_과목명_학점_연도,...,
        public string ParseSubjectList(List<Class> subjects)
        {
          List<string> subjectList = new List<string>();
          char columnSeparator = '_';
          char subjectSeperator = '%';

          foreach(Class subject in subjects)
          {
            List<string> subjectMembers = new List<string>()
            {
              subject.classCode,
              subject.className,
              subject.credit.ToString(),
              subject.year.ToString()
            };
            string temp = string.Join(columnSeparator, subjectMembers);

            subjectList.Add(temp);
          }
          return string.Join(subjectSeperator, subjectList);
        }
        // db에서 읽어온 과목 리스트를 Class List로 복구 
        public List<Class> ParseSubjectString(string subjectString)
        {
          char columnSeparator = '_';
          char subjectSeperator = '%';
          List<Class> subjectList = new List<Class>();
          List<string> subjects = subjectString.Split(subjectSeperator).ToList();

          foreach(string subject in subjects)
          {
            Class s = new Class();
            List<string> subjectColumns = subject.Split(columnSeparator).ToList();
            if(subjectColumns.Count > 3)
            {
              s.classCode = subjectColumns[0];
              s.className = subjectColumns[1];
              s.credit = Convert.ToInt32(subjectColumns[2]);
              s.year = Convert.ToInt32(subjectColumns[3]);
            }
            subjectList.Add(s);
          }
          return subjectList;
        }
    }
}



