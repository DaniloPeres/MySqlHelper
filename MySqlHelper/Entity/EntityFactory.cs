namespace MySqlHelper.Entity
{
    public class EntityFactory
    {
        private readonly string connectionString;

        public EntityFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SelectEntityBuilder<T> CreateSelectBuilder<T>() where T : new()
        {
            return new SelectEntityBuilder<T>(connectionString);
        }

        public void Delete<T>(T model) where T : new()
        {
            DeleteEntity.Delete(connectionString, model);
        }

        public void Insert<T>(T model) where T : new()
        {
            InsertEntity.Insert(connectionString, model);
        }

        public void Update<T>(T model) where T : new()
        {
            // TODO
        }
    }
}
