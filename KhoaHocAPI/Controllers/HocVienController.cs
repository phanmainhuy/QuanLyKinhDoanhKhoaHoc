using KhoaHocAPI.Mapper;
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
    public class HocVienController : ApiController
    {
        private readonly NguoiDungDAO db = new NguoiDungDAO();
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, int userId)
        {
            var item = db.LayNguoiDungTheoId(userId);
            if (item != null)
                return request.CreateResponse(HttpStatusCode.OK, UserMapper.MapUser(item));
            else
                return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
