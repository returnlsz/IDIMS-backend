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

//�����ɱ��Ϣ��¼����
builder.Services.AddTransient<IDisinfectionService,ServiceSolution>();

//��ӹ����������
builder.Services.AddTransient<ICreateNotice, Solution>();
builder.Services.AddTransient<IUpdateNotice, Solution>();

//��ӹ��������������
builder.Services.AddTransient<IWAInterface, WASolution>();

//��Ӳ�ѯ��Ⱦ�������
builder.Services.AddTransient<IGWIInterface, GWISolution>();

//��ӿ�ԭ�������
builder.Services.AddTransient<IAntigenService, AntigenService>();

//���ҽ�Ƶ��������
builder.Services.AddTransient<IHosInfoService, HosInfoService>();

//���ҽ�Ƶ㹤����Ա�������
builder.Services.AddTransient<IMedicalStaffService, MedicalStaffService>();

//��Ӿٱ��������
builder.Services.AddTransient<IReportService, ReportService>();

//���ԤԼ�������
builder.Services.AddTransient<IReservevationService, ReservevationService>();

//��ӷ��յ����������
builder.Services.AddTransient<IRiskLevelService, RiskLevelService>();

//����û��г��������
builder.Services.AddTransient<IUserRoutingService, UserRoutingService>();

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

//����swagger����Authorization����ͷ 
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
