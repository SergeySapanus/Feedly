namespace Entities.Extensions
{
    public static class UserExtensions
    {
        public static void Map(this User dbUser, User user)
        {
            dbUser.Name = user.Name;
            dbUser.Password = user.Password;
        }
    }
}
