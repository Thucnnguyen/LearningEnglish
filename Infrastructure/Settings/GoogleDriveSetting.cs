namespace Infrastructure.Settings;

public class GoogleDriveSetting
{
    public static readonly string ConfigSectionName = "GoogleDriveSetting";

    public required  string ClientId { get; set; }
    public required string ProjectId { get; set; }
    public required string AuthUri { get; set; }
    public required string TokenUri { get; set; }
    public required string AuthProviderUrl { get; set; }
    public required string ClientSecret { get; set; }
    public required string RedirectUris { get; set; }
    public required string ProjectName { get; set; }
}