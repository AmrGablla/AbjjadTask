using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WepAPI.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }  
    }
}
