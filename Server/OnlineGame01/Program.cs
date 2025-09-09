// --- Program.cs ---

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// �� �T�[�r�X�̓o�^�Z�N�V���� ��
// ----------------------------------------------------

// 1. �R���g���[���[�@�\���g����悤�ɓo�^
builder.Services.AddControllers();

// 2. Swagger/OpenAPI�̋@�\���g����悤�ɓo�^
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ----------------------------------------------------
// �� �A�v���P�[�V�����̍\�z ��
// ----------------------------------------------------
var app = builder.Build();


// ----------------------------------------------------
// �� HTTP���N�G�X�g�p�C�v���C���̐ݒ�Z�N�V���� ��
// ----------------------------------------------------

// �J�����̏ꍇ�̂݁ASwagger UI��L���ɂ���
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS�ւ̃��_�C���N�g��L���ɂ���
app.UseHttpsRedirection();

// �F�@�\��L���ɂ���
app.UseAuthorization();

// �R���g���[���[�ւ̃��[�e�B���O��L���ɂ���
app.MapControllers();


// ----------------------------------------------------
// �� �A�v���P�[�V�����̎��s ��
// ----------------------------------------------------
// ���̍s���v���O�����́u�N���X�C�b�`�v�̍Ō�̕����ł�
app.Run();