using Common.Exceptions;
using Data.Context;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Presentation.Protos;
using Presentation.Service;

namespace Presentation.GrpcService
{
    public class GrpcLevelService : level.levelBase
    {
        private readonly ILogger _logger;
        private readonly DBLearnContext _context;

        public GrpcLevelService(DBLearnContext context, ILogger<LevelLogic> logger)
        {
            _context = context;
            _logger = logger;
        }


        public override Task<LevelDto> GetLevel(GetLevelDtoRequest request,ServerCallContext context)
        {
            if (request.LevelId < 0)
                throw new BadHttpRequestException("levelId is wrong.");

            var level = _context.Levels.FirstOrDefaultAsync(x => x.Id == request.LevelId).Result;

            if (level != null)
            {
                LevelDto res = new()
                {
                    Id = level.Id,
                    Status = level.Status,
                    Order = level.Order,
                    TeacherId = level.TeacherId,
                    Title = level.Title,
                };

                return Task.FromResult(res);
            }
            else
            {
                throw new NotFoundException("level not found!.");
            }
        }
    }
}
