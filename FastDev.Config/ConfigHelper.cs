using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDev.Config
{
    /// <summary>
    /// 通用配置库，通过本类可以方便获取程序配置项的值
    /// 文件功能描述：公共类，系统配置，通过本类可以快速地访问Web.config及App.Config中的配置项
    /// 依赖说明：无依赖
    /// 异常处理：本类捕获并处理异常，当配置项不存在时，获取该类型的默认值
    /// 
    /// </summary>
    public class ConfigHelper
    {
        public static string GetConfigToString(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "";
            }
            var obj = GetConfigObject(name);
            try
            {
                if (obj == null)
                {
                    return "";
                }
                return obj.ToString();
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 若不存在或者错误返回int最小值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetConfigToInt(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return int.MinValue;
            }
            var obj = GetConfigObject(name);
            try
            {
                if (obj == null)
                {
                    return int.MinValue;
                }
                return Convert.ToInt32(obj);
            }
            catch
            {
                return int.MinValue;
            }
        }
        public static DateTime GetConfigToDateTime(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return DateTime.MinValue;
            }
            var obj = GetConfigObject(name);
            try
            {
                if (obj == null)
                {
                    return DateTime.MinValue;
                }
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static bool GetConfigToBool(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            var obj = GetConfigObject(name);
            try
            {
                if (obj == null)
                {
                    return false;
                }
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return false;
            }
        }
        public static object GetConfigObject(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// 应用程序根目录，包含最后分隔符
        /// </summary>
        public static string BaseDirectory
        {
            get
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;
                if (!path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                {
                    path = $"{path}{System.IO.Path.DirectorySeparatorChar.ToString()}";
                }
                return path;
            }
        }
        /// <summary>
        /// 零时文件夹，temp
        /// </summary>
        public  static string TempFilePath
        {
            get
            {
                string path = BaseDirectory + "temp" + System.IO.Path.DirectorySeparatorChar;
                if (!System.IO.Directory.Exists(path))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
                return path;
            }
        }


    }
}
