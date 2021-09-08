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
using ReadExcel.Controllers;

using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using MySql.Data.MySqlClient;


namespace TestProject.Controllers
{
    public class AdminController : Controller
    {
        public static List<Rule> _rules = new List<Rule>();
        public static List<string> fileNames = new List<string>();

        private IWebHostEnvironment environment;
        public AdminController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(ICollection<IFormFile> fileCollection)//파일 업로드
        {
          System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string fileName = "";
            string templateFilePath = "";
            var uploadDirectoryPath = Path.Combine(this.environment.WebRootPath, "upload"+Path.DirectorySeparatorChar);
            fileNames.Clear();
            foreach(IFormFile formFile in fileCollection)
            {
                if(formFile.Length > 0)
                {
                    fileName = Path.GetFileName
                    (
                        ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Value
                    );
                    fileNames.Add(fileName);// 업로드 파일리스트 추가.
                    templateFilePath = Path.Combine(uploadDirectoryPath, fileName);
                    using(FileStream stream = new FileStream(templateFilePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // ----------
            using (var stream = System.IO.File.Open(templateFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
            using (MySqlConnection connection = new MySqlConnection("Server=118.67.128.31;Port=5555;Database=test;Uid=CSDC;Pwd=1q2w3e4r"))
            {
              connection.Open();
              string ruleName = "2016-1-TEST";

              string insertQuery = "INSERT INTO RULE_NAME_TB(RULE_NAME) VALUES('" + ruleName + "')";

              MySqlCommand command = new MySqlCommand(insertQuery, connection);
              command.ExecuteNonQuery();

              var userController = new UserController(environment); 
              foreach(Rule rule in _rules)
              {
                insertQuery = "INSERT INTO RULE_TB(RULE_NAME, RULE_NUM, RULE_ALIAS, RULE_ATTRIBUTE, RULE_REFERENCE) VALUES("
                                    + "'" + ruleName
                                    + "'," + rule.sequenceNumber
                                    + ",'" + rule.question
                                    + "','" + ((rule.flag > 1) ? userController.ParseSubjectList(rule.requiredClasses) : rule.singleInput)
                                    + "','" + rule.reference
                                    + "')";
                command = new MySqlCommand(insertQuery, connection);
                command.ExecuteNonQuery();
              }
            }
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult Index()
        {
            /*
            1. Get RuleTemplate Name List form database
            2. Send to View
            */
            List<string> ruleNames = new List<string>();

            // RULE_NAME_TB : INDEX_NUM, RULE_NAME -> Admin View
            // RULE_TB : INDEX_NUM, RULE_{NAME, NUM(INT), ALIAS, ATTRIBUTE, REFERENCE} -> User View
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            
            using (MySqlConnection connection = new MySqlConnection("Server=118.67.128.31;Port=5555;Database=test;Uid=CSDC;Pwd=1q2w3e4r"))
            {
                string selectQuery = "SELECT * from RULE_NAME_TB";
                connection.Open();
                MySqlCommand command = new MySqlCommand(selectQuery, connection);


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ruleNames.Add(reader["RULE_NAME"].ToString());
                    }
                }
            }
            var t = new Tuple<List<string>>(ruleNames);
            return View(t);
        }
        //Default GET method
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
    }
}