using BookCatalogRestAPI.Services;
using Serilog;

namespace BookCatalogRestAPI
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

            // Dependecy injection
            builder.Services.AddSingleton<IBookService, BookService>();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day).CreateLogger();

            builder.Host.UseSerilog();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
