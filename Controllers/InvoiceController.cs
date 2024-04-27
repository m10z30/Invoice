using Invoice.Data;
using Invoice.Dtos;
using Invoice.Models;
using Invoice.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly DataContext _context;

        public InvoiceController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices([FromQuery] GetAllInvoicesQuery query)
        {
            var invoices = await _context.Invoices
                        .OrderByDescending(i => i.Id)
                        .Skip(query.Offset)
                        .Take(query.Limit)
                        .Where(i => i.CustomerName != "--reset--")
                        .ToListAsync();

            return Ok(invoices);
        }


        [HttpPost]
        public async Task<IActionResult> CreateInvoice(CreateInvoiceDto createInvoiceDto)
        {
            var lastInvoice = await _context.Invoices.OrderByDescending(i => i.Id).FirstOrDefaultAsync();
            var reset = false;
            var newInvoiceId = lastInvoice!.InvoiceId + 1;
            if (lastInvoice.InvoiceId % 1000 >= 900)
            {
                reset = true;
                newInvoiceId = lastInvoice!.InvoiceId - (lastInvoice.InvoiceId % 1000) + 1000;
            }

            var invoice = new InvoiceModel
            {
                CustomerName = createInvoiceDto.CustomerName,
                InvoiceId = newInvoiceId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.AddAsync(invoice);
            await _context.SaveChangesAsync();
            if (reset) {
                return CreatedAtAction("CreateInvoice", new { Message = "Created", Detail = "invoice count has been reset" });    
            }
            return CreatedAtAction("CreateInvoice", new { Message = "Created" });
        }


        [HttpGet("reset")]
        public async Task<IActionResult> ResetInvoiceId()
        {
            var lastInvoice = await _context.Invoices.OrderByDescending(i => i.Id).FirstOrDefaultAsync();
            var invoice = new InvoiceModel
            {
                CustomerName = "--reset--",
                InvoiceId = lastInvoice!.InvoiceId - (lastInvoice.InvoiceId % 1000) + 1000,
                CreatedAt = DateTime.UtcNow
            };
            await _context.AddAsync(invoice);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Invoice Count Reset" });
        }

    }
}