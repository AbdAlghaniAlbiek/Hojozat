using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HojozatServer.Repositories.RepoOperations;
using Refit;

namespace HojozatServer.Repositories.RemoteRepo
{
    public class FeedbackRepo
    {
        public static async Task<ObservableCollection<Models.Feedback>> GetUsersFeedbackAsync()
        {
            var feedbackAPIs =
                RestService.For<IFeedbackRepoOps>(Common.URL);

            ObservableCollection<Models.Feedback> usersFeedback =
                await feedbackAPIs.GetUsersFeedbackOpAsync(Common.Token);

            if (usersFeedback != null)
            {
                return usersFeedback;
            }

            return null;
        }

        public static async Task<ObservableCollection<Models.Feedback>> GetBrandsFeedbackAsync()
        {
            var feedbackAPIs =
                RestService.For<IFeedbackRepoOps>(Common.URL);

            ObservableCollection<Models.Feedback> brandsFeedback =
                await feedbackAPIs.GetBrandsFeedbackOpAsync(Common.Token);

            if (brandsFeedback != null)
            {
                return brandsFeedback;
            }

            return null;
        }
    }
}
