namespace Infrastructure.Settings;

public class AssemblyAISetting
{
    public static readonly string ConfigSectionName = "AssemblyAI";
    
    public required string APIKey { get; set; }
}