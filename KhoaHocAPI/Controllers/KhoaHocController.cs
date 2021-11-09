using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using Newtonsoft.Json;
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
        [HttpGet]
        public HttpResponseMessage GetByParentIDPaging(int maLoai, PagingVM paging)
        {
            int total;
            var items = Mapper.CourseMapper.MapListCourse(khDAO.LayRaKhoaHocTheoMaLoaiKhoaHocPaging(maLoai, paging.page, paging.pageSize, out total));
            if (items == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No content");
            var response = Request.CreateResponse(HttpStatusCode.OK, items);
            response.Content.Headers.Add("Access-Control-Expose-Headers", "pagingheader");
            response.Content.Headers.Add("pagingheader", JsonConvert.SerializeObject(total));
            return response;
        }

    }
}
