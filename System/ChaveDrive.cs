using Google.Apis.Services;
namespace System.ChaveDrive;
using Google.Apis.Drive.v3;


class ChaveDrive
{
    private string APIKey = "AIzaSyBsiOtGiDN2obfNJ1un7pzgG3u1Br0rPRk";
    private string ApplicationName = "PersonalCloud";

    private DriveService Service;

    public ChaveDrive()
    {
        var service = new DriveService(new BaseClientService.Initializer
        {
            ApplicationName = ApplicationName,
            ApiKey=APIKey,
        });
        Service = service;
    }
    
   public void ListFiles()
    {
        var request = Service.Files.List();
        var result = request.Execute();

        Console.WriteLine("Files in Google Drive:");
        foreach (var file in result.Files)
        {
            Console.WriteLine($"- {file.Name} ({file.Id})\n");
        }
    }

    public void SetPermission()
    {

    }

    public string GetJpgFileId(string fileName)
    {
        // Define o tipo MIME para arquivos JPEG
        const string jpegMimeType = "image/jpg";

        // Define os campos que você deseja recuperar
        string fields = "files(id, name, mimeType)";

        try
        {
            // Executa a consulta para listar todos os arquivos no Google Drive
            var request = Service.Files.List();
            request.Fields = fields;
            var fileList = request.Execute().Files;

            // Procura o arquivo JPEG específico
            foreach (var file in fileList)
            {
                if (file.MimeType == jpegMimeType && file.Name == fileName)
                {
                    // Retorna o ID do arquivo JPEG encontrado
                    Console.WriteLine($"- {file.Name} ({file.Id})\n");
                    return file.Id;
                }
            }

            // Se o arquivo não for encontrado, retorna null ou lança uma exceção, conforme necessário
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ao buscar o arquivo: {ex.Message}");
            return null;
        }
    }
    public string GetFolderId( string folderName)
    {
        var request = Service.Files.List();
        request.Q = $"mimeType='application/vnd.google-apps.folder' and name='{folderName}'";
        var folder = request.Execute().Files.FirstOrDefault()!;

        return folder.Id!;
    }
    
}