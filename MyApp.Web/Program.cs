using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyApp.Core.Interfaces;
using MyApp.Infrastructure;
using MyApp.Web;
using MyApp.Web.Controllers;
using MyApp.Web.Modules;
using MyApp.Repositories;
using MyApp.Core.Interfaces;
using MyApp.Services;

var builder = WebApplication.CreateBuilder(args);

// ========== 配置 Autofac ==========
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // 注册 Autofac 模块
    //containerBuilder.RegisterModule<RepositoryModule>();
    //containerBuilder.RegisterModule<ServiceModule>();

    //// 注册控制器
    ////containerBuilder.RegisterControllers(typeof(Program).Assembly);

    //containerBuilder.RegisterType<UsersController>().InstancePerLifetimeScope();


    //仓储 ->  服务 -> 控制器
    // 注册仓储
    containerBuilder.RegisterType<UserRepository>()
                   .As<IUserRepository>()
                   .InstancePerLifetimeScope();

    // 注册服务（关键！UsersController 需要这个）
    containerBuilder.RegisterType<UserService>()
                   .As<IUserService>()
                   .InstancePerLifetimeScope();

    // 注册控制器
    containerBuilder.RegisterType<UsersController>()
                   .InstancePerLifetimeScope();



    // 注册其他组件
    //containerBuilder.RegisterType<EmailSender>()
    //               .As<IEmailSender>()
    //               .SingleInstance();
});

// ========== 配置 EF Core ==========
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========== 标准服务配置 ==========
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApp API", Version = "v1" });
});

var app = builder.Build();

// ========== 中间件配置 ==========
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyApp API V1");
        c.RoutePrefix = "";  // ← 关键！设置为空字符串
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ========== 数据库迁移 ==========
//在应用程序启动时，自动检查并应用数据库迁移，确保数据库结构与代码中的模型保持一致
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();