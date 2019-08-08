using CJJ.Blog.Main.Models;
using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Models.Data;
using FastDev.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using CJJ.Blog.Main.Tools;

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
        public ActionResult AddOrEdit(string kid = "")
        {
            if (string.IsNullOrEmpty(kid))
            {
                return View(new Employee());
            }
            else
            {
                var emp = BlogHelper.GetModelByKID_Employee(kid.Toint());
                return View(emp);
            }
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEditPost(Employee model)
        {
            var res = new Result();
            try
            {
                var emp = EmployeeInfo?.Model;
                var dic = new Dictionary<string, object>();
                var opt = new Service.Models.View.OpertionUser();
                AddBaseInfo<Employee>(model, emp, out dic, out opt);
                if (model.KID > 0)
                {
                    dic.Add(nameof(Employee.UserName), model.UserName);
                    dic.Add(nameof(Employee.UserNikeName), model.UserNikeName);
                    dic.Add(nameof(Employee.UserPassword), model.UserPassword);
                    res = BlogHelper.UpdateByWhere_Employee(dic, new Dictionary<string, object>() {
                        {nameof(Employee.KID),model.KID }
                    }, opt);
                }
                else
                {
                    dic.Add(nameof(Employee.UserName), model.UserName);
                    dic.Add(nameof(Employee.UserNikeName), model.UserNikeName);
                    dic.Add(nameof(Employee.UserAcount), model.UserAcount);
                    dic.Add(nameof(Employee.IsAdmin), 1);
                    dic.Add(nameof(Employee.UserPassword), model.UserPassword);
                    res = BlogHelper.Adds_Employee(new List<Dictionary<string, object>>() { dic }, opt);
                }
            }
            catch (Exception ex)
            {
                res.IsSucceed = false;
                res.Message = ex.Message;
            }
            return MyJson(res);

        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Reset(int kid, string password)
        {
            var res = new Result();
            var emp = EmployeeInfo.Model;
            if (kid <= 0 || string.IsNullOrEmpty(password))
            {
                res.IsSucceed = false;
                res.Message = "参数错误";
                return MyJson(res);
            }

            res = BlogHelper.UpdateByWhere_Employee(new Dictionary<string, object>() {
                { nameof(Employee.UpdateTime),DateTime.Now},
                {nameof(Employee.UpdateUserId),emp.KID },
                {nameof(Employee.UpdateUserName),emp.UserName },
                {nameof(Employee.LastUpTime),DateTime.Now },
                {nameof(Employee.UserPassword),password.ToUpper() }
            }, new Dictionary<string, object>() {
                {nameof(Employee.KID),kid }
            }, new Service.Models.View.OpertionUser());
            return MyJson(res);
        }

        /// <summary>
        /// 用户授权页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EmpRole(int kid)
        {
            if (kid <= 0)
            {
                return View();
            }
            var emproleids = BlogHelper.GetList_Sysuserrole(new Dictionary<string, object>()
            {
                { nameof(Sysuserrole.Userid),kid},
                 { nameof(Sysuserrole.IsDeleted),0},
                  { nameof(Sysuserrole.States),0}
            });
            var allrole = BlogHelper.GetAllList_Sysrole();
            var reslist = new List<EmployeeView>();
            reslist = allrole.SerializeObject().DeserializeObject<List<EmployeeView>>();
            for (var i = 0; i < reslist.Count(); i++)
            {
                var role = emproleids?.FirstOrDefault(x => x.Roleid == reslist[i].KID.ToString());
                if (role != null)
                {
                    reslist[i].Checked = true;
                }
                //var role = reslist.FirstOrDefault(x => x.KID == emproleids[i].Roleid.Toint());
                //if (role != null)
                //{
                //    reslist.FirstOrDefault(x => x.KID == role.KID).Checked = true;
                //}
            }
            //被操作员工的KID
            ViewBag.KID = kid;
            return View(reslist);
        }
        /// <summary>
        /// 给用户赋角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetRole(int kid,string roleids = "")
        {
            var res = new Result() { IsSucceed = false };
            if (kid <= 0)
            {
                return MyJson(res);
            }
            try
            {
                res = BlogHelper.SetEmployeeRole(kid.ToString(), roleids);
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return MyJson(res);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delect(int kid)
        {
            var res = new Result() { IsSucceed = false };
            if (kid <= 0)
            {
                res.Message = "参数错误";
                return MyJson(res);
            }
            res = BlogHelper.DeleteByWhere_Employee(new Dictionary<string, object>()
            {
                {nameof(Employee.KID),kid }
            }, new Service.Models.View.OpertionUser());
            return MyJson(res);
        }
    }
}