var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Handle the root route ("/") and return HTML content
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        // HTML content as a string
        string htmlContent = File.ReadAllText("index.html");

        // Set the content type to HTML
        context.Response.ContentType = "text/html";

        // Return the HTML content
        await context.Response.WriteAsync(htmlContent);
    }
    else
    {
        // Continue processing for other routes
        await next.Invoke();
    }
});

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyHeader();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
