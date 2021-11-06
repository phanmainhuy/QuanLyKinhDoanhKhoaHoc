using KhoaHocData.DAO;
using KhoaHocData.EF;
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
    public class KhoaHocController : ApiController
    {
        private readonly KhoaHocDAO khDAO = new KhoaHocDAO();
        
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = khDAO.LayRaToanBoKhoaHoc();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return BadRequest();
        } 
        [HttpGet]
        public HttpResponseMessage Get(int maKhoa)
        {
            var item = Mapper.CourseMapper.MapCourse(khDAO.LayKhoaHocTheoMa(maKhoa));
            if (item != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No content");
        }
        [HttpGet]
        public HttpResponseMessage GetByParentID(int maLoai, int limit)
        {
            var items = Mapper.CourseMapper.MapListCourse(khDAO.LayRaKhoaHocTheoMaLoaiKhoaHoc(maLoai, limit));
            if (items != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No content");
        }

    }
}
