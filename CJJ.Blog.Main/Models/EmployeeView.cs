using CJJ.Blog.Service.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CJJ.Blog.Main.Models
{
    public class EmployeeView : Sysrole
    {
        [DataMember]
        public bool Checked { get; set; }
    }
}