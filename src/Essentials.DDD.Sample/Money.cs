using System.Numerics;
using System.Globalization;
using Essentials.Utils.Dictionaries;
using Essentials.DDD.Sample;
using Essentials.DDD.Generators.PrimitiveObjectGenerators;

[assembly: NewtonsoftJsonSerializable<Money>(Namespace = "test_project")]
namespace Essentials.DDD.Sample;

/// <summary>
/// Модель, описывающая деньги
/// </summary>
[PrimitiveObject<decimal>(GenerateImplicitOperators = true, NeedTransform = true)]
public readonly partial struct Money : IModulusOperators<Money, Money, Money>
{
    private static readonly Money _maxPercent = 100;
    private static readonly NumberFormatInfo _numberFormatInfo = NumberFormatInfo.GetInstance(KnownCultures.Ru);
    
    public static partial bool Validate(decimal value) => value >= 0;
    
    public static partial decimal Transform(decimal value) => Math.Round(value, 2);
    
    public static Money operator %(Money left, Money right) => left > right ? _maxPercent : left / right * _maxPercent;
    
    /// <summary>
    /// Преобразует цену в формат для отображения
    /// </summary>
    /// <param name="formatting">Форматирование</param>
    /// <returns>Цена в формате для отображения</returns>
    public string ToDisplayString(Formatting formatting = Formatting.WithCurrencySymbol)
    {
        var displayString = Value.ToString(Value % 1 == 0 ? "F0" : "F2", KnownCultures.Ru);
        return formatting switch
        {
            Formatting.WithCurrencySymbol => $"{displayString} {_numberFormatInfo.CurrencySymbol}",
            _ => displayString
        };
    }
    
    /// <summary>
    /// Форматирование цены
    /// </summary>
    public enum Formatting
    {
        /// <summary>
        /// С символом валюты
        /// </summary>
        WithCurrencySymbol,
        
        /// <summary>
        /// Без символа валюты
        /// </summary>
        WithoutCurrencySymbol
    }
}