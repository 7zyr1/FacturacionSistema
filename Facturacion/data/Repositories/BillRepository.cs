using Facturacion.domain;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Facturacion.data.interfaces;
using Facturacion.data.Utilities;

namespace Facturacion.data.Repositories
{
    public class BillRepository : IBillRepository
    {
        public required IClientRepository _clientRepository;
        public required IPaymentRepository _paymentRepository; 
        public required IDetailRepository _detailRepository;
        public bool SaveBill(Bill bill)
        {
            bool aux = true;
            SqlConnection cnn = DataHelper.GetInstance().GetConnection();
            SqlTransaction t = null;
            try
            {
                cnn.Open();
                t = cnn.BeginTransaction();
                SqlCommand cmd = new SqlCommand("Sp_INSERT_BILL", cnn, t);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter p = new SqlParameter("@id_factura", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p);
                cmd.Parameters.AddWithValue("@fecha", bill.dateTime);
                cmd.Parameters.AddWithValue("@id_forma_pago", bill.Payment.Id);
                cmd.Parameters.AddWithValue("@id_cliente", bill.Client.id);
                cmd.ExecuteNonQuery();
                int billId = (int)p.Value;

                foreach (var det in bill.Details)
                {
                    SqlCommand cmd2 = new SqlCommand("Sp_INSERT_BILL_ITEM", cnn, t);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@id_factura", billId);
                    cmd2.Parameters.AddWithValue("@id_articulo", det.Product.Id);
                    cmd2.Parameters.AddWithValue("@cantidad", det.Quantity);
                    cmd2.ExecuteNonQuery();
                }
                t.Commit();
            }
            catch (Exception ex)
            {
                aux = false;
                if (t != null)
                {
                    t.Rollback();
                }
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return aux;
        }

        public bool DeleteBill(int id)
        {
            throw new NotImplementedException();
        }

        public List<Bill> GetAllBill()
        {
            List<Bill> bills = new List<Bill>();
            DataTable? dt = DataHelper.GetInstance().ExecuteSPquery("Sp_GET_ALL_BILLS"); // Falta el nombre del SP
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    Bill bill = new Bill()
                    {

                        Id = (int)r["id_factura"],
                        dateTime = (DateTime)r["fecha"],
                        Client = _clientRepository.GetClientById((int)r["id_cliente"]),
                        Payment = _paymentRepository.GetPaymentById((int)r["id_payment"]),
                        Details = _detailRepository.GetDetailByBillId((int)r["id_factura"])
                    };
                    bills.Add(bill);
                }
                return bills;
            }
            else
            {
                return null;
            }
        }

        public Bill GetBillById(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBill(Bill bill)
        {
            throw new NotImplementedException();
        }
    }
}
