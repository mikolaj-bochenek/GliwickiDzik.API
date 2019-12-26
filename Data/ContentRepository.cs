using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GliwickiDzik.API.Models;

namespace GliwickiDzik.API.Data
{
    public class ContentRepository : IContentRepository
    {
        public void Add(MessageModel entity)
        {
            throw new NotImplementedException();
        }

        public void Add(CommentModel entity)
        {
            throw new NotImplementedException();
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

        public Task<CommentModel> GetCommentAsync(int commentId)
        {
            throw new NotImplementedException();
        }

        public Task<MessageModel> GetMessageAsync(int messageId)
        {
            throw new NotImplementedException();
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