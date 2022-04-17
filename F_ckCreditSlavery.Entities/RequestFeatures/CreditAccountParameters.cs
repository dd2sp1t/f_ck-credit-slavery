using F_ckCreditSlavery.Entities.Attributes.Swagger;

namespace F_ckCreditSlavery.Entities.RequestFeatures;

public class CreditAccountParameters : RequestParameters
{
    public CreditAccountParameters()
    {
        OrderBy = "startDate";
    }
    
    public DateTime MinStartDate { get; set; } = DateTime.MinValue;
    public DateTime MaxStartDate { get; set; } = DateTime.MaxValue;
    public DateTime MinEndDate { get; set; } = DateTime.MinValue;
    public DateTime MaxEndDate { get; set; } = DateTime.MaxValue;
    
    [SwaggerIgnore]
    public bool IsValidStartDataRange => MaxStartDate >= MinStartDate;
    [SwaggerIgnore]
    public bool IsValidEndDataRange => MaxEndDate >= MinEndDate;
}