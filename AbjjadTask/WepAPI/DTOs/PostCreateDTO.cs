using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WepAPI.DTOs;

namespace WepAPI.Models
{
    public class PostCreateDTO
    {
        public string Text { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string UserId { get; set; }
        public string ImageURL { get; set; }
        public ImageDTO Image { get; set; }
    }
}
