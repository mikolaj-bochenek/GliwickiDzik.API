using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GliwickiDzik.API.Data;
using GliwickiDzik.API.DTOs;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GliwickiDzik.API.Controllers
{
    //http://localhost:5000/api/userid/message
    [Route("api/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ActionFilter))]
    public class MessageController : ControllerBase
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{messageId}")]
        public async Task<IActionResult> GetMessageAsync(int userId, int messageId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var message = await _unitOfWork.Messages.FindOneAsync(m => m.MessageId == messageId);

            if (message == null)
                return BadRequest("Error: Message cannot be found!");
            
            var messageToReturn = _mapper.Map<MessageForCreateDTO>(message);

            return Ok(messageToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            messageParams.UserId = userId;

            var messagesFromRepo = await _unitOfWork.Messages.GetMessagesForUserAsync(messageParams);

            var messagesToReturn = _mapper.Map<IEnumerable<MessageForReturnDTO>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            foreach (var message in messagesToReturn)
            {
                message.MessageContainer = messageParams.MessageContainer;
            }

            return Ok(messagesToReturn);
        }

        [HttpGet("corresponded")]
        public async Task<IActionResult> GetCorrespondedUsersAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var listOfUsers = await _unitOfWork.Messages.GetCorrespondedUsersAsync(userId);

            if (listOfUsers == null)
                return NoContent();
             
            var listToReturn = _mapper.Map<IEnumerable<UserToConvDTO>>(listOfUsers);

            return Ok(listToReturn);
        }

        [HttpGet("{recipientId}/thread")]
        public async Task<IActionResult> GetMessageThreadAsync(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var recipient = await _unitOfWork.Users.FindOneAsync(u => u.UserId == recipientId);

            if (recipient == null)
                return BadRequest("Error: User cannot be found!");
            
            var messages = await _unitOfWork.Messages.GetMessageThreadAsync(userId, recipientId);

            if (messages == null)
                return BadRequest("The message thread cannot be found!");

            var messageThread = _mapper.Map<IEnumerable<MessageForCreateDTO>>(messages);

            return Ok(messageThread);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddMessageAsync(int userId, MessageForCreateDTO messageForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            messageForCreateDTO.SenderId = userId;

            var recipient = await _unitOfWork.Users.FindOneAsync(u => u.UserId == messageForCreateDTO.RecipientId);

            if (recipient == null)
                return BadRequest("Error: The user cannot be found!");
            
            var createdMessage = _mapper.Map<MessageModel>(messageForCreateDTO);

            _unitOfWork.Messages.Add(createdMessage);
            
            if (await _unitOfWork.SaveAllAsync())
                return StatusCode(201);

            throw new Exception("Error: Saving message to database failed!");
        }
        
        [HttpDelete("{messageId}")]
        public async Task<IActionResult> RemoveMessageAsync(int userId, int messageId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var message = await _unitOfWork.Messages.FindOneAsync(m => m.MessageId == messageId);

            if (message == null)
                return BadRequest("Error: The message cannot be found!");

            if (message.SenderId == userId)
            {
                if (message.SenderDeleted == true)
                    return StatusCode(304);
                
                else
                    message.SenderDeleted = true;
            }
            
            if (message.RecipientId == userId)
            {
                if (message.RecipientDeleted == true)
                    return StatusCode(304);
                
                else
                    message.SenderDeleted = true;
            }
            
            if (message.SenderDeleted == true && message.RecipientDeleted == true)
                _unitOfWork.Messages.Remove(message);
            
            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The message has been deleted.");

            throw new Exception("Error: Removing message from database failed!");
        }
        
        [HttpDelete("{recipientId}/thread")]
        public async Task<IActionResult> RemoveMessageThreadAsync(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var messages = await _unitOfWork.Messages.GetMessageThreadAsync(userId, recipientId);

            if (messages == null)
                return NoContent();

            var messagesToDelete = new List<MessageModel>();

            foreach(var message in messages)
            {
                if (message.SenderId == userId)
                {
                    switch(message.SenderDeleted)
                    {
                        case true:
                            break;

                        case false:
                            message.SenderDeleted = true;
                            break;
                    } 
                }
                    
                if (message.RecipientId == userId)
                {
                    switch(message.RecipientDeleted)
                    {
                        case true:
                            break;

                        case false:
                            message.RecipientDeleted = true;
                            break;
                    } 
                }
                               
                if (message.SenderDeleted == true && message.RecipientDeleted == true)
                    messagesToDelete.Add(message);
            }

            _unitOfWork.Messages.RemoveRange(messagesToDelete);

            messagesToDelete = null;

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The message thread has been deleted.");
            
            throw new Exception("Error: Removing messages from database failed!");
        }

        [HttpPost("{messageId}/IsRead")]
        public async Task<IActionResult> SetMessageIsReadAsync(int userId, int messageId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var message = await _unitOfWork.Messages.FindOneAsync(m => m.MessageId == messageId);

            if (message.RecipientId != userId)
                return Unauthorized();
            
            if (!message.IsRead)
            {
                message.IsRead = true;
                message.DateOfRead = DateTime.Now;
            }

            if (await _unitOfWork.SaveAllAsync())
                return Ok("Info: The message has been read");

            throw new Exception("Error: Saving readed message to database failed!");
        }
    }
}