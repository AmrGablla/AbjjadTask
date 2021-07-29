using System.Collections.Generic;
using WepAPI.Models;

namespace WepAPI.Interfaces
{
    public interface IPostService
    {
        Post Create(PostCreateDTO post);
        List<Post> Get();
        Post Get(string id);
        void Remove(string id);
    }
}