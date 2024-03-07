using CFC.Models.Manager;
using Dou;
using Dou.Controllers;
using Dou.Misc.Attr;
using Dou.Models.DB;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.Manager
{
    [MenuDef(Name = "使用者管理", MenuPath = "系統管理", Action = "Index", Index = 1, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class UserController : UserBaseControll<User, Role>
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        bool isTestSso = false;
        string SsoServer = "http://pj.ftis.org.tw/Sample/DouSsoImp/";
        //string SsoServer = "http://localhost:51513/";
        /// <summary>
        /// login remember me
        /// </summary>
        /// <param name="user"></param>
        /// <param name="returnUrl"></param>
        /// <param name="redirectLogin"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public ActionResult DouLoginRemember(User user, string returnUrl, bool redirectLogin = false, bool re = false)
        {
            if (user.Id == null && User.Identity.IsAuthenticated)//記憶1天自動登入 login remember me
            {
                user.Id = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(f => f.Type == "Id")?.Value;
                if (user.Id != null)
                {
                    user = GetModelEntity().Find(user.Id) ?? user;
                    redirectLogin = false;
                }
            }
            //SSO 測試
            if (isTestSso)
            {
                var _ssotoken = HttpContext.Request.QueryString["ssotoken"];
                if (_ssotoken != null)
                {
                    var ssou = GetUserInfoSSO(_ssotoken);
                    if ((bool)ssou.Success)
                    {
                        string ssoid = (string)ssou.User;
                        User u = FindUser(ssoid.Trim());
                        if (u != null)
                        {
                            user = u;
                            redirectLogin = false;
                        }
                        else
                        {
                            //to do somthing
                            //throw new Exception($"{ssoid} 使用者不存在!!");
                            user.Id = ssoid;
                        }
                    }//to do fail
                    else
                    {
                        ViewBag.ErrorMessage = ssou.Desc;
                    }
                }
                else
                {
                    return new RedirectResult(SsoServer + "User/DouLoginRemember?redirectLogin=true&returnUrl=" + HttpUtility.UrlEncode(HttpContext.Request.Url + ""));
                }
            }

            ActionResult v = base.DouLogin(user, returnUrl, redirectLogin);

            if (re && ViewBag.ErrorMessage == null && user.Id != null && !User.Identity.IsAuthenticated)//login remember me
            {
                var identity = new ClaimsIdentity(new[] {new Claim("Id", user.Id)
                                    }, "ApplicationCookie" + Dou.Context.Config.AppName);

                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(1), //記憶1天
                    IsPersistent = re,//rememberMe,
                    AllowRefresh = true
                }, identity);
            }
            return v is RedirectResult || v is RedirectToRouteResult ? v : PartialView("DouLoginRemember", user);
        }

        dynamic GetUserInfoSSO(string token)
        {
            //TODO:序列化
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonText = serializer.Serialize(new
            {
                token = token
            });

            var jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonText);

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(SsoServer + "User/UserInfo?token=" + token);
            request.Method = System.Net.WebRequestMethods.Http.Get;
            //request.ContentType = "application/json";
            //request.ContentLength = jsonBytes.Length;

            try
            {
                //using (var requestStream = request.GetRequestStream())
                //{
                //    requestStream.Write(jsonBytes, 0, jsonBytes.Length);
                //    requestStream.Flush();
                //}
                using (var response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        return Newtonsoft.Json.Linq.JObject.Parse(reader.ReadToEnd()); ;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                dynamic obj = new System.Dynamic.ExpandoObject();
                obj.Success = false;
                obj.Desc = ex.Message;
                return obj;
            }
        }

        protected override Dou.Models.DB.IModelEntity<User> GetModelEntity()
        {
            return new ModelEntity<User>(RoleController._dbContext); //與RoleController._dbContext共用這樣cache的RoleUsers才會一致
        }
        public override ActionResult DouLogoff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut("ApplicationCookie" + Dou.Context.Config.AppName); //清除login remember me
            if (isTestSso)
            {
                base.DouLogoff();
                return Redirect(SsoServer + "User/DouLogoff?returnUrl=" + HttpUtility.UrlEncode(Context.Config.LoginPage + ""));
            }
            else
                return base.DouLogoff();
        }

        //[TestAction]
        [MenuDef(AllowAnonymous =true)]
        public override ActionResult DouLogin(User user, string returnUrl, bool redirectLogin = false)
        {
            ActionResult v = base.DouLogin(user, returnUrl, redirectLogin);
            //return v is RedirectToRouteResult ? v : PartialView(user);
            if (v is RedirectResult)
            {
                if ((v as RedirectResult).Url == "" || (v as RedirectResult).Url == "~/")
                    v = RedirectToAction("Index");
            }
            else if(v is RedirectToRouteResult)
            { 
            }
            else
                v = PartialView(user);
            if (returnUrl == null && !redirectLogin && (user.Id == null) && ViewBag.ErrorMessage == "輸入資料不完整!")
                ViewBag.ErrorMessage = "";

            return v;
        }
    }
}