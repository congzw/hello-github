namespace HelloGithub
{
    public class DemoData
    {
        public string Owner { get; set; }
        public string Token { get; set; }
        public string Repo { get; set; }

        public static DemoData CreateForTest()
        {
            var owner = "ps201908"; //the owner
            var token = "b$f$8db60#599e3c#$7###7acfd9caed#d4abfb3"; //the token
            var repoName = "demo-files"; //the repo
            return new DemoData()
            {
                Owner = owner,
                Token = token.Replace("#","1").Replace("$","2"),
                Repo = repoName
            };
        }
    }
}