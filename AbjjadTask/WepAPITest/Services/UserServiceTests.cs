using Xunit;
using WepAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using WepAPI.Models;
using AutoMapper;
using MongoDB.Driver;
using Moq;
using WepAPI.Mapping;

namespace WepAPI.Services.Tests
{
    public class UserServiceTests
    {
        private Mock<IMongoCollection<ApplicationUser>> _mockCollection;

        private readonly IMapper _mapper;

        public UserServiceTests()
        {
            _mockCollection = new Mock<IMongoCollection<ApplicationUser>>();
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
        public void FindByNameAsyncTest()
        {
            PostsDatabaseSettings UsersDatabaseSettings = new PostsDatabaseSettings();
            UsersDatabaseSettings.ConnectionString = "mongodb://localhost:27017";
            UsersDatabaseSettings.DatabaseName = "abjjad";
            UsersDatabaseSettings.UsersCollectionName = "UsersCollectionName";

            var userService = new UserService(UsersDatabaseSettings);
            ApplicationUser createdUser = new ApplicationUser();
            ApplicationUser testUser;

            testUser = userService.FindByNameAsync("test");
            if (testUser == null)
            {
                createdUser = userService.Create(new ApplicationUser() { UserName = "test" });
                Assert.Equal(createdUser, testUser);
            }
            else
            {
                Assert.Equal("test", testUser.UserName);
            }
        }

        [Fact()]
        public void RemoveUser()
        {
            PostsDatabaseSettings UsersDatabaseSettings = new PostsDatabaseSettings();
            UsersDatabaseSettings.ConnectionString = "mongodb://localhost:27017";
            UsersDatabaseSettings.DatabaseName = "abjjad";
            UsersDatabaseSettings.UsersCollectionName = "UsersCollectionName";

            var userService = new UserService(UsersDatabaseSettings);

            ApplicationUser post = userService.Create(new ApplicationUser() { });
            userService.Remove(post.UserName);
            var testRemoved = userService.Get(post.UserName);

            Assert.Null(testRemoved);
        }
    }
}