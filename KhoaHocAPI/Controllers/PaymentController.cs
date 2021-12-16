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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PaymentController : ApiController
    {
        private PaymentDAO db_payment = new PaymentDAO();

        [HttpGet]
        public HttpResponseMessage Get(int MaHoaDon)
        {
            var result = db_payment.LayHoaDonTheoMa(MaHoaDon);
            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.PaymentMapper.MapOrder(result));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có hóa đơn này");
            }
        }
        [HttpPost]
        public HttpResponseMessage Post(HoaDonVM model)
        {
            int result = -1;
            if (model.MaKhoaHoc == null)
                result = db_payment.AddHoaDon(model.MaND, 0, "", "Chưa chọn", model.MaGioHang);
            else
                result = db_payment.TaoHoaDon1KhoaHoc(model.MaND, 0, "", "Chưa chọn", model.MaKhoaHoc.Value);
            if (result == -1)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi tạo hóa đơn");
            }
            else if(result == -2)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Vui lòng đăng nhập");
            }
            else if (result == -3)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Đã mua khóa học này rồi");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        [Route("api/Payment/CancelOther")]
        public HttpResponseMessage Post(int MaHoaDon)
        {
            var result = db_payment.CancelOrder(MaHoaDon);
            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi hủy hóa đơn");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">
        /// Truyền vào mã người dùng(Mã khách hàng), mã hóa đơn, Số điện thoại gọi khi thu tiền, Số tiền đơn hàng 
        /// </param>
        /// <returns>Trả về mã 200 khi thành công và 400 khi thất bại với Message lỗi</returns>
        [HttpPost]
        [Route("api/Payment/ReceiptOrder")]
        public HttpResponseMessage Post([FromBody]DonThuHoVM model)
        {
            string km = null;
            if (!string.IsNullOrEmpty(model.MaApDung))
                km = model.MaApDung;
            var result = db_payment.TaoDonThuTien(model.MaKH, model.MaHD, model.DiaChiThu, model.SDTThu, "", model.SoTienThu, 0, null, "", km);
            if (result == Common.AllEnum.KetQuaTraVe.ThanhCong)
                return Request.CreateResponse(HttpStatusCode.OK);
            else if(result == Common.AllEnum.KetQuaTraVe.DaTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đã tạo rồi, vui lòng không tạo lại");
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tạo đơn thu tiền không thành công");
        }
    }
}
