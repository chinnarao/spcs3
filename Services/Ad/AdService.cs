﻿using System;
using AutoMapper;
using Share.Models.Ad.Dtos;
using DbContexts.Ad;
using Repository;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Services.Commmon;
using Share._3rdParty;
using Microsoft.Extensions.Configuration;
using Services.Common;

namespace Services.Ad
{
    public class AdService : IAdService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IFileRead _fileReadService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IGoogleStorage _googleStorage;
        private readonly IRepository<Share.Models.Ad.Entities.Ad, AdDbContext> _adRepository;
        private readonly IJsonDataService _jsonDataService;

        public AdService(ILogger<AdService> logger, IMapper mapper, ICacheService cacheService, IFileRead fileReadService, IGoogleStorage googleStorage, 
            IRepository<Share.Models.Ad.Entities.Ad, AdDbContext> adRepository,
            IConfiguration configuration, IJsonDataService jsonDataService)
        {
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _fileReadService = fileReadService;
            _cacheService = cacheService;
            _googleStorage = googleStorage;
            _adRepository = adRepository;
            _jsonDataService = jsonDataService;
        }

        public AdDto GetAdDetail(long adId)
        {
            var ad = _adRepository.Entities.AsNoTracking().FirstOrDefault(i => i.AdId == adId);
            AdDto adDto = _mapper.Map<AdDto>(ad);
            return adDto;
        }

        public AdDto UpdateAd(AdDto adDto)
        {
            Share.Models.Ad.Entities.Ad adExisting = _adRepository.Entities.Single(a => a.AdId == Convert.ToInt64(adDto.AdId));
            adExisting = _mapper.Map<AdDto, Share.Models.Ad.Entities.Ad>(adDto, adExisting);
            int i = _adRepository.SaveChanges();
            AdDto adDtoNew = _mapper.Map<AdDto>(adExisting);
            return adDtoNew;
        }

        public HashSet<string> GetAllUniqueTags()
        {
            List<dynamic> list = _adRepository.Entities.Select(a => new { a.Tag1, a.Tag2, a.Tag3, a.Tag4, a.Tag5, a.Tag6, a.Tag7, a.Tag8, a.Tag9 }).ToList<dynamic>();
            HashSet<string> t = new HashSet<string>();
            foreach (var i in list)
            {
                t.Add(i.Tag1); t.Add(i.Tag2); t.Add(i.Tag3); t.Add(i.Tag4); t.Add(i.Tag5); t.Add(i.Tag6); t.Add(i.Tag7); t.Add(i.Tag8); t.Add(i.Tag9);
            }
            return t;
        }

        public List<AdDto> GetAllAds()
        {
            var ads = _adRepository.All();
            List<AdDto> adDtos = _mapper.Map<List<AdDto>>(ads);
            return adDtos;
        }
    }

    public interface IAdService
    {
        AdDto GetAdDetail(long adId);
        AdDto UpdateAd(AdDto adDto);
        HashSet<string> GetAllUniqueTags();
        List<AdDto> GetAllAds();
    }
}
