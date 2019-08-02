using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Main.Tools
{
    public static class CommonExension
    {
        public static int ToInt(this object data)
        {
            int res = int.MinValue;
            try
            {            
                res= Convert.ToInt32(data);
            }
            catch
            {
                res = int.MinValue;
            }
            return res;
        }
    }
}