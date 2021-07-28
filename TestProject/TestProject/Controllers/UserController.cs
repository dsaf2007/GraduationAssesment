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

        public IActionResult start()
        {
            return View();
        }
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

        [HttpGet]
        public IActionResult userview()
        {
            var filename = "./wwwroot/upload/input.xls";
            var grade_file = "./wwwroot/upload/Sheet1.xlsx";
            List<UserSubject> user_subject = new List<UserSubject>();
            UserCredit user_credit = new UserCredit();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            ClassList class_list = new ClassList();
            using (var stream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    for(int i = 0;i <8;i++)
                    {
                        reader.Read();
                    }
                    while (reader.Read())
                    {

                    }
                }
            }
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
                                value_arr[i] = grade_reader.GetValue(i).ToString();
                        }
                        if(value_arr[2] !="")
                        {
                            temp_year = value_arr[2];
                        }
                        if(value_arr[3] !="")
                        {
                            temp_sem = value_arr[3];
                        }
                        user_subject.Add(new UserSubject
                        {
                            year = temp_year,
                            semester = temp_sem,
                            completion_div = value_arr[4],
                            completion_div_feild =value_arr[5],
                            class_num = value_arr[6],
                            class_name = value_arr[8],
                            credit = value_arr[10],
                            engineering_factor = value_arr[16],
                            engineering_factor_detail = value_arr[17],
                            english = value_arr[18]
                        });

                    }
                }
                int public_lib = 0; int basic_lib = 0; int major = 0; int major_arc = 0; int msc = 0;int english = 0;
                
                foreach (UserSubject user in user_subject)
                {
                    if(user.engineering_factor_detail == "기초교양(교필)")
                    {
                        public_lib+=Convert.ToInt32(user.credit);
                        class_list.public_list.Add(user.class_num);
                    }
                    if(user.engineering_factor_detail == "기본소양")
                    {
                        basic_lib += Convert.ToInt32(user.credit);
                        class_list.basic_list.Add(user.class_num);
                    }
                    if(user.engineering_factor == "MSC/BSM")
                    {
                        msc += Convert.ToInt32(user.credit);
                        class_list.msc_list.Add(user.class_num);
                    }
                    if(user.engineering_factor == "전공")
                    {
                        major += Convert.ToInt32(user.credit);
                        if(user.completion_div == "전필")
                        {
                            class_list.major_essential_list.Add(user.class_num);
                        }
                        if(user.engineering_factor_detail == "전공설계")
                        {
                            major_arc += Convert.ToInt32(user.credit);
                            class_list.major_arc_list.Add(user.class_num);
                            continue;
                        }
                        class_list.major_list.Add(user.class_num);
                    }
                    if(user.english == "영어")
                    {
                        english += Convert.ToInt32(user.credit);
                        class_list.english_list.Add(user.class_num);
                    }
                }
                user_credit.english = english;
                user_credit.basic_lib = basic_lib;
                user_credit.major = major;
                user_credit.major_arc = major_arc;
                user_credit.msc = msc;
                user_credit.public_lib = public_lib;
            }
                        var t =new Tuple<IEnumerable<UserSubject>, UserCredit,ClassList>(user_subject, user_credit, class_list) { };
                        return View(t);
        }
    }
    
}