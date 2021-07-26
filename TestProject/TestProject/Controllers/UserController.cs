using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
        private IWebHostEnvironment environment;
        public UserController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        //Default GET method
        [HttpGet]
        public IActionResult Index()
        {
            List<UserModel> rules = new List<UserModel>();
            List<Models.Math> classList = new List<Models.Math>();//수학필수
            List<BasicLiberalArts> liberalarts = new List<BasicLiberalArts>();//기초교양필수
            List<BasicKnowledge> basic_knowldege = new List<BasicKnowledge>();//기본소양필수
            List<ScienceExperiment> science_experiment = new List<ScienceExperiment>();//과학실험
            List<MSC> msc = new List<MSC>();//MSC
            List<MajorRequired> major_required = new List<MajorRequired>();//전공필수

            const string filename = "./wwwroot/upload/testtest.xlsx";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int currentSheet = 1;
                    int sheetNum = 1;
                    string entireRule = "";
                    string mathTable = "";
                    string artTable = "";
                    string basicKnowledgeTable = "";
                    string scienceExperimentTable = "";
                    string mscTable = "";
                    string majorTable = "";
                    
                    string ruleType = "";
                    // 
                    while(reader.Read())
                    {
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

                        string ruleInput = "";
                        // string singleValue = "단수";
                        if(value_arr[5] == "단수") 
                        {
                          ruleInput = value_arr[3];
                        }
                        if(value_arr[5] == "목록") 
                        {
                          sheetNum++;
                          ruleInput = readFromSheet(sheetNum);
                          // string[] classes = readFromSheet(sheetNum);
                          // for(int i = 1 ; i < classes.Length; i++)
                          // {
                          //   ruleInput += classes[i]+"\n";
                          // }
                        }
                        UserModel newRule = new UserModel{
                            type = ruleType, // 구분
                            number = value_arr[1], // 일련번호
                            question = value_arr[2], // 질문
                            input = ruleInput, // 입력
                            flag = value_arr[4], // 응답유형
                            reference = value_arr[5] // 비고
                        };
                        rules.Add(newRule);
                        entireRule += newRule.ToString();

                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                            "Sheet"+currentSheet.ToString()+".txt"),
                          entireRule, System.Text.Encoding.GetEncoding("UTF-8"));

                    currentSheet++;
                    // Math      
                    reader.NextResult();//수학 필수
                    while (reader.Read())
                    {
                        string[] value_arr = new string[4];
                        for (int i = 0; i < 4; i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = reader.GetValue(i).ToString();
                        }
                        Models.Math newMath = new Models.Math
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        };
                        classList.Add(newMath);
                        mathTable += newMath.ToString();
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheet.ToString()+".txt"),
                          mathTable, System.Text.Encoding.GetEncoding("UTF-8"));
                    // art
                    currentSheet++;
                    reader.NextResult();//교양필수
                    while (reader.Read())
                    {
                        string[] value_arr = new string[4];
                        for (int i = 0; i < 4; i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = reader.GetValue(i).ToString();
                        }
                        BasicLiberalArts newArts = new BasicLiberalArts
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        };
                        liberalarts.Add(newArts);
                        artTable += newArts.ToString();
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheet.ToString()+".txt"),
                          artTable, System.Text.Encoding.GetEncoding("UTF-8"));
                    // basick knowledge
                    currentSheet++;
                    reader.NextResult();//기본소양
                    while (reader.Read())
                    {
                        string[] value_arr = new string[4];
                        for (int i = 0; i < 4; i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = reader.GetValue(i).ToString();
                        }
                        BasicKnowledge newBasicKnowledge = new BasicKnowledge
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        };
                        basic_knowldege.Add(newBasicKnowledge);
                        basicKnowledgeTable += newBasicKnowledge.ToString(); 
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheet.ToString()+".txt"),
                          basicKnowledgeTable, System.Text.Encoding.GetEncoding("UTF-8"));
                    // science experiment
                    currentSheet++;
                    reader.NextResult();//과학실험
                    while (reader.Read())
                    {
                        string[] value_arr = new string[4];
                        for (int i = 0; i < 4; i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = reader.GetValue(i).ToString();
                        }
                        ScienceExperiment newScienceExperiment = new ScienceExperiment
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        };
                        science_experiment.Add(newScienceExperiment);
                        scienceExperimentTable += newScienceExperiment.ToString();
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheet.ToString()+".txt"),
                          scienceExperimentTable, System.Text.Encoding.GetEncoding("UTF-8"));

                    // MSC
                    currentSheet++;
                    reader.NextResult();//MSC
                    while (reader.Read())
                    {
                        string[] value_arr = new string[4];
                        for (int i = 0; i < 4; i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = reader.GetValue(i).ToString();
                        }
                        MSC newMSC = new MSC
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        };
                        msc.Add(newMSC);
                        mscTable += newMSC.ToString();
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheet.ToString()+".txt"),
                          mscTable, System.Text.Encoding.GetEncoding("UTF-8"));
                    // major
                    currentSheet++;
                    reader.NextResult();//전공필수
                    while (reader.Read())
                    {
                        string[] value_arr = new string[5];
                        for (int i = 0; i < 5; i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = reader.GetValue(i).ToString();
                        }
                        MajorRequired newMajorReq = new MajorRequired
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3],
                            project = value_arr[4]
                        };
                        major_required.Add(newMajorReq);
                        majorTable += newMajorReq.ToString();
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheet.ToString()+".txt"),
                          majorTable, System.Text.Encoding.GetEncoding("UTF-8"));
                }
            }
            var t = new Tuple<IEnumerable<UserModel>, IEnumerable<Models.Math>, IEnumerable<BasicLiberalArts>,
                IEnumerable<BasicKnowledge>, IEnumerable<ScienceExperiment>, IEnumerable<MSC>, IEnumerable<MajorRequired>>
                (rules, classList,liberalarts,basic_knowldege,science_experiment,msc,major_required) { };
            return View(t);
        }
        public string readFromSheet(int sheetNum)
        {
          if(sheetNum <= 1) 
            return null;
          string filePath = Path.Combine(this.environment.WebRootPath, "sheet", "Sheet"+sheetNum.ToString()+".txt");
          if(System.IO.File.Exists(filePath))
          {
            string text = "";
            string[] classes = System.IO.File.ReadAllText(filePath).Split("\n");
            for(int i = 1; i < classes.Length; i++)
            {
              text += classes[i] + "_";
            }
            return text;
          }
          else
            return null;
        }
    }


}
