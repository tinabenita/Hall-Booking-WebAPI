
using BanquetHallProject.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//load .env file
DotNetEnv.Env.Load();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Configure connection string from environment variable
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DBCon")
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string not found. Ensure the .env file is correctly configured and placed in the root directory.");
}


    //------------------------------------
    // CONFIGURE POSTGRES CONNECTION
    builder.Services.AddDbContext<HallDbContext>(options =>
    options.UseNpgsql(connectionString));

/*builder.Services.AddDbContext<HallDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
*/
//-------------------------------------



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
