using Facturacion.data.interfaces;
using Facturacion.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.data.Utilities;
using System.Data;

namespace Facturacion.data.Repositories
{
    public class DetailRepository : IDetailRepository
    {
        public required IProductRepository _productRepository { get; set; }

        //public bool DeleteDetail(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Detail> GetAllDetails()
        //{
        //    throw new NotImplementedException();
        //}

        public List<Detail> GetDetailByBillId(int id)
        {
            var details = new List<Detail>();
            var parameters = new List<ParameterSP>
                    {
                        new ParameterSP { Name = "@id_factura", Value = id }
                    };

            var dt = DataHelper.GetInstance().ExecuteSPquery("Sp_GET_BILL_DETAILS", parameters);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var detail = new Detail
                    {
                        Id = (int)row["id_detalle"],
                        Quantity = (int)row["cantidad"],
                        //Price = Convert.ToDecimal(row["precio"]),
                        Product = _productRepository.GetProductById((int)row["id_articulo"])
                    };
                    details.Add(detail);
                }
            }

            return details;
        }

        //public bool SaveDetail(Detail detail)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool UpdateDetail(Detail detail)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
