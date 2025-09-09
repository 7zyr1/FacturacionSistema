using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.domain;

namespace Facturacion.data.interfaces
{
    public interface IProductRepository
    {
        Product GetProductById(int id);
        List<Product> GetAllProducts();
    }
}
