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
            1. Get RuleTemplate Name List form database
            2. Send to View
            */
            List<string> ruleNames = new List<string>();

            // RULE_NAME_TB : INDEX_NUM, RULE_NAME -> Admin View
            // RULE_TB : INDEX_NUM, RULE_{NAME, NUM(INT), ALIAS, ATTRIBUTE, REFERENCE} -> User View

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