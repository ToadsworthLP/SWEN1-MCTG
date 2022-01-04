using MCTG.Auth;
using MCTG.Models;
using MCTG.Services;
using Moq;
using NUnit.Framework;
using Rest;

namespace MCTG.Test
{
    internal class AuthProviderTests
    {
        [Test]
        [TestCase(Role.USER, true)]
        [TestCase(Role.ADMIN, true)]
        public void TestUserRestricted(Role userRole, bool shouldWork)
        {
            Mock<ITokenService> mock = new Mock<ITokenService>();
            mock.Setup(x => x.ReadToken("")).Returns(new User("Test", new byte[1], userRole));
            ITokenService mockTokenService = mock.Object;

            IAuthProvider authProvider = new AuthProvider(mockTokenService);

            bool result = authProvider.IsAuthorized(Role.USER, "");

            Assert.AreEqual(shouldWork, result);
        }

        [Test]
        [TestCase(Role.USER, false)]
        [TestCase(Role.ADMIN, true)]
        public void TestAdminRestricted(Role userRole, bool shouldWork)
        {
            Mock<ITokenService> mock = new Mock<ITokenService>();
            mock.Setup(x => x.ReadToken("")).Returns(new User("Admin", new byte[1], userRole));
            ITokenService mockTokenService = mock.Object;

            IAuthProvider authProvider = new AuthProvider(mockTokenService);

            bool result = authProvider.IsAuthorized(Role.ADMIN, "");

            Assert.AreEqual(shouldWork, result);
        }
    }
}
