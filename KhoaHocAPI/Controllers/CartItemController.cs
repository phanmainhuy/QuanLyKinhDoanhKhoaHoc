using KhoaHocAPI.Mapper;
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
    public class CartItemController : ApiController
    {
        private readonly CartDAO db = new CartDAO();


        public HttpResponseMessage Get(HttpRequestMessage request, int? pUserID)
        {
            if (pUserID == null || pUserID < 0)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            var item = db.LayGioHangTheoUserID(pUserID.Value);
            CourseCartVM returnedCart = CartMapper.MapCourseCart(item);
            if (returnedCart == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
            else
                return request.CreateResponse(HttpStatusCode.OK, (returnedCart));
        }
        public HttpResponseMessage Post(HttpRequestMessage request,[FromBody] CartItemVM model)
        {
            if (model != null)
            {
                var result = db.AddCartItem(model.UserID, model.CourseID, model.OriginPrice);
                if (result == Common.AllEnum.AddCartItemResult.ThanhCong)
                    return request.CreateResponse(HttpStatusCode.OK);
                else if (result == Common.AllEnum.AddCartItemResult.DaTonTai)
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Khóa học này đã có trong giỏ");
                else if (result == Common.AllEnum.AddCartItemResult.DaMua)
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Đã mua khóa học này rồi");
                else if (result == Common.AllEnum.AddCartItemResult.DangChoDuyet)
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Đơn hàng đang chờ duyệt, vui lòng chờ");
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }
        [HttpDelete]
        public HttpResponseMessage Delete(int maGioHang, int maKhoaHoc)
        {
            var result = db.DeleteCartItem(maGioHang, maKhoaHoc);
            if (result)
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Delete Cart Item Error");
        }
    }
}
