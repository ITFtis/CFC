using CFC.Controllers.Api;
using CFC.Models;
using CFC.Models.Api;
using CFC.Models.Prj;
using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using static CFC.Models.Api.ApiResultReturn;

namespace CFC.Controllers.CFC
{


    public class CFCController : Controller
    {

        private DouModelContext db = new DouModelContext();

        const string STREAMLINE = "streamline";

        // GET: CFC
        [Authorize]
        public ActionResult Index(User_Properties_Advance m)
        {
            //if (!User.Identity.IsAuthenticated)//記憶1天自動登入 login remember me
            //{
            //    return RedirectToAction("Login");
            //}

            // 取得cookie內使用者資訊(僅會記錄30分鐘)
            Boolean isGuest = m.Id == CFCDataController.GUEST;

            //ViewBag.UserID = m.Id;
            //ViewBag.UserName = m.Name;
            ViewBag.UserID = Session["loginUserId"];
            ViewBag.UserName = Session["loginUserName"];

            if (Session["loginUserId"].ToString().Trim().ToUpper() == "GUEST")
                isGuest = true;

            ViewBag.IsGuest = isGuest;
            if (STREAMLINE == m.Name)
                ViewBag.STREAMLINE = m.Name;

            string userID = Convert.ToString(ViewBag.UserID);
            m.Id = userID;

            // 專案儲存用
            ViewBag.IndustriaTypes = DateViewController.AllIndustrialType.OrderBy(e => e.DisplayOrder);
            ViewBag.Cities = DateViewController.AllCity.OrderBy(e => e.DisplayOrder);
            //ViewBag.ProjectLogs = this.db.UserProjects.Where(e => e.UserID == m.Id).ToList();
            //ViewBag.ProjectInputs = this.db.userInputAdvance.Where(e => e.UserID == m.Id && e.IsSave == true).ToList();
            ViewBag.ProjectLogs = this.db.UserProjects.Where(e => e.UserID == userID).ToList();
            ViewBag.ProjectInputs = this.db.userInputAdvance.Where(e => e.UserID == userID && e.IsSave == true).ToList();


            // 燃料
            var thisFuelProperties = DateViewController.AllFuelProperties;
            ViewBag.FuelProperties_solid = thisFuelProperties.Where(s => s.FuelType == "solid");
            ViewBag.FuelProperties_staticFluid = thisFuelProperties.Where(s => s.FuelType == "staticFluid");
            ViewBag.FuelProperties_gas = thisFuelProperties.Where(s => s.FuelType == "gas");
            ViewBag.FuelProperties_dynamicFluid = thisFuelProperties.Where(s => s.FuelType == "dynamicFluid");

            // 電力
            var theElecProperties = DateViewController.AllElecProperties.OrderByDescending(s => s.year);
            ViewBag.ElecProperties = theElecProperties;

            // 冷媒逸散
            var theRefrigerantEquip = DateViewController.AllRefrigerantEquip;
            ViewBag.RefrigerantEquip = theRefrigerantEquip;

            var theRefrigerantType = DateViewController.AllRefrigerantType;
            ViewBag.RefrigerantType = theRefrigerantType;

            // 其他逸散
            ViewBag.EscapeType = DateViewController.AllEscapeType;
            ViewBag.EscapeProperties = DateViewController.AllEscapeProperties;

            // 特定製程
            ViewBag.SpecificType = DateViewController.AllSpecificType;
            ViewBag.SpecificProperties = DateViewController.AllSpecificProperties;

            // 類別3
            ViewBag.Type3 = new Type3Mark[] {
                new Type3Mark{name = "類別三-運輸-上游原物料配送" , title="Tr01" },
                new Type3Mark{name = "類別三-運輸-商務旅遊" , title="Tr02" },
                new Type3Mark{name = "類別三-運輸-員工通勤" , title="Tr03" },
                new Type3Mark{name = "類別三-運輸-下游的運輸及配送" , title="Tr04" },
                new Type3Mark{name = "類別四-組織使用產品-採購" , title="Cp01" },
                new Type3Mark{name = "類別四-組織使用產品-資本" , title="Cp02" },
                new Type3Mark{name = "類別四-組織使用產品-能源相關活動" , title="Cp03" },
                new Type3Mark{name = "類別四-組織使用產品-營運廢棄物" , title="Cp04" },
                new Type3Mark{name = "類別四-組織使用產品-上游資產租賃" , title="Cp05" },
                //new Type3Mark{name = "類別五-使用組織產品-加工" , title="Us01" },
                new Type3Mark{name = "類別五-使用組織產品-使用" , title="Us02" },
                new Type3Mark{name = "類別五-使用組織產品-報廢" , title="Us03" },
                new Type3Mark{name = "類別五-使用組織產品-下游租賃" , title="Us04" },
                new Type3Mark{name = "類別五-使用組織產品-加盟" , title="Us05" },
                new Type3Mark{name = "類別五-使用組織產品-投資" , title="Us06" },
                new Type3Mark{name = "類別六-其他排放" , title="Other" },
            };

            return PartialView(m);
        }


