using Microsoft.CodeAnalysis;
using Essentials.DDD.Generators.Services;
using Essentials.DDD.Generators.Extensions;
using static Essentials.DDD.Generators.Services.AttributesBuilder;
using static Essentials.DDD.Generators.Dictionaries.KnownNamespaces;
using static Essentials.DDD.Generators.PrimitiveObjectGenerators.Dictionaries.SerializationNamespaces;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.SerializationGeneration.NewtonsoftJson;

/// <summary>
/// Генератор конвертера Newtonsoft.Json для модели
/// </summary>
internal static class NewtonsoftJsonConverterGenerator
{
    /// <summary>
    /// Генерирует код
    /// </summary>
    /// <param name="context">Контекст</param>
    /// <param name="model">Модель с опциями сериализации</param>
    public static void Generate(SourceProductionContext context, SerializableModel model)
    {
        var compilation = model.Compilation;
        var primitiveObjectTypeName = compilation.GetRequiredTypeName(model.PrimitiveObjectTypeName);
        
        var excludeFromCodeCoverageAttribute = BuildExcludeFromCodeCoverageAttribute(compilation);
        var threadStaticAttribute = BuildThreadStaticAttribute(compilation);
        var unsafeFieldAccessorAttribute = BuildUnsafeFieldAccessorAttribute(compilation, "_value");
        
        var jsonConverterType = compilation.GetRequiredGenericTypeName(
            $"{NEWTONSOFT_JSON}.JsonConverter`1",
            model.StructType);
        
        var jsonWriterType = compilation.GetRequiredTypeName($"{NEWTONSOFT_JSON}.JsonWriter");
        var jsonReaderType = compilation.GetRequiredTypeName($"{NEWTONSOFT_JSON}.JsonReader");
        var jsonSerializerType = compilation.GetRequiredTypeName($"{NEWTONSOFT_JSON}.JsonSerializer");
        var jObjectType = compilation.GetRequiredTypeName($"{NEWTONSOFT_JSON_LINQ}.JObject");
        
        var stringComparisonType = compilation.GetRequiredTypeName($"{SYSTEM}.StringComparison");
        var exceptionType = compilation.GetRequiredTypeName($"{SYSTEM}.InvalidOperationException");
        
        var setValueMethodName = $"Set{model.StructTypeShortName}Value";
        var sourceText = $$"""
                           namespace {{model.Namespace}};
                           
                           {{excludeFromCodeCoverageAttribute}}
                           public class {{model.StructTypeShortName}}NewtonsoftJsonConverter : {{jsonConverterType}}
                           {
                               {{threadStaticAttribute}}
                               private static bool _writeDisabled;
                               
                               public override bool CanWrite => !_writeDisabled;
                               
                               public override void WriteJson(
                                   {{jsonWriterType}} writer,
                                   {{model.StructTypeGlobalName}} value,
                                   {{jsonSerializerType}} serializer)
                               {
                                   if (_writeDisabled)
                                       return;
                                   
                                   _writeDisabled = true;
                                   try
                                   {
                                       serializer.Serialize(writer, value);
                                   }
                                   finally
                                   {
                                       _writeDisabled = false;
                                   }
                               }
                               
                               public override {{model.StructTypeGlobalName}} ReadJson(
                                   {{jsonReaderType}} reader,
                                   {{SYSTEM}}.Type objectType,
                                   {{model.StructTypeGlobalName}} existingValue,
                                   bool hasExistingValue,
                                   {{jsonSerializerType}} serializer)
                               {
                                   var jObject = {{jObjectType}}.Load(reader);
                                   if (!jObject.TryGetValue("Value", {{stringComparisonType}}.Ordinal, out var token))
                                       throw new {{exceptionType}}("Ошибка десериализации типа '{{primitiveObjectTypeName}}'");
                                   
                                   var result = token.ToObject(typeof({{primitiveObjectTypeName}}), serializer);
                                   if (result is null)
                                       throw new {{exceptionType}}("Ошибка десериализации типа '{{primitiveObjectTypeName}}'");
                                   
                                   {{setValueMethodName}}(ref existingValue) = ({{primitiveObjectTypeName}}) result;
                                   return existingValue;
                               }
                               
                               {{unsafeFieldAccessorAttribute}}
                               private static extern ref {{primitiveObjectTypeName}} {{setValueMethodName}}(ref {{model.StructTypeGlobalName}} @this);
                           }
                           """;
        
        context.AttachSource(
            FileNameReceiver.Receive(model.StructType, $"NewtonsoftJsonConverter.{Guid.NewGuid():N}"),
            () => sourceText);
    }
}