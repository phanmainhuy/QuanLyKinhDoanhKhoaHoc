using KhoaHocData.EF;
using System;
using System.Configuration;
using System.Linq;

namespace KhoaHocData.OnlineParty
{
    public class VnPayDAO
    {
        private QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();

        public string GetPayURL(long OrderId, long Amount, string BankCode, string MaApDung = null)
        {
            var hd = db.HoaDons.FirstOrDefault(x => x.MaHD == OrderId);

            var km = db.KhuyenMais.FirstOrDefault(x => x.MaApDung == MaApDung);
            if (km != null)
            {
                var km_kh = db.KhuyenMai_KhachHang.FirstOrDefault(x => x.MaND == hd.MaND && x.MaKM == km.MaKM);
                if (km_kh != null)
                {
                    Amount = Amount - (long)km.GiaTri.Value > 10000 ? Amount - (long)km.GiaTri.Value :
                        10000;
                    km_kh.IsSuDung = true;
                    hd.MaKM = km.MaKM;
                    db.SaveChanges();
                }
            }
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
            string ThoiHanThanhToan = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss");

            if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            {
                //  lblMessage.Text = "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config";
                return null;
            }
            DateTime CreatedDate = DateTime.Now;
            string locale = "vn";
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân,
            //phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND
            //(một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân),
            //sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_Amount", (Amount * 100).ToString());
            if (BankCode != null && !string.IsNullOrEmpty(BankCode))
            {
                vnpay.AddRequestData("vnp_BankCode", BankCode);
            }
            vnpay.AddRequestData("vnp_CreateDate", CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            if (!string.IsNullOrEmpty(locale))
            {
                vnpay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + OrderId);
            vnpay.AddRequestData("vnp_OrderType", "billpayment");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", OrderId.ToString());

            // Mã tham chiếu của giao dịch tại hệ thống của merchant.
            // Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY.
            // Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version

            vnpay.AddRequestData("vnp_ExpireDate", ThoiHanThanhToan);

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;
        }

        public ReturnObject GetConfirmResult(long orderId, long vnp_Amount, string vnpayTranId, string vnp_ResponseCode,
                                        string vnp_TransactionStatus, string vnp_SecureHash
                                        , string vnp_BankCode, string vnp_CardType, string vnp_OrderInfo
            , string vnp_PayDate, string vnp_TmnCode, string vnp_BankTranNo
                                        )
        {
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret key
            ReturnObject returnContent = new ReturnObject()
            {
                RspCode = "97",
                Message = "Invalid signature"
            };
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddResponseData("vnp_Amount", vnp_Amount.ToString());
            vnpay.AddResponseData("vnp_BankCode", vnp_BankCode);
            vnpay.AddResponseData("vnp_BankTranNo", vnp_BankTranNo);
            vnpay.AddResponseData("vnp_CardType", vnp_CardType);
            vnpay.AddResponseData("vnp_OrderInfo", vnp_OrderInfo);
            vnpay.AddResponseData("vnp_PayDate", vnp_PayDate);
            vnpay.AddResponseData("vnp_ResponseCode", vnp_ResponseCode);
            vnpay.AddResponseData("vnp_SecureHash", vnp_SecureHash);
            vnpay.AddResponseData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddResponseData("vnp_TransactionNo", vnpayTranId);
            vnpay.AddResponseData("vnp_TransactionStatus", vnp_TransactionStatus);
            vnpay.AddResponseData("vnp_TxnRef", orderId.ToString());

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            if (checkSignature)
            {
                //Cap nhat ket qua GD
                //Yeu cau: Truy van vao CSDL cua  Merchant => lay ra duoc OrderInfo
                //Giả sử OrderInfo lấy ra được như giả lập bên dưới
                HoaDon order = db.HoaDons.SingleOrDefault(x => x.MaHD == orderId); //get from DB
                                                                                   //0: Cho thanh toan,1: da thanh toan,2: GD loi
                                                                                   //Kiem tra tinh trang Order
                var cthds = order.CT_HoaDon.ToList();
                var lstKhoaHoc = db.KhoaHocs.ToList();
                lstKhoaHoc = lstKhoaHoc.Where(x => cthds.Any(y => y.MaKhoaHoc == x.MaKhoaHoc)).ToList();
                lstKhoaHoc.ForEach(x =>
                {
                    if (x.SoLuongMua == null)
                        x.SoLuongMua = 1;
                    else
                        x.SoLuongMua++;
                });
                if (order != null)
                {
                    var km = db.KhuyenMais.FirstOrDefault(x => x.MaKM == order.MaKM);
                    decimal giatri = 0;
                    if (db.KhuyenMai_KhachHang.Any(x => x.MaKM == km.MaKM && order.MaND == x.MaND))
                        giatri = km.GiaTri.Value;
                    if (order.TongTien.Value - giatri == vnp_Amount / 100)
                    {
                        if (order.TrangThai == "0" || order.TrangThai == "1")
                        {
                            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                            {
                                //Thanh toan thanh cong
                                order.TrangThai = "1";
                                order.ThanhToan = true;
                                order.HinhThucThanhToan = "VnPay";
                            }
                            else
                            {
                                order.TrangThai = "2";
                            }
                            returnContent = new ReturnObject()
                            {
                                RspCode = "00",
                                Message = "Confirm Success"
                            };
                        }
                        else
                        {
                            returnContent = new ReturnObject()
                            {
                                RspCode = "02",
                                Message = "Order already confirmed"
                            };
                        }
                    }
                    else
                    {
                        returnContent = new ReturnObject()
                        {
                            RspCode = "04",
                            Message = "invalid amount"
                        };
                    }
                }
                else
                {
                    returnContent = new ReturnObject()
                    {
                        RspCode = "01",
                        Message = "Order not found"
                    };
                }
            }
            try { db.SaveChanges(); }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return returnContent;
        }
    }

    public class ReturnObject
    {
        public string RspCode { get; set; }
        public string Message { get; set; }
    }
}