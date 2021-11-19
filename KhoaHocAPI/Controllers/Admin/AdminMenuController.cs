﻿using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KhoaHocAPI.Controllers.Admin
{
    public class AdminMenuController : ApiController
    {
        MenuDAO db_Menu = new MenuDAO();
        public HttpResponseMessage Get()
        {
            var result = Mapper.MenuTypeMapper.MapListMenuType( db_Menu.LayTatCaLoaiQuyen());
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có quyền nào hêt");
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}