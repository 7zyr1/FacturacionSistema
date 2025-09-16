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
            var dt = DataHelper.GetInstance().ExecuteSPquery("Sp_GET_ALL_PRODUCTS"); //crear SP
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    Product product = new Product()
                    {
                        Id = (int)r["Id"],
                        Name = (string)r["Name"],
                        Price = Convert.ToDecimal(r["Price"]),
                        Stock = (int)r["Stock"]
                    };
                    products.Add(product);
                }
                return products;
            }
            else
            {
                return products;
            }
        }

        public Product? GetProductById(int id)
        {
            List<ParameterSP> parameters = new List<ParameterSP>()
                    {
                        new ParameterSP()
                        {
                            Name = "@Id",
                            Value = id
                        }
                    };
            var dt = DataHelper.GetInstance().ExecuteSPquery("Sp_GET_PRODUCT_BY_ID", parameters); //crear SP
            if (dt != null && dt.Rows.Count > 0)
            {
                Product product = new Product()
                {
                    Id = (int)dt.Rows[0]["Id"],
                    Name = (string)dt.Rows[0]["Name"],
                    Price =Convert.ToDecimal(dt.Rows[0]["Price"]),
                    Stock = (int)dt.Rows[0]["Stock"]
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
