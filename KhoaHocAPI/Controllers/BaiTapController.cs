using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KhoaHocAPI.Controllers
{
    public class BaiTapController : ApiController
    {
        private BaiTapDAO db = new BaiTapDAO();

        public HttpResponseMessage Post(Models.BaiTapVM model)
        {
            var result = db.ThemBaiTap(model.MaBaiHoc, model.LinkPDF);
            if (result == Common.AllEnum.KetQuaTraVe.ChaKhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bài học không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm bài học không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
        }
    }
}
