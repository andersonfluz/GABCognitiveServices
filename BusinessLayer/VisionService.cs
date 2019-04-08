using GABCognitiveServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GABCognitiveServices.BusinessLayer
{
    public class VisionService
    {
        const string subscriptionKey = "85e15b8ef97144cebb3e907c2ee8a7c3";
        //Como o serviço foi para o datacenter do Sul do Brasil é necessario que a url seja do local brazilsouth
        const string endPoint = "https://brazilsouth.api.cognitive.microsoft.com/vision/v1.0/analyze";
        public async Task<ImageInfoViewModel> MakeAnalysisRequest(byte[] byteData)
        {
            
            var errors = new List<string>();
            ImageInfoViewModel responeData = new ImageInfoViewModel();
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
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".    
                    // The other content types you can use are "application/json"    
                    // and "multipart/form-data".    
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    // Make the REST API call.    
                    response = await client.PostAsync(uri, content);
                }
                // Get the JSON response.    
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    responeData = JsonConvert.DeserializeObject<ImageInfoViewModel>(result, new JsonSerializerSettings
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
            return responeData;
        }
    }
}
