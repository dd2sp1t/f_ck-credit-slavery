using System.ComponentModel.DataAnnotations;

namespace F_ckCreditSlavery.Entities.DataTransferObjects;

public class CreditAccountPostDto
{
    [Required(ErrorMessage = $"{nameof(Name)} is a required field")]
    [MaxLength(100, ErrorMessage = $"Maximum length for the {nameof(Name)} is 100 characters")]
    public string Name { get; set; } = null!;
    [Range(0.01, double.MaxValue, ErrorMessage = $"{nameof(InitialBalance)} is required and it can't be lower than 0.01")]
    public double InitialBalance { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = $"{nameof(CurrentDebtBalance)} is required and it can't be lower than 0.01")]
    public double CurrentDebtBalance { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = $"{nameof(MonthlyPaymentValue)} is required and it can't be lower than 0.01")]
    public double MonthlyPaymentValue { get; set; }
    [Required(ErrorMessage = $"{nameof(MonthlyPaymentDate)} is a required field")]
    public DateTime MonthlyPaymentDate { get; set; }
    [Required(ErrorMessage = $"{nameof(StartDate)} is a required field")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = $"{nameof(EndDate)} is a required field")]
    public DateTime EndDate { get; set; }
}