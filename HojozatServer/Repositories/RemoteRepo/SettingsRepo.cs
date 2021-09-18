using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HojozatServer.Models;
using HojozatServer.Repositories.RepoOperations;
using HojozatServer.Services.Security;
using Refit;

namespace HojozatServer.Repositories.RemoteRepo
{
    class SettingsRepo
    {
        public async static Task<Models.ServerUser> GetServerUserAsync()
        {
            var settingsAPIs =
                RestService.For<ISettingsRepoOps>(Common.URL);

            ServerUser serverUser =
                await settingsAPIs.GetServerUserOpAsync(Common.serverUserId, Common.Token);

            if (serverUser != null)
            {
                return new ServerUser()
                {
                    Email = AESCryptography.Decrypt(serverUser.Email),
                    Password = AESCryptography.Decrypt(serverUser.Password),
                    Name = serverUser.Name,
                    PhoneNumber = serverUser.PhoneNumber,
                    DateJoin = serverUser.DateJoin,
                    Verified = serverUser.Verified
                };

            }

            return null;
        }
    }
}
