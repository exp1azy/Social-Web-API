﻿using Microsoft.EntityFrameworkCore;
using SocialAPI.Data;
using SocialAPI.Repositories.Interfaces;
using SocialAPI.Resources;

namespace SocialAPI.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;

        public PostRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreatePostAsync(int authorId, string text, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == authorId, cancellationToken);
            if (user == null)
            {
                throw new ApplicationException(Error.UserNotExistingError);
            }

            await _dataContext.Posts.AddAsync(new Post
            {
                AuthorId = authorId,
                Text = text,
                Date = DateTime.UtcNow
            }, cancellationToken);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePostAsync(int id, CancellationToken cancellationToken)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (post == null)
            {
                throw new ApplicationException(Error.PostNotExistingError);
            }

            _dataContext.Remove(post);
        }

        public async Task<Post> GetPostAsync(int id, CancellationToken cancellationToken)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return post ?? throw new ApplicationException(Error.PostNotExistingError);
        }
    }
}
