# --- ステージ1: ビルド環境 ---
# .NET 9 SDKイメージをベースにする (SDKはビルドに必要な道具一式)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# プロジェクトファイルを先にコピーして、パッケージを復元する
# (こうすると、コードの変更だけの場合、毎回パッケージをダウンロードしなくて済む)
COPY ["*.csproj", "./"]
RUN dotnet restore

# 全てのソースコードをコピーして、アプリをビルドする
COPY . .
# Releaseモードで、自己完結しない形でビルド
RUN dotnet publish -c Release -o /app/publish --no-self-contained

# --- ステージ2: 実行環境 ---
# .NET 9 ASP.NETランタイムイメージをベースにする (ランタイムは実行に必要な最小限の道具)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# ビルド環境から、発行されたアプリのファイルだけをコピー
COPY --from=build /app/publish .

# コンテナが起動したときに、このコマンドでアプリを実行する
ENTRYPOINT ["dotnet", "OnlineGame01.dll"]