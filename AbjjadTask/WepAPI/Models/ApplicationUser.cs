using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WepAPI.Models
{
    public class ApplicationUser
    {  
        [BsonId] 
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
