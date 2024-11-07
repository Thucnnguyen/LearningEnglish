namespace Application.IService;

public interface IAssemblyAIService
{
    public Task<string> VideoToText(string videoUrl);
}