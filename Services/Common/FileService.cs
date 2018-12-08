using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Share.Utilities;

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

        public string FillContent(string content, dynamic anonymousDataObject)
        {
            var template = Scriban.Template.Parse(content);
            if (template.HasErrors)
                throw new Exception(string.Join<Scriban.Parsing.LogMessage>(',', template.Messages.ToArray()));
            string result = template.Render(anonymousDataObject);
            return result;
        }

        public string ReadJsonFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new Exception(nameof(path));
            string json = _cacheService.Get<string>(path);
            if (string.IsNullOrWhiteSpace(json))
            {
                path = Utility.HappyPath(path);
                if (!File.Exists(path))
                    throw new Exception(nameof(path));
                json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                    throw new Exception(nameof(json));
                json = _cacheService.GetOrAdd<string>(path, () => json, Utility.GetCacheExpireDateTime(_configuration["CacheExpireDays"]));
                if (string.IsNullOrEmpty(json)) throw new Exception(nameof(json));
            }
            return json;
        }
    }

    public interface IFileRead
    {
        string FillContent(string content, dynamic anonymousDataObject);
        string ReadJsonFile(string fileNameWithExtension);
    }
}
