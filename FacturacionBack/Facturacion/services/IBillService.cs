using Facturacion.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.services
{
    public interface IBillService
    {
        bool SaveBill(Bill bill);
        List<Bill> GetAllBills();
        Bill GetBillById(int id);
        bool Delete();
        bool Update();
    }
}
