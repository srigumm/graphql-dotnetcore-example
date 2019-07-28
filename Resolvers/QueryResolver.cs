namespace graphql_demo.Resolvers
{
    using graphql_demo.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using graphql_demo.Repository;
    using HotChocolate;
    public class QueryResolver
    {
        public IEnumerable<CreditCard> GetAllCardsAsync([Service]IRepository workspaceRepository)
        {
            return workspaceRepository.Get();
        }
        public CreditCard GetCardAsync([Service]IRepository workspaceRepository, int Id)
        {
            return workspaceRepository.Get(Id);
        }
    }
}