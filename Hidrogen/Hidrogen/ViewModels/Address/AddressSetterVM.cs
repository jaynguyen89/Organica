namespace Hidrogen.ViewModels.Address {
    
    public class AddressSetterVM {
        
        public int Id { get; set; } //Id of HidroAddress to set values
        
        public int HidrogenianId { get; set; }
        
        public string Field { get; set; } //The field to set: IsPrimary or ForDelivery
    }
}