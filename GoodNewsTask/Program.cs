using GoodNewsTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");//����� ���� ����� �������

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v3/swagger.json", "����� API V3"); }); //localhost:XXXX/swagger/v3/swagger.json - ������ �� JSON-file ���������� ������ OpenApiInfo (��. ��� ����)

app.Run();
