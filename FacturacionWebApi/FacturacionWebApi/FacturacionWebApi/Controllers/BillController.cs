using Facturacion.data.interfaces;
using Facturacion.domain;
using Facturacion.services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FacturacionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        public IBillService _BillService { get; set; }
        public BillController(IBillService billService)
        {
            _BillService = billService;
        }
        // GET: api/<BillController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_BillService.GetAllBills());
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<BillController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_BillService.GetBillById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        // POST api/<BillController>
        [HttpPost]
        public void Post([FromBody] string vale)
        {
        }

        // PUT api/<BillController>/5
        [HttpPut("{dateTime}/{id_payment}/{id_client}")]
        //public IActionResult Put(DateTime dateTime, int id_payment, int id_client,[FromBody] string value)
        //{
        //    try
        //    {
        //        return Ok(_BillService.SaveBill());
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        // DELETE api/<BillController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(_BillService.Delete(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
