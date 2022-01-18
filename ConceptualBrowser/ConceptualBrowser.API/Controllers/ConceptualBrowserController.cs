using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ConceptualBrowser.Business;
using ConceptualBrowser.Business.Common;
using ConceptualBrowser.Business.Entities;

namespace ConceptualBrowser.API.Controllers
{
    [Route("api/Summary")]
    [ApiController]
    public class ConceptualBrowserController : ControllerBase
    {
        private readonly ILogger<ConceptualBrowserController> _logger;

        public ConceptualBrowserController(ILogger<ConceptualBrowserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public JsonResult Summary([FromBody] string FileText)
        {
            Stopwatch Stopwatch = new Stopwatch();
            Stopwatch.Start();

            ConceptExtraction ce = new ConceptExtraction();
            List<OptimalConceptTreeItem> OptimalTree = ce.Extract(FileText, "eng", 1, null);
            Stopwatch.Stop();

            var MostOptimalConcept = OptimalTree.FirstOrDefault().OptimalConcept;
            var elapsedMilliseconds = Stopwatch.ElapsedMilliseconds;
            var summary = MostOptimalConcept.Sentences.Select(x => x.OriginalSentence).ToArray();

            return new JsonResult(new { Summary = summary, timeTaken = elapsedMilliseconds });
        }
    }
}
