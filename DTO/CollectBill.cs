using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CollectBill
    {
        string _id;
        int _collect_passbook;
        long _collect_money;
        DateTime? _collectdate;
        public CollectBill() { }

        public CollectBill(DataRow row)
        {
            this.Id = row["id"].ToString();
            this.Collect_passbook = int.Parse(row["collect_passbook"].ToString());
            this.Collect_money = long.Parse(row["collectmoney"].ToString());
            this.Collectdate = (DateTime?)row["collectdate"];
        }

        public string Id { get => _id; set => _id = value; }
        public int Collect_passbook { get => _collect_passbook; set => _collect_passbook = value; }
        public long Collect_money { get => _collect_money; set => _collect_money = value; }
        public DateTime? Collectdate { get => _collectdate; set => _collectdate = value; }
    }
}


