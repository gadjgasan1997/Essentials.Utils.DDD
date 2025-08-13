using Microsoft.CodeAnalysis;

namespace Essentials.DDD.Generators.Diagnostics;

/// <summary>
/// Хелперы для формирования диагностических сообщений
/// </summary>
internal static class DiagnosticsHelper
{
    public static DiagnosticDescriptor CreateInfo(string title, string content) =>
        Create(title, content, DiagnosticSeverity.Info);
    
    public static DiagnosticDescriptor Create(string title, string content, DiagnosticSeverity severity)
    {
        return new DiagnosticDescriptor(
            "utils_ddd_generators_message",
            title,
            content,
            "utils_ddd_generators_category",
            severity,
            true);
    }
}