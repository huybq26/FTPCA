using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FriendServiceGroup;
using UserGroup;

namespace FriendControllerGroup
{
    [ApiController]

    public class FriendControllerGroup : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [Route("searchfriend")]
        public async Task<IActionResult> SearchFriends(string term)
        {
            var data = await FriendService.SearchFriends(term);
            if (data != null)
            {
                return Ok(data);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        [Route("addfriendrequest")]
        public async Task<IActionResult> AddFriendRequest(int senderid, int receiverid)
        {
            try
            {
                // Console.WriteLine(senderid + " to " + receiverid);
                await FriendService.AddFriendRequest(senderid, receiverid);
                return Ok(new
                {
                    Message = "Friend request sent successfully",
                    Data = new { senderid, receiverid }
                });
            }
            catch (System.Exception)
            {
                return BadRequest(new
                {
                    Message = "The friend request may already be present",
                    Data = new { senderid, receiverid }
                });
            }
        }

        [HttpGet]
        // [Authorize]
        [Route("queryfriendrequest")]
        public async Task<IActionResult> GetFriendRequest([FromQuery] int userid)
        {
            try
            {
                List<User> list = await FriendService.QueryFriendRequest(userid);
                return Ok(new
                {
                    Message = "Friend request retrieved successfully",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Failed to retrieve friend request: " + ex.Message,
                });
            }
        }


    }
}