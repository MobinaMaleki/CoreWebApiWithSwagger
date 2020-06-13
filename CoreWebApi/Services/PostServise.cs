using CoreWebApi.Data;
using CoreWebApi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Services
{
   
    public class PostServise : IPostServise
    {
         ApplicationDbContext dbContext;
        public PostServise(ApplicationDbContext _dbcontext)
        {
            dbContext = _dbcontext;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await dbContext.Posts.AddAsync(post);
            var created = await dbContext.SaveChangesAsync();
            return created > 0;

        }

        public async Task <bool> DeletePostAsync(Guid postId)
        {
            var post =await GetPostByIdAsync(postId);
            dbContext.Posts.Remove(post);
            var deleted = await dbContext.SaveChangesAsync();
            return deleted > 0;
            
        }

        public async Task <Post> GetPostByIdAsync(Guid postId)
        {
            return await dbContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
        }

       

        public async Task<List<Post>> GetPostsAsync()
        {
            return await dbContext.Posts.ToListAsync();
        }

        public async Task< bool> UpdatePostAsync(Post posttoupdate)
        {
             dbContext.Posts.Update(posttoupdate);
            var updated = await dbContext.SaveChangesAsync();
           
            return updated>0;
        }

       

       
      
    }
}
