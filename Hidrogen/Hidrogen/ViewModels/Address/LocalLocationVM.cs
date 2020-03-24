using Hidrogen.Models;
using Hidrogen.ViewModels.Address.Generic;
using System;

namespace Hidrogen.ViewModels.Address {

    public class LocalLocationVM : GenericLocationVM {

        public int Id { get; set; }

        public string Group { get; set; }

        public string Lane { get; set; }

        public string Quarter { get; set; }

        public string Hamlet { get; set; }

        public string Commute { get; set; }

        public string Ward { get; set; }

        public string District { get; set; }

        public string Town { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public override string ProduceNormalizedAddress() {
            throw new NotImplementedException();
        }

        public static implicit operator LocalLocationVM(FineLocation location) {
            return new LocalLocationVM {
                Id = location.Id,
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

        public static implicit operator LocalLocationVM(RawLocation location) {
            return new LocalLocationVM {
                Id = location.Id,
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
    }
}