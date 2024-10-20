using Common.Exceptions;
using Data.Context;
using Grpc.Core;
using GrpcPresentation;
using Microsoft.EntityFrameworkCore;

namespace GrpcPresentation.Services
{
    public class LevelService : Level.LevelBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly DBLearnContext _context;
        public LevelService(ILogger<GreeterService> logger, DBLearnContext context)
        {
            _logger = logger;
            _context = context;
        }

        public override Task<LevelReply> GetLevels(LevelRequest request, ServerCallContext context)
        {
            var levels = _context.Levels.Where(x => x.TeacherId == request.Teacherid).Select(a => new LevelReply
            {
                Progress = 0,
                TimeInSum = "0",
                Title = a.Title,
            }).ToList();

            return Task.FromResult(levels.FirstOrDefault());
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
