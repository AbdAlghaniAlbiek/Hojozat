using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HojozatServer.Repositories.RepoOperations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace HojozatServer.Repositories.RemoteRepo
{
    public static class LoginRepo
    {

        public static async Task<bool> RegisterServerUserAsync(Models.ServerUser serverUser)
        {
            //Make object from the specified repoOps to determine the type of action that you need
            var registerOperation = RestService.For<ILoginRepoOps>(Common.URL);

            //make post action to server by sending serialized serverUser object
            bool response = 
                await registerOperation.RegisterServerUserOpAsync(serverUser);

            if (response)
            {
                //the process is successfull and the server user data stor it database
                return true;
            }
            else
            {
                //process failed because someone has the same account of server user in database
                return false;
            }
        }

        public static async Task<bool> VerifyAccountAsync(string code)
        {
            //Make object from the specified repoOps to call VerifyAccount method
            var verifyOperation = RestService.For<ILoginRepoOps>(Common.URL);

            //make post action to server by sending code to verify the account of server user
            var response =
                await verifyOperation.VerifyAccountOpAsync(code);

            //resopnse has serverUserId object
            if (response.Token != null)
            {
                Common.serverUserId = response.Id;
                Common.Token = response.Token;
                return true;
            }

            //resopnse doesn't has anything so the code that user enter it isn't correct 
            return false;
        }

        public static async Task<bool> LoginServerUserAsync(Models.ServerUser serverUser)
        {
            //Make object from the specified repoOps to call VerifyAccount method
            var loginOperation = RestService.For<ILoginRepoOps>(Common.URL);

            //make post action to server by sending code to verify the account of server user
            var response =
                await loginOperation.LoginServerUserOpAsync(serverUser);

            //resopnse has serverUserId object
            if (response != null)
            {
                Common.serverUserId = response.Id;
                Common.Token = "Bearer " + response.Token;
                return true;
            }

            //resopnse doesn't has anything so one or both Email or password aren't correct 
            return false;
        }
    }
}
