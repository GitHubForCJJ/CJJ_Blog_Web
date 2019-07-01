using CJJ.Blog.Main.Models;
using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LoginView = CJJ.Blog.Main.Models.LoginView;

namespace CJJ.Blog.Main.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResponse Login(LoginView model)
        {
            if (model == null)
            {
                return new JsonResponse("", 1);
            }
            var emp = BlogHelper.GetModelByWhere_Employee(new Dictionary<string, object>()
            {
                {nameof(Employee.UserAcount),model.UserAccount},
                {nameof(Employee.IsDeleted),0},
                {nameof(Employee.States),0}
            });
            if (emp == null || emp.KID <= 0 || emp.UserPassword != model.Password)
            {
                return new JsonResponse("", 1, "密码错误或者账户不存在");
            }

        }
    }
}