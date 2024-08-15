using Ardalis.GuardClauses;
using NetForemost.Core.Exceptions;

namespace NetForemost.Core.Guards;

public static class EntityNotFoundGuard
{
    public static void EntityNotFound<T>(this IGuardClause guardClause, object? entity, object key)
    {
        if (entity is null) throw new EntityNotFoundException(typeof(T).Name, key);
    }
}