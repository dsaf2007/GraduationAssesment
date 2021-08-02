using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestProject
{
    /// <summary>
    /// ����
    /// </summary>
    public class Startup
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region ���� - Configuration

        /// <summary>
        /// ����
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region ������ - Startup(configuration)

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="configuration">����</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region ���� �÷��� �����ϱ� - ConfigureServices(services)

        /// <summary>
        /// ���� �÷��� �����ϱ�
        /// </summary>
        /// <param name="services">���� �÷���</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        #endregion
        #region �����ϱ� - Configure(app, environment)

        /// <summary>
        /// �����ϱ�
        /// </summary>
        /// <param name="app">���ø����̼� ����</param>
        /// <param name="environment">�� ȣ��Ʈ ȯ��</param>
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