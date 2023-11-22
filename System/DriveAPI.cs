namespace googleDriveAPI.System;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using static Google.Apis.Drive.v3.DriveService;

class DriveAPI 
{
    private const string ClientId = "822523814003-rgov1ecq1oj41gjmh24fni1rmkf3ei30.apps.googleusercontent.com";
    private const string ClientSecret = "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";
    private const string RedirectUri = "http://localhost:3000";
    private const string ApplicationName = "PersonalCloud";
    private const string UserName ="firmanoster@gmail.com";
    private const string CredentialsPath = "C:/Users/lino/Projetos_Programação/google_drive_api/assats/client_secret.json";
    private const string TokenPath = "C:/Users/lino/Projetos_Programação/google_drive_api/assats/token.json";

    private string? AccessToken;
    private string? RefreshToken;



public DriveService GetService()
{
    GetCred();

    var tokenResponse = new TokenResponse
    {
        AccessToken = AccessToken,
        RefreshToken = RefreshToken,
    };


    
    var applicationName = ApplicationName; // Use the name of the project in Google Cloud
    var username = UserName; // Use your email
    
    var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
    {
        ClientSecrets = new ClientSecrets
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret
        },
        Scopes = new[] { Scope.Drive },
        DataStore = new FileDataStore(applicationName)
    });

    
    var credential = new UserCredential(apiCodeFlow, username, tokenResponse);
    
    var service = new DriveService(new BaseClientService.Initializer
    {
        HttpClientInitializer = credential,
        ApplicationName = applicationName
    });
    return service;

}



private async void  GetCred()
{
    using (var stream = new FileStream(CredentialsPath, FileMode.Open, FileAccess.Read))
    {
        var credential =  await GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            new[] { Scope.Drive },
            UserName,
            CancellationToken.None,
            new FileDataStore(TokenPath, true));
        AccessToken = credential.Token.AccessToken;
        RefreshToken = credential.Token.RefreshToken;
    }

}


public void GetFiles()
{
    var service = GetService();
    var request = service.Files.List();
    var result = request.Execute();

    foreach (var file in result.Files)
    {
        Console.WriteLine( file.Name, file.Id);
    }

}

private async Task<UserCredential> GetCredentialAsyncTwo()
{
    using (var stream = new FileStream(CredentialsPath, FileMode.Open, FileAccess.Read))
    {
        var credential =  await GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            new[] { DriveService.Scope.Drive },
            UserName,
            CancellationToken.None,
            new FileDataStore("DriveAPI"));
        return credential;
    }
}

public string CreateFolder(string parent, string folderName)
{
    var service = GetService();
    var driveFolder = new Google.Apis.Drive.v3.Data.File();
    driveFolder.Name = folderName;
    driveFolder.MimeType = "application/vnd.google-apps.folder";
    driveFolder.Parents = new string[] { parent };
    var command = service.Files.Create(driveFolder);
    var file = command.Execute();
    return file.Id;
 }
}