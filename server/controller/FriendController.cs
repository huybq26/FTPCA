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
        [Authorize]
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

        [HttpPost]
        [Authorize]
        [Route("acceptfriendrequest")]
        public async Task<IActionResult> AcceptFriendRequest(int senderid, int receiverid)
        {
            try
            {
                // Console.WriteLine(senderid + " to " + receiverid);
                await FriendService.AcceptFriendRequest(senderid, receiverid);
                return Ok(new
                {
                    Message = "Friend accepted sent successfully",
                    Data = new { senderid, receiverid }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Error occurs when accepting friend request: " + ex.Message,
                    Data = new { senderid, receiverid }
                });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("declinefriendrequest")]
        public async Task<IActionResult> RejectFriendRequest(int senderid, int receiverid)
        {
            try
            {
                // Console.WriteLine(senderid + " to " + receiverid);
                await FriendService.RejectFriendRequest(senderid, receiverid);
                return Ok(new
                {
                    Message = "Reject friend request successfully",
                    Data = new { senderid, receiverid }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Error occurs when rejecting friend request: " + ex.Message,
                    Data = new { senderid, receiverid }
                });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("deleteFriend")]
        public async Task<IActionResult> DeleteExistingFriend(int userid1, int userid2)
        {
            try
            {
                // Console.WriteLine(senderid + " to " + receiverid);
                await FriendService.DeleteExistingFriend(userid1, userid2);
                return Ok(new
                {
                    Message = "Friend accepted sent successfully",
                    Data = new { userid1, userid2 }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Error occurs when accepting friend request: " + ex.Message,
                    Data = new { userid1, userid2 }
                });
            }
        }

        // [HttpPost]
        // [Authorize]
        // [Route("cancelfriendrequest")]
        // public async Task<IActionResult> CancelFriendRequest(int senderid, int receiverid)
        // {
        //     try
        //     {
        //         // Console.WriteLine(senderid + " to " + receiverid);
        //         await FriendService.CancelFriendRequest(senderid, receiverid);
        //         return Ok(new
        //         {
        //             Message = "Cancel friend request successfully",
        //             Data = new { senderid, receiverid }
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(new
        //         {
        //             Message = "Error occurs when cancelling friend request: " + ex.Message,
        //             Data = new { senderid, receiverid }
        //         });
        //     }
        // }

        [HttpGet]
        [Authorize]
        [Route("friendlisting")]
        public async Task<IActionResult> ListAllFriends(int userid)
        {
            try
            {
                // Console.WriteLine(senderid + " to " + receiverid);
                List<User> results = await FriendService.ListAllFriends(userid);
                return Ok(new
                {
                    Message = "List friends completed",
                    Data = results
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Error occurs when listing friends: " + ex.Message,
                    Data = userid
                });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("checkrelationshipstatus")]
        public async Task<IActionResult> CheckRelationshipStatus(int userid1, int userid2)
        {
            try
            {
                // Console.WriteLine(senderid + " to " + receiverid);
                String results = await FriendService.CheckRelationshipStatus(userid1, userid2);
                return Ok(new
                {
                    Message = "CheckRelationshipStatus completed",
                    Data = results
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Error occurs when CheckRelationshipStatus: " + ex.Message,
                    Data = userid1
                });
            }
        }


    }
}