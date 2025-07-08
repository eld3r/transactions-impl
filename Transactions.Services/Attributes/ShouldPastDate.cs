using System.ComponentModel.DataAnnotations;

namespace Transactions.Services.Attributes;

public class ShouldPastDate() : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime dateTime)
        {
            return dateTime <= DateTime.Now;
        }

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be in past time";
    }
}