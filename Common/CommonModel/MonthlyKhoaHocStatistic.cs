﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CommonModel
{
    public class MonthlyKhoaHocStatistic
    {
        public int Thang { get; set; }
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public int SoLuongDKMoi { get; set; }
        public decimal TongThu { get; set; }
    }
}