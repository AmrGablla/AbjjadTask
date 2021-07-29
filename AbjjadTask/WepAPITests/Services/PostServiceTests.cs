using WepAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Moq;
using WepAPI.Models;
using AutoMapper;
using Xunit;
using WepAPI.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WepAPI.Services.Tests
{
    public class PostServiceTests
    {
        private Mock<IMongoCollection<Post>> _mockCollection;
        private Post _post;

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

        [Fact]
        public void CheckPostCreated()
        {
            PostsDatabaseSettings postsDatabaseSettings = new PostsDatabaseSettings();
            postsDatabaseSettings.ConnectionString = "mongodb://localhost:27017";
            postsDatabaseSettings.DatabaseName = "abjjad";
            postsDatabaseSettings.PostsCollectionName = "PostsCollectionName";



            var PostService = new PostService(postsDatabaseSettings, _mapper);

            //Assert 
            Assert.ThrowsException<Exception>(() => PostService.Create(new PostCreateDTO() { }));
        }
    }
}