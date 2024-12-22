using MyReptileFamilyLibrary.SQL.Abstractions;

namespace MyReptileFamilyLibrary.Extensions;

public static class TypeExtensions
{
    /// <summary>
    ///     Determines whether the given <see cref="Type" /> has any non-System interfaces
    /// </summary>
    /// <returns>True if it has any non-System interfaces, false otherwise</returns>
    public static bool HasNonSystemInterfaces(this Type _p_Type) => _p_Type.GetInterfaces().Exists(_p_InterfaceType => !_p_InterfaceType.FullName!.StartsWith("System"));

    /// <summary>
    ///     Determines whether the given <see cref="Type" /> only has direct interfaces
    /// </summary>
    /// <returns>True if it has interfaces not in the type's base class (or has no base class), false otherwise</returns>
    public static bool OnlyHasDirectInterfaces(this Type _p_Type)
    {
        var _interfaces = _p_Type.GetInterfaces();
        if (_p_Type.BaseType is null) return true;
        var _baseInterfaces = _p_Type.BaseType.GetInterfaces();

        var _directInterfacesOnly = _interfaces.Except(_baseInterfaces);
        return _directInterfacesOnly.Any();
    }

    /// <summary>
    ///     Determines whether provided type does not implement <see cref="IDapperSQL"/> or <see cref="IDapperQuery{T}" />
    /// </summary>
    public static bool DoesNotImplementDapperInterfaces(this Type _p_Type)
    {
        Type[] _dapperInterfaces = [typeof(IDapperSQL), typeof(IDapperStoredProcedure), typeof(IDapperTransaction)];
        Type[] _dapperOpenGenerics = [typeof(IDapperQuery<>), typeof(IDapperOutputStoredProcedure<>), typeof(IDapperTransaction<>)];
        Type[] _interfaces = _p_Type.GetInterfaces();

        if (_interfaces.Intersect(_dapperInterfaces).Any()) return false;
        return !_interfaces
            .Where(_p_Interface => _p_Interface.IsGenericType)
            .Select(_p_Interface => _p_Interface.GetGenericTypeDefinition())
            .Intersect(_dapperOpenGenerics).Any();
    }
}