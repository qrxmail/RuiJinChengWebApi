using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RuiJinChengWebApi.Models;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using Quartz.Impl;
using Quartz;
using RuiJinChengWebApi.Hubs;
using Microsoft.Extensions.FileProviders;
using RuiJinChengWebApi.Services;

namespace RuiJinChengWebApi
{
    public class Startup
    {
        // ����������ƣ���������ȡ
        private readonly string OriginName = "cors";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.EnableEndpointRouting = false; });

            // ʹ��PostgreSql���ݿ�
            services.AddDbContext<RuiJinChengWebContext>(option => option.UseNpgsql(Configuration.GetConnectionString("PostgreSql")));
            // ʹ��MySql���ݿ�
            //services.AddDbContext<SingleWellWebContext>(option => option.UseMySql(Configuration.GetConnectionString("MySql")));

            // ���ʱ����8Сʱ������
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                opt.JsonSerializerOptions.Converters.Add(new JsonDateTimeConvert());

            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // ����������ַ����
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(OriginName,
            //        builder =>
            //        {
            //            builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
            //        });
            //});

            // ����ָ����ַ�Ŀ�������
            services.AddCors(options =>
            {
                options.AddPolicy(OriginName,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:8000",
                                                          "http://localhost:8090");
                                  });
            });

            // ����session
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(60 * 120);//����session�Ĺ���ʱ��
                options.Cookie.HttpOnly = true;//���������������ͨ��js��ø�cookie��ֵ
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // HttpContextAccessor Ĭ��ʵ���������˷���HttpContext
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ֧��ʹ��cshtmlҳ��
            services.AddRazorPages();

            // ע��SignalR
            services.AddSignalR();

            // ע��ISchedulerFactory��ʵ��
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            JobSchedulerWork.Work();
        }

        public void Configure(IApplicationBuilder app)
        {
            //ʹ��session
            app.UseSession();

            app.UseRouting();

            // ����ָ����ַ�Ŀ�������(signalr����Ҫָ��)
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:8000")
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST")
                    .AllowCredentials();
            });

            // CORS �м����������Ϊ�ڶ� UseRouting �� UseEndpoints�ĵ���֮��ִ�С� ���ò���ȷ�������м��ֹͣ�������С�
            app.UseCors(OriginName);

            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapControllers();
                // SignalR ��ӵ� ASP.NET Core ������ϵע��ϵͳ���м���ܵ�
                endpoints.MapHub<ChatHub>("/chathub");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();

            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);

            StaticFileOptions staticFileOptions = new StaticFileOptions();
            staticFileOptions.FileProvider = new PhysicalFileProvider(@"D:\webapi\NFCInspectServer\wwwroot");
            app.UseStaticFiles(staticFileOptions);//������̬�ļ�
        }

    }

    /// <summary>
    /// ��ǰ�˴�������ʱ��ת��Ϊ����ʱ�䣨���ʱ����8Сʱ�����⣩
    /// </summary>
    public class JsonDateTimeConvert : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var paramString = reader.GetString();

            var localDateTime = Convert.ToDateTime(paramString);

            return localDateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
