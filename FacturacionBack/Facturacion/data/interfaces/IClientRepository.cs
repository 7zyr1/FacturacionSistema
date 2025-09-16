using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.data.Utilities;
using Facturacion.domain;

namespace Facturacion.data.interfaces
{
    public interface IClientRepository
    {
        List<Client>? GetALLClients();
        //bool Delete();
        bool Save(Client client);
        Client? GetClientById(int id);
    }
}
