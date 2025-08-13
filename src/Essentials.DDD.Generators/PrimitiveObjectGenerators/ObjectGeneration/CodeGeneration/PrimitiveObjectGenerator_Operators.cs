using Essentials.DDD.Generators.Services;
using Essentials.DDD.Generators.Extensions;
using static Essentials.DDD.Generators.Services.AttributesBuilder;
using static Essentials.DDD.Generators.Dictionaries.KnownNamespaces;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration.CodeGeneration;

// ReSharper disable once InconsistentNaming
internal class PrimitiveObjectGenerator_Operators
{
    public static string Generate(PrimitiveObjectModel model)
    {
        var structTypeName = model.StructTypeGlobalName;
        var methodImplAttribute = BuildMethodImplAttribute(model.Compilation);
        var excludeFromCodeCoverageAttribute = BuildExcludeFromCodeCoverageAttribute(model.Compilation);
        
        var implementedInterfaces = model.StructType.AllInterfaces
            .Select(symbol => symbol.ToFullyQualifiedString())
            .ToList();
        
        var additionOperatorsType = model.Compilation.GetRequiredGenericTypeName(
            $"{NUMERICS}.IAdditionOperators`3",
            model.StructType,
            model.StructType,
            model.StructType);
        
        var subtractionOperatorsType = model.Compilation.GetRequiredGenericTypeName(
            $"{NUMERICS}.ISubtractionOperators`3",
            model.StructType,
            model.StructType,
            model.StructType);
        
        var multiplyOperatorsType = model.Compilation.GetRequiredGenericTypeName(
            $"{NUMERICS}.IMultiplyOperators`3",
            model.StructType,
            model.StructType,
            model.StructType);
        
        var divisionOperatorsType = model.Compilation.GetRequiredGenericTypeName(
            $"{NUMERICS}.IDivisionOperators`3",
            model.StructType,
            model.StructType,
            model.StructType);
        
        var modulusOperatorsType = model.Compilation.GetRequiredGenericTypeName(
            $"{NUMERICS}.IModulusOperators`3",
            model.StructType,
            model.StructType,
            model.StructType);
        
        var interfacesToImplement = GetInterfacesToImplement().ToList();
        if (interfacesToImplement.Count == 0)
            return string.Empty;
        
        return new CodeBuilder()
            .AppendUsing(NUMERICS).AppendUsing(COMPILER_SERVICES).AppendLine()
            .AppendNamespace(model.Namespace).AppendLine()
            .AppendWithSpace(model.Accessibility).AppendWithSpace("partial struct")
            .AppendWithSpace(model.StructTypeShortName).AppendIf(":\r\n    ", interfacesToImplement.Count > 0)
            .Append(string.Join(",\r\n    ", interfacesToImplement))
            .AppendCodeBlock(builder =>
                builder
                    .AppendIf(
                        () => BuildOperatorOverload(
                            "+",
                            structTypeName,
                            methodImplAttribute,
                            excludeFromCodeCoverageAttribute),
                        interfacesToImplement.Contains(additionOperatorsType))
                    .AppendIf(
                        () => BuildOperatorOverload(
                            "-",
                            structTypeName,
                            methodImplAttribute,
                            excludeFromCodeCoverageAttribute),
                        interfacesToImplement.Contains(subtractionOperatorsType))
                    .AppendIf(
                        () => BuildOperatorOverload(
                            "*",
                            structTypeName,
                            methodImplAttribute,
                            excludeFromCodeCoverageAttribute),
                        interfacesToImplement.Contains(multiplyOperatorsType))
                    .AppendIf(
                        () => BuildOperatorOverload(
                            "/",
                            structTypeName,
                            methodImplAttribute,
                            excludeFromCodeCoverageAttribute),
                        interfacesToImplement.Contains(divisionOperatorsType))
                    .AppendIf(
                        () => BuildOperatorOverload(
                            "%",
                            structTypeName,
                            methodImplAttribute,
                            excludeFromCodeCoverageAttribute),
                        interfacesToImplement.Contains(modulusOperatorsType)))
            .Build();
        
        IEnumerable<string> GetInterfacesToImplement()
        {
            if (!implementedInterfaces.Contains(additionOperatorsType))
                yield return additionOperatorsType;
            if (!implementedInterfaces.Contains(subtractionOperatorsType))
                yield return subtractionOperatorsType;
            if (!implementedInterfaces.Contains(multiplyOperatorsType))
                yield return multiplyOperatorsType;
            if (!implementedInterfaces.Contains(divisionOperatorsType))
                yield return divisionOperatorsType;
            if (!implementedInterfaces.Contains(modulusOperatorsType))
                yield return modulusOperatorsType;
        }
    }
    
    private static string BuildOperatorOverload(
        string @operator,
        string structTypeName,
        string methodImplAttribute,
        string excludeFromCodeCoverageAttribute)
    {
        return new CodeBuilder()
            .AppendLine()
            .AppendIndent().AppendLine(methodImplAttribute)
            .AppendIndent().AppendLine(excludeFromCodeCoverageAttribute)
            .AppendIndent().AppendWithSpace("public static").AppendWithSpace(structTypeName).AppendOperator(@operator)
            .Append("(").AppendWithSpace(structTypeName).Append("left").AppendComma()
            .AppendWithSpace(structTypeName).Append("right").Append(")")
            .AppendCodeBlock(
                builder =>
                    builder
                        .AppendIndent(2).Append("var result = checked(left._value ")
                        .Append(@operator)
                        .Append(" right._value)")
                        .AppendSemicolon()
                        .AppendLine()
                        .AppendIndent(2).Append("return new ")
                        .Append(structTypeName)
                        .Append("(result)")
                        .AppendSemicolon(),
                needIndent: true)
            .AppendLine()
            .Build();
    }
}