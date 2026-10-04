#!/usr/bin/env bash
# Gera o CalculadoraClaude.app (autocontido, não precisa de .NET instalado) na pasta ./dist
set -euo pipefail
cd "$(dirname "$0")"

RID="${1:-$( [ "$(uname -m)" = arm64 ] && echo osx-arm64 || echo osx-x64 )}"
APP="dist/CalculadoraClaude.app"

rm -rf "$APP" dist/publish
dotnet publish src/CalculadoraClaude -c Release -r "$RID" --self-contained true -o dist/publish

mkdir -p "$APP/Contents/MacOS" "$APP/Contents/Resources"
cp -R dist/publish/. "$APP/Contents/MacOS/"
cp packaging/Info.plist "$APP/Contents/Info.plist"
[ -f packaging/AppIcon.icns ] && cp packaging/AppIcon.icns "$APP/Contents/Resources/"

chmod +x "$APP/Contents/MacOS/CalculadoraClaude"
codesign --force --deep --sign - "$APP"   # assinatura ad-hoc local
rm -rf dist/publish
echo "Pronto: $APP"
