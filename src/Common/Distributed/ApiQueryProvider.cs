﻿namespace Distributed
{
    public class ApiQueryProvider<TCollection, TEntity> : DistributedQueryProvider
    {
        protected string _apiName;

        public ApiQueryProvider(string backendUri, string apiName)
            : base(backendUri)
        {
            _apiName = apiName;
        }

        public virtual TCollection Get()
        {
            var query = string.Format("/api/{0}", _apiName);
            return MakeGetQuery<TCollection>(query);
        }

        public virtual TEntity Get(int id)
        {
            var query = string.Format("/api/{0}/{1}", _apiName, id);
            return MakeGetQuery<TEntity>(query);
        }

        public virtual QueryResult Post(TEntity data)
        {
            var query = string.Format("/api/{0}", _apiName);
            return MakePostQuery<QueryResult>(query, EntityToString(data));
        }

        public virtual void Put(int id, TEntity data)
        {
            var query = string.Format("/api/{0}/{1}", _apiName, id);
            MakePutQuery(query, EntityToString(data));
        }

        public virtual void Patch(int id, TEntity data)
        {
            var query = string.Format("/api/{0}/{1}", _apiName, id);
            MakePatchQuery(query, EntityToString(data));
        }

        public virtual QueryResult Delete(int id)
        {
            var query = string.Format("/api/{0}/{1}", _apiName, id);
            return MakeDeleteQuery(query);
        }
        //protected virtual string EntityToString(TEntity entity)
        //{
        //    //return entity.ToString();
        //    return JsonConvert.SerializeObject(entity);
        //}
    }
}