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
        private readonly IBillRepository _billRepository;

        public BillService(IBillRepository billRepository)
        {
            _billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
        }
        public bool SaveBill(Bill bill)
        {
            return _billRepository.SaveBill(bill);
        }

        public List<Bill> GetAllBills()
        {
            return _billRepository.GetAllBill();
        }

        public Bill GetBillById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }
}
