using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WepAPI.Interfaces;
using WepAPI.Models;

namespace WepAPI.Services
{
    public class PostService : IPostService
    {
        private readonly IMongoCollection<Post> _posts;
        private readonly IMapper _mapper;

        public PostService(IPostsDatabaseSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _posts = database.GetCollection<Post>(settings.PostsCollectionName);
            _mapper = mapper;
        }

        public List<Post> Get() =>
            _posts.Find(post => true).ToList();

        public Post Get(string id) =>
            _posts.Find<Post>(post => post.Id == id).FirstOrDefault();

        public Post Create(PostCreateDTO postDto)
        {
            Post post = _mapper.Map<Post>(postDto);
            _posts.InsertOne(post);
            return post;
        }
          
        public void Remove(string id) =>
            _posts.DeleteOne(post => post.Id == id);
    }
}

