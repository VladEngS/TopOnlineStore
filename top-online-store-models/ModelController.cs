using System.Reflection;

namespace top_online_store_models
{
    public class ModelController
    {
        private ModelController() { }
        public static Type[] GetModelTypes() =>
            Assembly.GetCallingAssembly()
                    .GetTypes()
                    .Where(x => typeof(IEntity).IsAssignableFrom(x) && x.IsClass)
                    .ToArray();
    }
}
