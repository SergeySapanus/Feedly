namespace MyFeedlyServer.Models
{
    public class AuthGetModel
    {
        public AuthGetModel(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
    }
}