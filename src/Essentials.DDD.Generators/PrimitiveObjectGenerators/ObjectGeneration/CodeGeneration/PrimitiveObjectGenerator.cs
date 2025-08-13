using Microsoft.CodeAnalysis;
using Essentials.DDD.Generators.Extensions;
using static Essentials.DDD.Generators.Services.FileNameReceiver;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration.CodeGeneration;

internal static class PrimitiveObjectGenerator
{
    public static void Generate(SourceProductionContext context, PrimitiveObjectModel model)
    {
        var guid = Guid.NewGuid().ToString("N");
        context
            .AttachSource(
                Receive(model.StructType, $"Main.{guid}"),
                () => PrimitiveObjectGenerator_Main.Generate(model))
            .AttachSource(
                Receive(model.StructType, $"Comparable.{guid}"),
                () => PrimitiveObjectGenerator_Comparable.Generate(model))
            .AttachSource(
                Receive(model.StructType, $"Conversions.{guid}"),
                () => PrimitiveObjectGenerator_Conversions.Generate(model),
                model.GenerateImplicitOperators)
            .AttachSource(
                Receive(model.StructType, $"Equatable.{guid}"),
                () => PrimitiveObjectGenerator_Equatable.Generate(model))
            .AttachSource(
                Receive(model.StructType, $"Operators.{guid}"),
                () => PrimitiveObjectGenerator_Operators.Generate(model));
    }
}