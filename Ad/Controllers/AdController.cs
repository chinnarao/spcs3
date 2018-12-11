using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Share.Models.Ad.Dtos;
using Services.Ad;
using System;
using System.Collections.Generic;
using Ad.Util;
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
        private readonly IAdCreateService _adCreateService;

        public AdController(IConfiguration configuration, ILogger<AdController> logger, IAdService adService, IJsonDataService jsonDataService, 
             IAdSearchService adSearchService, IAdCreateService adCreateService)
        {
            _configuration = configuration;
            _logger = logger;
            _adService = adService;
            _jsonDataService = jsonDataService;
            _adSearchService = adSearchService;
            _adCreateService = adCreateService;
        }

        [HttpPost]
        public IActionResult CreateAd([FromBody]AdDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Errors());
            
            KeyValuePair<bool, string> kvp = model.IsValidCreateAdInputs(_configuration, _jsonDataService);
            if (kvp.Key)
                return BadRequest("In valid inputs: " + kvp.Value);

            AdDto dto = _adCreateService.CreateAd(model);
            return Ok(dto);
        }
        
        [HttpPost]
        public IActionResult SearchAds([FromBody] AdSearchDto options)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Errors());

            KeyValuePair<bool, string> kvp = options.IsValidSearchInputs(_configuration, _jsonDataService);
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