﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ItForum.Services
{
    public class PostServices
    {
        private readonly DataContext _db;

        public PostServices(DataContext db)
        {
            _db = db;
        }

        public async Task<List<Container>> GetNewPosts()
        {
            var posts = await _db.Containers.Include(c => c.Post)
                .OrderByDescending(c => c.Post.PublishDate)
                .ToListAsync();
            return posts;
        }
    }

    public static class PostServicesExtensions
    {
        public static void AddPostServices(this IServiceCollection builder)
        {
            builder.AddScoped(typeof(PostServices));
        }
    }
}