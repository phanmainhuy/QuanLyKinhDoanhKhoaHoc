﻿using KhoaHocAPI.Mapper;
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
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Sản phẩm này đã có trong giỏ");
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }
    }
}