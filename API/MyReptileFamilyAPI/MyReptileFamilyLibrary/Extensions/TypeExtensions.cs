using MyReptileFamilyLibrary.SQL.Abstractions;

namespace MyReptileFamilyLibrary.Extensions;

public static class TypeExtensions
{
    /// <summary>
    ///     Determines whether the given <see cref="Type" /> has any non-System interfaces
    /// </summary>
    /// <returns>True if it has any non-System interfaces, false otherwise</returns>
    public static bool HasNonSystemInterfaces(this Type Type)
    {
        return Type.GetInterfaces().Exists(InterfaceType => !InterfaceType.FullName!.StartsWith("System"));
    }

    /// <summary>
    ///     Determines whether the given <see cref="Type" /> only has direct interfaces
    /// </summary>
    /// <returns>True if it has interfaces not in the type's base class (or has no base class), false otherwise</returns>
    public static bool OnlyHasDirectInterfaces(this Type Type)
    {
        Type[] _interfaces = Type.GetInterfaces();
        if (Type.BaseType is null) return true;
        Type[] _baseInterfaces = Type.BaseType.GetInterfaces();

        IEnumerable<Type> _directInterfacesOnly = _interfaces.Except(_baseInterfaces);
        return _directInterfacesOnly.Any();
    }

    /// <summary>
    ///     Determines whether provided type does not implement <see cref="IDapperSQL" /> or <see cref="IDapperQuery{T}" />
    /// </summary>
    public static bool DoesNotImplementDapperInterfaces(this Type Type)
    {
        Type[] _dapperInterfaces = [typeof(IDapperSQL), typeof(IDapperStoredProcedure), typeof(IDapperTransaction)];
        Type[] _dapperOpenGenerics =
            [typeof(IDapperQuery<>), typeof(IDapperOutputStoredProcedure<>), typeof(IDapperTransaction<>)];
        Type[] _interfaces = Type.GetInterfaces();

        if (_interfaces.Intersect(_dapperInterfaces).Any()) return false;
        return !_interfaces
            .Where(Interface => Interface.IsGenericType)
            .Select(Interface => Interface.GetGenericTypeDefinition())
            .Intersect(_dapperOpenGenerics).Any();
    }
}