using KhoaHocData.DAO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class CustomKhoaHocController : ApiController
    {
        private readonly CategoryDAO db = new CategoryDAO();
        private readonly KhoaHocDAO khDB = new KhoaHocDAO();

        [Route("TopCategory")]
        public HttpResponseMessage GetAllTopCategory(HttpRequestMessage request)
        {
            var item = db.LayHetDanhMucKhoaHoc();
            if (item != null)
            {
                return request.CreateResponse(HttpStatusCode.OK, item);
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
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
            if (item == null)
            {
                var lstCourseCartVM = Mapper.CourseMapper.MapListCourse(item);
                return Request.CreateResponse(HttpStatusCode.OK, lstCourseCartVM);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }
    }
}