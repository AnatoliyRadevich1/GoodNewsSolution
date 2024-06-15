using GoodNewsTask.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("ConnectionStringForGoodNewsDB")!;

builder.Services.AddDbContext<NewsContext>(options => options.UseSqlServer(connection));
/*ѕри необходимости помни о выполнении миграций, так как код пишес€ методикой Code first:
 1) ”становка пакета Microsoft.EntityFrameworkCore.Tools;
 2) View->Other Windows->Package Manager Console; 
 3) Add-Migration название_миграции;
 4) Update-Database надо делать после изменени€ моделей либо таблиц дл€ Ѕƒ.*/

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

app.UseStatusCodePagesWithRedirects("~/StatusCodeError/{0}"); //переадресаци€ на несуществующие страницы
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");//потом надо будет сменить

app.Run();
