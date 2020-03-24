using Hidrogen.Models;
using Hidrogen.ViewModels.Address.Generic;
using System;

namespace Hidrogen.ViewModels.Address {

    public class StandardLocationVM : GenericLocationVM {

        public int Id { get; set; }

        public string Suburb { get; set; }

        public string Postcode { get; set; }

        public string State { get; set; }

        public override string ProduceNormalizedAddress() {
            throw new System.NotImplementedException();
        }

        public static implicit operator StandardLocationVM(FineLocation location) {
            return new StandardLocationVM {
                Id = location.Id,
                BuildingName = location.BuildingName,
                StreetAddress = location.StreetAddress,
                AlternateAddress = location.AlternateAddress,
                Suburb = location.Suburb,
                State = location.State,
                Postcode = location.Postcode,
                Country = location.Country,
                Note = location.Note
            };
        }

        public static implicit operator StandardLocationVM(RawLocation location) {
            return new StandardLocationVM {
                Id = location.Id,
                BuildingName = location.BuildingName,
                StreetAddress = location.StreetAddress,
                AlternateAddress = location.AlternateAddress,
                Suburb = location.Suburb,
                State = location.State,
                Postcode = location.Postcode,
                Country = location.Country,
                Note = location.Note
            };
        }
    }
}