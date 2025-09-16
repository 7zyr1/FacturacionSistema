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
            if (bill == null) throw new ArgumentNullException(nameof(bill));
            if (bill.Client == null) throw new ArgumentNullException(nameof(bill.Client));
            if (bill.Payment == null) throw new ArgumentNullException(nameof(bill.Payment));
            if (bill.Details == null || !bill.Details.Any()) throw new ArgumentException("El detalle no puede ser nulo", nameof(bill.Details));

            bool aux = true;
            SqlConnection cnn = DataHelper.GetInstance().GetConnection();
            SqlTransaction? t = null;
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
                    if (det.Product == null) throw new ArgumentNullException(nameof(det.Product), "El detalle no puede ser nulo");

                    SqlCommand cmd2 = new SqlCommand("Sp_INSERT_BILL_ITEM", cnn, t);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@id_factura", billId);
                    cmd2.Parameters.AddWithValue("@id_articulo", det.Product.Id);
                    cmd2.Parameters.AddWithValue("@cantidad", det.Quantity);
                    cmd2.ExecuteNonQuery();
                }
                t.Commit();
            }
            catch (Exception)
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
            bool aux = true;
            SqlConnection cnn = DataHelper.GetInstance().GetConnection();
            SqlTransaction? t = null;
            try
            {
                cnn.Open();
                t = cnn.BeginTransaction();
                SqlCommand cmd = new SqlCommand("Sp_DELETE_BILL", cnn, t);
                cmd.CommandType = CommandType.StoredProcedure;
                aux = cmd.ExecuteNonQuery() == 1;
                t.Commit();
            }
            catch (Exception)
            {
                aux =false;
                if (t != null)
                {
                    t.Rollback();
                }
            }
            return aux;     
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
                        Payment = _paymentRepository.GetPaymentById((int)r["id_forma_pago"]),
                        Details = _detailRepository.GetDetailByBillId((int)r["id_factura"])
                    };
                    bills.Add(bill);
                }
                return bills;
            }
            else
            {
                return bills;
            }
        }

        public Bill? GetBillById(int id)
        {
            List<ParameterSP> parameters = new List<ParameterSP>();
            ParameterSP parameterSP = new ParameterSP()
            {
                Name = "@id",
                Value = id
            };
            parameters.Add(parameterSP);
            DataTable? dt = DataHelper.GetInstance().ExecuteSPquery("Sp_GET_BILL_BY_ID", parameters);
            if(dt != null) 
            {
                Bill bill = new Bill()
                {
                    Id = (int)dt.Rows[0]["id_factura"],
                    dateTime = (DateTime)dt.Rows[0]["fecha"],
                    Client = _clientRepository.GetClientById((int)dt.Rows[0]["id_cliente"]),
                    Payment = _paymentRepository.GetPaymentById((int)dt.Rows[0]["id_forma_pago"]),
                    Details = _detailRepository.GetDetailByBillId(id),
                };
                return bill;
            }
            else
            {
                return null;
            }
        }
        public bool UpdateBill(Bill bill)
        {
            throw new NotImplementedException();
        }
    }
}