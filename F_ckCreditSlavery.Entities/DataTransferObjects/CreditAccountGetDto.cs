namespace F_ckCreditSlavery.Entities.DataTransferObjects;

public class CreditAccountGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal InitialBalance { get; set; }
    public decimal CurrentDebtBalance { get; set; }
    public decimal MonthlyPaymentValue { get; set; }
    public DateTime MonthlyPaymentDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}