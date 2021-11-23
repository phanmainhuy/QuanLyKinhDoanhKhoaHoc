using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KhoaHocAPI.Controllers
{
    public class ThanhToanController : ApiController
    {
        private PaymentDAO db;
        public ThanhToanController()
        {
            db = new PaymentDAO();
        }
        //[HttpPost]
        //public HttpResponseMessage AddHoaDon()
        //{


        //}
    }
}
