using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MailMergeV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemplateController1 : Controller
    {
        [HttpGet]
        [Route("/list")]
        [Produces("application/json")]
        public IActionResult ListTemplates()
        {
            string[] aux = Directory.GetFiles("Templates", "*.*", SearchOption.TopDirectoryOnly);

            List<string> result = new List<string>(aux);
            return Ok(result);
        }

        [HttpGet]
        [Route("/getfields/{template}")]
        [Produces("application/json")]
        public IActionResult GetTemplate([FromRoute] string template)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@"Templates\" + template);
                List<string> tokens = new List<string>();
                bool processingToken = false;
                string token = "";
                char prevChar = '\0';
                foreach (char c in text)
                {
                    if (c == '{')
                    {
                        if (prevChar == '{')
                        {
                            processingToken = true;
                            prevChar = '\0';
                        }
                        else
                        {
                            prevChar = '{';
                        }
                    }
                    else if (c == '}')
                    {
                        prevChar = '\0';
                        if (processingToken)
                        {
                            processingToken = false;
                            tokens.Add(token);
                            token = "";
                        }
                    }
                    else
                    {
                        prevChar = '\0';
                        if (processingToken)
                        {
                            token += c;
                        }
                    }
                }
                return Ok(tokens);
            }
            catch
            {
                return StatusCode(500, "Error reading template");
            }

        }

        [HttpGet]
        [Route("/getcode/{template}")]
        [Produces("application/json")]
        public IActionResult GetCode([FromRoute] string template)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@"Templates\" + template);

                return Ok(text);
            }
            catch
            {
                return StatusCode(500, "Error reading template");
            }

        }
    }
}
