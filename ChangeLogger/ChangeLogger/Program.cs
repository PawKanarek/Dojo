using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bitbucket.Cloud.Net.Models.v2;

namespace ChangeLogger
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var confluenceClient = new ConfluenceClient();
            var bitbucketClient = new BitbucketClient();

            var allMergedPRTask = bitbucketClient.GetMergedPullRequestAsync();
            var pageUrlTask = confluenceClient.GetChangeLogUrlPageAsync();
            await Task.WhenAll(allMergedPRTask, pageUrlTask).ConfigureAwait(false);
            
            var changeLogs = GetChangeLogs(allMergedPRTask.Result);

            await confluenceClient.UpdateChangeLog(changeLogs, pageUrlTask.Result);

            Console.WriteLine("terminated");
        }

        private static List<ChangeLogModel> GetChangeLogs(IEnumerable<PullRequest> allMergedPR)
        {
            var jiraTaskRx = new Regex(@"[a-zA-Z]{2,8}-\d{1,6}");
            var changeLogs = new List<ChangeLogModel>(10);
            foreach (var pr in allMergedPR)
            {
                string jiraTask = null;
                var match = jiraTaskRx.Match(pr.Source.Branch.Name);
                if (match.Success)
                {
                    jiraTask = match.Value;
                }
                else
                {
                    match = jiraTaskRx.Match(pr.Title);
                    if (match.Success)
                    {
                        jiraTask = match.Value;
                    }
                }

                if (jiraTask != null)
                {
                    changeLogs.Add(new ChangeLogModel()
                    {
                        JiraTask = jiraTask,
                        Description = pr.Title
                    });
                }
            }

            return changeLogs;
        }
    }
}