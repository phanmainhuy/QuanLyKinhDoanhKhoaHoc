using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NguoiDungController : ApiController
    {
        private readonly NguoiDungDAO ndDAO = new NguoiDungDAO();
        // GET: NguoiDung
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            var result =  Mapper.UserMapper.MapListUser( ndDAO.LayHetNguoiDung());
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, result);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/GetHeThong")]
        public HttpResponseMessage GetHeThong()
        {
            var result = Mapper.UserMapper.MapListUser(ndDAO.LayHetHeThong());
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, result);
        }
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, NguoiDung model)
        {
            var result =  this.ndDAO.DangKy(model);
            if(result == Common.AllEnum.RegisterResult.ThanhCong)
                return request.CreateResponse(System.Net.HttpStatusCode.Created);
            else
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetHocVien(int pMaNhomNguoiDung)
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.OK,  Mapper.UserMapper.MapListUser( this.ndDAO.LayDanhSachTheoMaNhom(pMaNhomNguoiDung)));
        }
    }
}