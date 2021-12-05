using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KhoaHocAPI.Controllers
{
    public class KhuyenMaiController : ApiController
    {
        KhuyenMaiDAO db_km = new KhuyenMaiDAO();
        public HttpResponseMessage GetAll()
        {
            var result = db_km.LayToanBoKhuyenMai();
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Có lỗi trong quá trình xử lý");
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.SaleMapper.MapListSale(result));
        }
        public HttpResponseMessage GetById(int MaKM)
        {
            var result = db_km.LayKhuyenMaiTheoMa(MaKM);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Có lỗi trong quá trình xử lý");
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.SaleMapper.MapSale(result));
        }
        [Route("api/KhuyenMai/TaoKhuyenMai")]
        [HttpPost]
        public HttpResponseMessage TaoKhuyenMai(KhuyenMaiVM model)
        {
            var result = db_km.ThemKhuyenMai(model.MaNguoiTao, model.TenKM, model.HinhAnh, model.GiaTri);
            if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tên khóa học không được trùng");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tạo khóa học thất bại");
            else
                return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        [HttpPost]
        public HttpResponseMessage MuaKhuyenMai([FromBody]KhuyenMai_NguoiDungVM model)
        {
            var result = db_km.MuaKhuyenMai(model.MaHV, model.MaKM);
            if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đã mua mã khuyến mãi này rồi");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không đủ điểm để mua");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Mua khuyến mãi thất bại");
            else
                return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public HttpResponseMessage XoaKhuyenMaiDaMua(int MaHV, int MaKM)
        {
            var result = db_km.XoaKhuyenMaiDaMua(MaHV, MaKM);
            if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Xóa khuyến mãi thất bại");
            else
                return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}

