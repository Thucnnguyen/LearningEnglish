using Application.Common.Models.Request;
using Application.Infrastructure.IRepository;

using Google.Apis.Util.Store;
using MediatR;

namespace Application.Features.Audio.Handler;

public class CreateAudioHandler : IRequestHandler<CreateAudioCommand, AudioResponse>
{
    private readonly IUnitOfWork _unitOfWork;


    public Task<AudioResponse> Handle(CreateAudioCommand request, CancellationToken cancellationToken)
    {
        

        throw new NotImplementedException();
    }
}