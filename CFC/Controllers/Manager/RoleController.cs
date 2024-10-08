﻿using CFC.Models;
using CFC.Models.Manager;
using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFC.Controllers.Manager
{
    [MenuDef(Name = "角色管理", MenuPath = "系統管理", Action = "Index", Index = 1, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class RoleController : Dou.Controllers.RoleBaseController<Role>
    {
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        internal static System.Data.Entity.DbContext _dbContext = new DouModelContext();
        protected override Dou.Models.DB.IModelEntity<Role> GetModelEntity()
        {
            return new ModelEntity<Role>(_dbContext);
        }
    }
}