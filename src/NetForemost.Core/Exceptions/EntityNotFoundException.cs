namespace NetForemost.Core.Exceptions;

public class EntityNotFoundException : CodedException
{
    public EntityNotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }

    public override string Code => "entity-not-found";
}