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
            List<Models.Math> classList = new List<Models.Math>();//�����ʼ�
            List<BasicLiberalArts> liberalarts = new List<BasicLiberalArts>();//���ʱ����ʼ�
            List<BasicKnowledge> basic_knowldege = new List<BasicKnowledge>();//�⺻�Ҿ��ʼ�
            List<ScienceExperiment> science_experiment = new List<ScienceExperiment>();//���н���
            List<MSC> msc = new List<MSC>();//MSC
            List<MajorRequired> major_required = new List<MajorRequired>();//�����ʼ�

            const string filename = "./wwwroot/upload/testtest.xlsx";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    string entireRule = "";
                    string mathTable = "";
                    string artTable = "";
                    string BasicKnowledgeTable = "";
                    string scienceExperimentTable = "";
                    string mscTable = "";
                    string majorTable = "";
                    
                    string ruleType = "";
                    while(reader.Read())
                    {
                        string[] value_arr = new string[6]; // ��� string�ӿ� ����
                        
                        for(int i = 0; i < 6; i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = reader.GetValue(i).ToString();
                        }
                        // if (value_arr[5] == "�ܼ�")
                        // {
                        
                        if(value_arr[0] == "" || value_arr[0] == null)
                          value_arr[0] = ruleType;
                        else
                          ruleType = value_arr[0];

                        UserModel newRule = new UserModel{
                            type = ruleType, // ����
                            number = value_arr[1], // �Ϸù�ȣ
                            question = value_arr[2], // ����
                            input = value_arr[3], // �Է�
                            flag = value_arr[4], // ��������
                            reference = value_arr[5] // ���
                        };
                        rules.Add(newRule);
                        entireRule += newRule.ToString();

                        // }
                    }
                    System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet"+Path.DirectorySeparatorChar, "rule_result.txt"),
                          entireRule, System.Text.Encoding.GetEncoding("UTF-8"));
                    reader.NextResult();//���� �ʼ�
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
                        System.IO.File.WriteAllText(
                          Path.Combine(this.environment.WebRootPath, "sheet"+Path.DirectorySeparatorChar, "math.txt"),
                          entireRule, System.Text.Encoding.GetEncoding("UTF-8"));
                    }
                    reader.NextResult();//�����ʼ�
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
                        liberalarts.Add(new BasicLiberalArts
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        });
                    }
                    reader.NextResult();//�⺻�Ҿ�
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
                        basic_knowldege.Add(new BasicKnowledge
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        });
                    }
                    reader.NextResult();//���н���
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
                        science_experiment.Add(new ScienceExperiment
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        });
                    }

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
                        msc.Add(new MSC
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        });
                    }
                    reader.NextResult();//�����ʼ�
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
                        major_required.Add(new MajorRequired
                        {
                            classCode = value_arr[0],
                            className = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3],
                            project = value_arr[4]
                        });
                    }
                }
            }
            var t = new Tuple<IEnumerable<UserModel>, IEnumerable<Models.Math>, IEnumerable<BasicLiberalArts>,
                IEnumerable<BasicKnowledge>, IEnumerable<ScienceExperiment>, IEnumerable<MSC>, IEnumerable<MajorRequired>>
                (rules, classList,liberalarts,basic_knowldege,science_experiment,msc,major_required) { };
            return View(t);
        }
    }
}