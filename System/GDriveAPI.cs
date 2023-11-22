using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using static Google.Apis.Drive.v3.DriveService;

namespace google_Drive_api_System.System;

class GDriveApi
{
    private const string ClientId = "822523814003-rgov1ecq1oj41gjmh24fni1rmkf3ei30.apps.googleusercontent.com";
    private const string ClientSecret = "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";
            
    private const string RedirectUri = "http://localhost:3000";
    private const string ApplicationName = "PersonalCloud";
    private const string UserName ="firmanoster@gmail.com";
    private const string CredentialsPath = "C:/Users/lino/Projetos_Programação/google_drive_api/assats/client_secret.json";
    private const string TokenPath = "C:/Users/lino/Projetos_Programação/google_drive_api/assats/token.json";
    // private DriveService Service;


public DriveService  GetService()
{
    var credential = GetCredential();
    var driveService = new DriveService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName
    });
    return driveService;
}

private UserCredential GetCredential()
{
    // using (var stream = new FileStream("C:/Users/lino/Projetos_Programação/google_drive_api/assats/client_secret.json", FileMode.Open, FileAccess.Read))
    {
        var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        new ClientSecrets
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret
        },
        new[] { DriveService.Scope.Drive },
        "user",
        CancellationToken.None,
        new FileDataStore("DriveAPI")).Result;
        return credential;
    }
}

public async Task<DriveService> GetServiceAsync()
{
    var credential = await GetCredentialAsyncTwo();
    var token = credential.Token.AccessToken;
    var refreshToken = credential.Token.RefreshToken;

    var driveService = new DriveService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName
    });
    // Service = driveService;
    return driveService;
}

// private async Task<GoogleCredential> GetCredentialAsync()
// {
        
//         var credential = await GoogleCredential.FromFileAsync(CredentialsPath, CancellationToken.None);
//         return credential;
        
// }

 private async Task<UserCredential> GetCredentialAsyncTwo()
    {
        using (var stream = new FileStream(CredentialsPath, FileMode.Open, FileAccess.Read))
        {
            var credential =  await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                new[] { DriveService.Scope.Drive },
                UserName,
                CancellationToken.None,
                new FileDataStore(TokenPath, true));
            return credential;
        }
    }

    public void ListFiles()
    {
        DriveService driveService = GetService();
        var request = driveService.Files.List();
        var result = request.Execute();

        Console.WriteLine("Files in Google Drive:");
        foreach (var file in result.Files)
        {
            Console.WriteLine($"{file.Name} ({file.Id})");
        }
    }

    public async void ListFilesAsync()
    {
        Console.WriteLine("teste método");
        DriveService driveService = await GetServiceAsync();
        var request = driveService.Files.List();
        var result = request.Execute();
        Console.WriteLine(result);
        Print(result);
        
    }    

    public void Print(FileList list)
    {
        Console.WriteLine("Files in Google Drive:");
        foreach (var file in list.Files)
        {
            Console.WriteLine("arquivo");
            Console.WriteLine($"{file.Name} ({file.Id})");
        }
    }

    public async void GetFolderId( string folderName)
    {
        DriveService service = await  GetServiceAsync();
        var request = service.Files.List();
        request.Q = $"mimeType='application/vnd.google-apps.folder' and name='{folderName}'";
        var folder = request.Execute().Files.FirstOrDefault()!;
        Console.WriteLine(folder.Name);
        // return folder.Id!;
    }

    // public async void Auth()
    // {
        
    //     DriveService service = await GetServiceAsync();

    //     // Obtém o URL de autorização
    //     string authUrl = GoogleWebAuthorizationBroker.AuthorizeAsync(
    //         service,
    //         Scope.Drive,
    //         "user",
    //         CancellationToken.None,
    //         new FileDataStore(TokenPath, true)).Result;

    //     Console.WriteLine("Por favor, visite esta URL para autorizar a aplicação: ");
    //     Console.WriteLine(authUrl);
    //     Console.Write("Digite o código de autorização: ");

    //     // Aguarde o usuário inserir o código de autorização
    //     string authCode = Console.ReadLine();

    //     // Troque o código de autorização por tokens de acesso
    //     var token = GoogleWebAuthorizationBroker.AuthorizeAsync(
    //         service,
    //         Scope.Drive,
    //         "user",
    //         CancellationToken.None,
    //         new FileDataStore(TokenPath, true),
    //         new Google.Apis.Auth.OAuth2.Responses.CodeReceiver()).Result;
    // }

}