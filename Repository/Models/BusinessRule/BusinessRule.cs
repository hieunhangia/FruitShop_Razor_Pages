using Repository.Constants;

namespace Repository.Models.BusinessRule;

public class BusinessRule
{
    public BusinessRuleConstantType Type { get; set; }
    public required string Value { get; set; }
    public string? Description { get; set; }
}
