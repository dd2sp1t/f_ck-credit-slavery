using F_ckCreditSlavery.Entities.Enums;

namespace F_ckCreditSlavery.Entities.DataTransferObjects;

public class CreditAccountChangeGetDto
{
    public int Id { get; set; }
    public int CreditAccountId { get; set; }
    public decimal PaymentValue { get; set; }
    public PaymentType PaymentType { get; set; }
    public DateTime PaymentDate { get; set; }
}