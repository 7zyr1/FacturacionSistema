using Facturacion.data.interfaces;
using Facturacion.data.Utilities;
using Facturacion.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public Payment GetPaymentById(int id)
        {
            List<ParameterSP> parameters = new List<ParameterSP>();
            {
                new ParameterSP()
                {
                    Name = "@id",
                    Value = id,
                };
                var dt = DataHelper.GetInstance().ExecuteSPquery(" ", parameters); //crear SP
                if (dt != null && dt.Rows.Count > 0) 
                {
                    Payment payment = new Payment()
                    {
                        Id = (int)dt.Rows[0]["@id"],
                        Method = (string)dt.Rows[0]["@name"]
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
