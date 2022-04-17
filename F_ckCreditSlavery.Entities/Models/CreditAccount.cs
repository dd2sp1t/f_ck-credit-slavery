using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F_ckCreditSlavery.Entities.Models;

[Table(nameof(CreditAccount))]
public class CreditAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
        
    [Required(ErrorMessage = $"{nameof(Name)} is a required field")]
    [MaxLength(100, ErrorMessage = "Maximum length for the Name is 100 characters")]
    public string Name { get; set; }
        
    [Required(ErrorMessage = $"{nameof(InitialBalance)} is a required field")]
    [Range(0.01, double.MaxValue, ErrorMessage = $"{nameof(InitialBalance)} can't be lower than 0.01")]
    public double InitialBalance { get; set; }
        
    [Required(ErrorMessage = $"{nameof(CurrentDebtBalance)} is a required field")]
    [Range(0.01, double.MaxValue, ErrorMessage = $"{nameof(CurrentDebtBalance)} can't be lower than 0.01")]
    public double CurrentDebtBalance { get; set; }

    [Required(ErrorMessage = $"{nameof(MonthlyPaymentValue)} is a required field")]
    [Range(0.01, double.MaxValue, ErrorMessage = $"{nameof(MonthlyPaymentValue)} can't be lower than 0.01")]
    public double MonthlyPaymentValue { get; set; }
        
    [Required(ErrorMessage = $"{nameof(MonthlyPaymentDate)} is a required field")]
    public DateTime MonthlyPaymentDate { get; set; }
        
    [Required(ErrorMessage = $"{nameof(StartDate)} is a required field")]
    public DateTime StartDate { get; set; }
        
    [Required(ErrorMessage = $"{nameof(EndDate)} is a required field")]
    public DateTime EndDate { get; set; }
        
    public virtual ICollection<CreditAccountChange> CreditAccountChanges { get; set; }
}