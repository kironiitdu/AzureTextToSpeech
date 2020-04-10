using System.Threading.Tasks;

namespace TextToSpeechApp.BusinessLayer.Interface
{
    public interface ITextToSpeech
    {
        Task<byte[]> TranslateText(string token, string key, string content, string lang);
    }
}
