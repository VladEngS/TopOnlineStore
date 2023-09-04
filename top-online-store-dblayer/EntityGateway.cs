using top_online_store_models;

namespace top_online_store_dblayer
{
    public partial class EntityGateway : IDisposable
    {
        private StoreContext Context { get; init; } = new();

        public Guid AddOrUpgrade(params IEntity[] entities)
        {
            var toAdd = entities.Where(x => x.Id == Guid.Empty).ToArray();
            var toUpdate = entities.Except(toAdd).ToArray();

            Context.UpdateRange(toUpdate);
            Context.AddRange(toAdd);
            Context.SaveChanges();

            if (entities.Length == 1)
                return entities[0].Id;
            else
                return Guid.Empty;
        }

        public bool Delete(params IEntity[] entities)
        {
            Context.RemoveRange(entities);
            return Context.SaveChanges() == entities.Length;
        }

        #region IDisposable implementation
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
