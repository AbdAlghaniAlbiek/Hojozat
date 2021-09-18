using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace HojozatServer.Repositories.RepoOperations
{
    public interface ISettingsRepoOps
    {
        [Get("/getServerUser.php")]
        Task<Models.ServerUser> GetServerUserOpAsync([AliasAs("ServerUser_Id")] int serverUserId, [Header("Authorization")] string authorization);
    }
}
