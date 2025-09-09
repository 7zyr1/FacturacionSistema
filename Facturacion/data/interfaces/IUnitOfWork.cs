using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.data.interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClientRepository Clients { get; }
        IBillRepository Bills { get; }
        //IPaymentRepository Payments { get; }
        void Save();
        void Delete();
        void rollback();
        void commit();
        void BeginTransaction();
    }
}
