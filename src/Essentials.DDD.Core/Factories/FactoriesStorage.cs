namespace Essentials.DDD.Factories;

/// <summary>
/// Хранилище фабрик
/// </summary>
internal static class FactoriesStorage
{
    public static bool FactoriesAreConfigured;
    public static readonly object FactoriesLocker = new();
    public static readonly Dictionary<Type, object> FactoriesMap = [];
}