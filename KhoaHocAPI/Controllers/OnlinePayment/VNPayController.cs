using KhoaHocAPI.Models.VNPay;
using KhoaHocData.OnlineParty;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace KhoaHocAPI.Controllers.OnlinePayment
{
    public class VNPayController : ApiController
    {
        VnPayDAO db = new VnPayDAO();
        //Gửi yêu cầu thanh toán
        [HttpPost]
        public HttpResponseMessage PostThanhToanRequest([FromBody]VNPayRequestVM model)
        {
            string result = db.GetPayURL(model.OrderId, model.Amount, model.BankCode, model.MaApDung);
            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng kiểm tra lại dữ liệu");
        }
        [HttpPost]
        [Route("api/VNPay/vnpayreturn")]
        public async Task<HttpResponseMessage> GetXacNhanThanhToan([FromBody]VNPayConfirmVM model)
        {
            var resultContent = await db.GetConfirmResult(model.vnp_TxnRef, model.vnp_Amount, model.vnp_TransactionNo, 
                model.vnp_ResponseCode, model.vnp_TransactionStatus, model.vnp_SecureHash,
                model.vnp_BankCode, model.vnp_CardType, model.vnp_OrderInfo, model.vnp_PayDate, model.vnp_TmnCode, model.vnp_BankTranNo
                );
            if (resultContent.RspCode != "00")
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, resultContent.Message);
            return Request.CreateResponse(HttpStatusCode.OK, resultContent);
        }
        [HttpPut]
        public DateTime PutABC(DateTime abc)
        {
            return abc.AddYears(3);
        }
    }
}
