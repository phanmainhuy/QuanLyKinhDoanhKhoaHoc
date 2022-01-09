using KhoaHocAPI.Models;
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
                result = db_payment.AddHoaDon(model.MaND, model.MaApDung, "", model.HinhThucThanhToan, model.MaGioHang);
            else
                result = db_payment.TaoHoaDon1KhoaHoc(model.MaND, model.MaApDung, "", model.HinhThucThanhToan, model.MaKhoaHoc.Value);
            if (result == -1)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi tạo hóa đơn");
            }
            else if (result == -2)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Vui lòng đăng nhập");
            }
            else if (result == -3)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Đã mua khóa học này rồi");
            }
            else if (result == -4)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Hãy thêm khóa học vào giỏ hàng trước khi thanh toán");
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
        public HttpResponseMessage Post([FromBody] DonThuHoVM model)
        {
            string km = null;
            if (!string.IsNullOrEmpty(model.MaApDung))
                km = model.MaApDung;
            var result = db_payment.TaoDonThuTien(model.MaKH, model.MaHD, model.DiaChiThu, model.Email, model.SDTThu, "", model.SoTienThu, 0, null, "", km);
            if (result == Common.AllEnum.KetQuaTraVe.ThanhCong)
                return Request.CreateResponse(HttpStatusCode.OK);
            else if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đã tạo rồi, vui lòng không tạo lại");
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tạo đơn thu tiền không thành công");
        }
        [Route("api/payment/InstantRamen")]
        public async Task<HttpResponseMessage> Post3(InstantPayment model)
        {
            var result = await db_payment.ThanhToanNgay(model.MaHD, model.MaApDung);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng kiểm tra lại hóa đơn, khuyến mãi và thử lại");
            else if (result == Common.AllEnum.KetQuaTraVe.KhongHopLe)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Hóa đơn rỗng, hãy kiểm tra lại");
            else if (result == Common.AllEnum.KetQuaTraVe.KhongDuocPhep)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thanh toán ngay không phù hợp với đơn hàng này");
            else if (result == Common.AllEnum.KetQuaTraVe.KhongChinhXac)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Khuyến mãi đã được sử dụng hoặc hết hạn sử dụng");
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thực hiện thanh toán không thành công");
            else
                return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
    public class InstantPayment
    {
        public int MaHD { get; set; }
        public string MaApDung { get; set; }
    }
}

