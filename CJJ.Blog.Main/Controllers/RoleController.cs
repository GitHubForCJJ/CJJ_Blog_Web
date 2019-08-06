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
        /// 获取属于这个角色的员工
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
                var id = reslist[i].KID;
                var emp = emps.FirstOrDefault(x => x.KID == id);
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
            var where = CommonHelper.FormToDic(form);
            where.Add(nameof(Sysrole.IsDeleted), 0);
            where.Add(nameof(Sysrole.States), 0);
            var list = BlogHelper.GetJsonListPage_Sysrole(1, 10, "", where);
            return MyJson(list);
        }
    }
}