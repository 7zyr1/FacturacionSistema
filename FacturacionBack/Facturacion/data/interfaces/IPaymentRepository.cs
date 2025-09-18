using Facturacion.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.data.interfaces
{
    public interface IPaymentRepository
    {
        Payment? GetPaymentById(int id);
        List<Payment> GetAllPayments();
    }
}
