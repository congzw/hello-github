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
            var token = "b2f28db601599e3c1271117acfd9caed1d4abfb3"; //the token
            var repoName = "demo-files"; //the repo
            return new DemoData()
            {
                Owner = owner,
                Token = token,
                Repo = repoName
            };
        }
    }
}