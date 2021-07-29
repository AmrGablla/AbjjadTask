using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WepAPI.Interfaces;
using WepAPI.Models;

namespace WepAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<ApplicationUser> _users;

        public UserService(IPostsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<ApplicationUser>(settings.UsersCollectionName);
        }
         

        public ApplicationUser Get(string userName) =>
              _users.Find<ApplicationUser>(user => user.UserName == userName).FirstOrDefault();

        public ApplicationUser Create(ApplicationUser user)
        {
            _users.InsertOne(user);
            return user;
        }
          
        public void Remove(string userName) =>
            _users.DeleteOne(user => user.UserName == userName);

        public ApplicationUser FindByNameAsync(string userName)
        {
            return _users.Find<ApplicationUser>(user => user.UserName == userName).FirstOrDefault();
        }
    }
}

