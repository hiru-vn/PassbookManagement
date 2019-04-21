using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace DTO
{
    class Parameter
    {
        int _atleast_collectmoney;
        int _atleast_passbookbalance;
        public Parameter() { }
        public Parameter(DataRow row)
        {
            this.Atleast_collectmoney = int.Parse(row["atleast_collectmoney"].ToString());
            this.Atleast_passbookbalance = int.Parse(row["atleast_passbookmoaney"].ToString());

        }

        public int Atleast_collectmoney { get => _atleast_collectmoney; set => _atleast_collectmoney = value; }
        public int Atleast_passbookbalance { get => _atleast_passbookbalance; set => _atleast_passbookbalance = value; }
    }
}
