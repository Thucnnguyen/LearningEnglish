using Application.Common.Models.Request;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Audio;

public class CreateAudioCommand : IRequest<AudioResponse>
{
    public required string Title { get; set; }
    public IFormFile AudioFile { get; set; }
}