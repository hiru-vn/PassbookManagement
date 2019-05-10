using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DTO
{
    public class Customer
    {
        int _id;
        string _cus_name;
        string _cus_address;
        string _cmnd;

        public Customer() { }
        public Customer(DataRow row)
        {
            this.Id = int.Parse(row["ID"].ToString());
            this.Cus_name = row["cus_name"].ToString();
            this.Cus_address = row["cus_address"].ToString();
            this.Cmnd = row["cmnd"].ToString();
        }

        public int Id { get => _id; set => _id = value; }
        public string Cus_name { get => _cus_name; set => _cus_name = value; }
        public string Cus_address { get => _cus_address; set => _cus_address = value; }
        public string Cmnd { get => _cmnd; set => _cmnd = value; }
    }
}
