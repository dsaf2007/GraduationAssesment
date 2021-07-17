using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestProject
{
    /// <summary>
    /// 시작
    /// </summary>
    public class Startup
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 구성 - Configuration

        /// <summary>
        /// 구성
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - Startup(configuration)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="configuration">구성</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 서비스 컬렉션 구성하기 - ConfigureServices(services)

        /// <summary>
        /// 서비스 컬렉션 구성하기
        /// </summary>
        /// <param name="services">서비스 컬렉션</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        #endregion
        #region 구성하기 - Configure(app, environment)

        /// <summary>
        /// 구성하기
        /// </summary>
        /// <param name="app">애플리케이션 빌더</param>
        /// <param name="environment">웹 호스트 환경</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if(environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints
            (
                endpoints =>
                {
                    endpoints.MapControllerRoute
                    (
                        name    : "default",
                        pattern : "{controller=Home}/{action=Index}/{id?}"
                    );
                }
            );
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //            name: "default", template: "{controller=User}/{action=Index}/{id?}");
            //}            
            //)
        }

        #endregion
    }

}