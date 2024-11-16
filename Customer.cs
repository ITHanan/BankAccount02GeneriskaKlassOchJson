
namespace BankAccount02GeneriskaKlassOchJson
{
    public class Customer : IIdentifiable
    {
        public int Id { get; set; }   // Unique identifier
        public string Name { get; set; }
        public string Address { get; set; }

        public List<int> AccountIDs { get; set; } = new List<int>();


        public Customer(int id, string name,string address) 
        {

            Id = id;
            Name = name;
            Address = address; 
        
        }


    }
    
}
