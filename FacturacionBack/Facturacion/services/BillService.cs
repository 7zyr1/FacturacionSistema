using Facturacion.data.interfaces;
using Facturacion.domain;
using Facturacion.services;
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

        public Bill? GetBillById(int id)
        {
            return _billRepository.GetBillById(id);
        }

        public bool Delete(int id)
        {
            return _billRepository.DeleteBill(id);
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }
}
