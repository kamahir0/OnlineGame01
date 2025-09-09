// --- Program.cs ---

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// ▼ サービスの登録セクション ▼
// ----------------------------------------------------

// 1. コントローラー機能を使えるように登録
builder.Services.AddControllers();

// 2. Swagger/OpenAPIの機能を使えるように登録
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ----------------------------------------------------
// ▼ アプリケーションの構築 ▼
// ----------------------------------------------------
var app = builder.Build();


// ----------------------------------------------------
// ▼ HTTPリクエストパイプラインの設定セクション ▼
// ----------------------------------------------------

// 開発環境の場合のみ、Swagger UIを有効にする
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPSへのリダイレクトを有効にする
app.UseHttpsRedirection();

// 認可機能を有効にする
app.UseAuthorization();

// コントローラーへのルーティングを有効にする
app.MapControllers();


// ----------------------------------------------------
// ▼ アプリケーションの実行 ▼
// ----------------------------------------------------
// この行がプログラムの「起動スイッチ」の最後の部分です
app.Run();