using System.ComponentModel.DataAnnotations;

namespace Invoice.Dtos
{
    public class CreateInvoiceDto
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;
    }
}