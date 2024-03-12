using PdfSharp.Fonts;
using PdfSharpCore.Fonts;
using PdfSharpCore.Utils;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddControllersWithViews();


var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
