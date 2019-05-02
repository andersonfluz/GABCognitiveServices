using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GABCognitiveServices.BusinessLayer;
using GABCognitiveServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GABCognitiveServices.Controllers
{
    public class CognitiveServicesController : Controller
    {
        IConfiguration _iconfiguration;

        public CognitiveServicesController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }


        // GET: CognitiveServices/NovaAnalise
        public ActionResult NovaAnalise()
        {
            return View();
        }

        // POST: CognitiveServices/NovaAnalise
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovaAnalise(IFormFile upload, String urlText)
        {
            if (upload != null)
            {
                try
                {
                    ImageInfoViewModel imageInfo;
                    imageInfo = await new VisionService(_iconfiguration).MakeAnalysisRequest(upload);                    
                    ViewBag.imageFile = "data:" + upload.ContentType + ";base64, " + Convert.ToBase64String(imageInfo.imageByte, 0, imageInfo.imageByte.Length);
                    imageInfo.imageByte = null;
                    String json = JsonConvert.SerializeObject(imageInfo, Formatting.Indented);
                    ViewBag.imageInfoStr = json;
                    return View();
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                try
                {
                    ImageInfoViewModel imageInfo;
                    imageInfo = await new VisionService(_iconfiguration).MakeAnalysisRequest(null, urlText);
                    String json = JsonConvert.SerializeObject(imageInfo, Formatting.Indented);
                    ViewBag.imageInfoStr = json;
                    ViewBag.imageFile = urlText;
                    return View();
                }
                catch
                {
                    return View();
                }
            }   
            
        }

        
    }
}