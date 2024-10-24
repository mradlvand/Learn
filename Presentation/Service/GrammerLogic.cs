﻿using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Data.Context;
using Presentation.Dtos;
using System.Linq;

namespace Presentation.Service
{
    public interface IGrammerLogic
    {
        Task<List<GrammerDto>> GetGrammers(int lessonId);
        Task<GrammerDto> GetGrammer(int GrammerId);
    }

    public class GrammerLogic : IGrammerLogic
    {
        private readonly ILogger _logger;
        private readonly DBLearnContext _context;

        public GrammerLogic(DBLearnContext context, ILogger<GrammerLogic> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<GrammerDto>> GetGrammers(int lessonId)
        {
            try
            {
                if (lessonId < 0)
                    throw new BadRequestException("آیدی اشتباه می باشد.");

                var lists = await _context.Grammers.AsQueryable().Where(x => x.LessonId == lessonId).ToListAsync();

                var res = lists.Select(x => new GrammerDto
                {
                    Id = x.Id,
                    Context = x.Context,
                    Header = x.Header,
                    CreationDateTime = x.CreationDateTime,
                    Description = x.Description,
                    Video = x.Video,
                    Status = x.Status,
                    UpdateDateTime = x.UpdateDateTime,
                }).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw new ServerException(ex);
            }
        }

        public async Task<GrammerDto> GetGrammer(int speakingId)
        {
            if (speakingId < 0)
                throw new BadHttpRequestException("levelId is wrong.");

            var resData = await _context.Grammers.FirstOrDefaultAsync(x => x.Id == speakingId);

            if (resData != null)
            {
                return new GrammerDto()
                {
                    Id = resData.Id,
                    CreationDateTime = resData.CreationDateTime,
                    Description = resData.Description,
                    Video = resData.Video,
                    Status = resData.Status,
                    UpdateDateTime = resData.UpdateDateTime,
                    Header = resData.Header,
                    Context = resData.Context,
                };
            }
            else
            {
                throw new NotFoundException("level not found!.");
            }
        }
    }
}
