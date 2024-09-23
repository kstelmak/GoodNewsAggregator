using NewsAggregatorApp.Mappers;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorCQS.Queries.Comments;
using NewsAggregatorMVCModels;
using NewsAggregatorServices.RateClasses;
using Newtonsoft.Json;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewsAggregatorApp.Services
{
    public class ArticleRateService:IArticleRateService
    {
       
        //private readonly IArticleService _articleService;
        private const string Path = "..\\AFINN-ru.json";
        private readonly Dictionary<string, int?> afinn;

        public ArticleRateService(/*IArticleService articleService*/)
        {
            afinn = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int?>>
                (System.IO.File.ReadAllText(Path));
            //_articleService = articleService;
        }

        //public async Task SetArticleRateAsync(Guid articleId, double newRate, CancellationToken token)
        //{
        //    if (newRate is > 5 or < -5)
        //    {
        //        throw new ArgumentException("Incorrect rate", nameof(newRate));
        //    }
        //    var article = await _articleService.GetArticleByIdAsync(articleId, token);
        //    if (article != null)
        //    {
        //        await _articleService.SetRateAsync(articleId, newRate, token);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Article with that Id doesn't exist", nameof(articleId));
        //    }
        //}

        public async Task<double?> CalculateArticleRate(string data, CancellationToken token)
        {
            var key = "b5d685762b282197d0818c3f1553a1159125aa83";
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey={key}");

                request.Headers.Add("Accept", "application/json");
                request.Content = JsonContent.Create(new[]
                {
                    new { Text = data }
                });

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    var root = JsonConvert.DeserializeObject<List<Root>>(responseString);
                    var lemmas = (root[0].Annotations.Lemmas).Where(l=>l.Value!="").ToArray();

                    int summ = 0;
                    int count = 0;
                    foreach (var lemma in lemmas)
                    {
                        var word = lemma.Value;
                        if (afinn.ContainsKey(lemma.Value))
                        {
                            Console.WriteLine($"{lemma.Value} \t {afinn[lemma.Value]}");
                            summ += (int) afinn[lemma.Value];
                            count ++;
                        }
                    }
                    Console.WriteLine($"\n {summ} \t {count} \t {(double)summ / count}\n");
                    return (double)summ / count;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
