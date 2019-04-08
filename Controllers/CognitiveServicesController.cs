using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GABCognitiveServices.BusinessLayer;
using GABCognitiveServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GABCognitiveServices.Controllers
{
    public class CognitiveServicesController : Controller
    {
        // GET: CognitiveServices/NovaAnalise
        public ActionResult NovaAnalise()
        {
            return View();
        }

        // POST: CognitiveServices/NovaAnalise
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovaAnalise(IFormFile upload)
        {
            try
            {
                ImageInfoViewModel imageInfo;
                byte[] imageByte;
                using (var memoryStream = new MemoryStream())
                {
                    await upload.CopyToAsync(memoryStream);
                    imageInfo = await new VisionService().MakeAnalysisRequest(memoryStream.ToArray());
                    imageByte = memoryStream.ToArray();
                }
                String json = JsonConvert.SerializeObject(imageInfo, Formatting.Indented);
                ViewBag.imageInfoStr = json;
                ViewBag.imageFile = "data:"+upload.ContentType+";base64, " + Convert.ToBase64String(imageByte, 0, imageByte.Length);
                return View();
            }
            catch
            {
                return View();
            }
        }

        
    }
}