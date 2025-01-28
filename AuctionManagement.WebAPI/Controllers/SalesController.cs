using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
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
        [HttpPost]
        public ActionResult<SaleDTO> Add(SaleDTOCreate saleDTOCreate) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var createdSale = salesService.AddSale(saleDTOCreate);

                if (createdSale == null) {
                    return BadRequest();
                }

                return CreatedAtAction(nameof(Add), new { id = createdSale.Id }, createdSale);
            }
            catch (InvalidOperationException ex) {
                return BadRequest(new { message = ex.Message });
            } catch (FormatException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to get all sales
        /// </summary>
        /// <returns></returns>
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
        [HttpGet("{id}")]
        public ActionResult<SaleDTO> GetById(int id) {
            try {
                SaleDTO sale = salesService.GetSaleById(id);
                return Ok(sale);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }



        /// <summary>
        /// Method that update a sale by receiving its id and its JSON body
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sale"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult Update(int id, Sale sale) {
            try {
                SaleDTO updatedSale = salesService.UpdateSale(id, sale);

                return Ok(updatedSale);
            }
            catch (ArgumentException ex) {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            } catch (FormatException ex) {
                return BadRequest(new { message = ex.Message });
            }

        }


        /// <summary>
        /// Method that deletes an sale with the id received as parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            try {
                salesService.DeleteSale(id);
                return NoContent();
            }
            catch (ArgumentException ex) {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
