
using System.ChaveDrive;
using System.ComponentModel;
using google_Drive_api_System.System;
using googleDriveAPI.System;

Console.WriteLine("Hello, World!");

DriveAPI service = new();

//service.GetService();
service.GetFiles();

// GDriveApi service = new();
// service.GetServiceAsync();
// service.ListFilesAsync();
// service.GetFolderId("Yoga");


// ChaveDrive service = new();

// service.GetJpgFileId("370");

//service.ListFiles();

// string folderID = service.GetFolderId("Yoga");

// Console.WriteLine(folderID);

//189.120.76.2