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
        int min_passbookbalance;
        int min_collectmoney;
        public int Min_passbookbalance { get => min_passbookbalance; set => min_passbookbalance = value; }
        public int Min_collectmoney { get => min_collectmoney; set => min_collectmoney = value; }
        Parameter() { }
        Parameter(DataRow row)
        {
            this.Min_collectmoney = int.Parse(row["min_collectmoney"].ToString());
            this.Min_passbookbalance = int.Parse(row["min_passbookbalance"].ToString());
                   
        }

        
    }
}
