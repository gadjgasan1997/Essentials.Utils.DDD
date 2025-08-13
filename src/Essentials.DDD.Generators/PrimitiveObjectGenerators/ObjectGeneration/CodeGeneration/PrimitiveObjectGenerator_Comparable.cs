using Essentials.DDD.Generators.Extensions;
using static Essentials.DDD.Generators.Services.AttributesBuilder;
using static Essentials.DDD.Generators.Dictionaries.KnownNamespaces;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration.CodeGeneration;

// ReSharper disable once InconsistentNaming
internal class PrimitiveObjectGenerator_Comparable
{
    public static string Generate(PrimitiveObjectModel model)
    {
        var structTypeName = model.StructTypeGlobalName;
        var methodImplAttribute = BuildMethodImplAttribute(model.Compilation);
        var excludeFromCodeCoverageAttribute = BuildExcludeFromCodeCoverageAttribute(model.Compilation);
        
        var comparableType = model.Compilation.GetRequiredTypeName("System.IComparable");
        var comparableGenericType = model.Compilation.GetRequiredGenericTypeName(
            "System.IComparable`1",
            model.StructType);
        
        var comparisonOperatorsType = model.Compilation.GetRequiredGenericTypeName(
            $"{NUMERICS}.IComparisonOperators`3",
            model.StructType,
            model.StructType,
            model.Compilation.GetBooleanType());
        
        var argumentExceptionType = model.Compilation.GetRequiredTypeName("System.ArgumentException");
        
        var compareToMethod = model.NeedExplicitCompareTo
            ? $"public partial int CompareTo({structTypeName} value)"
            : $"public int CompareTo({structTypeName} value) => value._value.CompareTo(_value);";
        
        return $$"""
                  #nullable enable
                  using {{NUMERICS}};
                  using {{COMPILER_SERVICES}};
                  
                  namespace {{model.Namespace}};
                  
                  {{model.Accessibility}} partial struct {{model.StructTypeShortName}} :
                      {{comparableType}},
                      {{comparableGenericType}},
                      {{comparisonOperatorsType}}
                  {
                      {{excludeFromCodeCoverageAttribute}}
                      public int CompareTo(object? value)
                      {
                          if (value is null)
                              return 1;
                          
                          if (value is {{structTypeName}} c)
                              return CompareTo(c);
                          
                          throw new {{argumentExceptionType}}("Object is not a {{model.StructTypeShortName}}", nameof(value));
                      }
                      
                      {{excludeFromCodeCoverageAttribute}}
                      {{compareToMethod}}
                      
                      {{methodImplAttribute}}
                      {{excludeFromCodeCoverageAttribute}}
                      public static bool operator >({{structTypeName}} left, {{structTypeName}} right) => left._value > right._value;
                      
                      {{methodImplAttribute}}
                      {{excludeFromCodeCoverageAttribute}}
                      public static bool operator >=({{structTypeName}} left, {{structTypeName}} right) => left._value >= right._value;
                      
                      {{methodImplAttribute}}
                      {{excludeFromCodeCoverageAttribute}}
                      public static bool operator <({{structTypeName}} left, {{structTypeName}} right) => left._value < right._value;
                      
                      {{methodImplAttribute}}
                      {{excludeFromCodeCoverageAttribute}}
                      public static bool operator <=({{structTypeName}} left, {{structTypeName}} right) => left._value <= right._value;
                  }
                  #nullable disable
                  """;
    }
}