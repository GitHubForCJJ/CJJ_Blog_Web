using CJJ.Blog.Main.Models;
using CJJ.Blog.Main.Tools;
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
    public class RoleController : BaseController
    {
        // GET: Role
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrEdit(int kid,string rolename)
        {
            var result = new Result();
            if (string.IsNullOrEmpty(rolename))
            {
                result.IsSucceed = false;
                result.Message = "名称不能为空";
                return MyJson(result);
            }
            var emp = EmployeeInfo?.Model;
            if (kid <= 0)
            {
                var role = new Sysrole();
                role.CreateTime = DateTime.Now;
                role.CreateUserId = emp?.KID.ToString();
                role.CreateUserName = emp?.UserName;
                role.Rolename = rolename;
                role.UpdateTime = DateTime.Now;
                result = BlogHelper.AddByEntity_Sysrole(role, new Service.Models.View.OpertionUser());
            }
            else
            {
                result = BlogHelper.UpdateByWhere_Sysrole(new Dictionary<string, object>() {
                    {nameof(Sysrole.CreateTime),DateTime.Now },
                    {nameof(Sysrole.CreateUserId), emp?.KID.ToString()},
                    {nameof(Sysrole.CreateUserName), emp?.UserName},
                    {nameof(Sysrole.Rolename), rolename}
                }, new Dictionary<string, object>() { { nameof(Sysrole.KID), kid } }, new Service.Models.View.OpertionUser());
            }

            return MyJson(result);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int roleid)
        {
            var result = new Result();
            if (roleid <= 0)
            {
                result.IsSucceed = false;
                result.Message = "参数错误";
                return MyJson(result);
            }
            result = BlogHelper.DeleteByWhere_Sysrole(new Dictionary<string, object>()
            {
                {nameof(Sysrole.KID),roleid }
            }, new Service.Models.View.OpertionUser());

            return MyJson(result);

        }
        /// <summary>
        /// 授权
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Auth(int roleid)
        {
            ViewBag.Roleid = roleid;
            return View();
        }
        /// <summary>
        /// 设置角色的权限列表 (左边)
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRoleMenu(int roleid)
        {
            var allmenu = BlogHelper.GetAllList_Sysmenu();
            var reslist = new List<MenuView>();
            reslist = allmenu.SerializeObject().DeserializeObject<List<MenuView>>();
            var current = BlogHelper.GetModelByKID_Sysrole(roleid);
            if (current != null && !string.IsNullOrEmpty(current?.MenuList))
            {
                var list = current.MenuList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)?.ToList();
                for (var i = 0; i < reslist.Count(); i++)
                {
                    if (list.Contains(reslist[i].KID.ToString()))
                    {
                        reslist[i].Checked = true;
                    }
                }
            }
            return MyJson(reslist);

        }
        /// <summary>
        /// 获取所有的员工包括属于这个角色的，（角色操作页面右边）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRoleMember(int roleid)
        {
            var emps = BlogHelper.GetList_Sysuserrole(new Dictionary<string, object>() {
                {nameof(Sysuserrole.Roleid),roleid },
                {nameof(Sysuserrole.IsDeleted),0 },
                {nameof(Sysuserrole.States),0 }
            });
            var allemp = BlogHelper.GetAllList_Employee();

            var reslist = new List<Roleview>();
            if (allemp.Count > 0)
            {
                reslist = allemp.SerializeObject().DeserializeObject<List<Roleview>>();
            }

            for (var i = 0; i < reslist.Count; i++)
            {
                var id = reslist[i].KID.ToString();
                var emp = emps.FirstOrDefault(x => x.Userid == id);
                if (emp != null)
                {
                    reslist[i].Checked = true;
                }
                else
                {
                    reslist[i].Checked = false;
                }
            }

            return MyJson(reslist);
        }

        /// <summary>
        /// 设置角色的权限，分配角色给员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OperateRole(string roleid, string menuids, string userids)
        {
            var res = new Result();
            try
            {
                //设置角色权限
                res = BlogHelper.UpdateByWhere_Sysrole(new Dictionary<string, object>() {
                    { nameof(Sysrole.MenuList),menuids}
                }, new Dictionary<string, object>() {
                    { nameof(Sysrole.KID),roleid}
                }, new Service.Models.View.OpertionUser());
                //设置角色员工
                res = BlogHelper.SetRoleEmployees(roleid, userids);
           
            }
            catch(Exception ex)
            {
                res.IsSucceed = false;
                res.Message = ex.Message;
            }
            return MyJson(res);
        }

        /// <summary>
        /// 所有角色列表
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetListData(FormCollection form)
        {
            var where = form.TableWhere();
            where.DicWhere.Add(nameof(Sysrole.IsDeleted), 0);
            where.DicWhere.Add(nameof(Sysrole.States), 0);
            var list = BlogHelper.GetJsonListPage_Sysrole(where.Page,where.Limit, "", where.DicWhere);
            return MyJson(list);
        }
    }
}