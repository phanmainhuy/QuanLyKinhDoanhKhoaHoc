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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
            if(maND == -1)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Không có quyền");
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
        [HttpPut]
        public async Task<HttpResponseMessage> PutMultiple([FromBody] PermissionGroupVM model)
        {
            var result = await db.ThayDoiQuyenCuaNhom(model.MaNhomNguoiDung, Mapper.RoleMapper.MapListQuyen(model.DanhSachQuyen));
            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi bất ngờ");
            }
        }
        [HttpPut]
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
            var result =  await db.ThayDoiNhomNhieuNguoiDung(Mapper.UserMapper.MapListUserReverse(Users), MaNhomNguoiDung);
            if (result != null && result.Count() != 0)
            {
                return Request.CreateResponse(HttpStatusCode.Created, Mapper.UserMapper.MapListUser(result));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi bất ngờ");
            }
        }
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteNhomNguoiDung(int MaNhomNguoiDung)
        {
            var result =  await db.XoaNhomQuyen(MaNhomNguoiDung);
            if(result == Common.AllEnum.KetQuaTraVe.KhongDuocPhep)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không được xóa các nhóm mặc định");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Nhóm người dùng vốn đã không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Các nhóm người dùng này đã được sử dụng, vui lòng kiểm tra lại trước");
            }
            else if(result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Xóa không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpGet]
        public HttpResponseMessage Get([FromUri]int UserID)
        {
            if (UserID == -1)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Không có quyền");
            var result = Mapper.RoleMapper.MapListRole(db.GetRolesByUserID(UserID));
            if (result != null || result.Count() == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi bất ngờ");
            }
        }
        [Route("api/authorization/bygroupid")]
        [HttpGet]
        public HttpResponseMessage Get2([FromUri] int GroupID)
        {
            if (GroupID == -1)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Không có quyền");
            var result = Mapper.RoleMapper.MapListRole(db.GetRolesByGroupID(GroupID));
            if (result != null || result.Count() == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi bất ngờ");
            }
        }
        [Route("api/Authorization/IsFirstLogin")]
        public HttpResponseMessage Get3(int MaND)
        {
            if (MaND == -1)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden,"Vui lòng thử lại sau");
            }
            var result = db.isFirstLogin(MaND);
            if (!result)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
                return Request.CreateResponse(HttpStatusCode.NoContent);
        }

    }
}
