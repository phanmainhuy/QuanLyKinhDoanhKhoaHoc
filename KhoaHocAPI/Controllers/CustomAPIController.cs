using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomAPIController : ApiController
    {
        private UserGroupDAO db_UserGroup;

        public CustomAPIController()
        {
            db_UserGroup = new UserGroupDAO();
        }

        [System.Web.Http.Route("NhomNguoiDung")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetAllNhomNguoiDung()
        {
            var result = Mapper.OtherMapper.MapListUserGroup(db_UserGroup.LayTatCaNhomNguoiDung());
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Không tìm thấy nội dung");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

        [Route("KhoaHocTheoGiaoVien")]
        [HttpGet]
        public HttpResponseMessage GetKhoaHocTheoGiaoVien(int MaGV)
        {
            var result = new KhoaHocDAO().LayKhoaHocTheoMaGiaoVien(MaGV);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có khóa học nào");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.CourseMapper.MapListCourse(result));
            }
        }

        [HttpGet]
        [Route("Backup")]
        public async void Backup(string fileName)
        {
            GetDAO dbGETDAO = new GetDAO();
            string path = HttpContext.Current.Server.MapPath($"~/App_Data/{fileName}");
            await dbGETDAO.BackupDatabase(path);
            var fileBytes = File.ReadAllBytes(path);

            /*
             * var fileMemStream =
                new MemoryStream(fileBytes);
            var result =
                new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(fileMemStream);
            var headers = result.Content.Headers;

            headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment");
            headers.ContentDisposition.FileName = fileName;
            headers.ContentLength = fileMemStream.Length;
            */

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AppendHeader("content-length", fileBytes.Length.ToString());
            HttpContext.Current.Response.AppendHeader("content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
            HttpContext.Current.Response.Headers.Set("connection", "keep-alive");
            HttpContext.Current.Response.TransmitFile(path);
            HttpContext.Current.Response.End();
        }

        [Route("Restore")]
        [HttpPost]
        public async Task<HttpResponseMessage> Restore()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            GetDAO dbGETDAO = new GetDAO();
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (MultipartFileData file in provider.FileData)
                {
                    var x = file.LocalFileName.Split('\\');
                    var lastName1 = (file.Headers.ContentDisposition.FileName.ToString());
                    var lastName2 = lastName1.Substring(1, lastName1.Length - 4);
                    x[x.Length - 1] = lastName2.Split('.')[0];
                    string saveName = string.Join("\\", x);
                    if (!File.Exists(saveName))
                        File.Move(file.LocalFileName, saveName);
                    if (!File.Exists(Path.ChangeExtension(saveName, ".bak")))
                        File.Move(saveName, Path.ChangeExtension(saveName, ".bak"));
                    await dbGETDAO.RestoreDatabase(Path.ChangeExtension(saveName, ".bak"));
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("api/NhomNguoiDung/GetAll")]
        public HttpResponseMessage GetNhomNguoiDung()
        {
            var result = new UserGroupDAO().LayTatCaNhomNguoiDung();
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có dữ liệu");
            }
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.UserMapper.MapListUserGroup(result));
        }

        [HttpPost]
        [Route("api/UploadImage")]
        public HttpResponseMessage UploadImage(string path, string ImageName)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 15; //Size = 1 MB

                        List<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");
                            return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/Assets/images/" + path + "/" + ImageName);
                            //Userimage myfolder name where i want to save my image
                            postedFile.SaveAs(filePath);
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                return Request.CreateResponse(HttpStatusCode.NotFound, res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var res = string.Format("some Message");
                return Request.CreateResponse(HttpStatusCode.NotFound, res);
            }
        }
        [HttpPost]
        [Route("api/UploadVideo")]
        public HttpResponseMessage UploadVideoFiles()
        {
            var httpRequest = HttpContext.Current.Request;
            //Upload Image    
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            List<string> lstExtension = new List<string>()
            {
                ".mp4"
            };
            try
            {
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    HttpPostedFile hpf = hfc[iCnt];
                    var ext = hpf.FileName.Substring(hpf.FileName.LastIndexOf('.'));
                    if (!lstExtension.Contains(ext))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Vui lòng chỉ upload file mp4");
                    }
                    if (hpf.ContentLength > 0)
                    {
                        var filename = (Path.GetFileName(hpf.FileName));
                        var filePath = HttpContext.Current.Server.MapPath("~/Assets/video/" + filename);
                        hpf.SaveAs(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp lỗi khi lưu video");
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}