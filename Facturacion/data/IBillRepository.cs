using Facturacion.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.data
{
    public interface IBillRepository
    {
        List<Bill> GetAllBill();
        Bill GetBillById(int id);
        bool CreateBill(Bill bill);
        bool UpdateBill(Bill bill);
        bool DeleteBill(int id);
    }
}
