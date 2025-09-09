using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.data.interfaces;
using Facturacion.domain;

namespace Facturacion.data.Utilities
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataHelper _dataHelper;
        private IBillRepository _billRepository;
        private IClientRepository _clientRepository;
        public UnitOfWork(IClientRepository client, IBillRepository bill )
        { 

        }
        public void commit()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void rollback()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
