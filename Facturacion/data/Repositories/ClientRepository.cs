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
                        Name = "@Id",
                        Value = id
                    }
                };
            var dt = DataHelper.GetInstance().ExecuteSPquery("SP_GetClientById", parameters); //crear SP
            if (dt != null && dt.Rows.Count > 0)
            {
                Client client = new Client()
                {
                    id = (int)dt.Rows[0]["Id"],
                    name = (string)dt.Rows[0]["Name"],
                    phone = (int)dt.Rows[0]["Phone"]
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
