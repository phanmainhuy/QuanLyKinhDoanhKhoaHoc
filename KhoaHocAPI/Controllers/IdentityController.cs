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
    public class IdentityController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, UserLogin model)
        {
            var kq = new Account().Login(model.UserName, model.Password);
            if (kq != null)
            {
                if (kq.TrangThai == false)
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Tài khoản đã bị khóa");
                return request.CreateResponse(HttpStatusCode.OK, Mapper.UserMapper.MapUserLogon(kq));
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tài khoản hoặc mật khẩu không chính xác");
        }

        [HttpGet]
        public HttpResponseMessage GetPhanQuyen(HttpRequestMessage request, string userName)
        {
            if (userName != null)
            {
                var result = new Account().GetRoles(userName);
                if (result != null)
                    return request.CreateResponse(HttpStatusCode.OK, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi lúc get phân quyền");
        }

        [HttpGet]
        public async Task<HttpResponseMessage> SignUpStudentMobile(string userName, string password)
        {
            var kq = await new Account().Register(userName, password);
            if (kq != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.UserMapper.MapUserLogon(kq));
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tài khoản hoặc mật khẩu không chính xác");
        }
        [HttpPost]
        [Route("api/Identity/student")]
        public async Task<HttpResponseMessage> SignUpStudent(UserLogin model)
        {
            var kq = await new Account().Register(model.UserName, model.Password);
            if (kq != null)
            {
                if(kq.MaND == -1)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Tên đăng nhập đã tồn tại");
                }
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.UserMapper.MapUserLogon(kq));
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tài khoản hoặc mật khẩu không chính xác");
        }
        [HttpPost]
        [Route("api/Identity/employee")]
        public async Task<HttpResponseMessage> CreateEmployee(UserViewModel model)
        {
            var kq = await new Account().RegisterEmployee(model.UserName, model.GroupID, model.Name, model.CMND, model.HinhAnh
                , model.Number, model.Email, model.DoB, model.Address, model.Salary, model.Gender);
            if (kq != null)
            {
                if(kq.MaND == -1)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tên tài khoản đã tồn tại");
                if (kq.MaND == -2)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Chứng minh nhân dân đã được sử dụng");
                if (kq.MaND == -3)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Số điện thoại này đã được sử dụng");
                else
                    return Request.CreateResponse(HttpStatusCode.Created, Mapper.UserMapper.MapUser(kq));
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không thành công");
        }
    }
}
