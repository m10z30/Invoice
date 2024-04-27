namespace Invoice.Models;

public class InvoiceModel
{
    public int Id { set; get; }
    public string CustomerName { set; get; } = string.Empty;
    public DateTime CreatedAt { set; get; }
    public int InvoiceId { set; get; }
}
