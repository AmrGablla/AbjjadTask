using Microsoft.AspNetCore.Http;

namespace WepAPI.DTOs
{
    public class ImageDTO
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }
    }
}
