using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserGroup;
using DatabaseGroup;
using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SignalRChat.Hubs;

namespace MessageControllerGroup
{
    [ApiController]

    public class MessageControllerGroup : ControllerBase
    {
        [HttpPost]
        [Authorize]
        [Route("createconv")]
        public async Task<IActionResult> CreateConversation(string convname, string participantList, string creatorname, string creatorid)
        {
            // participantLists is the list of userid separated by comma
            List<int> userIdList = participantList.Split(',')
                                 .Select(id => int.Parse(id.Trim()))
                                 .ToList();
            try
            {
                Console.WriteLine(convname + " to " + participantList);
                string convid = await Database.CreateConversation(convname, userIdList, creatorname, creatorid);
                return Ok(new
                {
                    Message = "Friend request sent successfully",
                    Data = convid
                });
            }
            catch (System.Exception)
            {
                return BadRequest(new
                {
                    Message = "Problems when creating new conversation",
                    Data = new { convname, participantList }
                });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("addparticipant")]
        public async Task<IActionResult> AddParticipants(string convid, string participantList)
        {
            // participantLists is the list of userid separated by comma
            List<int> userIdList = participantList.Split(',')
                                 .Select(id => int.Parse(id.Trim()))
                                 .ToList();
            try
            {
                Console.WriteLine(convid + " to " + participantList);
                await Database.AddParticipants(convid, userIdList);
                return Ok(new
                {
                    Message = "Add participants successfully",
                    Data = convid
                });
            }
            catch (System.Exception)
            {
                return BadRequest(new
                {
                    Message = "Add participants unsuccessfully",
                    Data = convid

                });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("queryconversation")]
        public async Task<IActionResult> QueryConversation(int userid, DateTime lastmessage)
        {
            Console.WriteLine(userid.ToString() + " " + lastmessage.ToString());
            try
            {
                List<List<string>> conversationList = await Database.QueryConversation(userid, lastmessage);
                return Ok(new
                {
                    Message = "Conversations retrieved successfully",
                    Data = conversationList
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Failed to retrieve Conversations: " + ex.Message,
                });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("getparticipant")]
        public async Task<IActionResult> GetParticipantsFromConv(string convid)

        {
            Console.WriteLine(convid);
            try
            {
                List<List<string>> partiList = await Database.GetParticipantsFromConv(convid);
                return Ok(new
                {
                    Message = "Conversation parti retrieved successfully",
                    Data = partiList
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Failed to retrieve Conversations parti: " + ex.Message,
                });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("removeparticipant")]
        public async Task<IActionResult> RemoveParticipants(string convid, string participantList)
        {
            // participantLists is the list of userid separated by comma

            try
            {
                Console.WriteLine(convid + " remove " + participantList);
                await Database.RemoveParticipants(convid, participantList);
                return Ok(new
                {
                    Message = "Remove participants successfully",
                    Data = convid
                });
            }
            catch (System.Exception)
            {
                return BadRequest(new
                {
                    Message = "Remove participants unsuccessfully",
                    Data = convid

                });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("sendmessage")]
        public async Task<IActionResult> SendMessage(int senderid, string convid, string content, string fileid)
        {
            // participantLists is the list of userid separated by comma
            try
            {
                Console.WriteLine(senderid.ToString() + " " + convid + " to " + content + " file " + fileid);
                await Database.SendMessage(senderid, convid, content, fileid);
                // await ChatHub.SendMessageHub(senderid.ToString(), content);
                return Ok(new
                {
                    Message = "Send message successfully",
                    Data = content
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "Send message unsuccessfully " + e.Message,

                });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("querymessage")]
        public async Task<IActionResult> QueryMessage(string convid, DateTime lastMessageTime)
        {
            Console.WriteLine(convid.ToString() + " " + lastMessageTime.ToString());
            try
            {
                List<List<string>> messageList = await Database.QueryMessage(convid, lastMessageTime);
                return Ok(new
                {
                    Message = "Messages retrieved successfully",
                    Data = messageList
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Failed to retrieve messages: " + ex.Message,
                });
            }
        }


        [HttpPost]
        // [Authorize]
        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            try
            {
                string fileId = Guid.NewGuid().ToString();
                string uniqueFileName = fileId + "_" + file.FileName;
                int fileSizeInBytes = (int)file.Length;
                string fileType = file.ContentType;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                await Database.UploadFile(fileId, file.FileName, (string)filePath, fileSizeInBytes, fileType);

                return Ok(new
                {
                    Message = "Upload file successfully!",
                    Data = new
                    {
                        filePath = (string)filePath,
                        fileId = fileId,
                        fileName = file.FileName,
                        fileType = fileType,
                        fileSize = fileSizeInBytes
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Failed to upload file: " + ex.Message,
                });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("getfile")]
        public async Task<IActionResult> GetFileDetails(string fileid)
        {
            Console.WriteLine(fileid);
            try
            {
                List<string> File = await Database.GetFileDetails(fileid);
                return Ok(new
                {
                    Message = "File retrieved successfully",
                    Data = File
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Failed to retrieve File: " + ex.Message,
                });
            }
        }


    }
}