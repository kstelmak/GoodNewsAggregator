
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorApp.Services;
using Serilog.Events;
using Serilog;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorApp.Entities;
using NewsAggregatorCQS.Commands.Articles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NewsAggregatorServices;
using NewsAggregatorServices.Abstractions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Hangfire;

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
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1.0" });
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
					$"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
				//options.AddSecurityDefinition("Bearer",
				//    new OpenApiSecurityScheme
				//    {
				//        Description =
				//            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
				//        Name = "Authorization",
				//        In = ParameterLocation.Header,
				//        Type = SecuritySchemeType.ApiKey,
				//        Scheme = "Bearer"
				//    });
				//options.AddSecurityRequirement(new OpenApiSecurityRequirement
				//{
				//    {
				//        new OpenApiSecurityScheme
				//        {
				//            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
				//        },
				//        new string[] { }
				//    }
				//});
			});

			builder.Services.AddScoped<IArticleService, ArticleService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<ISourceService, SourceService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<ITokenService, TokenService>();

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

			var jwtIss = builder.Configuration.GetSection("Jwt:Iss").Get<string>();
			var jwtAud = builder.Configuration.GetSection("Jwt:Aud").Get<string>();
			var jwtKey = builder.Configuration.GetSection("Jwt:Secret").Get<string>();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opt =>
				{
					//opt.
					opt.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = jwtIss,
						ValidAudience = jwtAud,
						IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(jwtKey))
					};
				});
			builder.Services.AddAuthorization();

			builder.Services.AddMediatR(cfg => {
				cfg.RegisterServicesFromAssembly(typeof(InsertUniqueArticlesFromRssDataCommand).Assembly);
			});

			builder.Services.AddMemoryCache();
			//(options =>
			//{
			//    options.
			//    options.SizeLimit = 
			//});
			builder.Services.AddHangfire(conf => conf
				.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UseSqlServerStorage(builder.Configuration.GetConnectionString("Default")));
			builder.Services.AddHangfireServer();

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
