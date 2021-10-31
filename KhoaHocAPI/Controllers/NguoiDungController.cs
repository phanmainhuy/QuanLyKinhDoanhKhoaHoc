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
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class NguoiDungController : ApiController
    {
        private readonly NguoiDungDAO ndDAO = new NguoiDungDAO();
        // GET: NguoiDung
        [System.Web.Http.HttpGet]
        public IEnumerable<NguoiDung> Get()
        {
            var result =  ndDAO.LayDanhSachHocVien();
            return result;
        }
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, NguoiDung model)
        {
            var result =  this.ndDAO.DangKy(model);
            if(result == LoginResult.ThanhCong)
                return request.CreateResponse(System.Net.HttpStatusCode.Created);
            else
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }
        [System.Web.Http.HttpGet]
        public IEnumerable<NguoiDung> GetHocVien(int pMaNhomNguoiDung)
        {
            return this.ndDAO.LayDanhSachTheoMaNhom(pMaNhomNguoiDung);
        }
    }
}