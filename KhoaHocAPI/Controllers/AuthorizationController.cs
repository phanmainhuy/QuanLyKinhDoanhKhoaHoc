using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class AuthorizationController : ApiController
    {
        private readonly Account db = new Account();

        public HttpResponseMessage Get()
        {
            var result = db.LayTatCaQuyen();
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Lỗi lạ");
            return Request.CreateResponse(Mapper.RoleMapper.MapListRole(result));
        }
        public HttpResponseMessage Get(int maND, int maQuyen)
        {
            var result = db.hasPermission(maND, maQuyen);
            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Không có quyền");
            }
        }
        public async Task<HttpResponseMessage> Post([FromBody] PermissionGroupVM model)
        {
            if (model == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Dữ liệu không chính xác");
            if (model.TenNhomNguoiDung == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tên nhóm người dùng trống");
            var result = await db.ThemNhomNguoiDung(model.TenNhomNguoiDung, Mapper.RoleMapper.MapListQuyen(model.DanhSachQuyen));
            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Lỗi bất ngờ");
            }
        }
        public async Task<HttpResponseMessage> Put(int UserID, int GroupID)
        {
            var result = await db.ThayDoiNhomNguoiDung(UserID, GroupID);
            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi bất ngờ");
            }
        }
        public async Task<HttpResponseMessage> Post([FromBody] IEnumerable<UserViewModel> Users, [FromUri]int MaNhomNguoiDung)
        {
            var result = Mapper.UserMapper.MapListUser( await db.ThayDoiNhomNhieuNguoiDung(Mapper.UserMapper.MapListUserReverse(Users), MaNhomNguoiDung));
            if (result != null && result.Count() != 0)
            {
                return Request.CreateResponse(HttpStatusCode.Created, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi bất ngờ");
            }
        }
        public HttpResponseMessage Get(int maNhomNguoiDung)
        {
            var result = Mapper.RoleMapper.MapListRole(db.GetRolesByGroupID(maNhomNguoiDung));
            if (result != null || result.Count() == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi bất ngờ");
            }
        }
    }
}
