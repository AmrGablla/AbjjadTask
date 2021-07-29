using System.Collections.Generic;
using System.Threading.Tasks;
using WepAPI.Models;

namespace WepAPI.Interfaces
{
    public interface IUserService
    {
        ApplicationUser Create(ApplicationUser user); 
        ApplicationUser Get(string id); 
        void Remove(string id); 
        ApplicationUser FindByNameAsync(string userName);
    }
}