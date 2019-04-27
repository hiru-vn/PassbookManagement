using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Passbook
    {

        int _id;
        int _passbooktype;
        int _passbook_customer;
        long _passbook_balance;
        DateTime? opendate;

        public Passbook() { }

        public Passbook(DataRow row)
        {
            this.Id = int.Parse(row["ID"].ToString());
            this.Passbooktype = int.Parse(row["passbooktype"].ToString());
            this.Passbook_balance = long.Parse(row["passbook_balance"].ToString());
            this.Passbook_customer = int.Parse(row["passbook_customer"].ToString());
            this.Opendate = (DateTime?)row["opendate"];


        }

        public int Id { get => _id; set => _id = value; }
        public int Passbooktype { get => _passbooktype; set => _passbooktype = value; }
        public int Passbook_customer { get => _passbook_customer; set => _passbook_customer = value; }
        public long Passbook_balance { get => _passbook_balance; set => _passbook_balance = value; }

        public DateTime? Opendate { get => opendate; set => opendate = value; }
    }
}
