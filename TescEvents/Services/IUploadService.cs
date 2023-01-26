namespace TescEvents.Services; 

public interface IUploadService {
    string UploadFileToPath(string path, IFormFile file);
    void DeleteFileAtPath(string path);
}