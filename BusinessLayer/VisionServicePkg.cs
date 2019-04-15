using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GABCognitiveServices.BusinessLayer
{
    public class VisionServicePkg
    {
        // subscriptionKey = "0123456789abcdef0123456789ABCDEF"
        private const string subscriptionKey = "85e15b8ef97144cebb3e907c2ee8a7c3";

        // Specify the features to return
        private static readonly List<VisualFeatureTypes> features =
            new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags
        };

        public async Task<ImageAnalysis> MakeAnalysisRequest(string localImagePath= null, string remoteImageUrl= null)
        {
            ComputerVisionClient computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { });

            computerVision.Endpoint = "https://brazilsouth.api.cognitive.microsoft.com";
            if (remoteImageUrl != null)
                return await AnalyzeRemoteAsync(computerVision, remoteImageUrl);
            else                
                return await AnalyzeLocalAsync(computerVision, localImagePath);
        }

        // Analise de uma imagem remota
        private static async Task<ImageAnalysis> AnalyzeRemoteAsync(
        ComputerVisionClient computerVision, string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Console.WriteLine(
                    "\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
                return null;
            }

            ImageAnalysis analysis =
                await computerVision.AnalyzeImageAsync(imageUrl, features);
            return analysis;
        }

        // Analise de uma imagem local
        private static async Task<ImageAnalysis> AnalyzeLocalAsync(
            ComputerVisionClient computerVision, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine(
                    "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
                return null;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                
                ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                    imageStream, features);
                return analysis;
            }
        }
    }
}
