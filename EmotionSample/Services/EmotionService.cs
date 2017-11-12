using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using System.Reflection;

namespace EmotionSample.Services
{
    public static class EmotionService
    {
        private static async Task<Emotion[]> AnalyzeImageAsync(Stream stream)
        {
            var emotionClient = new EmotionServiceClient("40d5cb0944544b6689f3eb27d499099a");
            return await emotionClient.RecognizeAsync(stream);
        } 

        public static async Task<string> GetAverageEmotionAsync(Stream stream)
        {
            var emotions = await AnalyzeImageAsync(stream);

            if (emotions == null || !emotions.Any())
            {
                return "No emotions detected";
            }

           return GetHighEmotionScore(emotions.FirstOrDefault());
        }

        private static string GetHighEmotionScore(Emotion emotion)
        {
            var properties = emotion.Scores.GetType().GetRuntimeProperties();

            return properties.OrderByDescending(prop => prop.GetValue(emotion.Scores))
                             .Select(prop => 
                                     $"Emotion : {prop.Name} \n Score : {(float)prop.GetValue(emotion.Scores) * 100}")
                             .FirstOrDefault();
        }
    }
}
