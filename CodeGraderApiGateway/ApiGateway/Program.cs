
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Ocelot
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile($"ocelot.UserService.{builder.Environment.EnvironmentName}.json");
            builder.Services.AddOcelot(builder.Configuration);
            // add Swagger for Ocelot
            builder.Services.AddSwaggerForOcelot(builder.Configuration);

            // Add services to the container.

            builder.Services.AddControllers();

           




            // Add CORS để frontend có thể gọi API
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
            });

            app.MapControllers();

            app.UseOcelot().Wait();

            app.Run();
        }
    }
}
