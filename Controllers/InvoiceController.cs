using Invoice.Data;
using Invoice.Dtos;
using Invoice.Models;
using Invoice.Queries;
using Invoice.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly Numbers _numbers;
        private readonly DataContext _context;

        public InvoiceController(DataContext context, Numbers numbers)
        {
            _context = context;
            _numbers = numbers;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices([FromQuery] GetAllInvoicesQuery query)
        {
            var invoices = await _context.Invoices
                        .OrderByDescending(i => i.Id)
                        .Skip(query.Offset)
                        .Take(query.Limit)
                        .ToListAsync();

            return Ok(invoices);
        }


        [HttpPost]
        public async Task<IActionResult> CreateInvoice(CreateInvoiceDto createInvoiceDto)
        {
            
            var invoice = new InvoiceModel
            {
                CustomerName = createInvoiceDto.CustomerName,
                InvoiceId = await _numbers.GetNumberAsync(),
                CreatedAt = DateTime.UtcNow
            };

            await _context.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return CreatedAtAction("CreateInvoice", new { Message = "Created", Invoice = invoice });
        }


        [HttpGet("reset")]
        public IActionResult ResetInvoiceId()
        {
            _numbers.ResetNumber();

            return Ok(new { Message = "Invoice Count Reset" });
        }

    }
}