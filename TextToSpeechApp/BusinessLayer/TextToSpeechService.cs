using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TextToSpeechApp.BusinessLayer.Interface;

namespace TextToSpeechApp.BusinessLayer
{
    public class TextToSpeechService : ITextToSpeech
    {
        /// <summary>
        /// Translate text to speech
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="key">Azure subscription key</param>
        /// <param name="content">Text content for speech</param>
        /// <param name="lang">Speech conversion language</param>
        /// <returns></returns>
        public async Task<byte[]> TranslateText(string token, string key, string content, string lang)
        {
            //Request url for the speech api.
            string uri = "https://westus.tts.speech.microsoft.com/cognitiveservices/v1";
            //Generate Speech Synthesis Markup Language (SSML) 
            var requestBody = this.GenerateSsml(lang, "Female", this.ServiceName(lang), content);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Add("X-Microsoft-OutputFormat", "audio-16khz-64kbitrate-mono-mp3");
                request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");
                request.Content.Headers.Remove("Content-Type");
                request.Content.Headers.Add("Content-Type", "application/ssml+xml");
                request.Headers.Add("User-Agent", "TexttoSpeech");
                var response = await client.SendAsync(request);
                var httpStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                Stream receiveStream = httpStream;
                byte[] buffer = new byte[32768];

                using (Stream stream = httpStream)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] waveBytes = null;
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = stream.Read(buf, 0, 1024);
                            ms.Write(buf, 0, count);
                        } while (stream.CanRead && count > 0);

                        waveBytes = ms.ToArray();

                        return waveBytes;
                    }
                }
            }
        }

        /// <summary>
        /// Genereate Speech Synthesis Markup Language (SSML)
        /// </summary>
        /// <param name="lang">Language code ( Eg.en-US )</param>
        /// <param name="gender">Gender (Eg. Male or Female )</param>
        /// <param name="name">Service name mapping</param>
        /// <param name="text">Text for speech</param>
        /// <returns></returns>
        private string GenerateSsml(string lang, string gender, string name, string text)
        {
            var ssmlDoc = new XDocument(
                              new XElement("speak",
                                  new XAttribute("version", "1.0"),
                                  new XAttribute(XNamespace.Xml + "lang", "en-US"),
                                  new XElement("voice",
                                      new XAttribute(XNamespace.Xml + "lang", lang),
                                      new XAttribute(XNamespace.Xml + "gender", gender),
                                      new XAttribute("name", name),
                                      text)));
            return ssmlDoc.ToString();
        }

        /// <summary>
        /// Select appropriate service name mapping value from the given langaguge code.
        /// </summary>
        /// <param name="lang">Code for language</param>
        /// <returns></returns>
        private string ServiceName(string lang)
        {
            string description = string.Empty;
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("en-US", "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)");
            values.Add("en-IN", "Microsoft Server Speech Text to Speech Voice (en-IN, Heera, Apollo)");
            //values.Add("ta-IN", "Microsoft Server Speech Text to Speech Voice (ta-IN, Valluvar)");
            //values.Add("hi-IN", "Microsoft Server Speech Text to Speech Voice (hi-IN, Kalpana)");
            //values.Add("te-IN", "Microsoft Server Speech Text to Speech Voice (te-IN, Chitra)");
            values.Add("ko-KR", "Microsoft Server Speech Text to Speech Voice(ko-KR, HeamiRUS)");
           // values.Add("zh-CN", "Microsoft Server Speech Text to Speech Voice (zh-CN, XiaoxiaoNeural)");
           values.Add("es-ES", "Microsoft Server Speech Text to Speech Voice (es-ES, Laura, Apollo)");
            values.TryGetValue(lang, out description);
            return description;
        }
    }
}
