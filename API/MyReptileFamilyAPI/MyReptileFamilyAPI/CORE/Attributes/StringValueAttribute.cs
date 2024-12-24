namespace MyReptileFamilyAPI.CORE.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class StringValueAttribute(string value) : Attribute
{
    public string StringValue { get; } = value;
}