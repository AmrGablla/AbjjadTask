using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WepAPI.Models
{
    public class AuthenticationResponse
    { 
        public string Id { get; set; }
        public string UserName { get; set; }
        public string JWToken { get; set; }
    }
}
