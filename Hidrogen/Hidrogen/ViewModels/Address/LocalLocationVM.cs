using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.Models;
using Hidrogen.ViewModels.Address.Generic;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public List<int> VerifyGroup() {
            var errors = new List<int>();
            if (Group == null) return errors;

            Group = Group.Trim().Replace(HidroConstants.WHITE_SPACE, string.Empty);
            if (string.IsNullOrWhiteSpace(Group)) {
                Group = null;
                return errors;
            }

            if (!HelperProvider.IsNumber(Group))
                errors.Add(21);

            if (Group.Length > 3)
                errors.Add(22);

            return errors;
        }

        public List<int> VerifyLane() {
            var errors = new List<int>();
            if (Lane == null) return errors;

            Lane = Lane.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(Lane)) {
                Lane = null;
                return errors;
            }

            Lane = HelperProvider.CapitalizeFirstLetterOfEachWord(Lane);

            var lenTest = new Regex(@".{1,10}");
            if (!lenTest.IsMatch(Lane))
                errors.Add(23);

            var rx = new Regex(@"^[A-Za-z\d ]*$");
            if (!rx.IsMatch(Lane))
                errors.Add(24);

            return errors;
        }

        public List<int> VerifyQuarter() {
            var errors = new List<int>();
            if (Quarter == null) return errors;

            Quarter = Quarter.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(Quarter)) {
                Quarter = null;
                return errors;
            }

            Quarter = HelperProvider.CapitalizeFirstLetterOfEachWord(Quarter);

            var lenTest = new Regex(@".{1,40}");
            if (!lenTest.IsMatch(Quarter))
                errors.Add(25);

            var rx = new Regex(@"^[A-Za-z\d ]*$");
            if (!rx.IsMatch(Quarter))
                errors.Add(26);

            return errors;
        }

        public List<int> VerifyHamlet() {
            var errors = new List<int>();
            if (Hamlet == null) return errors;

            Hamlet = Hamlet.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(Hamlet)) {
                Hamlet = null;
                return errors;
            }

            Hamlet = HelperProvider.CapitalizeFirstLetterOfEachWord(Hamlet);

            var lenTest = new Regex(@".{1,40}");
            if (!lenTest.IsMatch(Hamlet))
                errors.Add(27);

            var rx = new Regex(@"^[A-Za-z\d ]*$");
            if (!rx.IsMatch(Hamlet))
                errors.Add(28);

            return errors;
        }

        public List<int> VerifyCommute() {
            var errors = new List<int>();
            if (Commute == null) return errors;

            Commute = Commute.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(Commute)) {
                Commute = null;
                return errors;
            }

            Commute = HelperProvider.CapitalizeFirstLetterOfEachWord(Commute);

            var lenTest = new Regex(@".{1,40}");
            if (!lenTest.IsMatch(Commute))
                errors.Add(29);

            var rx = new Regex(@"^[A-Za-z\d ]*$");
            if (!rx.IsMatch(Commute))
                errors.Add(30);

            return errors;
        }

        public List<int> VerifyWard() {
            var errors = new List<int>();
            if (Ward == null) return errors;

            Ward = Ward.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(Ward)) {
                Ward = null;
                return errors;
            }

            Ward = HelperProvider.CapitalizeFirstLetterOfEachWord(Ward);

            var lenTest = new Regex(@".{1,40}");
            if (!lenTest.IsMatch(Ward))
                errors.Add(31);

            var rx = new Regex(@"^[A-Za-z\d ]*$");
            if (!rx.IsMatch(Ward))
                errors.Add(32);

            return errors;
        }

        public List<int> VerifyDistrict() {
            var errors = new List<int>();
            if (District == null) return errors;

            District = District.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(District)) {
                District = null;
                return errors;
            }

            District = HelperProvider.CapitalizeFirstLetterOfEachWord(District);

            var lenTest = new Regex(@".{1,40}");
            if (!lenTest.IsMatch(District))
                errors.Add(33);

            var rx = new Regex(@"^[A-Za-z\d ]*$");
            if (!rx.IsMatch(District))
                errors.Add(34);

            return errors;
        }

        public List<int> VerifyTown() {
            var errors = new List<int>();
            if (Town == null) return errors;

            Town = Town.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(Town)) {
                Town = null;
                return errors;
            }

            Town = HelperProvider.CapitalizeFirstLetterOfEachWord(Town);

            var lenTest = new Regex(@".{1,40}");
            if (!lenTest.IsMatch(Town))
                errors.Add(35);

            var rx = new Regex(@"^[A-Za-z\d ]*$");
            if (!rx.IsMatch(Town))
                errors.Add(36);

            return errors;
        }

        public List<int> VerifyProvince() {
            var errors = new List<int>();
            if (Province == null) return errors;

            Province = Province.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(Province)) {
                Province = null;
                return errors;
            }

            Province = HelperProvider.CapitalizeFirstLetterOfEachWord(Province);

            var lenTest = new Regex(@".{1,40}");
            if (!lenTest.IsMatch(Province))
                errors.Add(37);

            var rx = new Regex(@"^[A-Za-z\d ]*$");
            if (!rx.IsMatch(Province))
                errors.Add(38);

            return errors;
        }

        public List<int> VerifyCity() {
            var errors = new List<int>();
            if (City == null) return errors;

            City = City.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(City)) {
                City = null;
                return errors;
            }

            City = HelperProvider.CapitalizeFirstLetterOfEachWord(City);

            var lenTest = new Regex(@".{1,40}");
            if (!lenTest.IsMatch(City))
                errors.Add(39);

            var rx = new Regex(@"^[A-Za-z\d ]*$");
            if (!rx.IsMatch(City))
                errors.Add(40);

            return errors;
        }
    }
}