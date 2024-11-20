using System.Text.Json;

namespace BankAccount02GeneriskaKlassOchJson
{
    public class BankGeneriskAdministration<T> where T : IIdentifiable
    {
        private List<T> _items = new List<T>();

        public void AddTo(T item)
        {
            _items.Add(item);
        }

        public List<T> GetAll() 
        {

         return _items;
        
        }

        public T GetByID(int id) 
        { 
         
         return _items.FirstOrDefault(x => x.Id == id)!;

        }

        public void Updater(T updatedList) 
        {
            var TheExistinItem = GetByID(updatedList.Id);
            if (TheExistinItem != null) 
            {
                int index = _items.IndexOf(TheExistinItem);
                _items[index]= updatedList; 
            }
        }

        public void RemoveThis(int id) 
        {
         var item = GetByID(id);
            if (item != null)
            {
              _items.Remove(item);
            
            }
        
        }
        public void SaveAllData(BankDB bankDB)
        {
            string dataJsonFilePath = "BankAccountData.json";

            string updatedBankDB = JsonSerializer.Serialize(bankDB, new JsonSerializerOptions { WriteIndented = true });


            File.WriteAllText(dataJsonFilePath, updatedBankDB);

            Console.WriteLine("The data has been saved");

        }

    }
}
