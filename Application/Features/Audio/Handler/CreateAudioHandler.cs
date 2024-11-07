using Application.Common.Models.Request;
using Application.Infrastructure.IRepository;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using MediatR;

namespace Application.Features.Audio.Handler;

public class CreateAudioHandler : IRequestHandler<CreateAudioCommand, AudioResponse>
{
    private readonly IUnitOfWork _unitOfWork;


    public Task<AudioResponse> Handle(CreateAudioCommand request, CancellationToken cancellationToken)
    {
        string[] scopes =
        [
            DriveService.Scope.DriveFile
        ];

        UserCredential userCredential;
        using (var stream = new FileStream(@".\Properties\credentials.json",FileMode.Open,FileAccess.Read))
        {
            string credPath = "token.json";
            userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;

            Console.WriteLine("Credential file saved to: " + credPath);
        }

        throw new NotImplementedException();
    }
}