
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorApp.Services;
using Serilog.Events;
using Serilog;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorApp.Entities;
using NewsAggregatorCQS.Commands.Articles;

namespace NewsAggregatorApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddScoped<IArticleService, ArticleService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<ISourceService, SourceService>();
			builder.Services.AddScoped<IUserService, UserService>();

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.File("log.log")
				.CreateBootstrapLogger();
			builder.Services.AddSerilog((services, lc) => lc
				   .ReadFrom.Configuration(builder.Configuration)
				   .ReadFrom.Services(services)
				   .Enrich.FromLogContext()
				   .WriteTo.File("log.log"));

			builder.Services.AddDbContext<AggregatorContext>(
				options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

			builder.Services.AddMediatR(cfg => {
				cfg.RegisterServicesFromAssembly(typeof(InsertUniqueArticlesFromRssDataCommand).Assembly);
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
