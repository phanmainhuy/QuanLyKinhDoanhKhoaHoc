using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers.Admin
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StatisticController : ApiController
    {
        PaymentDAO db_Payment = new PaymentDAO();
        public HttpResponseMessage Get( [FromUri] bool isThanhToan, 
                                        [FromUri] int? MaND, 
                                        [FromUri] DateTime? NgayBatDau, 
                                        [FromUri] DateTime? NgayKetThuc,
                                        [FromUri] PagingVM model)
        {
            int total;
            var result = db_Payment.LayToanBoHoaDonDieuKienPaging(isThanhToan, MaND, NgayBatDau, NgayKetThuc, model.page, model.pageSize, out total);
            if(result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đã sảy ra lỗi");
            }
            else
            {
                var lstCourseVM = Mapper.PaymentMapper.MapListSimpleOrder(result);
                var response = Request.CreateResponse(HttpStatusCode.OK, lstCourseVM);
                response.Content.Headers.Add("Access-Control-Expose-Headers", "pagingheader");
                response.Content.Headers.Add("pagingheader", JsonConvert.SerializeObject(total));
                return response; 
            }    
        }
    }
}
