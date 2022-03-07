using System;
using System.Collections.Generic;
using YoutubeDownloader.Domain.Access;
using YoutubeDownloader.Domain.History;
using YoutubeDownloader.Infrastructure.Database.Contexts;

namespace YoutubeDownloader.Integration.Tests.Data
{
    public static class DataHelper
    {
        public static void PopulateTestData(YoutubeDownloaderContext dbContext)
        {
            dbContext.Users.AddRange(GetTestUsers());
            dbContext.SaveChanges();
        }


        public static List<User> GetTestUsers()
        {
            return new List<User>
           {
               new User(Guid.NewGuid().ToString(), "Joe"),
               new User(Guid.NewGuid().ToString(), "Andrew"),
               new User(Guid.NewGuid().ToString(), "Kamil"),
           };
        }
    }
}
