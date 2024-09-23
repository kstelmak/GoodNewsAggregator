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
            //	"���� �� ���������� ����� �������� � �������, �� ��� ��� ��� ����� ������: ���������� ������� ������ ���� � ������� �� ��������� � ��� ����� ���� ������ �� ������� 85%. ������ ��� ������ ����� � ������������ Qantas ����� 19 000 ��������. ��-�� ���� ����� 300 ������������� ������ ������ �� �� ����� ������������ �� 3400 ��������, ���� ������ �� ���������.�� ���������, ��� ��� ������, ����� ����� ��� ������� �����, ����� ���� �������, � ������� � ������������.��� �� ����� Qantas �� ������������ ��������� �� ������ ������, � ��������� ��������������� �� � ������-����� �� �������� ����� ������ ���� ��� �������������� �����. ����� ����, ���������, ������� �� ���������� ������-�����, ����� �������� ������ ������� �����.������� ������-������� �� ������ Qantas ����� ���������� � ��� ������ ����� ����� 11 000 ��������, �������� CNN.��� �� ������ ������, ����� ������������ �� ������ ��������� ������ �� ������� ������ �����. � ������ ��� ������������� ���������� ���������� � ������ ��������.�� ������ � ��������. ��������, ������������ British Airways �������� ��������� �� 40 �������� ������ �� �������� ������� � �����, � ����� ������ ���� ����������, �� ���������� ������ ����� ������� �� 300 ��������.������������ American Airlines ���������� ������������ ����� ������� ������ �� ��� � ��������� ���������� �� 20 000 ��������, ������� ��� ��������� �� ���� ������-������ � 1100 ��������. ������ ����� ��� ���������� ������� �� 200 ��������.";
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
