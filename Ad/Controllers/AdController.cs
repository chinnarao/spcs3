using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Share.Models.Ad.Dtos;
using Services.Ad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ad.Util;
using Microsoft.AspNetCore.Authorization;
using Services.Common;

namespace Ad.Controllers
{
    //https://github.com/aspnet/Docs/blob/master/aspnetcore/fundamentals/logging/index/sample2/Controllers/TodoController.cs
    //[Authorize]
    [Route("api/[controller]/[action]")]
    public class AdController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IAdService _adService;
        private readonly IAdSearchService _adSearchService;
        private readonly IJsonDataService _jsonDataService;

        public AdController(IConfiguration configuration, ILogger<AdController> logger, IAdService adService, IAdSearchService adSearchService, IJsonDataService jsonDataService)
        {
            _configuration = configuration;
            _logger = logger;
            _adService = adService;
            _jsonDataService = jsonDataService;
            _adSearchService = adSearchService;
        }

        [HttpPost]
        public IActionResult CreateAd([FromBody]AdDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Errors());
            model.Defaults(_configuration);
            if (_jsonDataService.IsValidCategory(model.AdCategoryId))
                return BadRequest(nameof(model.AdCategoryId));
            if (_jsonDataService.IsValidCallingCode(int.Parse(model.UserPhoneCountryCode)))
                return BadRequest(nameof(model.UserPhoneCountryCode));
            model.GoogleStorageAdFileDto = new GoogleStorageAdFileDto();
            model.GoogleStorageAdFileDto.Values(_configuration, model.AttachedAssetsInCloudStorageId.Value);
            AdDto dto = _adService.CreateAd(model);
            return Ok(dto);
        }
        
        [HttpPost]
        public IActionResult SearchAds([FromBody] AdSearchDto options)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Errors());

            KeyValuePair<bool, string> kvp = options.IsValidSearchInputs(_jsonDataService);
            if (kvp.Key)
                return BadRequest( "In valid inputs: " + kvp.Value);

            var anonymous = _adSearchService.SearchAds(options);
            return Ok(anonymous);
        }

        [HttpGet("{adId}")]
        public IActionResult GetAdDetail(long adId)
        {
            if (adId <= 0) throw new ArgumentOutOfRangeException(nameof(adId));
            AdDto dto = _adService.GetAdDetail(adId);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult UpdateAd([FromBody] AdDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AdDto adDto = _adService.UpdateAd(model);
            return Ok(adDto);
        }

        [HttpGet]
        public IActionResult GetAllUniqueTags()
        {
            HashSet<string> set = _adService.GetAllUniqueTags();
            return Ok(set);
        }
        
        [HttpGet]
        public IActionResult GetAllAds()
        {
            List<AdDto> dtos = _adService.GetAllAds();
            return Ok(dtos);
        }
    }
}