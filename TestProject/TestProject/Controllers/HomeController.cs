using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

using TestProject.Models;

namespace TestProject.Controllers
{
    /// <summary>
    /// 홈 컨트롤러
    /// </summary>
    public class HomeController : Controller
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 로그 작업자
        /// </summary>
        private readonly ILogger<HomeController> logger;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - HomeController(logger)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="logger">로그 작업자</param>
        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 인덱스 페이지 처리하기 - Index()

        /// <summary>
        /// 인덱스 페이지 처리하기
        /// </summary>
        /// <returns>액션 결과</returns>
        public IActionResult Index()
        {
            return View();
        }

        #endregion
        #region 프라이버시 페이지 처리하기 - Privacy()

        /// <summary>
        /// 프라이버시 페이지 처리하기
        /// </summary>
        /// <returns>액션 결과</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        #endregion
        #region 에러 페이지 처리하기 - Error()

        /// <summary>
        /// 에러 페이지 처리하기
        /// </summary>
        /// <returns>액션 결과</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestID = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}