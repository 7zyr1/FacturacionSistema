using Facturacion.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.services
{
    public interface IProductService
    {
        List<Product> GetAllProcucts();
        bool Save();
        bool Delete();
        Product GetProductById(int id);
    }
}
