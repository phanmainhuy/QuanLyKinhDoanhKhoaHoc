using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class IdentityController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, UserLogin model)
        {
                var kq = new Account().Login(model.UserName, model.Password);
                if (kq != null)
                {
                    return request.CreateResponse(HttpStatusCode.OK, Mapper.UserMapper.MapUserLogon(kq));
                }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }

        [HttpGet]
        public HttpResponseMessage GetPhanQuyen(HttpRequestMessage request, string userName)
        {
            if(userName != null)
            {
                var result = new Account().GetRoles(userName);
                if (result != null)
                    return request.CreateResponse(HttpStatusCode.OK, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi lúc get phân quyền");
        }
    }
}
