using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace SimpleWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleWebApplication", Version = "v1" });
            });

            //�Զ���װActionResult
            services.AddResponseAutoWrapper(options =>
            {
                //����
                //options.ActionNoWrapPredicate Action��ɸѡί�У�Ĭ�ϻ���˵������NoResponseWrapAttribute�ķ���
                //options.DisableOpenAPISupport ����OpenAPI֧�֣�Swagger��������ʾ��װ��ĸ�ʽ��Ҳ������Ӧ���ͱ���Ϊobject���͵�����
                //options.HandleAuthorizationResult ������Ȩ�����������Ч����Ҫ���в��ԣ�
                //options.HandleInvalidModelState ������Чģ��״̬
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //�����쳣����200״̬������󣬰�װ��Ӧ
            app.UseResponseAutoWrapper(options =>
            {
                //����
                //options.MiddlewareStatusCodePredicate ״̬���ɸѡί�У�Ĭ�ϻ���˵�3**��״̬��
                //options.CatchExceptions �Ƿ񲶻��쳣
                //options.ThrowCaughtExceptions �����쳣����������Ƿ��ٽ��쳣�׳�
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleWebApplication v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
