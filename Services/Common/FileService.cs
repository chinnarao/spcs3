using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Share.Utilities;
using Share.Extensions;

namespace Services.Commmon
{
    public class FileRead : IFileRead
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;

        public FileRead(IConfiguration configuration, ICacheService cacheService)
        {
            _configuration = configuration;
            _cacheService = cacheService;
        }

        public string ReadFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new Exception(nameof(path));
            string cacheKey = string.Concat(nameof(ReadFile), path);
            string content = _cacheService.Get<string>(cacheKey);
            if (string.IsNullOrWhiteSpace(content))
            {
                path = path.ConvertToHappyPath();
                if (!File.Exists(path))
                    throw new Exception(nameof(path));
                content = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(content))
                    throw new Exception(nameof(content));
                content = _cacheService.GetOrAdd<string>(cacheKey, () => content, _configuration["CacheExpireDays"].ConvertToCacheExpireDateTime());
                if (string.IsNullOrEmpty(content)) throw new Exception(nameof(content));
            }
            return content;
        }
    }

    public interface IFileRead
    {
        string ReadFile(string FolderPathForCountryJson);
    }
}
