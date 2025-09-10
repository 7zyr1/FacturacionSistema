using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.data.interfaces;
using Facturacion.data.Utilities;
using Facturacion.domain;

namespace Facturacion.data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        public Client? GetClientById(int id)
        {
            List<ParameterSP> parameters = new List<ParameterSP>()
                    {
                        new ParameterSP()
                        {
                            Name = "@id",
                            Value = id
                        }
                    };
            var dt = DataHelper.GetInstance().ExecuteSPquery("Sp_GET_CLIENT_BY_ID", parameters); //crear SP
            if (dt != null && dt.Rows.Count > 0)
            {
                Client client = new Client()
                {
                    id = (int)dt.Rows[0]["id"],
                    name = (string)dt.Rows[0]["name"],
                    phone = (int)dt.Rows[0]["phone"]
                };
                return client;
            }
            else
            {
                return null;
            }
        }
    }
}
