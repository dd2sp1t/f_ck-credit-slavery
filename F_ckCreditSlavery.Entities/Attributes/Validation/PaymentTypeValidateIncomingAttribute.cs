using F_ckCreditSlavery.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace F_ckCreditSlavery.Entities.Attributes.Validation;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public class PaymentTypeValidateIncomingAttribute : ValidationAttribute
{
    private const string CannotBeNullErrorMessage = "Type cannot be null";
    private const string MustBeAnEnumErrorMessage = "Type must be an enum";
    private const string EnumValueIsNotDefinedErrorMessage = "Enum value is not defined";
    private const string PaymentMustBeIncomingErrorMessage = "Payment must be incocimg";
    
    public Type EnumType { get; }
    
    public PaymentTypeValidateIncomingAttribute(Type enumType) : base("Enumeration")
    {
        EnumType = enumType;
    }
    
    public override bool IsValid(object value)
    {
        if (EnumType == null)
        {
            ErrorMessage = CannotBeNullErrorMessage;
            return false;
        }
        if (!EnumType.IsEnum)
        {
            ErrorMessage = MustBeAnEnumErrorMessage;
            return false;
        }
        if (!Enum.IsDefined(EnumType, value))
        {
            ErrorMessage = EnumValueIsNotDefinedErrorMessage;
            return false;
        }
        if ((PaymentType) value != PaymentType.Incoming)
        {
            ErrorMessage = PaymentMustBeIncomingErrorMessage;
            return false;
        }

        return true;
    }
}