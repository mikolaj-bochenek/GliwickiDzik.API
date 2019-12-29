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

        [HttpGet("GetMessage/{messageId}", Name="GetMessage")]
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
        public async Task<IActionResult> CreateMessageAsync(int userId, MessageForCreateDTO messageForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            messageForCreateDTO.SenderId = userId;

            var recipient = await _userRepository.GetOneUserAsync(messageForCreateDTO.RecipientId);

            if (recipient == null)
                return BadRequest("User cannot be found!");
            
            var createdMessage = _mapper.Map<MessageModel>(messageForCreateDTO);

            _repository.Add(createdMessage);

            //var messageToReturn = _mapper.Map<MessageForCreateDTO>(createdMessage);
            
            if (!await _repository.SaveContentAsync())
                throw new Exception("Error occured while trying to save changes in database!");
            
            //return CreatedAtRoute("GetMessage", new {messageId = createdMessage.MessageId}, messageToReturn);
            return CreatedAtRoute("GetMessage", new {messageId = createdMessage.MessageId}, createdMessage);
        }
        
        [HttpDelete("DeleteMessage/{messageId}")]
        public async Task<IActionResult> RemoveMessageAsync(int userId, int messageId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var message = await _repository.GetMessageAsync(messageId);

            if (message == null)
                BadRequest("Error: The message cannot be found!");

            if (message.SenderId == userId)
                message.SenderDeleted = true;
            if (message.RecipientId == userId)
                message.RecipientDeleted = true;
            
            if (message.SenderDeleted == true && message.RecipientDeleted == true)
                _repository.Remove(message);
            
            if (!await _repository.SaveContentAsync())
                throw new Exception("Error occuerd while trying to save changes in database");
            
            return NoContent();
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
                throw new Exception("Error uccured while trying to save changes in database");
            
            return NoContent();
        }
    
        #endregion

        #region = "COMMENTS"

        [HttpGet("GetComment/{commentId}")]
        public async Task<IActionResult> GetCommentAsync(int commentId)
        {
            var comment = await _repository.GetCommentAsync(commentId);

            if (comment == null)
                return BadRequest("Comment cannot be found!");
            
            var commentToReturn = _mapper.Map<CommentForReturnDTO>(comment);

            return Ok(commentToReturn);
        }

        [HttpGet("GetComments/{trainingPlanId}")]
        public async Task<IActionResult> GetCommentsForTrainingPlanAsync(int trainingPlanId, [FromQuery]CommentParams commentParams)
        {
            var commentsToList = await _repository.GetAllCommentsAsync(trainingPlanId, commentParams);

            if (commentsToList == null)
                return BadRequest("Comments cannot be found!");
            
            var commentsToReturn = _mapper.Map<IEnumerable<CommentForReturnDTO>>(commentsToList);

            Response.AddPagination(commentsToList.CurrentPage, commentsToList.PageSize, commentsToList.TotalCount, commentsToList.TotalPages);

            return Ok(commentsToReturn);
        }

        [HttpPost("AddComment/{planId}")]
        public async Task<IActionResult> CreateCommentAsync(int userId, int trainingPlanId, CommentForCreateDTO commentForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var comment = _mapper.Map<CommentModel>(commentForCreateDTO);
            comment.CommenterId = userId;
            comment.TrainingPlanId = trainingPlanId;

            _repository.Add(comment);

            if (!await _repository.SaveContentAsync())
                throw new Exception("Error occurred while trying save changes to database!!");
            
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
                throw new Exception("Error occurred while trying save changes to database!");
            
            return NoContent();
        }

        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> RemoveCommentAsync(int userId, int commentId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var commentToDelete = await _repository.GetCommentAsync(commentId);

            if (commentToDelete == null)
                return BadRequest("The comment cannnot be found!");
            
            if (userId != commentToDelete.CommenterId)
                return BadRequest("You are not allowed to remove this comment!");
            _repository.Remove(commentToDelete);

            if (await _repository.SaveContentAsync())
                return NoContent();

            throw new Exception("Error occurred while trying save changes to database!");   
        }
        #endregion
    }
}