using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string UserId { get; set; } // Bu urun kime ait? cevabini verir.


    }
}
