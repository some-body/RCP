using System.Linq;

namespace Distributed
{
    public class CustomApiQueryProvider : DistributedQueryProvider
    {
        public CustomApiQueryProvider(string backendUri)
            : base(backendUri)
        {
        }

        public virtual T Get<T>(string action, params string[] p)
        {
            var query = "";
            if (p != null && p.Any()) {
                query = string.Format("/{0}?{1}", action, string.Join(", ", p));
            }
            else
            {
                query = string.Format("/{0}", action);
            }

            return MakeGetQuery<T>(query);
        }

        public virtual TResult Post<TResult, TData>(string action, TData data)
        {
            var query = string.Format("/{0}", action);
            return MakePostQuery<TResult>(query, EntityToString(data));
        }

        protected virtual string EntityToString(object entity)
        {
            return entity.ToString();
        }
    }
}