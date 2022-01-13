using BooksApi.Models;
using BooksApi.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Books API",
        Description = "An ASP.NET Core Web API for managing Books",
        TermsOfService = null,
        Contact = null,
        License = null
    });
});
builder.Services.Configure<BookstoreDatabaseSettings>(builder.Configuration.GetSection(nameof(BookstoreDatabaseSettings)));

    builder.Services.AddSingleton<IBookstoreDatabaseSettings>(sp => 
    sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);
    
builder.Services.AddSingleton<BookService>();


builder.Services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
