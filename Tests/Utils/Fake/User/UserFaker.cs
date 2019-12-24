using System;
using App.DL.Repository.User;
using Micron.DL.Module.Misc;
using UserModel = App.DL.Model.User.User;

namespace Tests.Utils.Fake.User {
    public static class UserFaker {
        public static UserModel Create() {
            var rand = new Random();
            var email = Rand.RandomString() + "@mail.com";
            var id = UserModel.Create(
                email,
                "test-login" + Rand.RandomString(),
                Guid.NewGuid().ToString()
            );
            return UserRepository.Find(id);
        }
    }
}