﻿using System;
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
            //trả về bảng gồm các hàng STT, IDPassbook, CustomerName, PassbookType, Balance, DateOpenPassbook, WithDrawDate (WithDrawDate là ngày mở + thời hạn rút, nếu không có thời hạn trả về null)
            string query = "select ROW_NUMBER() over(order by passbook.id) STT, passbook.id IDPassbook, cus_name CustomerName, typename PassbookType, passbook_balance Balance, opendate DateOpenPassbook, withdrawday WithDrawDate from dbo.passbook, dbo.customer, dbo.typepassbook where passbook_customer = customer.id and passbook_type = typepassbook.id and cus_name like '%" + CusName + "%'";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;    
        }
        public DataTable GetPassInfoByPassID(int PassbookID)
        {
            //tìm kiếm chính xác ID
            //trả về bảng gồm 1 hàng STT, IDPassbook, CustomerName, PassbookType, Balance, DateOpenPassbook, WithDrawDate (WithDrawDate là ngày mở + thời hạn rút, nếu không có thời hạn thì ghi là --)
            string query = "select ROW_NUMBER() over(order by passbook.id) STT, passbook.id IDPassbook, cus_name CustomerName, typename PassbookType, passbook_balance Balance, opendate DateOpenPassbook, withdrawday WithDrawDate from dbo.passbook, dbo.customer, dbo.typepassbook where passbook_customer = customer.id and passbook_type = typepassbook.id and passbook.id like '%" + PassbookID + "%'";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
        public long GetBalanceMoneyByCollectBillID(string BillID)
        {
            string query = "select passbook_balance from dbo.passbook, dbo.collectbill where collect_passbook =passbook.id and collectbill.id='" + BillID + "'";
            long result = (long)DataProvider.Instance.ExcuteScarar(query);
            return result;
        }
        public string GetPassbookTypeNameByCollectBillID(string BillID)
        {


            string query = "select typename from dbo.typepassbook, dbo.passbook, dbo.collectbill where collect_passbook= passbook.id and passbook.passbook_type = typepassbook.id and collectbill.id='" + BillID + "'";
            string  result =(string)DataProvider.Instance.ExcuteScarar(query).ToString();
            return result;
        }
        public long GetBalanceMoneyByWithdrawBillID(string BillID)
        {
            string query = "select passbook_balance from dbo.passbook, dbo.withdrawbill where withdraw_passbook =passbook.id and withdrawbill.id='" + BillID + "'";
            long result = (long)DataProvider.Instance.ExcuteScarar(query);
            return result;
        }
        public string GetPassbookTypeNameByWithdrawBillID(string BillID)
        {
            string query = "select typename from dbo.typepassbook, dbo.passbook, dbo.withdrawbill where withdraw_passbook= passbook.id and passbook.passbook_type = typepassbook.id and withdrawbill.id='" + BillID + "'";
            string result = (string)DataProvider.Instance.ExcuteScarar(query).ToString();
            return result;
        }
    }
}
