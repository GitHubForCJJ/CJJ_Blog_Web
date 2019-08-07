using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Models.Data;
using FastDev.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Main.Controllers
{
    public class EmployeeController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 所有员工列表
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetListData(FormCollection form)
        {
            var where = form.TableWhere();
            where.DicWhere.Add(nameof(Employee.IsDeleted), 0);
            var list = BlogHelper.GetJsonListPage_Employee(where.Page, where.Limit, "", where.DicWhere);
            return MyJson(list);
        }
        /// <summary>
        /// 启用禁用员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StartOrClose(int kid, int states)
        {
            var res = new Result();
            var emp = EmployeeInfo.Model;
            res = BlogHelper.UpdateByWhere_Employee(new Dictionary<string, object>()
            {
                {nameof(Employee.UpdateTime),DateTime.Now },
                {nameof(Employee.CreateUserId),emp.KID },
                { nameof(Employee.CreateUserName),emp.UserName },
                {nameof(Employee.States),states }
            }, new Dictionary<string, object>() { {
                    nameof(Employee.KID),kid
                } }, new Service.Models.View.OpertionUser());
            return MyJson(res);
        }
        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOrEdit(string kid)
        {
            if (string.IsNullOrEmpty(kid))
            {
                return MyJson(new Employee());
            }
            else
            {
                var emp = BlogHelper.GetModelByKID_Employee(kid.Toint());
                return MyJson(emp);
            }
        }

        /// <summary>
        /// 用户授权页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EmpRole()
        {
            var emp = EmployeeInfo?.Model;
            var emproleid = BlogHelper.GetModelByWhere_Sysuserrole(new Dictionary<string, object>()
            {
                { nameof(Sysuserrole.Userid),emp.KID},
                 { nameof(Sysuserrole.IsDeleted),0},
                  { nameof(Sysuserrole.States),0}
            });
            ViewBag.UserRoleid = emproleid.Roleid;
            var allrole = BlogHelper.GetAllList_Sysrole();
            return View(allrole);
        }
        /// <summary>
        /// 给用户赋角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetRole(string empid, string roleids)
        {
            var res = new Result() { IsSucceed = false };
            try
            {
                if (string.IsNullOrEmpty(empid) || string.IsNullOrEmpty(roleids))
                {
                    res.Message = "参数不合法";
                }
                res = BlogHelper.SetEmployeeRole(empid, roleids);

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return MyJson(res);
        }
    }
}