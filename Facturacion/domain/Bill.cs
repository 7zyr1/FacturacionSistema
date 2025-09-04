using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.domain
{
    public class Bill
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public DateTime dateTime { get; set; }
        public Payment Payment { get; set; }
    }
}
