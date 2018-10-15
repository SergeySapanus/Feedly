namespace MyFeedlyServer.Models
{
    public class AuthGetModel
    {
        public AuthGetModel(string token, int authorizedUserId)
        {
            Token = token;
            AuthorizedUserId = authorizedUserId;
        }

        public int AuthorizedUserId { get; set; }

        public string Token { get; set; }
    }
}