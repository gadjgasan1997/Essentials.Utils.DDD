using Essentials.DDD.Generators.Extensions;
using static Essentials.DDD.Generators.Services.AttributesBuilder;
using static Essentials.DDD.Generators.Dictionaries.KnownNamespaces;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration.CodeGeneration;

// ReSharper disable once InconsistentNaming
internal class PrimitiveObjectGenerator_Main
{
    public static string Generate(PrimitiveObjectModel model)
    {
        var primitiveObjectTypeName = model.Compilation.GetRequiredTypeName(model.PrimitiveObjectTypeName);
        var methodImplAttribute = BuildMethodImplAttribute(model.Compilation);
        var excludeFromCodeCoverageAttribute = BuildExcludeFromCodeCoverageAttribute(model.Compilation);
        var debuggerDisplayAttributeType = model.Compilation.GetRequiredTypeName("System.Diagnostics.DebuggerDisplayAttribute");
        var obsoleteAttributeType = model.Compilation.GetRequiredTypeName("System.ObsoleteAttribute");
        
        var transformBlock = model.NeedTransform
            ? $"""
               
               /// <summary>
               /// Преобразует полученное значение в требуемый формат
               /// </summary>
               public static partial {primitiveObjectTypeName} Transform({primitiveObjectTypeName} value);
               
               """
            : "";
        
        return $$"""
                 using {{NUMERICS}};
                 using {{COMPILER_SERVICES}};
                 
                 namespace {{model.Namespace}};
                 
                 [{{debuggerDisplayAttributeType}}("{_value}")]
                 {{model.Accessibility}} partial struct {{model.StructTypeShortName}}
                 {
                     private readonly {{primitiveObjectTypeName}} _value;
                     
                     {{excludeFromCodeCoverageAttribute}}
                     [{{obsoleteAttributeType}}("Нельзя создавать экземпляры моделей доменной области с использованием конструктора по-умолчанию", true)]
                     public {{model.StructTypeShortName}}() { }
                     
                     /// <summary>
                     /// Конструктор
                     /// </summary>
                     /// <param name="value">Значение</param>
                     /// <exception cref="ArgumentException">Исключение, выбрасываемое в случае невалидного значения</exception>
                     {{excludeFromCodeCoverageAttribute}}
                     public {{model.StructTypeShortName}}({{primitiveObjectTypeName}} value)
                     {
                         if (!Validate(value))
                         {
                             throw new ArgumentException(
                                 $"Передано невалидное значение '{value}' для создания объекта " + 
                                 $"с типом '{{model.StructTypeShortName}}'");
                         }
                         
                         _value = {{(model.NeedTransform ? "Transform(value)" : "value")}};
                     }
                     
                     /// <summary>
                     /// Проверяет значение на валидность
                     /// </summary>
                     public static partial bool Validate({{primitiveObjectTypeName}} value);
                     {{transformBlock}}
                     /// <summary>
                     /// Значение
                     /// </summary>
                     public {{primitiveObjectTypeName}} Value => _value;
                     
                     /// <inheritdoc cref="object.ToString" />
                     {{methodImplAttribute}}
                     {{excludeFromCodeCoverageAttribute}}
                     public override string ToString() => _value.ToString();
                 }
                 """;
    }
}