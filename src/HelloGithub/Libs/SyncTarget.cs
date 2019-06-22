using System;

namespace HelloGithub.Libs
{
    public class SyncTarget
    {
        public SyncTarget(string owner, string repo)
        {
            if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(repo))
            {
                throw new ArgumentException($"无效的初始化参数: {owner}, {repo}");
            }
            Owner = owner;
            Repo = repo;
        }

        public string Owner { get; set; }
        public string Repo { get; set; }

        public SyncFileArgs CreateSyncFileArgs(string message, string content, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new InvalidOperationException("无效的参数Path:" + path);
            }

            if (string.IsNullOrWhiteSpace(Owner))
            {
                throw new InvalidOperationException("无效的初始化Owner:" + Owner);
            }

            if (string.IsNullOrWhiteSpace(Repo) || string.IsNullOrWhiteSpace(Repo))
            {
                throw new InvalidOperationException("无效的初始化Repo:" + Repo);
            }

            return new SyncFileArgs()
            {
                Message = message,
                Content = content,
                SyncPath = path,
                SyncRepo = Repo,
                SyncOwner = Owner
            };
        }
        
        public static SyncTarget Init(string owner, string repo)
        {
            return new SyncTarget(owner, repo);
        }
    }

    public class SyncFileArgs
    {
        public string Message { get; set; }
        public string Content { get; set; }

        public string SyncOwner { get; set; }
        public string SyncRepo { get; set; }
        public string SyncPath { get; set; }
    }
}