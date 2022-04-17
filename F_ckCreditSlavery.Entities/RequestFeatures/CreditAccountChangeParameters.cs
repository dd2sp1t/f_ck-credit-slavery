using F_ckCreditSlavery.Entities.Attributes.Swagger;

namespace F_ckCreditSlavery.Entities.RequestFeatures;

public class CreditAccountChangeParameters : RequestParameters
{
    public DateTime MinDate { get; set; } = DateTime.MinValue;
    public DateTime MaxDate { get; set; } = DateTime.MaxValue;

    [SwaggerIgnore]
    public bool IsValidDataRange => MaxDate >= MinDate;

    public CreditAccountChangeParameters()
    {
        OrderBy = "PaymentDate desc";
    }
}