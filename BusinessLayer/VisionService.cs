using GABCognitiveServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GABCognitiveServices.BusinessLayer
{
    
    public class VisionService
    {
        IConfiguration _iconfiguration;
        string subscriptionKey = null;
        public VisionService(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            subscriptionKey = _iconfiguration["subscriptionKey"];
        }

        
        //Como o serviço foi para o datacenter do Sul do Brasil é necessario que a url seja do local brazilsouth
        const string endPoint = "https://brazilsouth.api.cognitive.microsoft.com/vision/v1.0/analyze";
        public async Task<ImageInfoViewModel> MakeAnalysisRequest(IFormFile localFile = null, string remotePath = null)
        {
            byte[] imageByte;
            if(localFile != null) {
                imageByte = await convertLocalPathToByte(localFile);
            }else
            {
                imageByte = convertRemotePathToByte(remotePath);
            }
            var errors = new List<string>();
            ImageInfoViewModel responseData = new ImageInfoViewModel();
            try
            {
                HttpClient client = new HttpClient();
                // Cabeçalho da requisição passando a chave da assinatura do Cognitive Services.    
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                // Parametros da Requisição.    
                string requestParameters = "visualFeatures=Categories,Description,Color&details=Celebrities";
                // Construção da URI para chamada no REST API.    
                string uri = endPoint + "?" + requestParameters;
                HttpResponseMessage response;
                using (ByteArrayContent content = new ByteArrayContent(imageByte))
                {
                    // Esse exemplo usa contentType "application/octet-stream".    
                    // Mas você pode usar outros contentTypes como o "application/json"    
                    // e o "multipart/form-data".    
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    // Chamada da API Rest.    
                    response = await client.PostAsync(uri, content);
                }
                // Pegando o retorno em JSON.    
                var result = await response.Content.ReadAsStringAsync();
                // Em caso de sucesso deserializa o JSON no objeto ImageInfoView
                if (response.IsSuccessStatusCode)
                {
                    responseData = JsonConvert.DeserializeObject<ImageInfoViewModel>(result, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs earg) {
                            errors.Add(earg.ErrorContext.Member.ToString());
                            earg.ErrorContext.Handled = true;
                        }
                    });
                    responseData.imageByte = imageByte;
                    responseData.OCR = await AnalisysOCR(imageByte);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            return responseData;
        }

        private async Task<byte[]> convertLocalPathToByte(IFormFile localFile)
        {
            byte[] imageByte;
            using (var memoryStream = new MemoryStream())
            {
                await localFile.CopyToAsync(memoryStream);
                imageByte = memoryStream.ToArray();
                return imageByte;
            }
        }

        private byte[] convertRemotePathToByte(string remotePath)
        {
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(remotePath);
            return imageBytes;
        }

        private async Task<OpticalCharacterRecognition> AnalisysOCR(byte[] imageByte)
        {
            OpticalCharacterRecognition responseData = new OpticalCharacterRecognition();
            var errors = new List<string>();
            try
            {
                HttpClient client = new HttpClient();
                // Cabeçalho da requisição passando a chave da assinatura do Cognitive Services.    
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                // Parametros da Requisição.    
                string requestParameters = "language=pt";
                // Construção da URI para chamada no REST API.    
                string endPointOCR = "https://brazilsouth.api.cognitive.microsoft.com/vision/v1.0/ocr";
                string uri = endPointOCR + "?" + requestParameters;
                HttpResponseMessage response;
                using (ByteArrayContent content = new ByteArrayContent(imageByte))
                {
                    // Esse exemplo usa contentType "application/octet-stream".    
                    // Mas você pode usar outros contentTypes como o "application/json"    
                    // e o "multipart/form-data".    
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    // Chamada da API Rest.    
                    response = await client.PostAsync(uri, content);
                }
                // Pegando o retorno em JSON.    
                var result = await response.Content.ReadAsStringAsync();
                // Em caso de sucesso deserializa o JSON no objeto ImageInfoView
                if (response.IsSuccessStatusCode)
                {
                    responseData = JsonConvert.DeserializeObject<OpticalCharacterRecognition>(result, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs earg) {
                            errors.Add(earg.ErrorContext.Member.ToString());
                            earg.ErrorContext.Handled = true;
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            return responseData;

        }

    }
}
