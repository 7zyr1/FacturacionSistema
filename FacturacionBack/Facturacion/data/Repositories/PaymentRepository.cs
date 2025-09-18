using Facturacion.data.interfaces;
using Facturacion.data.Utilities;
using Facturacion.domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public List<Payment> GetAllPayments()
        {
            List<Payment> payments = new List<Payment>();
            DataTable? dt = DataHelper.GetInstance().ExecuteSPquery("Sp_GET_ALL_PAYMENTS);");
            if(dt!=null){
                foreach (DataRow r in dt.Rows) {
                    Payment payment = new Payment()
                    {
                        Id = (int)r["Id"],
                        Method = (string)r["method"],
                    };
                }
            }
            return payments;
        }
        public Payment? GetPaymentById(int id)
        {
            List<ParameterSP> parameters = new List<ParameterSP>();
            {
                new ParameterSP()
                {
                    Name = "@id",
                    Value = id,
                };
                var dt = DataHelper.GetInstance().ExecuteSPquery("Sp_GET_PAYMENT_BY_ID", parameters); //crear SP
                if (dt != null && dt.Rows.Count > 0) 
                {
                    Payment payment = new Payment()
                    {
                        Id = (int)dt.Rows[0]["id_forma_pago"],
                        Method = (string)dt.Rows[0]["nombre"]
                    };
                    return payment;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
