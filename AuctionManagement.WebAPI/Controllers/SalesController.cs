using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.WebAPI.Controllers {
    /// <summary>
    /// Classe que controla as requisições HTTP
    /// </summary>
    [Route("api/[controller]/")]
    [ApiController]
    public class SalesController : Controller {

        private readonly ISalesService salesService;

        public SalesController(ISalesService salesService) {
            this.salesService = salesService;
        }


        /// <summary>
        /// Method that register a new sale to the database
        /// </summary>
        /// <param name="saleDTOCreate"></param>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult<SaleDTO> Add(SaleDTOCreate saleDTOCreate) {
            try {
                var createdSale = salesService.AddSale(saleDTOCreate);

                if (createdSale == null) {
                    return BadRequest();
                }

                return CreatedAtAction(nameof(Add), new { id = createdSale.Id }, createdSale);
            }
            catch (InvalidOperationException ex) {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to get all sales
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<SaleDTO>> GetSales() {
            try {
                var salesDTO = salesService.GetSales();

                return Ok(salesDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }

        }


        /// <summary>
        /// Method to get a sale by id
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, User")]
        [HttpGet("{id}")]
        public ActionResult<SaleDTO> GetById(int id) {
            try {
                SaleDTO sale = salesService.GetSaleById(id);
                return Ok(sale);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        //[Authorize(Roles = "Admin")]
        //[HttpPut("{id}")]
        //public ActionResult Update(int id, Sale sale) {
        //    try {
        //        SaleDTO updatedSale = salesService.UpdateSale(id, sale);

        //        return Ok(updatedSale);
        //    }
        //    catch (ArgumentException ex) {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (InvalidOperationException ex) {
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (FormatException ex) {
        //        return BadRequest(new { message = ex.Message });
        //    }

        //}


        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id) {
        //    try {
        //        salesService.DeleteSale(id);
        //        return NoContent();
        //    }
        //    catch (ArgumentException ex) {
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (InvalidOperationException ex) {
        //        return BadRequest(new { message = ex.Message });
        //    }

        //}




        /*
         * ----------------- EXTRA ENDPOINTS -----------------
         */

        [Authorize(Roles = "Admin")]
        [HttpGet("total-value")]
        public ActionResult<decimal> GetTotalSalesValue() {
            try {
                return Ok(new { totalValue = salesService.GetTotalSalesValue() });
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("total-value-by-category/{categId}")]
        public ActionResult<decimal> GetTotalSalesValueByCategory(int categId) {
            try {
                return Ok(new { totalValue = salesService.GetTotalSalesValueByCategory(categId) });
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("total-quantity")]
        public ActionResult<decimal> GetTotalSalesQuantity() {
            try {
                return Ok(new { totalQuantity = salesService.GetTotalSalesQuantity() });
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("total-quantity-by-category/{categId}")]
        public ActionResult<decimal> GetTotalSalesQuantity(int categId) {
            try {
                return Ok(new { totalQuantity = salesService.GetTotalSalesQuantityByCategory(categId)});
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("per-period/date1/{date1}/date2/{date2}")]
        public ActionResult<IEnumerable<SaleDTO>> GetSalesPerPeriod(DateOnly date1, DateOnly date2) {
            try {
                return Ok(salesService.GetSalesPerPeriod(date1, date2));
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("above-value/{value}")]
        public ActionResult<IEnumerable<SaleDTO>> GetTotalSalesAboveValue(decimal value) {
            try {
                return Ok(salesService.GetSalesAboveValue(value));
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex) {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
