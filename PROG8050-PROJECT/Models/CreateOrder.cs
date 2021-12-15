using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT.Models
{
    public class CreateOrder
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public Int64 Price { get; set; }
        public Int64 Quantity { get; set; }
        public Int64 TotalPrice { get; set; }

        public static List<CreateOrder> createOrders=new List<CreateOrder>();
    }
}
