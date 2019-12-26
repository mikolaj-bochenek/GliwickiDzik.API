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

namespace GliwickiDzik.API.Controllers
{
    //http://localhost:5000/api/users/userid/content
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    [Authorize]
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

            var recipient = await _userRepository.GetUserByIdAsync(messageForCreateDTO.RecipientId);

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
    }
}