using Azure;
using Microsoft.Azure.Cosmos;

namespace azure_web_api.Data
{
    public class EngineerService : IEngineerService
    {
        public readonly string CosmosDbConnectionString = "AccountEndpoint=https://azure-dev-cosmos-db.documents.azure.com:443/;AccountKey=UrMiRa7zkfilj5k6DhgAdSXZJQHOMpVed8Kb4fUUQiagiISx9Nx2pNHqnNNVCDirb0hPtXXUhz09ACDbCTQQjw==;";
        public readonly string CosmosDbName = "Constractors";
        public readonly string CosmosDbContainerName = "Engineers";

        private Container GetContainerClient()
        {
            var cosmosDbClient = new CosmosClient(CosmosDbConnectionString);
            var container = cosmosDbClient.GetContainer(CosmosDbName, CosmosDbContainerName);
            return container;
        }
        public async Task<string> AddEngineer(Engineer engineer)
        {
            try
            {
                engineer.id = Guid.NewGuid();
                var container = GetContainerClient();
                var response = await container.CreateItemAsync(engineer, new PartitionKey(engineer.id.ToString()));
                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public async Task<string> UpdateEngineer(Engineer engineer)
        {
            try
            {
                var container = GetContainerClient();
                var response = await container.UpsertItemAsync(engineer, new PartitionKey(engineer.id.ToString()));
                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public async Task<string> DeleteEngineer(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                var response = await container.DeleteItemAsync<Engineer>(id, new PartitionKey(partitionKey));
                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public async Task<List<Engineer>> GetEngineerDetails()
        {
            List<Engineer> engineers = new List<Engineer>();
            try
            {
                var container = GetContainerClient();
                var sqlQuery = "select * from c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<Engineer> queryResultSetIterator = container.GetItemQueryIterator<Engineer>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Engineer> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (Engineer engineer in currentResultSet)
                    {
                        engineers.Add(engineer);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return engineers;
        }
        public async Task<Engineer> GetEngineerDetailsById(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                ItemResponse<Engineer> response = await container.ReadItemAsync<Engineer>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception", ex);
            }
        }
    }
}
