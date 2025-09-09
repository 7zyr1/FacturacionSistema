using Facturacion.domain;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Facturacion.data.interfaces;
using Facturacion.data.Utilities;

namespace Facturacion.data.Repositories
{
    public class BillRepository : IBillRepository
    {
        public required IClientRepository _clientRepository;
        public required IPaymentRepository _paymentRepository; 
        public bool SaveBill(Bill bill)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBill(int id)
        {
            throw new NotImplementedException();
        }

        public List<Bill> GetAllBill()
        {
            List<Bill> bills = new List<Bill>();
            DataTable? dt = DataHelper.GetInstance().ExecuteSPquery(""); // Falta el nombre del SP
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    Bill bill = new Bill()
                    {

                        Id = (int)r["Id"],
                        dateTime = (DateTime)r["DateTime"],
                        Client = _clientRepository.GetClientById((int)r["ClientId"]),
                        Payment = _paymentRepository.GetPaymentById((int)r["PaymentId"])

                    };
                    bills.Add(bill);
                }
                return bills;
            }
            else
            {
                return null;
            }
        }

        public Bill GetBillById(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBill(Bill bill)
        {
            throw new NotImplementedException();
        }
    }
}
