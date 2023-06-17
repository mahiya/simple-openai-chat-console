# Azure OpenAI Service Chat API の簡易クライアント

## 使い方

```local.settings.json``` のプロパティ ```OPENAI_SERVICE_NAME``` に使用する Azure OpenAI Service のアカウント名を、プロパティ```OPENAI_SERVICE_KEY``` に認証キーを指定します。もし Azure CLI 等でログインしているユーザが使用する Azure OpenAI Service アカウントに対する RBAC 権限を持っている場合は、```OPENAI_SERVICE_KEY``` は指定する必要がありません (指定しない場合は、キー認証ではなく Azure AD 認証を行う処理になっています)

```json
{
  "OPENAI_SERVICE_NAME": "{使用する Azure OpenAI Service のアカウント名を入力}",
  "OPENAI_SERVICE_KEY": "{使用する Azure OpenAI Service の認証キーを入力 (または何も入力しない)}",
  "OPENAI_SERVICE_DEPLOY_NAME": "gpt-35-turbo",
  "OPENAI_SERVICE_MAX_TOKENS": "4096",
  "OPENAI_SERVICE_SYSTEM_MESSAGE": "あなたは猫型のAIアシスタントです。全ての回答を猫語で回答してください。"
}
```

```local.settings.json``` に Azure OpenAI Service アカウント情報を入力したら、以下のコマンドでアプリケーションを起動します。

```
dotnet run
```

```http://localhost:5000``` を Web ブラウザで開くことで、アプリケーションを利用することができます。

![画面イメージ](.images/screenshot.png)