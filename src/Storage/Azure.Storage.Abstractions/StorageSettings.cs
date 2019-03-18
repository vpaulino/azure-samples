namespace Azure.Storage.Abstractions
{
    public class StorageSettings
    {
        
        
        public StorageSettings(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        
        public string ConnectionString { get; }
    }
}