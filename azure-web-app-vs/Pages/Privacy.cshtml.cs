using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace azure_web_app_vs.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string methodName = "Privacy OnGet";
            //throw new Exception("Testing Insights");
            _logger.LogDebug($"Debug {methodName}");
            _logger.LogInformation($"Information {methodName}");
            _logger.LogWarning($"Warning {methodName}");
            _logger.LogError($"Error {methodName}");
            _logger.LogCritical($"Critical {methodName}");
        }
    }
}