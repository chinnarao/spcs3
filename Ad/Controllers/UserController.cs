using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Ad;

namespace Ad.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public UserController(IConfiguration configuration, ILogger<AdController> logger, IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAds(string userIdOrEmail, string userSocialProviderName) => Ok(_userService.GetAds(userIdOrEmail, userSocialProviderName));

        [HttpGet]
        public IActionResult Activate(long adId) => Ok(_userService.Activate(adId));

        [HttpGet]
        public IActionResult DeActivate(long adId) => Ok(_userService.DeActivate(adId));

        [HttpGet]
        public IActionResult Delete(long adId) => Ok(_userService.Delete(adId));
    }
}