using Shouldly;
using System;
using System.Linq;
using Xunit;
using YoutubeDownloader.Domain.Access;
using YoutubeDownloader.Domain.Exceptions;

namespace YoutubeDownloader.Domain.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_WhenCreatingNewUserWithCorrectData_ThenUserIsCreated()
        {
            // given
            string username = "Joe";
            string userId = Guid.NewGuid().ToString();
            // when
            var user = new User(userId, username);

            // then
            user.ShouldNotBeNull();
            user.Name.ShouldBe(username);
            user.Id.ShouldBe(userId);
        }

        [Fact]
        public void User_WhenCreatingNewUserWithIncorrectId_ThenExceptionIsThrown()
        {
            // given
            string emptyGuidId = Guid.Empty.ToString();
            string nullId = null;
            string whitespaceId = " ";
            string emptyId = "";

            string correctUsername = "Joe";

            // when then
            Should.Throw<ArgumentException>(() => new User(emptyGuidId, correctUsername));
            Should.Throw<ArgumentException>(() => new User(nullId, correctUsername));
            Should.Throw<ArgumentException>(() => new User(whitespaceId, correctUsername));
            Should.Throw<ArgumentException>(() => new User(emptyId, correctUsername));
        }

        [Fact]
        public void User_WhenCreatingNewUserWithIncorrectUsername_ThenExceptionIsThrown()
        {
            // given
            string nullUsername = null;
            string whitespaceUsername = " ";
            string emptyUsername = "";
            string tooLongUsername = string.Join("", Enumerable.Repeat(0, 101).Select(n => (char)new Random().Next(127)));

            string correctId = Guid.NewGuid().ToString();

            // when then
            Should.Throw<ArgumentException>(() => new User(correctId, nullUsername));
            Should.Throw<ArgumentException>(() => new User(correctId, whitespaceUsername));
            Should.Throw<ArgumentException>(() => new User(correctId, emptyUsername));
            Should.Throw<DomainException>(() => new User(correctId, tooLongUsername), "Too long uername");
        }
    }
}