        [TrackingTime]
        [TestAction]
        public ActionResult Login(User_Properties_Advance u, bool? login)
        {
            //var asdd=Guid.NewGuid().ToString();
            ModelState.Clear();
            AddRecord();

            ViewBag.IndustrailTypes = Api.DateViewController.AllIndustrialType;
            ViewBag.City = Api.DateViewController.AllCity;
            ViewBag.IndustrialArea = Api.DateViewController.AllIndustrialArea;

            // 訪客登入，或是歷史登入的
            if ((u?.Id == CFCDataController.GUEST) || (login != null && login.Value))
            {
                if (u.Id == null || u.Id.Trim() == "")
                    ModelState.AddModelError("ErrMsg", "請輸入帳號資訊");
                else if (u.Pass == null || u.Pass.Trim() == "")
                    ModelState.AddModelError("ErrMsg", "密碼資訊不可為空");

                // 判斷帳號密碼是否正確
                else
                {
                    var loginUser = DateViewController.AllUserProperties.FirstOrDefault(e => e.Id.Equals(u.Id) && e.Pass.Equals(u.Pass));
                    if (loginUser != null)// 登入成功
                    {
                        // 塞一個cookie進去
                        var identity = new ClaimsIdentity(
                            new[] {
                                new Claim("Id", loginUser.Id),
                                new Claim("Name", loginUser.Name),
                                new Claim("IsGuest" , loginUser.Id.Equals(CFCDataController.GUEST)?"true":"false"),// 判斷是否為訪客
                            }, "ApplicationCookie_CFC");
                        HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30), //記憶30分
                            IsPersistent = false,//rememberMe,
                            AllowRefresh = true
                        }, identity);
                        Session["loginUserId"] = loginUser.Id;
                        Session["loginUserName"] = loginUser.Name;
                        //return RedirectToAction("Index", new User_Properties_Advance { Name = loginUser.Name, Id = loginUser.Id });
                        return RedirectToAction("Index");
                        //寫SESSION, 給48行讀取

                    }

