using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GliwickiDzik.API.Data;
using GliwickiDzik.API.DTOs;
using GliwickiDzik.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GliwickiDzik.API.Helpers;

namespace GliwickiDzik.API.Controllers
{
    //http://localhost:5000/api/userid/content
    [Route("api/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ActionFilter))]
    public class ContentController : ControllerBase
    {
        private readonly IContentRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ContentController(IContentRepository repository, IUserRepository userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        #region = "MESSAGES"

        [HttpGet("GetMessage/{messageId}")]
        public async Task<IActionResult> GetMessageAsync(int userId, int messageId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var message = await _repository.GetMessageAsync(messageId);

            if (message == null)
                return BadRequest("Error: Message cannot be found!");
            
            var messageToReturn = _mapper.Map<MessageForCreateDTO>(message);

            return Ok(messageToReturn);
        }

        [HttpGet("GetAllMessages")]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            messageParams.UserId = userId;

            var messagesFromRepo = await _repository.GetMessagesForUserAsync(messageParams);

            var messagesToReturn = _mapper.Map<IEnumerable<MessageForReturnDTO>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            foreach (var message in messagesToReturn)
            {
                message.MessageContainer = messageParams.MessageContainer;
            }

            return Ok(messagesToReturn);
        }
        
        [HttpGet("GetMessageThread/{recipientId}")]
        public async Task<IActionResult> GetMessageThreadAsync(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var messages = await _repository.GetMessageThreadAsync(userId, recipientId);

            if (messages == null)
                return BadRequest("The message thread cannot be found!");

            var messageThread = _mapper.Map<IEnumerable<MessageForCreateDTO>>(messages);

            return Ok(messageThread);
        }
        
        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddMessageAsync(int userId, MessageForCreateDTO messageForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            messageForCreateDTO.SenderId = userId;

            var recipient = await _userRepository.GetOneUserAsync(messageForCreateDTO.RecipientId);
            var sender = await _userRepository.GetOneUserAsync(userId);

            if (recipient == null)
                return BadRequest("Error: The user cannot be found!");
            
            if(!recipient.Conversation.Contains(userId))
                recipient.Conversation.Add(userId);

            if(!sender.Conversation.Contains(messageForCreateDTO.RecipientId))
                sender.Conversation.Add(messageForCreateDTO.RecipientId);
            
            var createdMessage = _mapper.Map<MessageModel>(messageForCreateDTO);

            _repository.Add(createdMessage);

            //var messageToReturn = _mapper.Map<MessageForCreateDTO>(createdMessage);
            
            if (!await _repository.SaveContentAsync())
                throw new Exception("Error: Saving message to database failed!");
            
            //return CreatedAtRoute("GetMessage", new {messageId = createdMessage.MessageId}, messageToReturn);
            //return CreatedAtRoute("GetMessage", new {messageId = createdMessage.MessageId}, createdMessage);
            return StatusCode(201);
        }
        
        [HttpDelete("RemoveMessage/{messageId}")]
        public async Task<IActionResult> RemoveMessageAsync(int userId, int messageId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var message = await _repository.GetMessageAsync(messageId);

            if (message == null)
                return BadRequest("Error: The message cannot be found!");

            if (message.SenderId == userId)
            {
                if (message.SenderDeleted == true)
                    return BadRequest("The message has been already deleted!");
                
                else
                    message.SenderDeleted = true;
            }
            
            if (message.RecipientId == userId)
            {
                if (message.RecipientDeleted == true)
                    return BadRequest("The message has been already deleted!");
                
                else
                    message.SenderDeleted = true;
            }
            
            if (message.SenderDeleted == true && message.RecipientDeleted == true)
                _repository.Remove(message);
            
            if (!await _repository.SaveContentAsync())
                throw new Exception("Error: Removing message from database failed!");
            
            return NoContent();
        }
        
        [HttpDelete("RemoveMessageThread/{recipientId}")]
        public async Task<IActionResult> RemoveMessageThreadAsync(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var messages = await _repository.GetMessageThreadAsync(userId, recipientId);

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

            _repository.RemoveRange(messagesToDelete);

            messagesToDelete = null;

            if (await _repository.SaveContentAsync())
                return NoContent();
            
            throw new Exception("Error: Removing messages from database failed!");
        }

        [HttpPost("{messageId}/IsRead")]
        public async Task<IActionResult> SetMessageIsReadAsync(int userId, int messageId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var message = await _repository.GetMessageAsync(messageId);

            if (message.RecipientId != userId)
                return Unauthorized();
            
            message.IsRead = true;
            message.DateOfRead = DateTime.Now;

            if (!await _repository.SaveContentAsync())
                throw new Exception("Error: Saving readed message to database failed!");
            
            return NoContent();
        }
    
        #endregion

        #region = "COMMENTS"

        [HttpGet("GetComment/{commentId}")]
        public async Task<IActionResult> GetCommentAsync(int commentId)
        {
            var comment = await _repository.GetCommentAsync(commentId);

            if (comment == null)
                return BadRequest("Error: The comment cannot be found!");
            
            var commentToReturn = _mapper.Map<CommentForReturnDTO>(comment);

            return Ok(commentToReturn);
        }

        [HttpGet("GetComments/{trainingPlanId}")]
        public async Task<IActionResult> GetCommentsForTrainingPlanAsync(int trainingPlanId, [FromQuery]CommentParams commentParams)
        {
            var commentsToList = await _repository.GetAllCommentsAsync(trainingPlanId, commentParams);

            if (commentsToList == null)
                return BadRequest("Error: Comments cannot be found!");
            
            var commentsToReturn = _mapper.Map<IEnumerable<CommentForReturnDTO>>(commentsToList);

            Response.AddPagination(commentsToList.CurrentPage, commentsToList.PageSize, commentsToList.TotalCount, commentsToList.TotalPages);

            return Ok(commentsToReturn);
        }

        [HttpPost("AddComment/{planId}")]
        public async Task<IActionResult> AddCommentAsync(int userId, int trainingPlanId, CommentForCreateDTO commentForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var trainingPlan = await _repository.GetCommentAsync(trainingPlanId);

            if (trainingPlan == null)
                return BadRequest("Error: The training plan cannot be found!");
            
            var comment = _mapper.Map<CommentModel>(commentForCreateDTO);
            comment.CommenterId = userId;
            comment.TrainingPlanId = trainingPlanId;

            _repository.Add(comment);

            if (!await _repository.SaveContentAsync())
                throw new Exception("Error: Saving comment to database failed!");
            
            return NoContent();
        }

        [HttpPut("EditComment/{commentId}")]
        public async Task<IActionResult> EditCommentAsync(int userId, int commentId, CommentForEditDTO commentForEditDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var commentToEdit = await _repository.GetCommentAsync(commentId);

            if ( commentToEdit == null)
                return BadRequest("The comment cannot be found!");
            
            var editedComment = _mapper.Map(commentToEdit, commentForEditDTO);

            if (!await _repository.SaveContentAsync())
                throw new Exception("Error: Saving edited comment to database failed!");
            
            return NoContent();
        }

        [HttpDelete("RemoveComment/{commentId}")]
        public async Task<IActionResult> RemoveCommentAsync(int userId, int commentId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var commentToDelete = await _repository.GetCommentAsync(commentId);

            if (commentToDelete == null)
                return BadRequest("The comment cannnot be found!");
            
            if (userId != commentToDelete.CommenterId)
                return Unauthorized();

            _repository.Remove(commentToDelete);

            if (await _repository.SaveContentAsync())
                return NoContent();

            throw new Exception("Error: Removing comment from database failed!");   
        }
        
        #endregion
    }
}