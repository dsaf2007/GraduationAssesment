using System.Collections;
using System.ComponentModel;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadExcel.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
// 인코딩..
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
            List<List<Class>> resultList = new List<List<Class>>();

            List<UserModel> userModels = new List<UserModel>();
            // TODO 0: 사용자 이수교과목 데이터 파싱 및 저장(파일?)해서 룰에 대입할수있게
            // TODO 1: 실제 Sheet 에 맞게 모델 추가, 수정(컬럼 개수; 설계학점 등) 디비 정보 반영 및 순서 변경 
            // TODO 2: 하드코딩 되어있는것 반복문&클래스설계로 바꿔야할듯..?
            // TODO 3: 예외처리용 동일,대체교과목 설계
            List<BasicLiberalArts> liberalarts = new List<BasicLiberalArts>(); // 공통교양
            List<BasicKnowledge> basic_knowldege = new List<BasicKnowledge>(); // 기본소양
            List<Models.Math> classList = new List<Models.Math>();//수학필수
            List<ScienceExperiment> science_experiment = new List<ScienceExperiment>();//과학실험
            // TODO 과학필수, 전산학필수, 전공필수, (기초,요소,종합)설계, 전공동일교과, MSC대체, 타학과전공
            List<MajorRequired> major_required = new List<MajorRequired>();//전공필수
            List<MSC> msc = new List<MSC>();//MSC

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
                    // will be passed to View

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
                            // sheetNum++;
                            // multiInput = readClassesFromSheet(sheetNum);
                            ruleFlag = (value_arr[2].Contains("필수") 
                              || value_arr[2].Contains("기초설계") 
                              || value_arr[2].Contains("종합설계")) 
                              ? 3 : 2;
                            // newRule.multiInput = multiInput; // 나중에 sheet[n].txt 완성하면서 넣어줌
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
                        entireRule += newRule.ToString();
                        // -----------------------------------------------
                    }
                    // ------ rule test start 
                    // string ruleTest = "";

                    // for(int i = 0 ; i < rules[5].multiInput.Length; i++)
                    // {
                    //   ruleTest += rules[5].multiInput[i] + "\n";
                    // }
                    // ruleTest += rules[17].check().ToString() + "\n";
                    
                    // System.IO.File.WriteAllText(
                    //   Path.Combine(this.environment.WebRootPath, "sheet",
                    //     "test.txt"),
                    //   ruleTest, System.Text.Encoding.GetEncoding("UTF-8"));
                    // test end -----------

                    System.IO.File.WriteAllText(
                      Path.Combine(this.environment.WebRootPath, "sheet",
                        "Rules.txt"),
                      entireRule.ToString(), System.Text.Encoding.GetEncoding("UTF-8"));
                    
                    // System.IO.File.WriteAllText(
                    //       Path.Combine(this.environment.WebRootPath, "sheet",
                    //         "Sheet"+currentSheetNum.ToString()+".txt"),
                    //       entireUserModel, System.Text.Encoding.GetEncoding("UTF-8"));

                    // currentSheetNum++;
                    // ---- Rule End ------------------------

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
                      System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet",
                          "Sheet"+currentSheetNum.ToString()+".txt"),
                          classTable.Trim(), System.Text.Encoding.GetEncoding("UTF-8"));
                      // 응답유형이 목록인 룰의 input : Sheet2에 저장
                      // sheet rule: 1부터 시작하므로 -1
                      rules[multiInputRuleNumber[currentSheetNum-2]-1].multiInput = classTable.Trim().Split("\n");
                    }
                }
            }
            var t = new Tuple<IEnumerable<UserModel>, List<List<Class>>, List<Rule>> (userModels, resultList, rules) {};
            return View(t);
        }
    
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
