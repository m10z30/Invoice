using Invoice.Data;
using Invoice.Dtos;
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
        public async Task<IActionResult> GetAllInvoices(GetAllInvoicesQuery query)
        {
            var invoices = await _context.Invoices
                        .Skip(query.Offset)
                        .Take(query.Limit)
                        .ToListAsync();
            
            return Ok(invoices);
        }


        [HttpPost]
        public async Task<IActionResult> CreateInvoice(CreateInvoiceDto createInvoiceDto)
        {
            
        }

    }
}