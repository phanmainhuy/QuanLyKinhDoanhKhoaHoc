using KhoaHocAPI.Mapper;
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
    public class CartItemController : ApiController
    {
        private readonly CartDAO db = new CartDAO();

        public object CartVM { get; private set; }

        public HttpResponseMessage Get(HttpRequestMessage request, int? pUserID)
        {
            if (pUserID == null || pUserID < 0)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            var item = db.LayGioHangTheoUserID(pUserID.Value);
            CourseCartVM returnedCart = CartMapper.MapCourseCart(item);
            if (returnedCart == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            else
                return request.CreateResponse(HttpStatusCode.OK, (returnedCart));
        }
        public HttpResponseMessage Post(HttpRequestMessage request, CartItemVM model)
        {
            if (model != null)
            {
                var result = db.AddCartItem(model.UserID, model.CourseID, model.OriginPrice);
                if (result == Common.AllEnum.AddCartItemResult.ThanhCong)
                    return request.CreateResponse(HttpStatusCode.OK);
                else if (result == Common.AllEnum.AddCartItemResult.DaTonTai)
                    return new HttpResponseMessage(HttpStatusCode.Conflict);
                else
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
