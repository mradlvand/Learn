using Data.Context;
using Grpc.Core;
using GrpcPresentation.Protos;
using Microsoft.EntityFrameworkCore;

namespace GrpcPresentation.Services
{
    public class HomeService : Home.HomeBase
    {
        private readonly ILogger<HomeService> _logger;
        private readonly DBLearnContext _context;
        public HomeService(ILogger<HomeService> logger, DBLearnContext context)
        {
            _logger = logger;
            _context = context;
        }

        public override async Task<PodcastCategoryReply> GetPodcastCategory(PodcastCategoryRequest request, ServerCallContext context)
        {
            var query = await _context.PodcastCategories.Select(x => new PodcastCategory()
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
            }).ToListAsync();

            var replyModel = new PodcastCategoryReply();
            replyModel.PodcastCategories.AddRange(query);
            return replyModel;
        }

        public override async Task<PodcastsReply> GetPodcasts(PodcastsRequest request, ServerCallContext context)
        {
            var query = await _context.Podcasts
                .Where(x=>x.PodcastCategoryId == request.PodcastCategoryId)
                .Select(x => new Podcast()
            {
                Title = x.Title,
                Sound = x.Sound,
                Image = x.Image,
            }).Skip(request.Take * (request.Page - 1)).Take(request.Take).ToListAsync();

            var replyModel = new PodcastsReply();
            replyModel.Podcasts.AddRange(query);
            return replyModel;
        }

        public override async Task<StorisReply> GetStoris(StorisRequest request, ServerCallContext context)
        {
            var query = await _context.Stories.Select(x => new Story()
            {
                FullName = x.FullName,
                Location = x.Location,
                Video = x.Video,
                Picture = x.Picture
            }).ToListAsync();

            var replyModel = new StorisReply();
            replyModel.Storis.AddRange(query);
            return replyModel;
        }
    }
}
