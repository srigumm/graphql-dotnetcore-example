namespace graphql_demo.Repository
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using graphql_demo.Models;
    using System.Linq;
    public class InMemoryRepository : IRepository
    {
        public InMemoryRepository()
        {
            source.Add(new CreditCard(){Id=1,Name="CashRewards",IssuedBy="Chase"});
            source.Add(new CreditCard(){Id=2,Name="TravelCard",IssuedBy="Chase"});
            source.Add(new CreditCard(){Id=3,Name="GoldCard",IssuedBy="Discover"});
            source.Add(new CreditCard(){Id=4,Name="GoldCard",IssuedBy="Discover"});
            source.Add(new CreditCard(){Id=5,Name="GoldCard",IssuedBy="Discover"});
        }
        public List<CreditCard> source = new List<CreditCard>();

        public IEnumerable<CreditCard> Get(){
            return source;
        }
        public CreditCard Get(int id){
            return source.FirstOrDefault(p=>p.Id==id);
        }
        
    }
}