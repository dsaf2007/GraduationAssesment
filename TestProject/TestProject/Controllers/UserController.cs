using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadExcel.Models;
using ExcelDataReader;
namespace ReadExcel.Controllers
{
    public class UserController : Controller
    {
        //Default GET method
        [HttpGet]
        public IActionResult Index()
        {
            List<UserModel> users = new List<UserModel>();
            List<Models.Math> classList = new List<Models.Math>();//수학필수
            List<BasicLiberalArts> liberalarts = new List<BasicLiberalArts>();//기초교양필수
            List<BasicKnowledge> basic_knowldege = new List<BasicKnowledge>();//기본소양필수
            List<ScienceExperiment> science_experiment = new List<ScienceExperiment>();//과학실험
            List<MSC> msc = new List<MSC>();//MSC
            List<MajorRequired> major_required = new List<MajorRequired>();//전공필수


            var filename = "./wwwroot/upload/testtest.xlsx";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while(reader.Read())
                    {
                        string[] value_arr=new string[5];
                        for(int i = 0; i< 5;i++)
                        {
                            if (reader.GetValue(i) == null)
                                value_arr[i] = "";
                            else
                                value_arr[i] = reader.GetValue(i).ToString();
                        }
                        if (value_arr[4] == "단수")
                        {
                            users.Add(new UserModel
                            {
                                A = value_arr[0],
                                B = value_arr[1],
                                C = value_arr[2],
                                D = value_arr[3],
                                E = value_arr[4]
                            });
                        }
                    }
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
                        classList.Add(new Models.Math
                        {
                            class_num = value_arr[0],
                            class_name = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        });
                    }
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
                        liberalarts.Add(new BasicLiberalArts
                        {
                            class_num = value_arr[0],
                            class_name = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        });
                    }
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
                        basic_knowldege.Add(new BasicKnowledge
                        {
                            class_num = value_arr[0],
                            class_name = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        });
                    }
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
                        science_experiment.Add(new ScienceExperiment
                        {
                            class_num = value_arr[0],
                            class_name = value_arr[1],
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
                            class_num = value_arr[0],
                            class_name = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3]
                        });
                    }
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
                        major_required.Add(new MajorRequired
                        {
                            class_num = value_arr[0],
                            class_name = value_arr[1],
                            credit = value_arr[2],
                            year = value_arr[3],
                            project = value_arr[4]
                        });
                    }
                }
            }
            var t = new Tuple<IEnumerable<UserModel>, IEnumerable<Models.Math>, IEnumerable<BasicLiberalArts>,
                IEnumerable<BasicKnowledge>, IEnumerable<ScienceExperiment>, IEnumerable<MSC>, IEnumerable<MajorRequired>>
                (users, classList,liberalarts,basic_knowldege,science_experiment,msc,major_required) { };
            return View(t);
        }
    }
}