using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhoaHocData.DAO
{
    public class SalaryDAO
    {
        QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();

        public Luong getLichSuLuong(int pMaND)
        {
            return db.Luongs.FirstOrDefault(x => x.MaND == pMaND);
        }
    }
}
