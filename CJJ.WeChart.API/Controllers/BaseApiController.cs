using FastDev.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;


namespace CJJ.WeChart.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseApiController : ApiController
    {
        public JsonResponse FastResponse(object data, int code = 0, string msg = "", int rescut = 0)
        {
            return new JsonResponse
            {
                Data = data,
                Count = rescut,
                Code = code,
                Msg = msg
            };
        }
    }
}
