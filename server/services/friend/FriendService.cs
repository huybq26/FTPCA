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
            List<string> results = await DatabaseGroup.SearchFriends(term);
            return results;
        }

        public static async Task AddFriendRequest(int senderid, int receiverid)
        {
            // if two people are friend already, then return false directly
            await DatabaseGroup.AddFriendRequest(senderid, receiverid);
        }

        public static async Task AcceptFriendRequest(int senderid, int receiverid)
        {
            await DatabaseGroup.AcceptFriendRequest(senderid, receiverid);
        }

        public static async Task RejectFriendRequest(int senderid, int receiverid)
        {
            await DatabaseGroup.RejectFriendRequest(senderid, receiverid);
        }

        public static async Task<List<string>> QueryFriendRequest(int userid)
        {
            List<string> results = await DatabaseGroup.QueryFriendRequest(userid);
            return results;
        }

        public static async Task<List<string>> ListAllFriends(int userid)
        {
            List<string> results = await DatabaseGroup.ListAllFriends(userid);
            return results;
        }

        public static async Task DeleteExistingFriend(int userid, int friendid)
        {
            await DatabaseGroup.DeleteExistingFriend(userid, friendid);
        }
    }
}