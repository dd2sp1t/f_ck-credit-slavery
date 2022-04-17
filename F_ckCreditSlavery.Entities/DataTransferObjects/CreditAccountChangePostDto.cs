using System.ComponentModel.DataAnnotations;
using F_ckCreditSlavery.Entities.Attributes.Validation;
using F_ckCreditSlavery.Entities.Enums;

namespace F_ckCreditSlavery.Entities.DataTransferObjects;

public class CreditAccountChangePostDto
{
    [Range(0.01, double.MaxValue, ErrorMessage = $"{nameof(PaymentValue)} is required and it can't be lower than 0.01")]
    public double PaymentValue { get; set; }
    [PaymentTypeValidateIncoming(typeof(PaymentType))]
    [Required(ErrorMessage = $"{nameof(PaymentType)} is a required field")]
    public PaymentType PaymentType { get; set; }
    [Required(ErrorMessage = $"{nameof(PaymentDate)} is a required field")]
    public DateTime PaymentDate { get; set; }
}