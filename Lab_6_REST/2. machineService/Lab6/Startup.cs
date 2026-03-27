using Lab6.Database;
using Lab6.Models;
using Lab6.Repositories.Implementations;
using Lab6.Repositories.Interfaces;
using Lab6.Services.Implementations;
using Lab6.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Lab6
{
public class Startup
{
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        string connection = Configuration.GetConnectionString("DefaultConnection");

        services.AddMvc();
        services.AddDbContext<ApplicationContext>(options => {
            options.UseSqlServer(connection);
        });

        services.AddTransient<IRepairService, RepairService>();
        services.AddTransient<IBaseRepository<Document>, DocumentRepository/*BaseRepository<Document>*/>();
        services.AddTransient<IBaseRepository<Car>, BaseRepository<Car>>();
        services.AddTransient<IBaseRepository<Worker>, BaseRepository<Worker>>();

        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lab6", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab6 v1"));
        }

        app.UseStaticFiles();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }
}
}
