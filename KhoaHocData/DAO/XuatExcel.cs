using Common.CommonModel;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KhoaHocData.DAO
{

    public class XuatExcel
    {
        const string FILE_EXT_DOC = ".doc";
        const string FILE_EXT_DOCS = ".docx";
        const string FILE_EXT_XLS = ".xls";
        const string FILE_EXT_XLSs = ".xlsx";
        const string T_DOANHTHUNGAY = "HoaDon";
        const string T_DOANHTHUTHANG = "HoaDon2";
        const string T_TRUYCAPNGAY = "TruyCap";
        const string T_TRUYCAPTHANG = "TruyCap2";
        const string T_KHOAHOCNGAY = "KhoaHoc";
        const string T_KHOAHOCTHANG = "KhoaHoc2";
        const string TMP_ROW = "[TMP]";
        const string FOLDER_TEMPLATES = "Templates";
        string appPath = string.Empty;
        ThongKeClass db_ThongKe = new ThongKeClass();
        PaymentDAO db_Payment = new PaymentDAO();


        public string ExportDoanhThuNgay(List<SimpleHoaDonVM> dataSource, DateTime dateStart, DateTime dateEnd, ref string fileName, bool isPrintPreview)
        {
            // Check if data is null
            if (dataSource == null || (dataSource != null && dataSource.Count == 0))
            {
                return "";
            }
            Dictionary<string, string> replacer = new Dictionary<string, string>();
            // Set the So thu tu

            for (int i = 1; i <= dataSource.Count; i++)
            {
                dataSource[i - 1].STT = i.ToString();
            }
            decimal TongTienD = dataSource.Sum(t => t.TongThanhToan);
            string NgayBatDau = dateStart.ToString("dd/MM/yyyy");
            string NgayKetThuc = dateEnd.ToString("dd/MM/yyyy");
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");  
            string TongTien = TongTienD.ToString("#,###", cul.NumberFormat);

            replacer.Add("%TongDoanhThu", TongTien);
            replacer.Add("%NgayBatDau", NgayBatDau);
            replacer.Add("%NgayKetThuc", NgayKetThuc);

            return OutSimpleReport(dataSource, replacer, T_DOANHTHUNGAY, isPrintPreview, ref fileName);
        }
        public string ExportDoanhThuThang(List<MonthlyStatistic> dataSource, int Year, ref string fileName, bool isPrintPreview)
        {
            // Check if data is null
            if (dataSource == null || (dataSource != null && dataSource.Count == 0))
            {
                return "";
            }
            Dictionary<string, string> replacer = new Dictionary<string, string>();
            // Set the So thu tu

            for (int i = 1; i <= dataSource.Count; i++)
            {
                dataSource[i - 1].STT = i.ToString();
            }
            double TongTienD = dataSource.Sum(t => t.DoanhThu);
            string NamThongKe = "Năm " + Year;
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            string TongTien = TongTienD.ToString("#,###", cul.NumberFormat);

            replacer.Add("%TongDoanhThu", TongTien);
            replacer.Add("%NamThongKe", NamThongKe);

            return OutSimpleReport(dataSource, replacer, T_DOANHTHUTHANG, isPrintPreview, ref fileName);
        }
        public string ExportTruyCapNgay(List<DailyAccessStatistic> dataSource, DateTime start, DateTime end, ref string fileName, bool isPrintPreview)
        {
            if (dataSource == null || (dataSource != null && dataSource.Count == 0))
            {
                return "";
            }
            Dictionary<string, string> replacer = new Dictionary<string, string>();
            // Set the So thu tu

            for (int i = 1; i <= dataSource.Count; i++)
            {
                dataSource[i - 1].STT = i.ToString();
            }
            double TongTienD = dataSource.Sum(t => t.CourseSellCount);
            string NgayBatDau = start.ToString("dd/MM/yyyy");
            string NgayKetThuc = end.ToString("dd/MM/yyyy");
            string TongKhoaHocTieuThu = TongTienD.ToString();
            string TongHocVienMoi = dataSource.Sum(t => t.NewStudent).ToString();

            replacer.Add("%TongKhoaHocTieuThu", TongKhoaHocTieuThu);
            replacer.Add("%TongHocVienMoi", TongHocVienMoi);
            replacer.Add("%NgayBatDau", NgayBatDau);
            replacer.Add("%NgayKetThuc", NgayKetThuc);

            return OutSimpleReport(dataSource, replacer, T_TRUYCAPNGAY, isPrintPreview, ref fileName);
        }
        public string ExportTruyCapThang(List<MonthlyAccessStatistic> dataSource, int Year, ref string fileName, bool isPrintPreview)
        {
            if (dataSource == null || (dataSource != null && dataSource.Count == 0))
            {
                return "";
            }
            Dictionary<string, string> replacer = new Dictionary<string, string>();
            // Set the So thu tu

            for (int i = 1; i <= dataSource.Count; i++)
            {
                dataSource[i - 1].STT = i.ToString();
            }
            double TongTienD = dataSource.Sum(t => t.CourseSellCount);
            string NamThongKe = "Năm "+Year.ToString();
            string TongKhoaHocTieuThu = TongTienD.ToString();
            string TongHocVienMoi = dataSource.Sum(t => t.NewStudent).ToString();

            replacer.Add("%TongKhoaHocTieuThu", TongKhoaHocTieuThu);
            replacer.Add("%TongHocVienMoi", TongHocVienMoi);
            replacer.Add("%NamThongKe", NamThongKe);

            return OutSimpleReport(dataSource, replacer, T_TRUYCAPTHANG, isPrintPreview, ref fileName);
        }
        public string ExportThongTinKhoaHocNgay(List<KhoaHocDailyStatistic> dataSource, DateTime start, DateTime end, ref string fileName, bool isPrintPreview)
        {
            if (dataSource == null || (dataSource != null && dataSource.Count == 0))
            {
                return "";
            }
            Dictionary<string, string> replacer = new Dictionary<string, string>();
            // Set the So thu tu

            for (int i = 1; i <= dataSource.Count; i++)
            {
                dataSource[i - 1].STT = i.ToString();
            }
            decimal TongTienD = dataSource.Sum(t => t.TongThu);
            string TongSoLuongMua = dataSource.Sum(t => t.SoLuongDKMoi).ToString();
            string NgayBatDau = start.ToString("dd/MM/yyyy");
            string NgayKetThuc = end.ToString("dd/MM/yyyy");
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            string TongDoanhThu = TongTienD.ToString("#,###", cul.NumberFormat);

            replacer.Add("%TongSoLuongMua", TongSoLuongMua);
            replacer.Add("%TongDoanhThu", TongDoanhThu);
            replacer.Add("%NgayBatDau", NgayBatDau);
            replacer.Add("%NgayKetThuc", NgayKetThuc);

            return OutSimpleReport(dataSource, replacer, T_KHOAHOCNGAY, isPrintPreview, ref fileName);
        }
        public string ExportThongTinKhoaHocThang(List<MonthlyKhoaHocStatistic> dataSource, int Year, ref string fileName, bool isPrintPreview)
        {
            if (dataSource == null || (dataSource != null && dataSource.Count == 0))
            {
                return "";
            }
            Dictionary<string, string> replacer = new Dictionary<string, string>();
            // Set the So thu tu

            for (int i = 1; i <= dataSource.Count; i++)
            {
                dataSource[i - 1].STT = i.ToString();
            }
            decimal TongTienD = dataSource.Sum(t => t.TongThu);
            string TongSoLuongMua = dataSource.Sum(t => t.SoLuongDKMoi).ToString();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            string TongDoanhThu = TongTienD.ToString("#,###", cul.NumberFormat);
            string NamThongKe = "Năm " + Year;

            replacer.Add("%TongSoLuongMua", TongSoLuongMua);
            replacer.Add("%TongDoanhThu", TongDoanhThu);
            replacer.Add("%NamThongKe", NamThongKe);
            
            return OutSimpleReport(dataSource, replacer, T_KHOAHOCTHANG, isPrintPreview, ref fileName);
        }

        //public bool ExportNhapHang(List<TEMPNHAPHANG> dataSource, ref string fileName, bool isPrintPreview)
        //{
        //    // Check if data is null
        //    if (dataSource == null || (dataSource != null && dataSource.Count == 0))
        //    {
        //        return false;
        //    }
        //    Dictionary<string, string> replacer = new Dictionary<string, string>();
        //    // Set the So thu tu

        //    for (int i = 1; i <= dataSource.Count; i++)
        //    {
        //        dataSource[i - 1].STT = i.ToString();
        //    }
        //    string TongTien = dataSource.Sum(t => t.TongChiPhi).ToString();

        //    replacer.Add("%TongGiaTri", TongTien);
        //    return OutSimpleReport(dataSource, replacer, T_NHAPHANG, isPrintPreview, ref fileName);
        //}

        //public List<TEMPNHAPHANG> GetListTempNhapHang(List<TEMPNHAPHANG> lstDonHang)
        //{
        //    List<TEMPNHAPHANG> lstTempDonHang = new List<TEMPNHAPHANG>();
        //    int i = 1;
        //    foreach (NHAPHANG dh in lstDonHang)
        //    {
        //        TempDonHang tdh = new TempDonHang();
        //        tdh.MaDonHang = dh.MADONHANG;
        //        tdh.TongGiaTri = (double)dh.TONGGIATRI.Value;
        //        tdh.TKKhachHang = dh.TENDN;
        //        tdh.NgayLap = dh.NGAYLAP.ToString();
        //        lstTempDonHang.Add(tdh);
        //    }
        //    return lstTempDonHang;
        //}

        private string OutSimpleReport<T>(List<T> dataSource, Dictionary<string, string> replaceValues, string viewName, bool isPrintPreview, ref string fileName)
        {
            string file = string.Empty;
            string filePath = HttpContext.Current.Server.MapPath("~/Temp/");
            string result = "";

            // Get template stream
            MemoryStream stream = GetTemplateStream(viewName);

            // Check if data is null
            if (stream == null)
            {
                return "";
            }

            // Create excel engine
            ExcelEngine engine = new ExcelEngine();
            IWorkbook workBook = engine.Excel.Workbooks.Open(stream);

            IWorksheet workSheet = workBook.Worksheets[0];
            ITemplateMarkersProcessor markProcessor = workSheet.CreateTemplateMarkersProcessor();

            // Replace value
            if (replaceValues != null && replaceValues.Count > 0)
            {
                // Find and replace values
                foreach (KeyValuePair<string, string> replacer in replaceValues)
                {
                    Replace(workSheet, replacer.Key, replacer.Value);
                }
            }

            // Fill variables
            markProcessor.AddVariable(viewName, dataSource);



            // End template
            markProcessor.ApplyMarkers(UnknownVariableAction.ReplaceBlank);

            // Delete temporary row
            IRange range = workSheet.FindFirst(TMP_ROW, ExcelFindType.Text);

            // Delete
            if (range != null)
            {
                workSheet.DeleteRow(range.Row);
            }
            string[] splitNek = (Path.GetTempFileName() + FILE_EXT_XLSs).Split('\\');

            file = splitNek[splitNek.Length - 1];

            fileName = filePath + file;


            // Output file
            if (!IsFileOpenOrReadOnly(fileName))
            {
                workBook.SaveAs(fileName);
                result = file;
            }

            // Close
            workBook.Close();
            engine.Dispose();

            // Print preview
            if (!string.IsNullOrEmpty(result) && isPrintPreview)
            {
                PrintExcel(fileName);
            }

            return result;
        }
        private static void Replace(IWorksheet workSheet, string findValue, string replaceValue)
        {
            // Find and replace
            if (workSheet != null && !string.IsNullOrEmpty(findValue))
            {
                // Get current cells
                IRange[] cells = workSheet.Range.Cells;
                IRange range = null;

                // Loop cells to replace
                for (int i = 0; i < cells.Count(); i++)
                {
                    // Current cell
                    range = cells[i];

                    // Find and replace values
                    if (range != null && range.DisplayText.Contains(findValue))
                    {
                        range.Text = range.Text.Replace(findValue, replaceValue);
                        break;
                    }
                }
            }
        }
        public string AppPath
        {
            get
            {
                if (string.IsNullOrEmpty(appPath))
                {
                    appPath = AppDomain.CurrentDomain.BaseDirectory;
                }
                return appPath;
            }
        }

        private MemoryStream GetTemplateStream(string viewName)
        {
            MemoryStream stream = null;
            byte[] arrByte = new byte[0];

            //Create Temp Folder if it does not exist
            if (!Directory.Exists(AppPath + FOLDER_TEMPLATES))
            {
                Directory.CreateDirectory(AppPath + FOLDER_TEMPLATES);
            }
            string assemblyFile = HttpContext.Current.Server.MapPath("~/File");
            // Get template by view name
            switch (viewName)
            {
                #region ---- Lấy file report----
                case T_DOANHTHUNGAY:
                    arrByte = File.ReadAllBytes(assemblyFile + "/BaoCaoDoanhThu__Ngay.xlsx");
                    break;
                case T_DOANHTHUTHANG:
                    arrByte = File.ReadAllBytes(assemblyFile + "/BaoCaoDoanhThu__Thang.xlsx");
                    break;
                case T_TRUYCAPNGAY:
                    arrByte = File.ReadAllBytes(assemblyFile + "/BaoCaoTruyCap_Ngay.xlsx");
                    break;
                case T_TRUYCAPTHANG:
                    arrByte = File.ReadAllBytes(assemblyFile + "/BaoCaoTruyCap_Thang.xlsx");
                    break;
                case T_KHOAHOCNGAY:
                    arrByte = File.ReadAllBytes(assemblyFile + "/BaoCaoThongTinKhoaHoc_Ngay.xlsx");
                    break;
                case T_KHOAHOCTHANG:
                    arrByte = File.ReadAllBytes(assemblyFile + "/BaoCaoThongTinKhoaHoc_Thang.xlsx");
                    break;
                    #endregion
            }
            // Get stream
            if (arrByte.Count() > 0)
            {
                stream = new MemoryStream(arrByte);
            }

            return stream;
        }
        public static bool IsFileOpenOrReadOnly(string fileName)
        {
            try
            {
                // Check if file is not existed
                if (!File.Exists(fileName))
                {
                    return false;
                }

                // First make sure it's not a read only file
                if ((File.GetAttributes(fileName) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
                {
                    // First we open the file with a FileStream
                    using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                    {
                        try
                        {
                            stream.ReadByte();
                            return false;
                        }
                        catch (IOException)
                        {
                            return true;
                        }
                        finally
                        {
                            stream.Close();
                            stream.Dispose();
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }
        public static void PrintExcel(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = null;

            try
            {
                wb = excelApp.Workbooks.Open(fileName);

                if (wb != null)
                {
                    // Show print preview
                    excelApp.Visible = true;
                    wb.PrintPreview(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Cleanup:
                GC.Collect();
                GC.WaitForPendingFinalizers();

                wb.Close(false, Type.Missing, Type.Missing);
                Marshal.FinalReleaseComObject(wb);

                excelApp.Quit();
                Marshal.FinalReleaseComObject(excelApp);
            }
        }
    }
}
