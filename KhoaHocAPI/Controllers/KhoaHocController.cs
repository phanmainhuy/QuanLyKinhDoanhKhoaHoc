using KhoaHocData.DAO;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace KhoaHocAPI.Controllers
{
    public class KhoaHocController : ApiController
    {
        private readonly KhoaHocDAO khDAO = new KhoaHocDAO();
        
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = khDAO.LayRaToanBoKhoaHoc();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return BadRequest();
        } 
        [HttpGet]
        public IHttpActionResult Get(string maKhoa)
        {
            var item = khDAO.LayKhoaHocTheoMa(maKhoa);
            if (item != null)
            {
                return Ok(item);
            }
            else
                return BadRequest();
        }

    }
}
