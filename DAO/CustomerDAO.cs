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
            //code
            return true;
        }
        public string GetCustomerName(int IDCustomer)
        {
            //code
            return "";
        }
        public string GetCustomerCardNumber(int IDCustomer)
        {
            //code
            return "";
        }
        public string GetCustomerAddress(int IDCustomer)
        {
            //code
            return "";
        }
        public bool CheckCustomerHasAccountType(int customerID, string AccountType)
        {
            bool check = false;
            //code
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
