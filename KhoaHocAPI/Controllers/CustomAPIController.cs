using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class CustomAPIController : ApiController
    {
        private UserGroupDAO db_UserGroup;
        public CustomAPIController()
        {
            db_UserGroup = new UserGroupDAO();
        }

        [Route("NhomNguoiDung")]
        [HttpGet]
        public HttpResponseMessage GetAllNhomNguoiDung()
        {
            var result = Mapper.OtherMapper.MapListUserGroup( db_UserGroup.LayTatCaNhomNguoiDung());
            if(result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Không tìm thấy nội dung");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
    }
}
