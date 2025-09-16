using Facturacion.data.interfaces;
using Facturacion.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.services
{
    public class ProductService : IProductService
    {
        public readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }
        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProcucts()
        {
            return _productRepository.GetAllProducts();
        }

        public Product GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
