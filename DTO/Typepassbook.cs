using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DTO
{
    public class TypePassbook
    {
        int _id;
        string _typename;
        float _interest_rate;
        int term;

        public int Id { get => _id; set => _id = value; }
        public string Typename { get => _typename; set => _typename = value; }
        public float Interest_rate { get => _interest_rate; set => _interest_rate = value; }
        public int Term { get => term; set => term = value; }
        public TypePassbook() { }
        public TypePassbook(DataRow row)
        {
            this.Id = int.Parse(row["id"].ToString());
            this.Typename = row["typename"].ToString();
            this.Interest_rate = float.Parse(row["interest_rate"].ToString());
            this.term = int.Parse(row["term"].ToString());

        }
        
    }
}
