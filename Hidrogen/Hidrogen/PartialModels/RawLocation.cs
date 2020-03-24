using Hidrogen.ViewModels.Address;

namespace Hidrogen.Models {

    public partial class RawLocation {

        public static implicit operator RawLocation(LocalLocationVM location) {
            return new RawLocation {
                BuildingName = location.BuildingName,
                StreetAddress = location.StreetAddress,
                AlternateAddress = location.AlternateAddress,
                Group = location.Group,
                Lane = location.Lane,
                Quarter = location.Quarter,
                Hamlet = location.Hamlet,
                Commute = location.Commute,
                Ward = location.Ward,
                District = location.District,
                Town = location.Town,
                Province = location.Province,
                City = location.City,
                Country = location.Country,
                Note = location.Note
            };
        }

        public static implicit operator RawLocation(StandardLocationVM location) {
            return new RawLocation {
                BuildingName = location.BuildingName,
                StreetAddress = location.StreetAddress,
                AlternateAddress = location.AlternateAddress,
                Suburb = location.Suburb,
                Postcode = location.Postcode,
                State = location.State,
                Country = location.Country,
                Note = location.Note
            };
        }
    }
}
