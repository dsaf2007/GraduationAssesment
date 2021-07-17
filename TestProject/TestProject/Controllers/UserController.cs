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
            var filename = "./wwwroot/upload/Users.xlsx";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while(reader.Read())
                    {
                        users.Add(new UserModel
                        {
                            Name = reader.GetValue(0).ToString(),
                            Email = reader.GetValue(1).ToString(),
                            Phone = reader.GetValue(2).ToString()
                        });
                    }
                }
            }
            return View(users);
        }
    }
}