using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Domain.Exceptions;

namespace YoutubeDownloader.Domain.Access
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public User()
        {
            // EF Constructor
        }
        public User(string id, string name)
        {
            EnsureArg.IsNotNullOrWhiteSpace(id);
            EnsureArg.IsNotNullOrWhiteSpace(name);
            EnsureArg.IsNotEqualTo(id, Guid.Empty.ToString());

            if(name.Length > 100)
            {
                throw new DomainException("Too long uername");
            }

            Id = id;
            Name = name;
        }
    }
}
