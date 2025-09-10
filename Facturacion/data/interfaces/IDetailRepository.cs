using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.domain;

namespace Facturacion.data.interfaces
{
    public interface IDetailRepository
    {
        //List<Detail> GetAllDetails();
        List<Detail> GetDetailByBillId(int id);
        //bool SaveDetail(Detail detail);
        //bool UpdateDetail(Detail detail);
        //bool DeleteDetail(int id);
    }
}
