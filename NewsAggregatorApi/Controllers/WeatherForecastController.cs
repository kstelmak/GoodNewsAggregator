using Hangfire;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorApp.Services.Abstractions;
using Newtonsoft.Json.Linq;

namespace NewsAggregatorApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly IArticleService _articleService;
		//private readonly IArticleRateService _articleRateService;
        private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(IArticleService articleService/*, IArticleRateService articleRateService*/, ILogger<WeatherForecastController> logger)
		{
			_articleService = articleService;
			//_articleRateService = articleRateService;
			_logger = logger;
		}



		[HttpPost]
		public async Task<IActionResult> FireAndForget()
		{
			var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("hello"));
			var delayedJobId = BackgroundJob.Schedule(() => Console.WriteLine("delayed"),
				TimeSpan.FromMinutes(15));
			var jobAfterFaFId = BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("world"));

			return Ok();
		}


		[HttpGet]
		public async Task<IActionResult> RecurringJobInit()
		{
           
            //CancellationToken cancellationToken = new CancellationToken();

            ////var data = (await _articleService.GetArticleByIdAsync(Guid.Parse("3A0767D2-2AC1-468A-A24D-00179F9D1E80"), cancellationToken)).Text;
            ////await _articleRateService.CalculateArticleRate(data, cancellationToken);

            //await _articleService.RateArticlesWithoutRate(cancellationToken);

            //CancellationToken cancellationToken = new CancellationToken();
            //RecurringJob.AddOrUpdate("ArticleAggregation",
            //	() => _articleService.AggregateAsync(cancellationToken),
            //		Cron.Hourly);


            //RecurringJob.AddOrUpdate(
            //	"RssDataAggregation",
            //		() => _articleService.AggregateAsync(cancellationToken),"0/30 * * * *");
            //RecurringJob.AddOrUpdate(
            //	"WebScrapping",
            //	() => _articleService.AggregateAsync(cancellationToken),"0/30 * * * *");






            //optional algorithm for article rate
            //const string data =
            //	"Цены на авиабилеты редко приводят в восторг, но это как раз такой случай: авиабилеты первого класса туда и обратно из Австралии в США можно было купить со скидкой 85%. Обычно эти билеты стоят у авиакомпании Qantas около 19 000 долларов. Из-за сбоя около 300 счастливчиков смогли купить их на сайте авиакомпании за 3400 долларов, пока ошибку не исправили.«К сожалению, это тот случай, когда тариф был слишком хорош, чтобы быть правдой», — заявили в авиакомпании.Тем не менее Qantas не аннулировала проданные по ошибке билеты, а пообещала перебронировать их в бизнес-класс «в качестве жеста доброй воли» без дополнительной платы. Кроме того, пассажиры, которых не устраивает бизнес-класс, могут получить полный возврат денег.Перелет бизнес-классом на рейсах Qantas между Австралией и США обычно стоит около 11 000 долларов, уточняет CNN.Это не первый случай, когда авиакомпании по ошибке продавали билеты по вопиюще низким ценам. И иногда они действительно перевозили пассажиров с такими билетами.Но бывает и наоборот. Например, авиакомпания British Airways ошибочно продавала за 40 долларов билеты из Северной Америки в Индию, а когда ошибка была обнаружена, то предложила вместо этого ваучеры на 300 долларов.Авиакомпания American Airlines отказалась предоставить места первого класса из США в Австралию стоимостью до 20 000 долларов, которые она продавала по цене эконом-класса — 1100 долларов. Вместо этого она предложила ваучеры на 200 долларов.";
            //var key = "5bb1435cfbfff510af2d0ae87bf1d15aeee5bbdf";
            //using (var client = new HttpClient())
            //{
            //	var request = new HttpRequestMessage(HttpMethod.Post,
            //		$"http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey={key}");

            //	request.Headers.Add("Accept", "application/json");
            //	request.Content = JsonContent.Create(new[]
            //	{
            //		new { Text = data }
            //	});

            //	var response = await client.SendAsync(request);

            //	if (response.IsSuccessStatusCode)
            //	{
            //		var responseString = await response.Content.ReadAsStringAsync();


            //		//var lemmas = //JsonConvert.DeserializeObject<>

            //		//lemmasDictionary

            //		//based on dict calculate rate of your article

            //		return Ok(responseString);
            //	}
            //	else
            //	{
            //		return StatusCode(500);
            //	}
            //}


            return Ok();
		}
	}
}
