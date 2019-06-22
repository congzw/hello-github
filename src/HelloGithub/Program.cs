using System;
using HelloGithub.Libs;

namespace HelloGithub
{
    class Program
    {
        static void Main(string[] args)
        {
            var demoData = DemoData.CreateForTest();
            var owner = demoData.Owner;
            var token = demoData.Token;
            var repoName = demoData.Repo;

            var config = MyGithubConfig.Create(owner, token);
            var myGithubApi = MyGithubApi.Create(config);

            //myGithubApi.ListRepositories(owner).Wait();
            //myGithubApi.Miscellaneous().Wait();

            var syncTarget = SyncTarget.Init(owner, repoName);
            var timeInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var syncFileArgs = syncTarget.CreateSyncFileArgs("ApiCall: " + timeInfo, "hello, world!", "_api/a.txt");
            myGithubApi.SyncFile(syncFileArgs).Wait();
            myGithubApi.DeleteFile(syncFileArgs).Wait();

            Console.Read();
        }
    }
}
