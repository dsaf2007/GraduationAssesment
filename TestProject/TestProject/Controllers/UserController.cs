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
            List<classnum> classList = new List<classnum>();
            var filename = "./wwwroot/upload/test2.xlsx";
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
                        users.Add(new UserModel
                        {
                            A = value_arr[0],
                            B = value_arr[1],
                            C = value_arr[2],
                            D = value_arr[3],
                            E = value_arr[4]
                        });
                    }
                    reader.NextResult();
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
                        users.Add(new UserModel
                        {
                            A = value_arr[0],
                            B = value_arr[1],
                            C = value_arr[2],
                            D = value_arr[3],
                            E = value_arr[4]
                        });
                        //string[] value_arr = new string[4];
                        //for (int i = 0; i < 4; i++)
                        //{
                        //    if (reader.GetValue(i) == null)
                        //        value_arr[i] = "";
                        //    else
                        //        value_arr[i] = reader.GetValue(i).ToString();
                        //}
                        //classList.Add(new classnum
                        //{
                        //    num = value_arr[0],
                        //    name = value_arr[1],
                        //    grade = value_arr[2],
                        //    year = value_arr[3]
                        //});
                    }
                }
            }
            return View(users);
        }
    }
}