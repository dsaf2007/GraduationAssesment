using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TestProject.Controllers
{
    /// <summary>
    /// 테스트 컨트롤러
    /// </summary>
    public class TestController : Controller
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 웹 호스트 환경
        /// </summary>
        private IWebHostEnvironment environment;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - TestController(environment)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="environment">웹 호스트 환경</param>
        public TestController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 업로드 페이지 처리하기 - Upload()

        /// <summary>
        /// 업로드 페이지 처리하기
        /// </summary>
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        #endregion
        #region 업로드 페이지 처리하기 - Upload(fileCollection)

        /// <summary>
        /// 업로드 페이지 처리하기
        /// </summary>
        ///// <param name="fileCollection">파일 컬렉션</param>
        /// <returns>액션 결과 태스크</returns>
        [HttpPost]
        public async Task<IActionResult> Upload(ICollection<IFormFile> fileCollection)
        {
            var uploadDirectoryPath = Path.Combine(this.environment.WebRootPath, "upload\\");

            foreach(IFormFile formFile in fileCollection)
            {
                if(formFile.Length > 0)
                {
                    string fileName = Path.GetFileName
                    (
                        ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Value
                    );

                    using(FileStream stream = new FileStream(Path.Combine(uploadDirectoryPath, fileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return View();
        }

        #endregion
        #region 다운로드 페이지 처리하기 - Download(fileName)

        /// <summary>
        /// 다운로드 페이지 처리하기
        /// </summary>
        public FileResult Download(string fileName = "Test.txt")
        {
            byte[] fileByteArray = System.IO.File.ReadAllBytes
            (
                Path.Combine(this.environment.WebRootPath, "upload", fileName)
            );

            return File(fileByteArray, "application/octet-stream", fileName);
        }

        #endregion
    }
}