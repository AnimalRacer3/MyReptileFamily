namespace MyReptileFamilyAPI.AppSettings;

public class OwnerSettings
{
    public Range PasswordRange { get; set; } = 5..32;
    public Range UsernameRange { get; set; } = 3..16;
}