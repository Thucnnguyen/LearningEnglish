using Application.IService;
using AssemblyAI;
using AssemblyAI.Transcripts;
using Infrastructure.Settings;

namespace Infrastructure.Service;

public class AssemblyAIService : IAssemblyAIService
{
    private readonly AssemblyAISetting _assemblyAISetting;

    public AssemblyAIService(AssemblyAISetting assemblyAiSetting)
    {
        _assemblyAISetting = assemblyAiSetting;
    }

    public async Task<string> VideoToText(string videoUrl)
    {
        if(string.IsNullOrEmpty(videoUrl)) return string.Empty;
        //setup params
        var client = new AssemblyAIClient(_assemblyAISetting.APIKey);
        var TranscriptParams = new TranscriptParams()
        {
            AudioUrl = videoUrl,
        };
        //call
        var transcript = await client.Transcripts.TranscribeAsync(TranscriptParams);
        transcript.EnsureStatusCompleted();
        return transcript.Text;
    }
}