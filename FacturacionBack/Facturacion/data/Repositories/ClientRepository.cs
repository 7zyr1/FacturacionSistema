using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
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
        //public bool Delete(int id)
        //{
            //try
            //{
            //    ParameterSP parameter = new ParameterSP()
            //    {
            //        Name = "@id",
            //        Value = id
            //    };
            //    return DataHelper.GetInstance().ExecuteSPquery("Sp_DELETE_CLIENT");
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            
        //}

        public List<Client>? GetALLClients()
        {
            List<Client> clients = new List<Client>();
            DataTable? dt = DataHelper.GetInstance().ExecuteSPquery("Sp_GET_ALL_CLIENTS");
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    Client client = new Client()
                    {
                        id = (int)r["Id"],
                        name = (string)r["Name"],
                        phone = (int)r["Phone"]
                    };
                    clients.Add(client);
                }
                return clients;
            }
            else
            {
                return null;
            }
        }

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

        public bool Save(Client client)
        {
            List<ParameterSP> parameters = new List<ParameterSP>();
            parameters.Add(new ParameterSP() { Name = "Name", Value = client.name });
            parameters.Add(new ParameterSP() { Name = "Phone", Value = client.phone });
            return DataHelper.GetInstance().ExecuteSPquery("Sp_INSERT_CLIENT", parameters) != null;
        }
    }
}
