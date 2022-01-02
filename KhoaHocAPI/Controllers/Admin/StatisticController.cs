using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using static Common.AllEnum;

namespace KhoaHocAPI.Controllers.Admin
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StatisticController : ApiController
    {
        ThongKeClass db_ThongKe = new ThongKeClass();
        PaymentDAO db_Payment = new PaymentDAO();
        public  HttpResponseMessage Get([FromUri] bool isThanhToan,
                                        [FromUri] int? MaND,
                                        [FromUri] DateTime? NgayBatDau,
                                        [FromUri] DateTime? NgayKetThuc,
                                        [FromUri] PagingVM model)
        {
            int total;
            var result = db_Payment.LayToanBoHoaDonDieuKienPaging(isThanhToan, MaND, NgayBatDau, NgayKetThuc, model.page, model.pageSize, out total);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đã sảy ra lỗi");
            }
            else
            {
                var lstCourseVM = Mapper.PaymentMapper.MapListOrder(result);
                var response = Request.CreateResponse(HttpStatusCode.OK, lstCourseVM);
                response.Content.Headers.Add("Access-Control-Expose-Headers", "pagingheader");
                response.Content.Headers.Add("pagingheader", JsonConvert.SerializeObject(total));
                return response;
            }
        }
        public async Task<HttpResponseMessage> Get([FromUri] DateTime? start,
                                        [FromUri] DateTime? end)
        {
            var result = db_Payment.LayToanBoHoaDonDieuKien(start, end);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đã sảy ra lỗi");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, await Mapper.PaymentMapper.MapListSimpleOrder(result));
            }
        }
        [HttpGet]
        [Route("api/statistic/DailyRevenue")]
        public HttpResponseMessage DoanhThuTheoNgay(DateTime start, DateTime end)
        {
            var result = db_ThongKe.ThongKeDoanhThuTheoNgay(start, end);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đã sảy ra lỗi");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        public HttpResponseMessage GetStatisticByYearinMonth(int Year)
        {
            var result = db_ThongKe.ThongKeTheoTungThang(Year);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else if (result.Count() == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        [HttpGet]
        [Route("api/Statistic/AccessMonth")]
        public HttpResponseMessage GetAccessStatisticByYearInMonth(int Year)
        {
            var result = db_ThongKe.ThongKeTruyCapTungThang(Year);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else if (result.Count() == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        [Route("api/Statistic/AccessDay")]
        public HttpResponseMessage GetAccessStatisticByDay(DateTime start, DateTime end)
        {
            var result = db_ThongKe.ThongKeTruyCapTungNgay(start, end);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else if (result.Count() == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        [HttpGet]
        [Route("api/statistic/CourseDay")]
        public HttpResponseMessage GetCourseStatisticByDay(DateTime start, DateTime end)
        {
            var result = db_ThongKe.ThongKeKhoaHocTungNgay(start, end);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else if (result.Count() == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        [HttpGet]
        [Route("api/statistic/CourseDaySorting")]
        public HttpResponseMessage GetCourseStatisticByDaySorting(DateTime start, DateTime end, int type)
        {
            var result = db_ThongKe.ThongKeKhoaHocTungNgaySorting(start, end, (SortingType)type);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else if (result.Count() == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        [HttpGet]
        [Route("api/statistic/CourseMonth")]
        public HttpResponseMessage GetCourseStatisticByMonth(int Year)
        {
            var result = db_ThongKe.ThongKeKhoaHocTheoNam(Year);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else if (result.Count() == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        [HttpGet]
        [Route("api/statistic/CourseMonthSorting")]
        public HttpResponseMessage GetCourseStatisticByMonthSorting(int Year, int type)
        {
            var result = db_ThongKe.ThongKeKhoaHocTheoNamSorting(Year, (SortingType)type);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else if (result.Count() == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi xử lý thao tác");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

    }
}
