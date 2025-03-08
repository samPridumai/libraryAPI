using System.Data;
using Dapper;
using exam8.Extensions;
using exam8.Interfaces;
using exam8.Repositories;
using exam8.Services;
using exam8.Validators;
using Npgsql;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbConnection, NpgsqlConnection>(_ =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<BookValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<UserValidator>();
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBorrowedBookRepository, BorrowedBookRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBorrowedBookService, BorrowedBookService>();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
    InitializeDatabase(dbConnection);
}

app.MapControllers();
app.Run();




void InitializeDatabase(IDbConnection connection)
{
    var user = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM users");
    var book = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM books");

    if (user > 0 || book > 0)
    {
        Console.WriteLine("бд не пуста");
    }
    else
    {
        Console.WriteLine("бд пуста");
        SeedDatabase(connection);
    }
}

void SeedDatabase(IDbConnection connection)
{
    int userCount = 10;
    int bookCount = 15;
    var random = new Random();

    for (int i = 1; i <= userCount; i++)
    {
        var insertUser = """
                         INSERT INTO users (firstname, lastname, email, phone_number) 
                         VALUES (@FirstName, @LastName, @Email, @PhoneNumber)
                         ON CONFLICT (email) DO NOTHING;
                         """;

        var user = new
        {
            FirstName = $"User{i}",
            LastName = $"Lastname{i}",
            Email = $"user{i}@example.com",
            PhoneNumber = $"+7701{random.Next(100000, 999999)}"
        };

        connection.Execute(insertUser, user);
    }

    for (int i = 1; i <= bookCount; i++)
    {
        var insertBook = """
                         INSERT INTO books (title, author, status) 
                         VALUES (@Title, @Author, @Status)
                         ON CONFLICT (title, author) DO NOTHING;
                         """;

        var book = new
        {
            Title = $"Book {i}",
            Author = $"Author {random.Next(1, 10)}",
            Status = "available"
        };

        connection.Execute(insertBook, book);
    }
}

