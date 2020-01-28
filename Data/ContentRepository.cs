using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GliwickiDzik.API.Helpers;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.API.Data
{
    public class ContentRepository : IContentRepository
    {
        private readonly DataContext _context;

        public ContentRepository(DataContext context)
        {
            _context = context;
        }
        public void Add(MessageModel entity)
        {
            _context.MessageModel.Add(entity);
        }

        public void Add(CommentModel entity)
        {
            _context.CommentModel.Add(entity);
        }

        public void AddRange(IEnumerable<MessageModel> entities)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<CommentModel> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageModel>> FindAsync(Expression<Func<MessageModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CommentModel>> FindAsync(Expression<Func<CommentModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<CommentModel>> GetAllCommentsAsync(int trainingPlanId, CommentParams commentParams)
        {
            var comments = _context.CommentModel
                            .Where(c => c.TrainingPlanId == trainingPlanId)
                            .OrderByDescending(c => c.VoteCounter);  

            if(!string.IsNullOrEmpty(commentParams.OrderBy))
            {
                switch(commentParams.OrderBy)
                {
                    case "Newest":
                        comments = comments.OrderByDescending(c => c.DateOfCreated);
                        break;

                    case "Oldest":
                        comments = comments.OrderBy(c => c.DateOfCreated);
                        break;
                        
                    default:
                        comments = comments.OrderByDescending(c => c.VoteCounter);
                        break;
                }
            }
            return await PagedList<CommentModel>.CreateListAsync(comments, commentParams.PageSize, commentParams.PageNumber);                               
        }

        public Task<IEnumerable<MessageModel>> GetAllMessagesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<CommentModel> GetCommentAsync(int commentId)
        {
            return await _context.CommentModel.FirstOrDefaultAsync(c => c.CommentId == commentId);
        }

        public async Task<MessageModel> GetMessageAsync(int messageId)
        {
            return await _context.MessageModel.FirstOrDefaultAsync(m => m.MessageId == messageId);
        }

        public async Task<IEnumerable<MessageModel>> GetMessageThreadAsync(int userId, int recipientId)
        {
            return await _context.MessageModel
                //.Include(s => s.SenderId == userId)
                //.Include(r => r.RecipientId == recipientId)
                .Where(m => m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false
                         || m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false)
                .OrderByDescending(m => m.DateOfSent).ToListAsync();
        }

        public void Remove(MessageModel entity)
        {
            _context.MessageModel.Remove(entity);
        }

        public void Remove(CommentModel entity)
        {
            _context.CommentModel.Remove(entity);
        }

        public void RemoveRange(IEnumerable<MessageModel> entities)
        {
            _context.RemoveRange(entities);
        }

        public void RemoveRange(IEnumerable<CommentModel> entities)
        {
            _context.RemoveRange(entities);
        }

        public async Task<bool> SaveContentAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}