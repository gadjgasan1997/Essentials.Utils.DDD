using Essentials.Utils.Dictionaries;
using Essentials.DDD.Sample;
using Essentials.DDD.Generators.PrimitiveObjectGenerators;

[assembly: NewtonsoftJsonSerializable<Volume>(Namespace = "test_project")]
namespace Essentials.DDD.Sample;

/// <summary>
/// Модель, описывающая объем
/// </summary>
[PrimitiveObject<decimal>]
public readonly partial struct Volume
{
    public static partial bool Validate(decimal value) => value >= 0;
    
    /// <summary>
    /// Преобразует объем в формат для отображения
    /// </summary>
    /// <param name="formatting">Форматирование</param>
    /// <returns>Объем в формате для отображения</returns>
    public string ToDisplayString(Formatting formatting = Formatting.WithLiterSymbol)
    {
        var displayString = Value.ToString(Value % 1 == 0 ? "F0" : "F2", KnownCultures.Ru);
        return formatting switch
        {
            Formatting.WithLiterSymbol => $"{displayString} л",
            _ => displayString
        };
    }
    
    /// <summary>
    /// Форматирование объема
    /// </summary>
    public enum Formatting
    {
        /// <summary>
        /// С символом объема
        /// </summary>
        WithLiterSymbol,
        
        /// <summary>
        /// Без символа объема
        /// </summary>
        WithoutLiterSymbol
    }
}