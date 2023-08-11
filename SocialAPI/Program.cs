using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SocialAPI.Data;
using SocialAPI.RabbitMq;
using SocialAPI.Repositories;
using SocialAPI.Repositories.Interfaces;
using SocialAPI.Services;
using SocialAPI.Startup;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwagger();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IPostRepository, PostRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient<ILikeRepository, LikeRepository>();

builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<SubscriptionService>();
builder.Services.AddTransient<MessageService>();
builder.Services.AddTransient<PostService>();
builder.Services.AddTransient<CommentService>();
builder.Services.AddTransient<LikeService>();

builder.Services.AddScoped<RabbitMqService>();
builder.Services.AddSingleton<RabbitConnection>();

var config = builder.Configuration.GetSection("Jwt");
var iss = config["ValidIssuer"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = config["ValidIssuer"],
            ValidateAudience = true,
            ValidAudience = config["ValidAudience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Key"]!)),
            ValidateIssuerSigningKey = false,
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
