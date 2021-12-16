using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
    public class CreateOrder
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public long TotalPrice { get; set; }

        //public static List<CreateOrder> createOrders=new List<CreateOrder>();
    }
}
