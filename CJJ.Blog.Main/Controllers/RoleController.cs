using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Main.Controllers
{
    public class RoleController : BaseController
    {
        // GET: Role
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 授权
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Auth(int Roleid)
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetListData(FormCollection form)
        {
            var where = CommonHelper.FormToDic(form);
            where.Add(nameof(Sysrole.IsDeleted), 0);
            where.Add(nameof(Sysrole.States), 0);
            var list = BlogHelper.GetJsonListPage_Sysrole(1, 10, "", where);
            return MyJson(list);
        }
    }
}