using Facturacion.data.interfaces;
using Facturacion.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.data.Utilities;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Facturacion.data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            var dt = DataHelper.GetInstance().ExecuteSPquery("SP_GetAllProducts"); //crear SP
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    Product product = new Product()
                    {
                        Id = (int)r["Id"],
                        Name = (string)r["Name"],
                        Price = (decimal)r["Price"]
                    };
                    products.Add(product);
                }
                return products;
            }
            else
            {
                return null;
            }
        }

        public Product GetProductById(int id)
        {
            List<ParameterSP> parameters = new List<ParameterSP>()
                    {
                        new ParameterSP()
                        {
                            Name = "@Id",
                            Value = id
                        }
                    };
            var dt = DataHelper.GetInstance().ExecuteSPquery("SP_GetProductById", parameters); //crear SP
            if (dt != null && dt.Rows.Count > 0)
            {
                Product product = new Product()
                {
                    Id = (int)dt.Rows[0]["Id"],
                    Name = (string)dt.Rows[0]["Name"],
                    Price = (decimal)dt.Rows[0]["Price"]
                };
                return product;
            }
            else
            {
                return null;
            }
        }
    }
}
