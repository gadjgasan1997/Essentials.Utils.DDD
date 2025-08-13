using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Essentials.DDD.Generators.Extensions;

internal static class SourceProductionContextExtensions
{
    public static SourceProductionContext AttachSource(
        this SourceProductionContext context,
        string fileName,
        Func<string> sourceTextFactory,
        bool condition = true)
    {
        if (!condition)
            return context;
        
        var sourceText = sourceTextFactory();
        if (string.IsNullOrEmpty(sourceText))
            return context;
        
        var code = SourceText.From(sourceText, Encoding.UTF8);
        context.AddSource(fileName, code);
        return context;
    }
}