using UserGroup;
using DatabaseGroup;


namespace FriendServiceGroup
{
    public class FriendService
    {
        public static async Task<List<string>> SearchFriends(string term)
        {
            // should be able to search all people, including friends
            // should return if two people has been friend or not
            // later, should make a priority queue instead to show friends before the strangers
            List<string> results = await Database.SearchFriends(term);
            return results;
        }

        public static async Task AddFriendRequest(int senderid, int receiverid)
        {
            // if two people are friend already, then return false directly
            await Database.AddFriendRequest(senderid, receiverid);
        }

        public static async Task AcceptFriendRequest(int senderid, int receiverid)
        {
            await Database.AcceptFriendRequest(senderid, receiverid);
        }

        public static async Task RejectFriendRequest(int senderid, int receiverid)
        {
            await Database.RejectFriendRequest(senderid, receiverid);
        }

        public static async Task<List<User>> QueryFriendRequest(int userid)
        {
            List<string> usernameList = await Database.QueryFriendRequest(userid);
            List<User> UserList = new List<User>();

            foreach (string username in usernameList)
            {
                List<string> list = await Database.GetDetailsFromUsername(username);
                UserList.Add(new User
                {
                    Userid = list[0],
                    Username = list[1],
                    PhoneNumber = list[2],
                    Email = list[3],
                    Name = list[4],
                    HashedPassword = ""
                });
            }

            return UserList;
        }


        public static async Task<List<User>> ListAllFriends(int userid)
        {
            List<string> usernameList = await Database.ListAllFriends(userid);
            List<User> UserList = new List<User>();

            foreach (string username in usernameList)
            {
                List<string> list = await Database.GetDetailsFromUsername(username);
                UserList.Add(new User
                {
                    Userid = list[0],
                    Username = list[1],
                    PhoneNumber = list[2],
                    Email = list[3],
                    Name = list[4],
                    HashedPassword = ""
                });
            }

            return UserList;
        }

        public static async Task DeleteExistingFriend(int userid, int friendid)
        {
            await Database.DeleteExistingFriend(userid, friendid);
        }
    }
}