using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace Carcassone.Web.MVC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public Dictionary<string, string> AndroidBuilds = new Dictionary<string, string>();
        public Dictionary<string, string> DesctopBuilds = new Dictionary<string, string>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            AndroidBuilds = GetVersions("*.apk");
            DesctopBuilds = GetVersions("*.zip");
        }

        private Dictionary<string, string> GetVersions(string pattern)
        {
            string rootPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot");
            var buildFileNames = Directory.GetFiles(rootPath, pattern).Select(file => Path.GetFileName(file));
            buildFileNames = buildFileNames.OrderByDescending(fileName =>
            {
                Regex pattern = new Regex(@"\d+(\.\d+)+");
                Match m = pattern.Match(fileName);
                string version = m.Value;
                var sortingValue = Convert.ToInt32(version.Replace(".", ""));
                return sortingValue;
            }).ToList();

            var result = new Dictionary<string, string>();
            foreach (var buildFileName in buildFileNames)
            {
                result.Add(buildFileName, buildFileName);
            }

            return result;
        }

        public void OnGet()
        {

        }
    }
}