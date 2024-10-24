using Common.Exceptions;
using Data.Context;
using Grpc.Core;
using GrpcPresentation;
using Microsoft.EntityFrameworkCore;

namespace GrpcPresentation.Services
{
    public class LevelService : Level.LevelBase
    {
        private readonly ILogger<LevelService> _logger;
        private readonly DBLearnContext _context;
        public LevelService(ILogger<LevelService> logger, DBLearnContext context)
        {
            _logger = logger;
            _context = context;
        }

        public override Task<LevelsReply> GetLevels(LevelsRequest request, ServerCallContext context)
        {
            var query = _context.Levels.Join(_context.UserProgresses,
                        lev => lev.Id,
                        userp => userp.LevelId,
                        (lev, userp) => new
                        {
                            levelId = lev.Id,
                            title = lev.Title,
                            userId = userp.UserId,
                            progress = userp.ProgressPercent,
                            timeInSum = userp.TimeInSum
                        })
                        .Where(x => x.userId == request.UserId)
                        .Select(x => new LevelModel()
                         {
                             LevelId = x.levelId,
                             Progress = x.progress,
                             TimeInSum = x.timeInSum,
                             Title = x.title,
                         }).ToList();

            var replyModel = new LevelsReply();
            replyModel.Levels.AddRange(query);

            return Task.FromResult(replyModel);
        }

        public override Task<HelloReply2> SayHello(HelloRequest2 request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply2
            {
                Message = "Hello " + request.Name
            });
        }

    }
}