                    // 登入失敗
                    else
                        ModelState.AddModelError("ErrMsg", "帳號密碼錯誤");

                }
            }

            ViewBag.RecordCount = _RecordCount.Count + "";
            return PartialView(u);
        }
        public ActionResult Guest()
        {
            return RedirectToAction("Login", new User_Properties_Advance { Id = CFCDataController.GUEST, Pass = "Ftis01801726" });
        }
        public ActionResult SL()
        {
            //return RedirectToAction("Login", new User_Properties { IndustryId = DateViewController.GUEST, Name = STREAMLINE });
            Index(new User_Properties_Advance());
            AddRecord();
            var userid = "guest";
            ViewBag.NO = userid;
            ViewBag.STREAMLINE = STREAMLINE;
            return PartialView("Index", new User_Properties_Advance());
        }

        // 取得特定 USER 關聯的工廠清單
        [HttpPost]
        public JsonResult GetFactoriesByUser(string userId)
        {
            var factories = from uf in db.UserFactory
                            join f in db.SysFactory on uf.FACTORY_REGISTRATION equals f.FACTORY_REGISTRATION
                            where uf.USER_ID == userId
                            select new
                            {
                                f.FACTORY_NAME,
                                f.FACTORY_REGISTRATION
                            };

            return Json(factories.ToList(), JsonRequestBehavior.AllowGet);
        }


        // 取得特定工廠的詳細資訊
        [HttpPost]
        public JsonResult GetFactoryDetails(string factoryRegistration)
        {
            //var factory = db.SysFactory
            //                .Where(f => f.FACTORY_REGISTRATION == factoryRegistration)
            //                .Select(f => new
            //                {
            //                    f.FACTORY_NAME,
            //                    f.FACTORY_REGISTRATION,
            //                    f.FACTORY_CITY,
            //                    f.FACTORY_DISTRICT,
            //                    f.FACTORY_ADDRESS,
            //                    f.FACTORY_INDUSTRIAL,
            //                    f.FACTORY_INDUSTRIAL_AREA
            //                }).FirstOrDefault();
            var factory = (from f in db.SysFactory
                           join gi in db.GlobalIndustrial on f.FACTORY_INDUSTRIAL equals gi.Id
                           join gia in db.GlobalIndustrialArea on f.FACTORY_INDUSTRIAL_AREA equals gia.Id
                           where f.FACTORY_REGISTRATION == factoryRegistration
                           select new
                           {
                               f.FACTORY_NAME,
                               f.FACTORY_REGISTRATION,
                               f.FACTORY_CITY,
                               f.FACTORY_DISTRICT,
                               f.FACTORY_ADDRESS,
                               FACTORY_INDUSTRIAL = gi.Name,            // 替換成 Global_Industrial.Name
                               FACTORY_INDUSTRIAL_AREA = gia.Name        // 替換成 Global_IndustrialArea.Name
                           }).FirstOrDefault();

            return Json(factory, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetFactoryProperties(string FactoryRegistration)
        {
            var factoryProperties = DateViewController.All_SYS_FACTORY_properties.FirstOrDefault(e => e.FACTORY_REGISTRATION.Equals(FactoryRegistration));

            // 將資料轉換為 JSON 格式返回前端
            return Json(factoryProperties);
        }

        [HttpPost]
        public JsonResult GetCompanyProperties(string CompanyUniformNumber)
        {
            var CompanyProperties = DateViewController.All_SYS_COMPANY_properties.FirstOrDefault(e => e.COMP_UNIFORM_NUMBER.Equals(CompanyUniformNumber));

            // 將資料轉換為 JSON 格式返回前端
            if (CompanyProperties == null)
            {
                CompanyProperties = new SYS_COMPANY();
                CompanyProperties.COMP_NAME = "";
                CompanyProperties.COMP_SIZE = "";
            }
            return Json(CompanyProperties);
        }



        // 新增用戶
        [HttpPost]
        public ActionResult CreateUser(User_Properties_Advance user)
        {
            //for (int i = 0; i < user.FactoryList.Count; i++)
            //{
            //    SYS_FACTORY cFactory = (SYS_FACTORY)(user.FactoryList[i]);
            //    this.db.SysFactory.Add(cFactory);
            //    this.db.SaveChanges();
            //    //console.log(factories[i].FACTORY_NAME); // 取出工廠名稱
            //    //console.log(factories[i].FACTORY_REGISTRATION); // 取出工廠登記證
            //    //console.log(factories[i].FACTORY_CITY); // 取出工廠所在縣市
            //    // ...繼續取出其他欄位
            //}


            StringBuilder errorMes = new StringBuilder();
            if (user.Id == null || user.Id.Trim() == "")
                errorMes.Append("請填寫帳號資料<br/>");
            else if (DateViewController.AllUserProperties.Where(e => e.Id.Equals(user.Id)).FirstOrDefault() != null)
                errorMes.Append("已有該帳號資料<br/>");

            if (user.Pass == null || user.Pass.Trim() == "")
                errorMes.Append("密碼不可為空<br/>");

            if (user.UniformNumber == null || user.UniformNumber.Trim() == "")
            {
                errorMes.Append("統一編號不可為空<br/>");
            }
            else
            {
                if ((user.UniformNumber.Length == 8 && user.UniformNumber.All(char.IsDigit)) == false)
                    errorMes.Append("統一編號只能是數字且8碼<br/>");
            }

            if (user.Name == null || user.Name.Trim() == "")
                errorMes.Append("請填寫公司名稱<br/>");

            if (user.Contact == null || user.Contact.Trim() == "")
                errorMes.Append("請填寫聯絡人名稱<br/>");

            if (user.PhoneNumber == null || user.PhoneNumber.Trim() == "")
                errorMes.Append("請填寫聯絡電話<br/>");
            else if (!user.PhoneNumber.All(char.IsDigit))
                errorMes.Append("聯絡電話必須全為數字<br/>");

            if (user.Email == null || user.Email.Trim() == "")
                errorMes.Append("請填寫電子郵件<br/>");
            else
            {
                // 定義檢查 Email 格式的正規表示式
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

                if (!Regex.IsMatch(user.Email, emailPattern))
                {
                    errorMes.Append("請輸入有效的電子郵件地址<br/>");
                }
            }

            //若是製造業, 要檢查工廠登記證是不是都是數字或字母
            if (user.Manufacturing == "Manufacturing")
            {
                if (user.FactoryList == null || user.FactoryList.Count == 0)
                {
                    errorMes.Append("製造業會員必須填寫工廠資料<br/>");
                }

                if (user.FactoryList != null)
                {
                    for (int i = 0; i < user.FactoryList.Count; i++)
                    {
                        string s = ((SYS_FACTORY)(user.FactoryList[i])).FACTORY_REGISTRATION;
                        if (Regex.IsMatch(s, @"^[a-zA-Z0-9]+$") == false)
                        {
                            errorMes.Append("工廠登記證( " + s + " )非數字或英文字母<br/>");
                        }
                    }
                }
            }


            // 任何有錯誤都拋出去
            if (errorMes.Length != 0)
                return Json(new { success = false, desc = errorMes.ToString(), JsonRequestBehavior.AllowGet });
            // 紀錄一筆資訊
            else
            {
                //var sCompany = this.db.SysCompany.Where(a => a.COMP_UNIFORM_NUMBER == user.UniformNumber).First();

                //先把公司資料存起來統一編號存起來
                if (this.db.SysCompany.Any(a => a.COMP_UNIFORM_NUMBER == user.UniformNumber))
                {
                    var sCompany = this.db.SysCompany
                        .Where(a => a.COMP_UNIFORM_NUMBER == user.UniformNumber)
                        .First();
                    // 使用 sCompany 進行後續操作
                    sCompany.COMP_NAME = user.Name;
                    sCompany.COMP_SIZE = user.CompanySize;
                    sCompany.UId = user.Id;
                    sCompany.UDate = DateFormat.ToDate4(DateTime.Now);
                }
                else
                {
                    SYS_COMPANY newCompany = new SYS_COMPANY();
                    newCompany.COMP_NAME = user.Name;
                    newCompany.COMP_SIZE = user.CompanySize;
                    newCompany.COMP_UNIFORM_NUMBER = user.UniformNumber;
                    newCompany.BId = user.Id;
                    newCompany.BDate = DateFormat.ToDate4(DateTime.Now);
                    this.db.SysCompany.Add(newCompany);
                    
                }
                SYS_COMPANYNameSelectItems.Reset();

                //製造業才有1對多個工廠的關係
                if (user.Manufacturing == "Manufacturing")
                {
                    //這裡要先存FACTORY
                    //先判斷該登記證，若不存在, 就新增一筆, 若存在, 就更新
                    //var factoryProperties = DateViewController.All_SYS_FACTORY_properties.FirstOrDefault(e => e.FACTORY_REGISTRATION.Equals(FactoryRegistration));
                    for (int i = 0; i < user.FactoryList.Count; i++)
                    {
                        SYS_FACTORY cFactory = (SYS_FACTORY)(user.FactoryList[i]);
                        var factoryProperties = DateViewController.All_SYS_FACTORY_properties.FirstOrDefault(e => e.FACTORY_REGISTRATION.Equals(cFactory.FACTORY_REGISTRATION));
                        if (factoryProperties == null)
                        {
                            cFactory.BId = user.Id;
                            cFactory.BDate = DateFormat.ToDate4(DateTime.Now);
                            this.db.SysFactory.Add(cFactory);
                        }
                        else
                        {
                            var f = this.db.SysFactory.Where(a => a.FACTORY_REGISTRATION == factoryProperties.FACTORY_REGISTRATION).First();
                            f.FACTORY_NAME = cFactory.FACTORY_NAME;
                            f.FACTORY_CITY = cFactory.FACTORY_CITY;
                            f.FACTORY_DISTRICT = cFactory.FACTORY_DISTRICT; ;
                            f.FACTORY_ADDRESS = cFactory.FACTORY_ADDRESS;
                            f.FACTORY_INDUSTRIAL = cFactory.FACTORY_INDUSTRIAL;
                            f.FACTORY_INDUSTRIAL_AREA = f.FACTORY_INDUSTRIAL_AREA;
                            //f.UDate = DateTime.Now.ToString("yyyymmddhhmmss");
                            //f.UId = cFactory.UId;
                            f.UId = user.Id;
                            f.UDate = DateFormat.ToDate4(DateTime.Now);
                        }

                        SYS_FACTORY.ResetGetAllDatas();

                        //會員與工廠的關聯
                        G_USER_FACTORY uf = new G_USER_FACTORY();
                        uf.USER_ID = user.Id;
                        uf.FACTORY_REGISTRATION = cFactory.FACTORY_REGISTRATION;
                        uf.BDate = cFactory.BDate;
                        uf.BId = cFactory.BId;
                        this.db.UserFactory.Add(uf);

                        //console.log(factories[i].FACTORY_NAME); // 取出工廠名稱
                        //console.log(factories[i].FACTORY_REGISTRATION); // 取出工廠登記證
                        //console.log(factories[i].FACTORY_CITY); // 取出工廠所在縣市
                        // ...繼續取出其他欄位
                    }
                }

                user.BDate = DateTime.Now;
                user.BId = user.Id;

                this.db.userPropertiesAdvance.Add(user);

                try
                {
                    this.db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                    throw; // 可選：重新拋出異常以便進一步處理
                }

                //for (int i = 0; i < user.FactoryList.Count; i++)
                //{
                //    SYS_FACTORY cFactory = (SYS_FACTORY)(user.FactoryList[i]);



                //}
                //this.db.SaveChanges();

                // 然後登入
                return Json(new { success = true, desc = "新增成功，請重新登入", JsonRequestBehavior.AllowGet });
            }
        }

        public ActionResult SaveProject(SaveProjectModel saveProject)
        {
            var userInput = this.db.userInputAdvance.Where(e => e.RowID == saveProject.RowID
                && e.UserID == saveProject.UserID).FirstOrDefault();

            if (userInput == null)
                return Json(new { success = false, desc = "查無此紀錄" }, JsonRequestBehavior.DenyGet);

            

            userInput.IsSave = true;
            userInput.FACTORY_REGISTRATION = saveProject.FactoryRegistration;
            userInput.StartDate = saveProject.StartDate;
            userInput.EndDate = saveProject.EndDate;
            //userInput.ProjectAddress = saveProject.ProjectAddress;
            //userInput.ProjectCity = saveProject.ProjectCity;
            userInput.ProjectName = saveProject.ProjectName;
            //userInput.ProjectIndustrialType = saveProject.ProjectIndustrialType;
            //userInput.ProjectIndustrialID = saveProject.ProjectIndustrialID;
            userInput.BDate = saveProject.BDate;
            userInput.BId = saveProject.BId;
            userInput.Memo = saveProject.ProjectMemo;

            try
            {
                this.db.SaveChanges();

                //(LogCount次數) 儲存專案
                this.db.LogCount.Add(new Log_count()
                {
                    Type = 3,
                    MapId = userInput.RowID,
                    BDate = DateTime.Now,
                    BId = userInput.UserID,
                });
                this.db.SaveChanges();
                Log_count.ResetGetAllDatas();

                return Json(new { success = true, desc = "紀錄成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, desc = e.ToString() }, JsonRequestBehavior.DenyGet);
            }
        }

        /*
         取的專案內容
         */
        [HttpPost]
        public ActionResult GetProject(SaveProjectModel project)
        {
            // 這邊的project只是借用model而已，僅需要UserID 跟 RowID

            var userInput = this.db.userInputAdvance.Where(e => e.RowID == project.RowID
                  && e.UserID == project.UserID).FirstOrDefault();
            if (userInput == null)
                return Json(new { success = false, desc = "查無此紀錄" }, JsonRequestBehavior.DenyGet);

            CalInputModel outInput = new CalInputModel();
            outInput.UserID = project.UserID;
            outInput.RowID = project.RowID;
            outInput.electInput = new ElectInput
            {
                elecVolume = userInput.elecVolume,
                elecYear = userInput.elecYear
            };

            outInput.calModel = userInput.ARType;
            outInput.fuelInputs = this.db.FuelVolumes.Where(e => e.RowId == userInput.RowID)
            .Select(e => new FuelInputs
            {
                UseVolume = e.UseVolume,
                FuelId = e.FuelId
            }).ToList();

            outInput.escapeInputs = this.db.EscapeVolume.Where(e => e.RowId == userInput.RowID)
                .Select(e => new EscapeInput
                {
                    UseVolume = e.UseVolume,
                    EscapeType = e.EscapeType,
                    EscapeId = e.EscapeId
                }).ToList();

            outInput.refrigerantInputs = this.db.RefrigerantVolume.Where(e => e.RowId == userInput.RowID)
                .Select(e => new RefrigerantInput
                {
                    UseVolume = e.UseVolume,
                    RefrigerantType = e.RefrigerantType,
                    RefrigerantEquip = e.RefrigerantEquip

                }).ToList();

            outInput.steamInput = new SteamInput
            {
                SteamVolume = userInput.SteamVolume,
                SteamCoe = userInput.SteamCoe,
            };

            outInput.specialInputs = this.db.SpecificVolume.Where(e => e.RowId == userInput.RowID)
                .Select(e => new SpecialInput
                {
                    UseVolume = e.UseVolume,
                    CreateType = e.CreateType,
                    CreateId = e.CreateId,
                }).ToList();

            outInput.Tr01 = userInput.Tr01;
            outInput.Tr02 = userInput.Tr02;
            outInput.Tr03 = userInput.Tr03;
            outInput.Tr04 = userInput.Tr04;
            outInput.Cp01 = userInput.Cp01;
            outInput.Cp02 = userInput.Cp02;
            outInput.Cp03 = userInput.Cp03;
            outInput.Cp04 = userInput.Cp04;
            outInput.Cp05 = userInput.Cp05;
            outInput.Us01 = userInput.Us01;
            outInput.Us02 = userInput.Us02;
            outInput.Us03 = userInput.Us03;
            outInput.Us04 = userInput.Us04;
            outInput.Us05 = userInput.Us05;
            outInput.Us06 = userInput.Us06;
            outInput.Other = userInput.Other;

            return Json(new { success = true, data = outInput, desc = "查詢成功" }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult DeleteProject(User_Input_Advance userInput)
        {
            var projectDetail = db.userInputAdvance.Where(e => e.UserID == userInput.UserID && e.RowID == userInput.RowID).FirstOrDefault();
            if (projectDetail == null)
                return Json(new { success = false, desc = "查無此筆紀錄" }, JsonRequestBehavior.AllowGet);

            db.userInputAdvance.Remove(projectDetail);
            db.SaveChanges();
            return Json(new { success = true, desc = "刪除成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditProject(SaveProjectModel saveProject)
        {
            var projectDetail = db.userInputAdvance.Where(e => e.UserID == saveProject.UserID && e.RowID == saveProject.RowID).FirstOrDefault();
            if (projectDetail == null)
                return Json(new { success = false, desc = "查無此筆紀錄" }, JsonRequestBehavior.AllowGet);

            return Json(new { success = true, data = projectDetail, desc = "讀取該筆項目" }, JsonRequestBehavior.AllowGet);
        }


        // 登出
        public ActionResult Logoff()
        {
            // 清除cookie資訊
            HttpContext.GetOwinContext().Authentication.SignOut("ApplicationCookie_CFC"); //清除login remember me
            return RedirectToAction("Login");
        }


        // 到這個畫面的紀錄次數
        //------------------------------------------------------------------------------------------------

        // 加上一筆登入紀錄(會吃到專案目錄下的 Config/RecordCount.xml)
        RecordCount _RecordCount;
        public void AddRecord()
        {
            string f = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(("~/Config")), "RecordCount.xml");
            if (_RecordCount == null)
            {
                if (System.IO.File.Exists(f))
                    _RecordCount = DouHelper.Misc.DeSerialize<RecordCount>(f);
                else
                {
                    _RecordCount = new RecordCount { Count = 435, LastDateTime = DateTime.Now };
                }
            }
            _RecordCount.Count++;
            _RecordCount.LastDateTime = DateTime.Now;
            DouHelper.Misc.SerializeBinary(_RecordCount, f);
        }

        //計算
        [HttpPost]
        public async Task<ActionResult> cal(CalInputModel input)
        {
            if (input.fuelInputs == null) input.fuelInputs = new List<FuelInputs>();
            if (input.refrigerantInputs == null) input.refrigerantInputs = new List<RefrigerantInput>();
            if (input.escapeInputs == null) input.escapeInputs = new List<EscapeInput>();
            if (input.specialInputs == null) input.specialInputs = new List<SpecialInput>();

            CFCDataController c = new CFCDataController();
            var result = await c.CalAsync(input);

            var jstr = JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        //匯出Excel
        public ActionResult DownloadExcel(SaveProjectModel input)
        {
            Controllers.FileDownload.ExcelManagerF.ReturnModel result = new Controllers.FileDownload.ExcelManager().DownloadExcel(input);

            var jstr = JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");

        }

        public class RecordCount
        {
            public int Count { get; set; }
            public DateTime LastDateTime { get; set; }
        }
        //------------------------------------------------------------------------------------------------
    }
}