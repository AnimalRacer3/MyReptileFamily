namespace MyReptileFamilyLibrary.Extensions;

/// <summary>
/// Allowing static <see cref="Array" /> methods to be invoked like extension methods.
/// </summary>
public static class ArrayExtensions
{
    /// <inheritdoc cref="Array.Exists{T}"/>
    public static bool Exists<T>(this T[] _p_Array, Predicate<T> _p_Expression) => Array.Exists(_p_Array, _p_Expression);

    /// <inheritdoc cref="Array.Find{T}"/>
    public static T? Find<T>(this T[] _p_Array, Predicate<T> _p_Match) => Array.Find(_p_Array, _p_Match);
}