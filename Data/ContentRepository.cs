using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public Task<IEnumerable<CommentModel>> GetAllCommentsAsync()
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<MessageModel>> GetMessageThreadAsync(int userId, int recipientId)
        {
            throw new NotImplementedException();
        }

        public void Remove(MessageModel entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(CommentModel entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<MessageModel> entities)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<CommentModel> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveContentAsync()
        {
            throw new NotImplementedException();
        }
    }
}