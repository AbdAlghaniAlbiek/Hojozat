using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace HojozatServer.Repositories.RepoOperations
{
    public interface IFeedbackRepoOps
    {
        [Get("/getUsersFeedback.php")]
        Task<ObservableCollection<Models.Feedback>> GetUsersFeedbackOpAsync([Header("Authorization")] string authorization);

        [Get("/getBrandsFeedback.php")]
        Task<ObservableCollection<Models.Feedback>> GetBrandsFeedbackOpAsync([Header("Authorization")] string authorization);
    }
}
