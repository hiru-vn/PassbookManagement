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
        string _name;
        string _cus_address;
        int _savingbook;
        string _cmnd;

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Cus_address { get => _cus_address; set => _cus_address = value; }
        public int Savingbook { get => _savingbook; set => _savingbook = value; }
        public string Cmnd { get => _cmnd; set => _cmnd = value; }
        public Customer() { }
        public Customer(DataRow row)
        {
            this.Id = int.Parse(row["ID"].ToString());
            this.Name = row["NAME"].ToString();
            this.Cus_address = row["CUS_ADDRESS"].ToString();
            this.Savingbook = int.Parse(row["SAVINGBOOK"].ToString());
            this.Cmnd = row["CMND"].ToString();
        }
    }
}
