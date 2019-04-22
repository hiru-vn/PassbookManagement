using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class CustomerDAO
    {
        private static CustomerDAO instance;

        public static CustomerDAO Instance
        {
            get { if (instance == null) instance = new CustomerDAO(); return instance; }
            private set { instance = value; }
        }
        private CustomerDAO() { }

        public int GetCurrentMaxCustomerID()
        {
            int value = 0;
            value = (int)DataProvider.Instance.ExcuteScarar("select MAX(ID) from dbo.CUSTOMER");
            return value;
        }
        public bool CheckExistID(int ID)
        {
            int result = (int)DataProvider.Instance.ExcuteScarar("select count(*) from dbo.customer where id=" + ID);
            if (result == 0)
                return false;
            return true;
        }
        public string GetCustomerName(int IDCustomer)
        {
  
            string query = "select * from dbo.customer where id='" + IDCustomer + "'";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            if (data.Rows.Count > 0)
            {
                Customer cus = new Customer(data.Rows[0]);
                return cus.Cus_name;
            }
            return "Khong co ID khach hang";
        }
        public string GetCustomerCardNumber(int IDCustomer)
        {
            DataRow row = DataProvider.Instance.ExcuteQuery("select cmnd from dbo.customer where id=" + IDCustomer).Rows[0];
            Customer result = new Customer(row);
            return result.Cmnd;
        }
        public string GetCustomerAddress(int IDCustomer)
        {
            string query = "select * from dbo.customer where id=" + IDCustomer;
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            if (data.Rows.Count > 0)
            {
                Customer cus = new Customer(data.Rows[0]);
                return cus.Cus_address;
            }
            return "Khong co ID khach hang";
        }
        public bool CheckCustomerHasAccountType(int customerID , string TypePassbook)
        {
            bool check = false;
            string query = "select count(*) from passbook,typepassbook where passbook.passbook_customer=" + customerID + "and passbook.passbook_type=typepassbook.id and typename=" + TypePassbook;
            int result = (int)DataProvider.Instance.ExcuteScarar(query);
            if (result != 0)
                check = true;
            return check;
        }
        public Customer GetCustomer(int IDcustomer)
        {
            string query = string.Format("select * from dbo.customer where id = {0}", IDcustomer);
            DataRow row = DataProvider.Instance.ExcuteQuery(query).Rows[0];
            Customer cus = new Customer(row);
            return cus;
        }
        public void UpdateCustomer(Customer cus)
        {
            //code
        }
    }
}
