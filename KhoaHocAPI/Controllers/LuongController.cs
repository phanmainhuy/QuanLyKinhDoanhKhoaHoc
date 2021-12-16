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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LuongController : ApiController
    {
        SalaryDAO db = new SalaryDAO();
        public HttpResponseMessage GetById(int MaND)
        {
            var result = db.getLichSuLuong(MaND);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.SalaryMapper.MapSalary(result));
            }
        }
        public HttpResponseMessage GetAllSystem()
        {
            var result = new NguoiDungDAO().LayHetHeThong();
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi lấy dữ liệu");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.UserMapper.MapListUserSalary(result));
            }

        }
    }
}
