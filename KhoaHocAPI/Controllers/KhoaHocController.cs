using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class KhoaHocController : ApiController
    {
        private readonly KhoaHocDAO khDAO = new KhoaHocDAO();
        private readonly NguoiDungDAO ndDAO = new NguoiDungDAO();

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
        public async Task<HttpResponseMessage> Get(int maKhoa)
        {
            var item = await Mapper.CourseMapper.MapCourse(khDAO.LayKhoaHocTheoMa(maKhoa));
            if (item != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No content");
        }
        [Route("api/hockhoahoc")]
        [HttpGet]
        public HttpResponseMessage GetKhoaHocLearn(int maKhoa)
        {
            var item = khDAO.LayKhoaHocTheoMa(maKhoa, true);
            if (item != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.LearnMapper.MapLearnVM(item));
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No content");
        }
        [Route("api/hocthukhoahoc")]
        [HttpGet]
        public HttpResponseMessage GetKhoaHocLearnDemo(int maKhoa)
        {
            var item = khDAO.LayKhoaHocTheoMa(maKhoa);
            if (item != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.LearnMapper.MapLearnVM(item));
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No content");
        }

        [HttpGet]
        public HttpResponseMessage GetPermission(int MaND, int MaKhoaHoc)
        {
            var result = ndDAO.DaMuaKhoaHoc(MaND, MaKhoaHoc);
            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Không có quyền");
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetByParentID(int maLoai, int limit, bool isShow = true)
        {
            var items = await Mapper.CourseMapper.MapListCourse(khDAO.LayRaKhoaHocTheoMaLoaiKhoaHoc(maLoai, limit, isShow));
            if (items != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No content");
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetByParentIDPaging([FromUri] int maLoai, [FromUri] PagingVM paging, [FromUri] bool isShow = true)
        {
            int total;
            var items = await Mapper.CourseMapper.MapListCourse(khDAO.LayRaKhoaHocTheoMaLoaiKhoaHocPaging(maLoai, paging.page, paging.pageSize, out total, isShow));
            if (items == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No content");
            var response = Request.CreateResponse(HttpStatusCode.OK, items);
            response.Content.Headers.Add("Access-Control-Expose-Headers", "pagingheader");
            response.Content.Headers.Add("pagingheader", JsonConvert.SerializeObject(total));
            return response;
        }

        public HttpResponseMessage Post(CourseVM model)
        {
            var resust = khDAO.ThemKhoaHoc(model.MaLoai.Value, model.TenKhoaHoc, model.DonGia.Value, model.HinhAnh, model.MaGV.Value, model.GioiThieu);
            if (resust == Common.AllEnum.KetQuaTraVeKhoaHoc.ThanhCong)
                return Request.CreateResponse(HttpStatusCode.Created);
            else if (resust == Common.AllEnum.KetQuaTraVeKhoaHoc.DaTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Khóa học đã tồn tại");
            else if (resust == Common.AllEnum.KetQuaTraVeKhoaHoc.TheLoaiKhongTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thể loại không tồn tại");
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm khóa học không thành công");
        }
        [HttpPut]
        public HttpResponseMessage PutThongTinKhoaHoc(CourseVM model)
        {
            var resust = khDAO.ThayDoiThongTinKhoaHoc(model.MaKhoaHoc, model.MaLoai, model.TenKhoaHoc, model.DonGia, model.HinhAnh, model.MaGV, model.GioiThieu);
            if (resust == Common.AllEnum.KetQuaTraVe.ThanhCong)
                return Request.CreateResponse(HttpStatusCode.OK);
            else if (resust == Common.AllEnum.KetQuaTraVe.ChaKhongTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng kiểm tra lại dữ liệu");
            else if (resust == Common.AllEnum.KetQuaTraVe.DaTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tên khóa học bị trùng, hãy nhập tên khác");
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thay đổi thông tin khóa học không thành công");
        }
        [HttpPatch]
        public HttpResponseMessage PatchTrangThaiKhoaHoc([FromBody]IEnumerable<CourseVM> lstModel, bool isHienThi)
        {
            var resust = khDAO.ThayDoiTrangThaiKhoaHoc(Mapper.CourseMapper.MapListCourseReverse(lstModel), isHienThi);
            if (resust == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        public HttpResponseMessage DeleteKhoaHoc(int MaKhoaHoc)
        {
            var result = khDAO.XoaKhoaHoc(MaKhoaHoc);
            if (result == Common.AllEnum.KetQuaTraVe.TrangThaiKichHoat)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Khóa học đang được mở bán, hãy ẩn đi trước khi thực hiện thao tác");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Xóa khóa học không thành công");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Khóa học không tồn tại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

        }
    }
}