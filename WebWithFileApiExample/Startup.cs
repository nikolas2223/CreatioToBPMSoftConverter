using WebWithFileApiExample.Interfaces;
using WebWithFileApiExample.Helpers;
using WebWithFileApiExample.HostedServices;

namespace WebWithFileApiExample
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Добавлен Windows Event логер
            services.AddLogging(builder => builder.AddEventLog(settings =>
            {
                settings.SourceName = "WebConverterLogs";
                settings.LogName = "WebConverterApplicationLog";
            }));


            services.AddSingleton<IFileHelper, FileHelper>();
            services.AddSingleton<IProcessHelper, ProcessHelper>(implementationFactory =>
            {
                var clioPath = Configuration["Utilities:ClioPath"];
                var changerPath = Configuration["Utilities:ClassChangerPath"];
                var processLogger = LoggerFactory.Create(builder =>
                builder.AddEventLog(settings =>
                {
                    settings.SourceName = "WebConverterLogs";
                    settings.LogName = "WebConverterApplicationProcessLog";
                })).CreateLogger<ProcessHelper>();
                return new ProcessHelper(clioPath, changerPath, processLogger);
            });

            //Сервис очистки папки с обработанными пакетами раз в N минут
            services.AddHostedService<EraseDirectoryService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
