namespace MyReptileFamilyAPI.CORE.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class StringValueAttribute(string value) : Attribute
{
    public string StringValue { get; } = value;
}