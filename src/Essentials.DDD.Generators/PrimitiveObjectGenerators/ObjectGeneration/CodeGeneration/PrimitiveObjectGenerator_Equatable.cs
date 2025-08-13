using static Essentials.DDD.Generators.Services.AttributesBuilder;
using static Essentials.DDD.Generators.Dictionaries.KnownNamespaces;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration.CodeGeneration;

// ReSharper disable once InconsistentNaming
internal class PrimitiveObjectGenerator_Equatable
{
    public static string Generate(PrimitiveObjectModel model)
    {
        var structTypeName = model.StructTypeGlobalName;
        var methodImplAttribute = BuildMethodImplAttribute(model.Compilation);
        var excludeFromCodeCoverageAttribute = BuildExcludeFromCodeCoverageAttribute(model.Compilation);
        
        var equalsMethod = model.NeedExplicitEquals
            ? $"public partial bool Equals({structTypeName} other);"
            : $"public bool Equals({structTypeName} other) => _value == other._value;";
        
        return $$"""
                 #nullable enable
                 using {{COMPILER_SERVICES}};
                 
                 namespace {{model.Namespace}};
                 
                 {{model.Accessibility}} partial struct {{model.StructTypeShortName}} : IEquatable<{{structTypeName}}>
                 {
                     {{excludeFromCodeCoverageAttribute}}
                     {{equalsMethod}}
                     
                     {{excludeFromCodeCoverageAttribute}}
                     public override bool Equals(object? obj) => obj is {{structTypeName}} other && Equals(other);
                     
                     {{excludeFromCodeCoverageAttribute}}
                     public override int GetHashCode() => _value.GetHashCode();
                     
                     {{methodImplAttribute}}
                     {{excludeFromCodeCoverageAttribute}}
                     public static bool operator ==({{structTypeName}} left, {{structTypeName}} right) => left.Equals(right);
                     
                     {{methodImplAttribute}}
                     {{excludeFromCodeCoverageAttribute}}
                     public static bool operator !=({{structTypeName}} left, {{structTypeName}} right) => !(left == right);
                 }
                 #nullable disable
                 """;
    }
}