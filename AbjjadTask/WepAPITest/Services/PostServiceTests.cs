using Xunit;
using WepAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Moq;
using WepAPI.Models;
using AutoMapper;
using WepAPI.Mapping;

namespace WepAPI.Services.Tests
{
    public class PostServiceTests
    {
        private Mock<IMongoCollection<Post>> _mockCollection;

        private readonly IMapper _mapper;

        public PostServiceTests()
        {
            _mockCollection = new Mock<IMongoCollection<Post>>();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MapProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact()]
        public void RemoveTest()
        {
            PostsDatabaseSettings postsDatabaseSettings = new PostsDatabaseSettings();
            postsDatabaseSettings.ConnectionString = "mongodb://localhost:27017";
            postsDatabaseSettings.DatabaseName = "abjjad";
            postsDatabaseSettings.PostsCollectionName = "PostsCollectionName";

            var PostService = new PostService(postsDatabaseSettings, _mapper);
            Post post = PostService.Create(new PostCreateDTO() { });
            PostService.Remove(post.Id);
            var testRemoved = PostService.Get(post.Id);

            Assert.Null(testRemoved);
        }

        [Fact]
        public void CheckPostCreated()
        {
            PostsDatabaseSettings postsDatabaseSettings = new PostsDatabaseSettings();
            postsDatabaseSettings.ConnectionString = "mongodb://localhost:27017";
            postsDatabaseSettings.DatabaseName = "abjjad";
            postsDatabaseSettings.PostsCollectionName = "PostsCollectionName";

            var PostService = new PostService(postsDatabaseSettings, _mapper);
            Post post = PostService.Create(new PostCreateDTO() { });
            var postId = PostService.Get(post.Id)?.Id;

            Assert.Equal(postId, post.Id);
        }

        [Fact()]
        public void GetTest()
        {
            PostsDatabaseSettings postsDatabaseSettings = new PostsDatabaseSettings();
            postsDatabaseSettings.ConnectionString = "mongodb://localhost:27017";
            postsDatabaseSettings.DatabaseName = "abjjad";
            postsDatabaseSettings.PostsCollectionName = "PostsCollectionName";

            var PostService = new PostService(postsDatabaseSettings, _mapper);
            Post post = PostService.Create(new PostCreateDTO() { });
            List<Post> posts = PostService.Get();
            Assert.NotNull(posts);
        }
    }
}