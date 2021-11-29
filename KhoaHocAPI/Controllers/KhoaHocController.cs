using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        public HttpResponseMessage GetByParentIDPaging([FromUri] int maLoai, [FromUri] PagingVM paging)
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

        public HttpResponseMessage Post(CourseVM model)
        {
            var resust = khDAO.ThemKhoaHoc(model.MaLoai, model.TenKhoaHoc, model.DonGia, model.HinhAnh, model.MaGV, model.GioiThieu);
            if (resust == Common.AllEnum.KetQuaTraVeKhoaHoc.ThanhCong)
                return Request.CreateResponse(HttpStatusCode.Created);
            else if (resust == Common.AllEnum.KetQuaTraVeKhoaHoc.KhoaHocDaTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Khóa học đã tồn tại");
            else if (resust == Common.AllEnum.KetQuaTraVeKhoaHoc.TheLoaiKhongTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thể loại không tồn tại");
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm khóa học không thành công");
        }
    }
}