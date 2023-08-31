using System.Security.Cryptography;
using System.Text;
using Dm;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using test.Service.Config;
using test.Service.Token;
using test.Service.User;
using test.Service.Management;
using test.Service.Management.DisinfectionRecord;
using test.Service.Management.Notice;
using test.Service.Management.WorkArrange;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));
}
);

builder.Services.Configure<JWTToken>(builder.Configuration.GetSection("JWTToken"));
builder.Services.AddTransient<IJWTService, JWTService>();

//�����ɱ��Ϣ��¼����
builder.Services.AddTransient<IDisinfectionService,ServiceSolution>();

//��ӹ����������
builder.Services.AddTransient<ICreateNotice, Solution>();
builder.Services.AddTransient<IUpdateNotice, Solution>();

//��ӹ��������������
builder.Services.AddTransient<IWAInterface, WASolution>();

//���User����
builder.Services.AddTransient<IUserService, UserService>();
//��֤
JWTToken token = new JWTToken();
builder.Configuration.Bind("JWTToken", token);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
    (options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience=true,
            ValidateLifetime=true,
            ValidateIssuerSigningKey=true,
            ValidAudience=token.Audience,
            ValidIssuer=token.Issuer,
            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.SecurityKey))
        };
    }
    );

//automapperӳ��
builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
