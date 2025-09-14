// Program.cs
using OnlineGame01;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- �T�[�r�X�̓o�^�Z�N�V���� ---

// CORS�|���V�[��ǉ�
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// DbContext�̓o�^���@��ύX
// appsettings.json��"ConnectionStrings"����ڑ�����ǂݍ���
builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT�F�؂̐ݒ���X�V
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = false, // �J�����͌��؂𖳌���
            ValidateAudience = false // �J�����͌��؂𖳌���
        };
    });


// --- �A�v���P�[�V�����̍\�z ---
var app = builder.Build();

// �A�v���P�[�V�����̃X�R�[�v���쐬
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // GameDbContext�C���X�^���X���擾
        var context = services.GetRequiredService<GameDbContext>();
        // �ۗ����̃}�C�O���[�V�������f�[�^�x�[�X�ɓK�p����
        // �f�[�^�x�[�X�����݂��Ȃ��ꍇ�́A�f�[�^�x�[�X���쐬�����
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // �G���[�n���h�����O�i�J�����̓��O�ɏo�͂���̂���ʓI�j
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during migration.");
    }
}

// --- HTTP���N�G�X�g�p�C�v���C���̐ݒ�Z�N�V���� ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS��L���ɂ���
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- �A�v���P�[�V�����̎��s ---
app.Run();