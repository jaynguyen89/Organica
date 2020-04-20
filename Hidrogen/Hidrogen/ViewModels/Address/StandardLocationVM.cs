using System.Collections.Generic;
using System.Text.RegularExpressions;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.Models;
using Hidrogen.ViewModels.Address.Generic;

namespace Hidrogen.ViewModels.Address {

    public class StandardLocationVM : GenericLocationVM {

        public int Id { get; set; }

        public string Suburb { get; set; }

        public string Postcode { get; set; }

        public string State { get; set; }

        public override string ProduceNormalizedAddress() {
            var po = string.IsNullOrEmpty(PoBox) ? string.Empty : PoBox + ", ";
            var building = string.IsNullOrEmpty(BuildingName) ? string.Empty : BuildingName + ", ";
            var street = string.IsNullOrEmpty(StreetAddress) ? string.Empty : StreetAddress + ", ";
            var suburb = string.IsNullOrEmpty(Suburb) ? string.Empty : Suburb + ", ";
            var state = string.IsNullOrEmpty(State) ? string.Empty : State + " ";
            var post = string.IsNullOrEmpty(Postcode) ? string.Empty : Postcode + ", ";
            var country = string.IsNullOrEmpty(Country.Name) ? string.Empty : Country.Name;

            return $"{po}{building}{street}{suburb}{state}{post}{country}";
        }

        public static implicit operator StandardLocationVM(FineLocation location) {
            return new StandardLocationVM {
                Id = location.Id,
                PoBox = location.PoBoxNumber,
                BuildingName = location.BuildingName,
                StreetAddress = location.StreetAddress,
                AlternateAddress = location.AlternateAddress,
                Suburb = location.Suburb,
                State = location.State,
                Postcode = location.Postcode,
                Country = new CountryVM {
                    Id = location.CountryId,
                    Name = location.Country.CountryName,
                    Code = location.Country.CountryCode
                }
            };
        }

        public static implicit operator StandardLocationVM(RawLocation location) {
            return new StandardLocationVM {
                Id = location.Id,
                PoBox = location.PoBoxNumber,
                BuildingName = location.BuildingName,
                StreetAddress = location.StreetAddress,
                AlternateAddress = location.AlternateAddress,
                Suburb = location.Suburb,
                State = location.State,
                Postcode = location.Postcode,
                Country = new CountryVM {
                    Id = location.CountryId,
                    Name = location.Country.CountryName,
                    Code = location.Country.CountryCode
                }
            };
        }

        public List<int> VerifySuburb() {
            var errors = new List<int>();
            if (Suburb == null) return new List<int> { 12 };

            Suburb = Suburb.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(Suburb))
                return new List<int> { 12 };

            Suburb = HelperProvider.CapitalizeFirstLetterOfEachWord(Suburb);

            var lenTest = new Regex(@".{1,50}");
            if (!lenTest.IsMatch(Suburb))
                errors.Add(13);

            var rx = new Regex(@"^[A-Za-z'.\- ]*$");
            if (!rx.IsMatch(Suburb))
                errors.Add(14);

            return errors;
        }

        public List<int> VerifyPostcode() {
            var errors = new List<int>();
            if (Postcode == null) return new List<int> { 15 };

            Postcode = Postcode.Trim().Replace(HidroConstants.WHITE_SPACE, string.Empty);
            if (string.IsNullOrWhiteSpace(Postcode))
                return new List<int> { 15 };

            var lenTest = new Regex(@".{1,10}");
            if (!lenTest.IsMatch(Postcode))
                errors.Add(16);

            var rx = new Regex(@"^[\d]*$");
            if (!rx.IsMatch(Postcode))
                errors.Add(17);

            return errors;
        }

        public List<int> VerifyState() {
            var errors = new List<int>();
            if (State == null) return new List<int> { 18 };

            State = State.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(State))
                return new List<int> { 18 };

            State = HelperProvider.CapitalizeFirstLetterOfEachWord(State);

            var lenTest = new Regex(@".{1,40}");
            if (!lenTest.IsMatch(State))
                errors.Add(19);

            var rx = new Regex(@"^[A-Za-z'.\- ]*$");
            if (!rx.IsMatch(State))
                errors.Add(20);

            return errors;
        }
    }
}