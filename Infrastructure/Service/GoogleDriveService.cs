using Application.IService;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using File = Google.Apis.Drive.v3.Data.File;

namespace Infrastructure.Service;

public class GoogleDriveService(GoogleDriveSetting googleDriveSetting) : IGoogleDriveService
{
    private readonly GoogleDriveSetting _googleDriveSetting = googleDriveSetting;

    public async Task<string> UploadFile(IFormFile file)
    {
        //set param
        string[] scopes = [DriveService.Scope.DriveFile];

        UserCredential credential;
        var googleClientSecrets = new GoogleClientSecrets()
        {
            Secrets =
            {
                ClientId = _googleDriveSetting.ClientId,
                ClientSecret = _googleDriveSetting.ClientSecret,
            }
        };

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            googleClientSecrets.Secrets,
            scopes,
            "user",
            CancellationToken.None).Result;

        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = _googleDriveSetting.ProjectName
        });

        var fileMetadata = new File()
        {
            Name = file.FileName,
            MimeType = file.ContentType,
        };

        string fileId;
        await using var stream = file.OpenReadStream();
        var request = service.Files.Create(fileMetadata, stream,file.ContentType);
        request.Fields = "Id";
        // upload
        var fileGGdrive = await request.UploadAsync();
        if (fileGGdrive.Status == UploadStatus.Completed)
        {
            return string.Empty;
        }

        var id = request.ResponseBody.Id;
        // public file for everyone
        var permission = new Permission()
        {
            Role = "reader",
            Type = "anyone"
        };

        await service.Permissions.Create(permission,id).ExecuteAsync();
        
        return id;
    }
}