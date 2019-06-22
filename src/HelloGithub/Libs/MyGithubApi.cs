using System;
using System.Linq;
using System.Threading.Tasks;
using HelloGithub.Libs.Extensions;
using Octokit;

namespace HelloGithub.Libs
{
    public class MyGithubApi
    {
        public MyGithubApi(MyGithubConfig config)
        {
            var client = new GitHubClient(new ProductHeaderValue(config.Username));
            if (!string.IsNullOrWhiteSpace(config.PersonalAccessToken))
            {
                client.Credentials = new Credentials(config.PersonalAccessToken);
                //var basicAuth = new Credentials("ps201908", "DDkk1212");
                //client.Credentials = basicAuth;
            }
            Client = client;
        }

        public GitHubClient Client { get; set; }

        public async Task ListRepositories(string username)
        {
            var user = await Client.User.Get(username);
            Console.WriteLine("{0} has {1} public repositories - go check out their profile at {2}",
                user.Name,
                user.PublicRepos,
                user.Url);
        }
        
        public void GetLastApiInfo()
        {
            // Prior to first API call, this will be null, because it only deals with the last call.
            var apiInfo = Client.GetLastApiInfo();

            // If the ApiInfo isn't null, there will be a property called RateLimit
            var rateLimit = apiInfo?.RateLimit;

            var howManyRequestsCanIMakePerHour = rateLimit?.Limit;
            var howManyRequestsDoIHaveLeft = rateLimit?.Remaining;
            var whenDoesTheLimitReset = rateLimit?.Reset; // UTC time
            Console.WriteLine(howManyRequestsCanIMakePerHour);
            Console.WriteLine(howManyRequestsDoIHaveLeft);
            Console.WriteLine(whenDoesTheLimitReset);
        }
        
        public async Task Miscellaneous()
        {
            var miscellaneousRateLimit = await Client.Miscellaneous.GetRateLimits();
            Console.WriteLine(miscellaneousRateLimit.ToJson(true));
        }

        public async Task SyncFile(SyncFileArgs args)
        {
            var message = args.Message;
            var content = args.Content;

            var syncOwner = args.SyncOwner;
            var syncRepo = args.SyncRepo;
            var syncPath = args.SyncPath;

            var repoClient = Client.Repository;
            var myRepo = await repoClient.Get(syncOwner, syncRepo);
            if (myRepo == null)
            {
                throw new InvalidOperationException($"未知仓储: {syncOwner}/{syncRepo}");
            }

            var myRepoId = myRepo.Id;
            var repoClientContent = repoClient.Content;
            RepositoryContent theFile = null;
            try
            {
                var contents = await repoClientContent.GetAllContents(myRepoId, syncPath);
                theFile = contents.SingleOrDefault();
                if (theFile == null)
                {
                    // if file is not found, create it
                    var createChangeSet = await repoClientContent.CreateFile(myRepoId, syncPath, new CreateFileRequest(message, content));
                    Console.WriteLine("Created<{2}>: {0} => {1}", syncPath, content, createChangeSet.Commit.Sha.Substring(0, 4));
                }
                else
                {
                    //file exist, if content changed, update it
                    //theFile.Content

                    var theFileContent = theFile.Content ?? string.Empty;
                    HashHelper.CompareMd5Hash(theFileContent, content);

                    var md5Hash = HashHelper.GetMd5Hash(theFileContent);
                    var md5Hash2 = HashHelper.GetMd5Hash(content);
                    var same = md5Hash.Equals(md5Hash2, StringComparison.OrdinalIgnoreCase);


                    Console.WriteLine(md5Hash);
                    Console.WriteLine(md5Hash2);

                    if (!same)
                    {
                        var updateChangeSet = await repoClientContent.UpdateFile(myRepoId, syncPath, new UpdateFileRequest(message, content, theFile.Sha));
                        Console.WriteLine("Updated<{2}>: {0} => {1}", syncPath, content, updateChangeSet.Commit.Sha.Substring(0, 4));
                    }
                    else
                    {
                        Console.WriteLine("no change!");
                    }
                }
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                //file not exist!
                var createChangeSet = await repoClientContent.CreateFile(myRepoId, syncPath, new CreateFileRequest(message, content));
                Console.WriteLine("Created<{2}>: {0} => {1}", syncPath, content, createChangeSet.Commit.Sha.Substring(0, 4));
            }
        }

        public async Task DeleteFile(SyncFileArgs args)
        {
            var message = args.Message;
            var syncOwner = args.SyncOwner;
            var syncRepo = args.SyncRepo;
            var syncPath = args.SyncPath;

            var repoClient = Client.Repository;
            var myRepo = await repoClient.Get(syncOwner, syncRepo);
            if (myRepo == null)
            {
                throw new InvalidOperationException($"未知仓储: {syncOwner}/{syncRepo}");
            }

            var myRepoId = myRepo.Id;
            var repoClientContent = repoClient.Content;
            RepositoryContent theFile = null;
            try
            {
                var contents = await repoClientContent.GetAllContents(myRepoId, syncPath);
                theFile = contents.SingleOrDefault();
                if (theFile == null)
                {
                    // if file is not found, create it
                    Console.WriteLine("not found file: {0}", syncPath);
                }
                else
                {
                    //file exist, delete it
                    // delete file
                    await repoClientContent.DeleteFile(myRepoId, syncPath, new DeleteFileRequest(message, theFile.Sha));
                    Console.WriteLine("delete file: {0}", syncPath);
                }
            }
            catch (NotFoundException ex)
            {
                //file not exist!
                Console.WriteLine(ex.Message);
            }
        }

        //helpers
        public static MyGithubApi Create(MyGithubConfig config)
        {
            //https://laedit.net/2016/11/12/GitHub-commit-with-Octokit-net.html
            return new MyGithubApi(config);
        }
    }
}