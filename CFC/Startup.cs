using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(CFC.Startup))]

namespace CFC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {


            // 如需如何設定應用程式的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkID=316888
            var sd = System.Environment.MachineName;
            bool isDebug = true;// System.Environment.MachineName.StartsWith("090");
            //var sd = Flood.FloodCal.DeSerialize<MenuDefAttribute[]>(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Config"), "DouMenuExt.xml"));
            Dou.Context.Init(new Dou.DouConfig
            {
                //SystemManagerDBConnectionName = "DouModelContextExt",
                DefaultPassword = "1234@1qaz#EDC",
                PasswordEncode = (p) =>
                {
                    return System.Web.Helpers.Crypto.HashPassword(p);
                },
                VerifyPassword = (ep, vp) =>
                {
                    return System.Web.Helpers.Crypto.VerifyHashedPassword(ep, vp);
                },
                //LoggerExpired=13,
                SessionTimeOut = 20,
                SqlDebugLog = isDebug,
                AllowAnonymous = false,
                //LoginPage = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Action("DouLoginRemember", "User"),
                LoginPage = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Action("DouLogin", "User"),
                LoggerListen = (log) =>
                {
                    if (log.WorkItem == Dou.Misc.DouErrorHandler.ERROR_HANDLE_WORK_ITEM)
                    {
                        Debug.WriteLine("DouErrorHandler發出的錯誤!!\n" + log.LogContent);
                        Logger.Log.For(null).Error("DouErrorHandler發出的錯誤!!\n" + log.LogContent);
                    }
                }
            });

            //login Remember Me 用 
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie_CFC",
                CookieSameSite=SameSiteMode.Lax,
                LoginPath = new PathString("/CFC/Login"),
                Provider = new CookieAuthenticationProvider()
                //ExpireTimeSpan = TimeSpan.FromHours(12)
            });
        }
    }
}
