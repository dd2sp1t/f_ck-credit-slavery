using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using F_ckCreditSlavery.Entities.Enums;

namespace F_ckCreditSlavery.Entities.Models;

[Table(nameof(CreditAccountChange))]
public class CreditAccountChange
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(CreditAccount))]
    public int CreditAccountId { get; set; }
        
    [Required(ErrorMessage = $"{nameof(PaymentValue)} is a required field.")]
    [Range(0.01, double.MaxValue, ErrorMessage = $"{nameof(PaymentValue)} can't be lower than 0.01")]
    public double PaymentValue { get; set; }
        
    [Required(ErrorMessage = $"{nameof(PaymentType)} is a required field.")]
    public PaymentType PaymentType { get; set; }
        
    [Required(ErrorMessage = $"{nameof(PaymentDate)} is a required field.")]
    public DateTime PaymentDate { get; set; }
        
    public virtual CreditAccount CreditAccount { get; set; }
}