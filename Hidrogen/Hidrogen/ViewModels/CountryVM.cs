namespace Hidrogen.ViewModels {
    
    public class CountryVM {
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Code { get; set; }

        public string CombinedName => $"{Code} - {Name}";
        
        public string Continent { get; set; }
        
        public CurrencyVM Currency { get; set; }
    }

    public class CurrencyVM {
        
        public string Name { get; set; }
        
        public string Code { get; set; }
    }
}