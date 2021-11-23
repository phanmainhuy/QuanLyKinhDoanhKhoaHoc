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
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class PaymentController : ApiController
    {
        private PaymentDAO db_payment = new PaymentDAO();

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
        public HttpResponseMessage Post(HoaDonVM model)
        {
            var result = db_payment.AddHoaDon(model.MaND, 0, "", "Chưa chọn", model.MaGioHang);
            if(result != -1)
            {
                return Request.CreateResponse(HttpStatusCode.Created, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi tạo hóa đơn");
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
    }
}
