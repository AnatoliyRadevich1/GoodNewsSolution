using GoodNewsTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using Hangfire;
using GoodNewsTask.Controllers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("ConnectionStringForGoodNewsDB")!;

builder.Services.AddDbContext<NewsContext>(options => options.UseSqlServer(connection));
/*��� ������������� ����� � ���������� ��������, ��� ��� ��� ������ ��������� Code first:
 1) ��������� ������ Microsoft.EntityFrameworkCore.Tools;
 2) ������� � ���, ��� ������������ ������� �� �������� ����������;
 3) View->Other Windows->Package Manager Console; 
 4) Add-Migration ��������_��������;
 5) Update-Database ���� ������ ����� ��������� ������� ���� ������ ��� ��.

 ���������� Error Number:2714,State:6,Class:16 (� ���� ������ ��� ���������� ������ � ������ "Articles" 
 ��. https://stackoverflow.com/questions/26095751/there-is-already-an-object-named-migrationhistory-in-the-database )*/

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));//��������� ��������� ����������� Serilog-��

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v3", new OpenApiInfo
	{
		Version = "v3",//�������� ������ (� ����� �����)
		Title = "��������� ��� �������� ����������� ��������� GoodNewsSolution",
		Description = "���������� ������ ��� ���������� Swagger UI � GoodNewsSolution",
	});
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
	options.MinimumSameSitePolicy=SameSiteMode.None;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

//https://subbnet.ru/blogdetails/c47c57d2-c9c6-4b1e-b605-dc555f8d96ab ��� HANGFIRE ������

//builder.Services.AddHangfire(configuration => configuration.UseSqlServerStorage("ConnectionStringForGoodNewsDB"));//���������� HangFire
//builder.Services.AddHangfireServer();//���������� HangFire
builder.Services.AddHangfire(conf => conf.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                         .UseSimpleAssemblyNameTypeSerializer()
                                         .UseRecommendedSerializerSettings()
                                         .UseSqlServerStorage(builder.Configuration.GetConnectionString("ConnectionStringForGoodNewsDB")));
builder.Services.AddHangfireServer();//���������� HangFire




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePagesWithRedirects("~/StatusCodeError/{0}"); //������������� �� �������������� ��������
app.UseSerilogRequestLogging();//���������� ��������� ��� ����������� �������� � ��������������� Serilog-�

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();//���������� HangFire Dashboard
                           //��������� � ���������� ���������� DisplayController ����� DI https://zzzcode.ai/answer-question?id=78e93942-8106-4698-966e-6af27486637a
                           //var serviceProvider = app.Services; // ��������� ���������� DisplayController ����� DI
                           //var displayController = serviceProvider.GetRequiredService<DisplayController>(); // ��������� ���������� DisplayController ����� DI
app.UseHangfireServer(); //����������� � HangFire-������� (��. ������� HangFire � GoodNewsDB)

//RecurringJob.AddOrUpdate("update-view-data",() => displayController.TestHangFireMethod(),Cron.Minutely);//������������� ���������� ������ �� HangFire

//RecurringJob.AddOrUpdate("TestHangFireMethod1", () => Console.WriteLine($"������� �����: {DateTime.Now.ToString("dd.MM.yyyy, HH:mm:ss")}"), Cron.Minutely);

//RecurringJob.AddOrUpdate("filtering-job1", () => Console.WriteLine($"������� �����: {DateTime.Now.ToString("dd.MM.yyyy, HH:mm:ss")}"), Cron.Minutely);


    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");//����� ���� ����� �������

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v3/swagger.json", "����� API V3"); }); //localhost:XXXX/swagger/v3/swagger.json - ������ �� JSON-file ���������� ������ OpenApiInfo (��. ��� ����)

app.Run();
