using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;

namespace TextToSpeechApp.Models
{
    public class SpeechModel
    {
        public string Content { get; set; }

        public string SubscriptionKey { get; set; } = "7699ff13567c4e68a226c503b204f24c";

        [DisplayName("Language Selection :")]
        public string LanguageCode { get; set; } = "NA";

        public List<SelectListItem> LanguagePreference { get; set; } = new List<SelectListItem>
        {
        new SelectListItem { Value = "NA", Text = "-Select-" },
        new SelectListItem { Value = "en-US", Text = "English (United States)"  },
        new SelectListItem { Value = "en-IN", Text = "English (India)"  },
        //new SelectListItem { Value = "ta-IN", Text = "Tamil (India)"  },
        //new SelectListItem { Value = "hi-IN", Text = "Hindi (India)"  },
        //new SelectListItem { Value = "te-IN", Text = "Telugu (India)"  },
        new SelectListItem { Value = "ko-KR", Text = "ko-KR-HeamiRUS"  },
        new SelectListItem { Value = "es-ES", Text = "es-ES,(Spanish)"  },
        //new SelectListItem { Value = "zh-CN", Text = "Chinese (Mainland)"  }
        };
    }
}
