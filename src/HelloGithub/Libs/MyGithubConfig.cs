namespace HelloGithub.Libs
{
    public class MyGithubConfig
    {
        public string Username { get; set; }
        public string PersonalAccessToken { get; set; }

        public static MyGithubConfig Create(string username, string personalAccessToken)
        {
            return new MyGithubConfig(){Username = username, PersonalAccessToken = personalAccessToken};
        }
    }
}