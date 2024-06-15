using GoodNewsTask.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("ConnectionStringForGoodNewsDB")!;

builder.Services.AddDbContext<NewsContext>(options => options.UseSqlServer(connection));
/*��� ������������� ����� � ���������� ��������, ��� ��� ��� ������ ��������� Code first:
 1) ��������� ������ Microsoft.EntityFrameworkCore.Tools;
 2) View->Other Windows->Package Manager Console; 
 3) Add-Migration ��������_��������;
 4) Update-Database ���� ������ ����� ��������� ������� ���� ������ ��� ��.*/

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
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");//����� ���� ����� �������

app.Run();
