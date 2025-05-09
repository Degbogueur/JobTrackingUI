namespace JobTrackingUI.Helpers;

public class FileHelpers(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public string? GetFilePath(string? relativePath)
    {
        var apiUrl = _configuration["WebApiBaseUrl"] ?? string.Empty;
        return relativePath is null 
               ? null
               : $"{apiUrl.TrimEnd('/')}/Uploads/{relativePath.TrimStart('/')}";
    }
}
