using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomKhoaHocController : ApiController
    {
        private readonly CategoryDAO db = new CategoryDAO();
        private readonly KhoaHocDAO khDB = new KhoaHocDAO();

        [Route("TopCategory")]
        public HttpResponseMessage GetAllTopCategory(HttpRequestMessage request)
        {
            var item = Mapper.CategoryMapper.MapListTopCategory(db.LayHetDanhMucKhoaHoc());
            if (item != null)
            {
                return request.CreateResponse(HttpStatusCode.OK, item);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No content");
        }

        [Route("TopCategoryByID")]
        public HttpResponseMessage GetAllTopCategory(int ID)
        {
            var item = Mapper.CategoryMapper.MapTopCategory(db.LayDanhMucKhoaHocTheoMa(ID));
            if (item != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có kết quả");
        }

        [Route("LatestCourse")]
        public HttpResponseMessage GetLatestCourse(HttpRequestMessage request, int limit)
        {
            var item = khDB.LayKhoaHocMoiNhat(limit);
            if (item != null)
            {
                var lstCourseCartVM = Mapper.CourseMapper.MapListCourse(item);
                return request.CreateResponse(HttpStatusCode.OK, lstCourseCartVM);
            }
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }

        [Route("MostBuyCourse")]
        public HttpResponseMessage GetMostBuyCourse(int limit)
        {
            var item = khDB.LayKhoaHocMuaNhieu(limit);
            if (item != null)
            {
                var lstCourseVM = Mapper.CourseMapper.MapListCourse(item);
                return Request.CreateResponse(HttpStatusCode.OK, lstCourseVM);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }

        [Route("Search")]
        public HttpResponseMessage GetKhoaHocTheoTenPaging([FromUri] string searchString, [FromUri] PagingVM paging)
        {
            int total;
            var item = khDB.TimKiemKhoaHocPaging(searchString, out total, paging.page, paging.pageSize);
            if (item != null)
            {
                var lstCourseVM = Mapper.CourseMapper.MapListCourse(item);
                var response = Request.CreateResponse(HttpStatusCode.OK, lstCourseVM);
                response.Content.Headers.Add("Access-Control-Expose-Headers", "pagingheader");
                response.Content.Headers.Add("pagingheader", JsonConvert.SerializeObject(total));
                return response;
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }

        [Route("Search")]
        public HttpResponseMessage GetKhoaHocTheoTenPaging([FromUri] string searchString, [FromUri] PagingVM paging, [FromUri] int type)
        {
            int total;
            var item = khDB.TimKiemKhoaHocPagingSorting(searchString, out total, paging.page, paging.pageSize, type);
            if (item != null)
            {
                var lstCourseVM = Mapper.CourseMapper.MapListCourse(item);
                var response = Request.CreateResponse(HttpStatusCode.OK, lstCourseVM);
                response.Content.Headers.Add("Access-Control-Expose-Headers", "pagingheader");
                response.Content.Headers.Add("pagingheader", JsonConvert.SerializeObject(total));
                return response;
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }

        [Route("SearchTheoLoai")]
        [HttpGet]
        public HttpResponseMessage SearchKhoaHocTheoMaTheLoaiPaging([FromUri] int maTheLoai, [FromUri] string searchString, [FromUri] PagingVM paging)
        {
            int total;
            var item = khDB.TimKiemKhoaHocTheoTheLoaiPaging(maTheLoai, searchString, out total, paging.page, paging.pageSize);
            if (item != null)
            {
                var lstCourseVM = Mapper.CourseMapper.MapListCourse(item);
                var response = Request.CreateResponse(HttpStatusCode.OK, lstCourseVM);

                response.Content.Headers.Add("Access-Control-Expose-Headers", "pagingheader");
                response.Content.Headers.Add("pagingheader", JsonConvert.SerializeObject(total));
                return response;
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }

        [Route("SearchTheoLoai")]
        [HttpGet]
        public HttpResponseMessage SearchKhoaHocTheoMaTheLoaiPaging([FromUri] int maTheLoai, [FromUri] string searchString, [FromUri] PagingVM paging, [FromUri] int type)
        {
            int total;
            var item = khDB.TimKiemKhoaHocTheoTheLoaiPagingSorting(maTheLoai, searchString, out total, paging.page, paging.pageSize, type);
            if (item != null)
            {
                var lstCourseVM = Mapper.CourseMapper.MapListCourse(item);
                var response = Request.CreateResponse(HttpStatusCode.OK, lstCourseVM);

                response.Content.Headers.Add("Access-Control-Expose-Headers", "pagingheader");
                response.Content.Headers.Add("pagingheader", JsonConvert.SerializeObject(total));
                return response;
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }

        //public HttpResponseMessage SearchTenKhoaHoc([FromUri] string searchString)
        //{
        //    var item =
        //}
    }
}