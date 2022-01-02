﻿using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers.Admin
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrderController : ApiController
    {
        PaymentDAO db_Payment = new PaymentDAO();
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri]PagingVM model)
        {
            int total;
            var result = db_Payment.LayToanBoHoaDonPaging(model.page, model.pageSize, out total);
            
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi truy vấn");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK,await  Mapper.PaymentMapper.MapListOrderToAccept(result));
            }
        }
        [HttpGet]
        [Route("api/Order/ChuaThanhToan")]
        public async Task<HttpResponseMessage> GetChuaThanhToan([FromUri] PagingVM model)
        {
            int total;
            var result = db_Payment.LayToanBoHoaDonChoDuyetPaging(model.page, model.pageSize, out total);

            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi truy vấn");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, await Mapper.PaymentMapper.MapListOrderToAccept(result));
            }
        }
        [HttpGet]
        [Route("api/Order/ChuaThanhToan")]
        public HttpResponseMessage GetHoaDon(int MaHD)
        {
            var result = db_Payment.LayHoaDonTheoMa(MaHD);
            var result2 = db_Payment.LayDonThuTienTheoMa(MaHD);

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.PaymentMapper.MapOrderToAccept(result, result2));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có hóa đơn này");
            }
        }
        [HttpGet]
        [Route("api/Order/DaThanhToan")]
        public async Task<HttpResponseMessage> GetDaThanhToan([FromUri] PagingVM model)
        {
            int total;
            var result = db_Payment.LayToanBoHoaDonDaDuyetPaging(model.page, model.pageSize, out total);

            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi truy vấn");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, await Mapper.PaymentMapper.MapListOrderToAccept(result));
            }
        }
        public HttpResponseMessage Get(int MaHD)
        {
            var result = db_Payment.LayHoaDonTheoMa(MaHD);
            if (result == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.PaymentMapper.MapOrder(result));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi truy vấn");
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> ConfirmOrder(int OrderID)
        {
            var result = await db_Payment.XacNhanThanhToanHoaDon(OrderID);
            if (result == Common.AllEnum.KetQuaTraVe.ThanhCong)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else if(result == Common.AllEnum.KetQuaTraVe.ChaKhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Hóa đơn không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Khách hàng chưa yêu cầu thanh toán, hóa đơn rác");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Xác nhận đơn thất bại, vui lòng thử lại sau");
            }
        }
    }
}
