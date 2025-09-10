using Facturacion.data.interfaces;
using Facturacion.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.services
{
    public class BillService : IBillService
    {
        private IBillRepository _billRepository;
        public BillService(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }
        public bool SaveBill(Bill bill)
        {
            return _billRepository.SaveBill(bill);
        }

        public List<Bill> GetAllBills()
        {
            return _billRepository.GetAllBill();
        }
    }
}
