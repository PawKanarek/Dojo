using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bitbucket.Cloud.Net;
using Bitbucket.Cloud.Net.Common.Authentication;
using Bitbucket.Cloud.Net.Models.v2;

namespace ChangeLogger
{
    public class BitbucketClient
    {
        private readonly Lazy<BitbucketCloudClient> bitbucketLazy = new Lazy<BitbucketCloudClient>(() =>
        {
            var passwordAuth = new AppPasswordAuthentication("pkanarek-insys", "UVL5xDUgXKTcGrLEHGKG");
            return new BitbucketCloudClient("https://bitbucket.org/api", passwordAuth);
        });
        
        public async Task<IEnumerable<PullRequest>> GetMergedPullRequestAsync()
        {
            try
            {
                return await this.bitbucketLazy.Value.GetRepositoryPullRequestsAsync("insysdev", "insysgo-sdk-mobile", 100, "MERGED").ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Could not get PR {e.Message} {e.StackTrace}");
                return null;
            }
        }

        public Task CreatePullRequestAsync(PullRequestCreationParameters args)
        {
            try
            {
                return this.bitbucketLazy.Value.CreateRepositoryPullRequestAsync("insysdev", "insysgo-sdk-mobile", args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Could not create PR {e.Message} {e.StackTrace}");
                return Task.CompletedTask;
            }
        }
    }
}