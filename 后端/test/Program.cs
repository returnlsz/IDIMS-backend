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
using test.Service.Management.GovWithInfectionInfo;
using test.Service.Management.Antigen;
using test.Service.Management.HosInfo;
using test.Service.Management.MedicalStaff;
using test.Service.Management.Report;
using test.Service.Management.Reservevation;
using test.Service.Management.RiskLevel;
using test.Service.Management.UserRouting;
using Microsoft.OpenApi.Models;

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

//添加消杀信息记录依赖
builder.Services.AddTransient<IDisinfectionService,ServiceSolution>();

//添加公告相关依赖
builder.Services.AddTransient<ICreateNotice, Solution>();
builder.Services.AddTransient<IUpdateNotice, Solution>();

//添加工作处理相关依赖
builder.Services.AddTransient<IWAInterface, WASolution>();

//添加查询感染相关依赖
builder.Services.AddTransient<IGWIInterface, GWISolution>();

//添加抗原相关依赖
builder.Services.AddTransient<IAntigenService, AntigenService>();

//添加医疗点相关依赖
builder.Services.AddTransient<IHosInfoService, HosInfoService>();

//添加医疗点工作人员相关依赖
builder.Services.AddTransient<IMedicalStaffService, MedicalStaffService>();

//添加举报相关依赖
builder.Services.AddTransient<IReportService, ReportService>();

//添加预约相关依赖
builder.Services.AddTransient<IReservevationService, ReservevationService>();

//添加风险地区相关依赖
builder.Services.AddTransient<IRiskLevelService, RiskLevelService>();

//添加用户行程相关依赖
builder.Services.AddTransient<IUserRoutingService, UserRoutingService>();

//添加User依赖
builder.Services.AddTransient<IUserService, UserService>();
//验证
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

//automapper映射
builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));

//配置swagger发送Authorization报文头 
builder.Services.AddSwaggerGen(
    e =>
    {
        var scheme = new OpenApiSecurityScheme()
        {
            Description = "Authorization header.",
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Authorization" },
            Scheme = "oauth2",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
        };
        e.AddSecurityDefinition("Authorization", scheme);
        var requirement = new OpenApiSecurityRequirement();
        requirement[scheme] = new List<string>();
        e.AddSecurityRequirement(requirement);
    }
    );

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
