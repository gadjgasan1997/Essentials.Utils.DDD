using Microsoft.CodeAnalysis;

namespace Essentials.DDD.Generators.Services;

/// <summary>
/// Сервис для получения названия генерируемого файла
/// </summary>
internal static class FileNameReceiver
{
    public static string Receive(INamedTypeSymbol symbol, string postfix) => $"{symbol.Name}.{postfix}";
}