using GoodNewsTask.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("ConnectionStringForGoodNewsDB")!;

builder.Services.AddDbContext<NewsContext>(options => options.UseSqlServer(connection));
/*При необходимости помни о выполнении миграций, так как код пишеся методикой Code first:
 1) Установка пакета Microsoft.EntityFrameworkCore.Tools;
 2) Убедись в том, что конструкторы моделей не содержат параметров;
 3) View->Other Windows->Package Manager Console; 
 4) Add-Migration название_миграции;
 5) Update-Database надо делать после изменения моделей либо таблиц для БД.

 Касательно Error Number:2714,State:6,Class:16 (В базе данных уже существует объект с именем "Articles" 
 см. https://stackoverflow.com/questions/26095751/there-is-already-an-object-named-migrationhistory-in-the-database )*/

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

app.UseStatusCodePagesWithRedirects("~/StatusCodeError/{0}"); //переадресация на несуществующие страницы
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");//потом надо будет сменить

app.Run();
