using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ExportController : ApiController
    {
        XuatExcel db_XuatExcel = new XuatExcel();
        PaymentDAO db_Payment = new PaymentDAO();
        ThongKeClass db_ThongKe = new ThongKeClass();
        [HttpPost]
        [Route("api/export/DoanhThuNgay")]
        public async Task<HttpResponseMessage> PostDoanhThuNgay([FromUri] int? MaND,
                                        [FromUri] DateTime NgayBatDau,
                                        [FromUri] DateTime NgayKetThuc)
        {
            var result1 = db_Payment.LayToanBoHoaDonDieuKien(NgayBatDau, NgayKetThuc);
            string filename = "";

            var result2 = db_XuatExcel.ExportDoanhThuNgay((await Mapper.PaymentMapper.MapListSimpleOrder(result1)), NgayBatDau, NgayKetThuc, ref filename, false);
            if(string.IsNullOrEmpty(result2))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp lỗi trong quá trình xử lý, vui lòng thử lại");
            }
            else
            { 
                return Request.CreateResponse(HttpStatusCode.OK, result2);
            }
        }
        [HttpPost]
        [Route("api/export/DoanhThuThang")]
        public HttpResponseMessage PostDoanhThuThang(int year)
        {
            var result = db_ThongKe.ThongKeTheoTungThang(year);

            string filename = "";

            var result2 = db_XuatExcel.ExportDoanhThuThang(result.ToList(), year, ref filename, false);
            if (string.IsNullOrEmpty(result2))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp lỗi trong quá trình xử lý, vui lòng thử lại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result2);

            }
        }
        [HttpPost]
        [Route("api/export/TruyCapNgay")]
        public HttpResponseMessage PostTruyCapNgay(DateTime start, DateTime end)
        {
            var result = db_ThongKe.ThongKeTruyCapTungNgay(start, end);


            string filename = "";

            var result2 = db_XuatExcel.ExportTruyCapNgay(result.ToList(), start, end, ref filename, false);
            if (string.IsNullOrEmpty(result2))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp lỗi trong quá trình xử lý, vui lòng thử lại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result2);

            }
        }
        [HttpPost]
        [Route("api/export/TruyCapThang")]
        public HttpResponseMessage PostTruyCapThang(int year)
        {
            var result = db_ThongKe.ThongKeTruyCapTungThang(year);


            string filename = "";

            var result2 = db_XuatExcel.ExportTruyCapThang(result.ToList(), year, ref filename, false);
            if (string.IsNullOrEmpty(result2))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp lỗi trong quá trình xử lý, vui lòng thử lại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result2);

            }
        }
        [HttpPost]
        [Route("api/export/KhoaHocNgay")]
        public HttpResponseMessage PostKhoaHocNgay(DateTime start, DateTime end)
        {
            var result = db_ThongKe.ThongKeKhoaHocTungNgay(start, end);


            string filename = "";

            var result2 = db_XuatExcel.ExportThongTinKhoaHocNgay(result.ToList(), start, end, ref filename, false);
            if (string.IsNullOrEmpty(result2))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp lỗi trong quá trình xử lý, vui lòng thử lại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result2);

            }
        }
        [HttpPost]
        [Route("api/export/KhoaHocThang")]
        public HttpResponseMessage PostKhoaHocThang(int year)
        {
            var result = db_ThongKe.ThongKeKhoaHocTheoNam(year);


            string filename = "";

            var result2 = db_XuatExcel.ExportThongTinKhoaHocThang(result.ToList(), year, ref filename, false);
            if (string.IsNullOrEmpty(result2))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp lỗi trong quá trình xử lý, vui lòng thử lại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result2);
            }
        }
    }
}
