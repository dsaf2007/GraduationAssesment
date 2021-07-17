using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TestProject
{
    /// <summary>
    /// 프로그램
    /// </summary>
    public class Program
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Public

        #region 프로그램 시작하기 - Main(argumentArray)

        /// <summary>
        /// 프로그램 시작하기
        /// </summary>
        /// <param name="argumentArray">인자 배열</param>
        public static void Main(string[] argumentArray)
        {
            CreateHostBuilder(argumentArray).Build().Run();
        }

        #endregion
        #region 호스트 빌더 생성하기 - CreateHostBuilder(argumentArray)

        /// <summary>
        /// 호스트 빌더 생성하기
        /// </summary>
        /// <param name="argumentArray">인자 배열</param>
        /// <returns>호스트 빌더</returns>
        public static IHostBuilder CreateHostBuilder(string[] argumentArray) =>
            Host.CreateDefaultBuilder(argumentArray)
                .ConfigureWebHostDefaults
                (
                    builder =>
                    {
                        builder.UseStartup<Startup>();
                    }
                );

        #endregion
    }
}