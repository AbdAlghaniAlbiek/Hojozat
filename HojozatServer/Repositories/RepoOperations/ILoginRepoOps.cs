using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace HojozatServer.Repositories.RepoOperations
{
    public interface ILoginRepoOps
    {
        [Get("/loginServerUser.php")]
        Task<Models.ServerUserId> LoginServerUserOpAsync([Body] Models.ServerUser serverUser);

        [Post("/verifyServerUser.php")]
        Task<Models.ServerUserId> VerifyAccountOpAsync([Body] string code);

        [Post("/registerServerUser.php")]
        Task<bool> RegisterServerUserOpAsync([Body] Models.ServerUser serverUser);
    }
}
