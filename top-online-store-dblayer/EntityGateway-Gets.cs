using top_online_store_models;

namespace top_online_store_dblayer
{
    public partial class EntityGateway
    {
        public T[] GetTable<T>(Func<T, bool> predicate) where T : class, IEntity =>
            Context.Set<T>().Where(predicate).ToArray();

        public T[] GetTable<T>() where T : class, IEntity =>
            Context.Set<T>().ToArray();
    }
}
