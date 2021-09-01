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


namespace TestProject.Controllers
{
    public class AdminController : Controller
    {
        private IWebHostEnvironment environment;
        public AdminController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            /*
            1. Get RuleTemplate List form database
            2. Send to View
            */
            ////동일유사전공교과목 처리
            List<SimillarMajor> simillarList = new List<SimillarMajor>();
            List<DiffMajor> diffMajorList = new List<DiffMajor>();
            
            List<Rule> rules = new List<Rule>();
            // RULE_NAME_TB : INDEX_NUM, RULE_NAME
            // RULE_TB : INDEX_NUM, RULE_{NAME, NUM(INT), ALIAS, ATTRIBUTE, REFERENCE}

            using (MySqlConnection connection = new MySqlConnection("Server=118.67.128.31;Port=5555;Database=test;Uid=CSDC;Pwd=1q2w3e4r"))
            {
                string selectQuery = "SELECT * from SIMILLAR";

                string selectQuery2 = "SELECT * FROM RULE_NAME_TB"; // for admin
                string selectQuery3 = "SELECT * FROM RULE_TB"; // for detail

                connection.Open();
                MySqlCommand command = new MySqlCommand(selectQuery, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["PREV_CLASS_START"].ToString());
                        if(reader["PREV_CLASS_START"].ToString() == "null")
                            simillarList.Add(new SimillarMajor
                            {
                                currClassName = reader["CURR_CLASS_NAME"].ToString(),
                                currClassStartYear = Convert.ToInt32(reader["CURR_CLASS_START"].ToString()),
                                prevClassName = reader["PREV_CLASS_NAME"].ToString(),
                                prevClassStartYear = 0,//시작년도가 없는 경우 0으로 대체
                                prevClassEndYear = Convert.ToInt32(reader["PREV_CLASS_END"].ToString())
                            });
                        else
                            simillarList.Add(new SimillarMajor
                            {
                                currClassName = reader["CURR_CLASS_NAME"].ToString(),
                                currClassStartYear = Convert.ToInt32(reader["CURR_CLASS_START"].ToString()),
                                prevClassName = reader["PREV_CLASS_NAME"].ToString(),
                                prevClassStartYear = Convert.ToInt32(reader["PREV_CLASS_START"].ToString()),
                                prevClassEndYear = Convert.ToInt32(reader["PREV_CLASS_END"].ToString())
                            });
                    }
                }

                selectQuery = "SELECT * FROM DIFF_MAJOR";
                command = new MySqlCommand(selectQuery, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        diffMajorList.Add(new DiffMajor
                        {
                            startYear = Convert.ToInt32(reader["START_YEAR"].ToString()),
                            endYear = Convert.ToInt32(reader["END_YEAR"].ToString()),
                            classCode = reader["CLASS_CODE"].ToString(),
                            className = reader["CLASS_NAME"].ToString(),
                            otherMajor = reader["OTHER_MAJOR"].ToString(),
                            otherClassCode = reader["OTHER_CLASS_CODE"].ToString(),
                            otherClassName = reader["OTHER_CLASS_NAME"].ToString()
                        });
                    }
                    //}
                    connection.Close();
                }
                temp = this.majorClasses;

                foreach (UserSubject major in temp)
                {
                    foreach (SimillarMajor simillar in simillarList)
                    {
                        if (major.className == simillar.prevClassName)// 수강한 과목이 이전 전공명과 동일 할 경우(ex. 14년도 교육과정 적용 학생이 주니어디자인프로젝트가 아닌 공개sw수강)
                        {
                            if(Convert.ToInt32(this.applicationYear) <= simillar.prevClassEndYear && Convert.ToInt32(this.applicationYear) >= simillar.prevClassStartYear)
                            {
                              //  exceptionList.Add(simillar.prevClassName + "과목이 동일유사전공교과목인 " + major.className + " 으로 수강되었는지 확인하십시오.");
                            }
                        }
                    }
                }

                foreach (string exceptionList_ in exceptionList)
                {
                    Console.WriteLine(exceptionList_);
                }

        }
            return View(t);
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
            var uploadDirectoryPath = Path.Combine(this.environment.WebRootPath, "upload" + Path.DirectorySeparatorChar);

            foreach (IFormFile formFile in fileCollection)
            {
                if (formFile.Length > 0)
                {
                    string fileName = Path.GetFileName
                    (
                        ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Value
                    );
                    using (FileStream stream = new FileStream(Path.Combine(uploadDirectoryPath, fileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            // return View();
            return RedirectToAction("Index", "Admin");
        }
    }
}