namespace MyReptileFamilyLibrary.Extensions;

/// <summary>
///     Allowing static <see cref="Array" /> methods to be invoked like extension methods.
/// </summary>
public static class ArrayExtensions
{
    /// <inheritdoc cref="Array.Exists{T}" />
    public static bool Exists<T>(this T[] ArrayBeingSearched, Predicate<T> Expression)
    {
        return Array.Exists(ArrayBeingSearched, Expression);
    }

    /// <inheritdoc cref="Array.Find{T}" />
    public static T? Find<T>(this T[] ArrayBeingSearched, Predicate<T> Match)
    {
        return Array.Find(ArrayBeingSearched, Match);
    }
}