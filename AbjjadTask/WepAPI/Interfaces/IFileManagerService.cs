using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WepAPI.DTOs;

namespace WepAPI.Interfaces
{
    public interface IFileManagerService
    {
        Task Upload(ImageDTO model);
        Task<byte[]> Get(string imageName);
    }
}
