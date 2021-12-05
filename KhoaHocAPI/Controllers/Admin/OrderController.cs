using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KhoaHocAPI.Controllers.Admin
{
    public class OrderController : ApiController
    {
        PaymentDAO db_Payment = new PaymentDAO();
        [HttpGet]
        public HttpResponseMessage Get([FromUri]PagingVM model)
        {
            int total;
            var result = db_Payment.LayToanBoHoaDonPaging(model.page, model.pageSize, out total);
            
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi truy vấn");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.PaymentMapper.MapListOrderToAccept(result));
            }
        }
    }
}
