namespace Augur.Security.Permissions.Components
{
    public abstract class EmptyComponent<T> : Component
        where T : EmptyComponent<T>
    {
    }
}