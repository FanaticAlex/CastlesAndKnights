using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Carcassone.Web.MVC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public Dictionary<string, string> MyBuilds = new Dictionary<string, string>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            string rootPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot");
            var buildFileNames = Directory.GetFiles(rootPath, "*.apk").Select(file => Path.GetFileName(file));

            foreach (var buildFileName in buildFileNames)
            {
                MyBuilds.Add(buildFileName, buildFileName);
            }
        }

        public void OnGet()
        {

        }
    }
}