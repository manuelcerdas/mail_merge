using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MailMerge.Controllers
{
    public class TemplateController : ControllerBase
    {
        private readonly ILogger<TemplateController> _logger;

        public TemplateController(ILogger<TemplateController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("list")]
        [Produces("application/json")]
        public IEnumerable<string> ListTemplates()
        {
            string[] aux = Directory.GetFiles("Templates", "*.*", SearchOption.AllDirectories);

            List<string> result = new List<string>(aux);
            return result;
        }
    }
}
