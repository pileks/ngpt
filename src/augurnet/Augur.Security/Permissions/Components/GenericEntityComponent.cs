namespace Augur.Security.Permissions.Components
{
    public abstract class GenericEntityComponent<T> : Component
        where T : GenericEntityComponent<T>
    {
        public static Activity Create;

        public static Activity Update;

        public static Activity Delete;

        public static Activity Get;

        public static Activity GetGrid;
    }
}