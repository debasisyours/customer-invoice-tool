namespace CustomerInvoice.Data.Models
{
    public class CustomerFeeModel
    {
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string SageReference { get; set; }
        public int ChargeHeadId { get; set; }
        public string ChargeHeadName { get; set; }
    }
}
