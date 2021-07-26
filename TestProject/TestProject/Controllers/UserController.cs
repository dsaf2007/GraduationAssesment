using System.ComponentModel;
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
            List<Rule> rules = new List<Rule>(); // rule list
            List<UserModel> userModels = new List<UserModel>();
            // TODO 0: 사용자 이수교과목 데이터 파싱 및 저장(파일?)해서 룰에 대입할수있게
            // TODO 1: 실제 Sheet 에 맞게 모델 추가, 수정(컬럼 개수; 설계학점 등) 디비 정보 반영 및 순서 변경 
            // TODO 2: 하드코딩 되어있는것 반복문&클래스설계로 바꿔야할듯..?
            // TODO 3: 예외처리용 동일,대체교과목 설계
            List<Models.Math> classList = new List<Models.Math>();//수학필수
            List<BasicLiberalArts> liberalarts = new List<BasicLiberalArts>();//기초교양필수
            List<BasicKnowledge> basic_knowldege = new List<BasicKnowledge>();//기본소양필수
            List<ScienceExperiment> science_experiment = new List<ScienceExperiment>();//과학실험
            List<MSC> msc = new List<MSC>();//MSC
            List<MajorRequired> major_required = new List<MajorRequired>();//전공필수

            const string filename = "./wwwroot/upload/template_test.xlsx";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // 엑셀 sheet 번호
                    int currentSheetNum = 1;
                    // 목록 List sheet
                    int sheetNum = 1;
                    string entireRule = "";
                    string entireUserModel = "";
                    string mathTable = "";
                    string artTable = "";
                    string basicKnowledgeTable = "";
                    string scienceExperimentTable = "";
                    string mscTable = "";
                    string majorTable = "";
                    
                    string ruleType = "";
                    reader.Read();
                    while(reader.Read())
                    {
                        string singleInput = "";
                        string[] multiInput;
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
                            input = ((value_arr[5] == "목록")? "[List]" : singleInput), // 입력
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
                            newRule.singleInput = singleInput;
                            ruleFlag = (value_arr[5] == "단수")? 0 : 1;
                          }
                          if(value_arr[5] == "목록") 
                          {
                            sheetNum++;
                            multiInput = readClassesFromSheet(sheetNum);
                            ruleFlag = value_arr[2].Contains("필수") ? 3 : 2;
                            newRule.multiInput = multiInput;
                          }
                        }
                        
                        newUserModel.flag = ruleFlag.ToString();
                        newRule.flag = ruleFlag;
                        // Web에 전체 출력
                        userModels.Add(newUserModel);
                        entireUserModel += newUserModel.ToString();
                        // 실제 Rule 저장
                        rules.Add(newRule);
                        entireRule += newRule.ToString();
                        // -----------------------------------------------
                    }

                    string multiInputTest = "";
                    for(int i = 0 ; i < rules[5].multiInput.Length; i++)
                    {
                      multiInputTest += rules[5].multiInput[i] + "\n";
                    }
                    multiInputTest += rules[5].check().ToString() + "\n";

                    System.IO.File.WriteAllText(
                      Path.Combine(this.environment.WebRootPath, "sheet",
                        "test.txt"),
                      multiInputTest, System.Text.Encoding.GetEncoding("UTF-8"));

                    System.IO.File.WriteAllText(
                      Path.Combine(this.environment.WebRootPath, "sheet",
                        "Rules.txt"),
                      entireRule.ToString(), System.Text.Encoding.GetEncoding("UTF-8"));
                      
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                            "Sheet"+currentSheetNum.ToString()+".txt"),
                          entireUserModel, System.Text.Encoding.GetEncoding("UTF-8"));

                    currentSheetNum++;
                    // Math      
                    reader.NextResult();//수학 필수
                    // 엑셀 첫 두줄 패스 (컬럼명, 예시)
                    reader.Read();reader.Read();
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
                        Models.Math newMath = new Models.Math
                        {
                            classCode = value_arr[1],
                            className = value_arr[2],
                            credit = value_arr[3],
                            year = value_arr[4]
                        };
                        classList.Add(newMath);
                        mathTable += newMath.ToString();
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheetNum.ToString()+".txt"),
                          mathTable.Trim(), System.Text.Encoding.GetEncoding("UTF-8"));
                    // art
                    currentSheetNum++;
                    reader.NextResult();//교양필수
                    // 엑셀 첫 두줄 패스 (컬럼명, 예시)
                    reader.Read();reader.Read();
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
                        BasicLiberalArts newArts = new BasicLiberalArts
                        {
                            classCode = value_arr[1],
                            className = value_arr[2],
                            credit = value_arr[3],
                            year = value_arr[4]
                        };
                        liberalarts.Add(newArts);
                        artTable += newArts.ToString();
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheetNum.ToString()+".txt"),
                          artTable.Trim(), System.Text.Encoding.GetEncoding("UTF-8"));
                    // basick knowledge
                    currentSheetNum++;
                    reader.NextResult();//기본소양
                    // 엑셀 첫 두줄 패스 (컬럼명, 예시)
                    reader.Read();reader.Read();
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
                        BasicKnowledge newBasicKnowledge = new BasicKnowledge
                        {
                            classCode = value_arr[1],
                            className = value_arr[2],
                            credit = value_arr[3],
                            year = value_arr[4]
                        };
                        basic_knowldege.Add(newBasicKnowledge);
                        basicKnowledgeTable += newBasicKnowledge.ToString(); 
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheetNum.ToString()+".txt"),
                          basicKnowledgeTable.Trim(), System.Text.Encoding.GetEncoding("UTF-8"));
                    // science experiment
                    currentSheetNum++;
                    reader.NextResult();//과학실험
                    // 엑셀 첫 두줄 패스 (컬럼명, 예시)
                    reader.Read();reader.Read();
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
                        ScienceExperiment newScienceExperiment = new ScienceExperiment{
                            classCode = value_arr[1],
                            className = value_arr[2],
                            credit = value_arr[3],
                            year = value_arr[4]
                        };
                        science_experiment.Add(newScienceExperiment);
                        scienceExperimentTable += newScienceExperiment.ToString();
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheetNum.ToString()+".txt"),
                          scienceExperimentTable.Trim(), System.Text.Encoding.GetEncoding("UTF-8"));

                    // MSC
                    currentSheetNum++;
                    reader.NextResult();//MSC
                    // 엑셀 첫 두줄 패스 (컬럼명, 예시)
                    reader.Read();reader.Read();
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
                        MSC newMSC = new MSC
                        {
                            classCode = value_arr[1],
                            className = value_arr[2],
                            credit = value_arr[3],
                            year = value_arr[4]
                        };
                        msc.Add(newMSC);
                        mscTable += newMSC.ToString();
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheetNum.ToString()+".txt"),
                          mscTable.Trim(), System.Text.Encoding.GetEncoding("UTF-8"));
                    // major
                    currentSheetNum++;
                    reader.NextResult();//전공필수
                    // 엑셀 첫 두줄 패스 (컬럼명, 예시)
                    reader.Read();reader.Read();
                    while (reader.Read())
                    {
                        string[] value_arr = new string[6];
                        for (int i = 0; i < 5; i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = reader.GetValue(i).ToString();
                        }
                        MajorRequired newMajorReq = new MajorRequired
                        {
                            classCode = value_arr[1],
                            className = value_arr[2],
                            credit = value_arr[3],
                            year = value_arr[4],
                            project = value_arr[5]
                        };
                        major_required.Add(newMajorReq);
                        majorTable += newMajorReq.ToString();
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheetNum.ToString()+".txt"),
                          majorTable.Trim(), System.Text.Encoding.GetEncoding("UTF-8"));
                }
            }
            var t = new Tuple<IEnumerable<UserModel>, IEnumerable<Models.Math>, IEnumerable<BasicLiberalArts>,
                IEnumerable<BasicKnowledge>, IEnumerable<ScienceExperiment>, IEnumerable<MSC>, IEnumerable<MajorRequired>>
                (userModels, classList,liberalarts,basic_knowldege,science_experiment,msc,major_required) { };
            return View(t);
        }
        // Sheet 과목 목록 읽어오기
        public string[] readClassesFromSheet(int sheetNum)
        {
          if(sheetNum <= 1) 
            return null;
          // (ex) .../sheet/Sheet1.txt
          string filePath = Path.Combine(this.environment.WebRootPath, "sheet", "Sheet"+sheetNum.ToString()+".txt");
          if(System.IO.File.Exists(filePath))
          {
            // string text = "";
            string[] tempClasses = System.IO.File.ReadAllText(filePath).Split("\n");
            // string[] classes = new string[tempClasses.Length];
            return tempClasses;
          }
          else
            return null;
        }
    }
}
