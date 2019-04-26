using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Transaction
    {
        //lop mo rong muc dich gop 2 rut tien va goi tien
        string _id;
        DateTime? _transactiondate;
        string _type; // gui tien/rut tien
        string _cusname;
        string _passbooktype;
        long _balancemoney; //so du

        public string Id { get => _id; set => _id = value; }
        public DateTime? Transactiondate { get => _transactiondate; set => _transactiondate = value; }
        public string Type { get => _type; set => _type = value; }
        public string Cusname { get => _cusname; set => _cusname = value; }
        public string Passbooktype { get => _passbooktype; set => _passbooktype = value; }
        public long Balancemoney { get => _balancemoney; set => _balancemoney = value; }

        public Transaction(CollectBill collectBill)
        {
            this.Id = collectBill.Id;
            this.Transactiondate = collectBill.Collectdate;
            this.Type = "Gởi tiền";
        }
        public Transaction(WithdrawBill withdrawBill)
        {
            this.Id = withdrawBill.Id;
            this.Transactiondate = withdrawBill.Withdrawdate;
            this.Type = "Rút tiền";
        }
    }
}
