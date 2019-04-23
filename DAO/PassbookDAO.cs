using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class PassbookDAO
    {
        private static PassbookDAO instance;

        public static PassbookDAO Instance
        {
            get { if (instance == null) instance = new PassbookDAO(); return instance; }
            private set { instance = value; }
        }
        private PassbookDAO() { }
        public int GetMaxID()
        {
            int value=0;
            return value;
        }
        public Passbook GetAccount(int ID)
        {
            string query = string.Format("select * from dbo.savingaccount where id = {0}", ID);
            DataRow row = DataProvider.Instance.ExcuteQuery(query).Rows[0];
            Passbook acc = new Passbook(row);
            return acc;
        }
        public DataTable GetPassInfoByCusName(string CusName)
        {
            //tìm kiếm gần đúng %CusName%
            //trả về bảng gồm các hàng STT, IDPassbook, CustomerName, PassbookType, Balance, DateOpenPassbook, WithDrawDate (WithDrawDate là ngày mở + thời hạn rút, nếu không có thời hạn thì ghi là --)
            // https://stackoverflow.com/questions/19268811/set-default-value-in-query-when-value-is-null
            return null;
        }
        public DataTable GetPassInfoByPassID(int PassbookID)
        {
            //tìm kiếm chính xác ID
            //trả về bảng gồm 1 hàng STT, IDPassbook, CustomerName, PassbookType, Balance, DateOpenPassbook, WithDrawDate (WithDrawDate là ngày mở + thời hạn rút, nếu không có thời hạn thì ghi là --)
            return null;
        }
    }
}
