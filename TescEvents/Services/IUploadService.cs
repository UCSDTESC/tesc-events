namespace TescEvents.Services; 

public interface IUploadService {
    void UploadFileToPath(IFormFile file, string path);
}