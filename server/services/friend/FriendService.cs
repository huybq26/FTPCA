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
            if (await Database.CheckRelationshipStatus(senderid, receiverid) == "Add Friend")
            {
                await Database.AddFriendRequest(senderid, receiverid);
            }
            else
            {
                Console.WriteLine("A friend request may already be sent, or they are already be friend. Please check again.");
            }
        }

        public static async Task AcceptFriendRequest(int senderid, int receiverid)
        {
            Console.WriteLine("Checking for senderid " + senderid + " receiverid " + receiverid);

            if (await Database.CheckRelationshipStatus(senderid, receiverid) == "Cancel Friend Request")
            {
                await Database.AcceptFriendRequest(senderid, receiverid);
            }
            else
            {
                Console.WriteLine("A friend request may already be sent, or they are already be friend. Please check again.");
            }
        }

        public static async Task RejectFriendRequest(int senderid, int receiverid)
        {
            if (await Database.CheckRelationshipStatus(senderid, receiverid) == "Cancel Friend Request")
            {
                await Database.RejectFriendRequest(senderid, receiverid);
            }
            else
            {
                Console.WriteLine("A friend request may already be sent, or they are already be friend. Please check again.");
            }
        }

        // public static async Task CancelFriendRequest(int senderid, int receiverid)
        // {
        //     if (Database.CheckRelationshipStatus(senderid, receiverid) == "Cancel Friend Request")
        //     {
        //         await Database.RejectFriendRequest(senderid, receiverid); // on purpose, to cancel request
        //     }
        //     else
        //     {
        //         Console.WriteLine("A friend request may already be sent, or they are already be friend. Please check again.");
        //     }
        // }

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
            Console.WriteLine("Checking for userid " + userid + " friendid " + friendid);
            if (await Database.CheckRelationshipStatus(userid, friendid) == "Friend")
            {
                await Database.DeleteExistingFriend(userid, friendid);
            }
            else
            {
                Console.WriteLine("Error. They may not be friend yet.");
            }
        }

        public static async Task<String> CheckRelationshipStatus(int userid1, int userid2)
        {
            return await Database.CheckRelationshipStatus(userid1, userid2);
        }
    }
}