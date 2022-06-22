using System;

namespace CustomerInvoice.Data.Models
{
    public class InvoiceExportModel
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ContactName { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitAmount { get; set; }
        public string AccountCode { get; set; }
        public string TaxType { get; set; }
    }
}
