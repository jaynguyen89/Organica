using System.Collections.Generic;
using System.Text.RegularExpressions;
using HelperLibrary;
using HelperLibrary.Common;

namespace Hidrogen.ViewModels.Address.Generic {

    public abstract class GenericLocationVM {

        public string BuildingName { get; set; }

        public string StreetAddress { get; set; }

        public string AlternateAddress { get; set; }

        public CountryVM Country { get; set; }

        public abstract string ProduceNormalizedAddress();

        public List<int> VerifyBuildingName() {
            var errors = new List<int>();
            if (BuildingName == null) return errors;

            BuildingName = BuildingName.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(BuildingName)) {
                BuildingName = null;
                return errors;
            }

            BuildingName = HelperProvider.CapitalizeFirstLetterOfEachWord(BuildingName);

            var lenTest = new Regex(@".{1,50}");
            if (!lenTest.IsMatch(BuildingName))
                errors.Add(0);

            var rx = new Regex(@"^[A-Za-z\d\-.,'() ]*$");
            if (!rx.IsMatch(BuildingName))
                errors.Add(1);

            return errors;
        }

        public List<int> VerifyStreetAddress() {
            var errors = new List<int>();
            if (StreetAddress == null) return new List<int> { 2 };

            StreetAddress = StreetAddress.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(StreetAddress))
                return new List<int> { 2 };

            StreetAddress = HelperProvider.CapitalizeFirstLetterOfEachWord(StreetAddress);

            var lenTest = new Regex(@".{1,50}");
            if (!lenTest.IsMatch(StreetAddress))
                errors.Add(3);

            var rx = new Regex(@"^[A-Za-z\d\-/.,'() ]*$");
            if (!rx.IsMatch(StreetAddress))
                errors.Add(4);

            return errors;
        }

        public List<int> VerifyAltAddress() {
            var errors = new List<int>();
            if (AlternateAddress == null) return errors;

            AlternateAddress = AlternateAddress.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(AlternateAddress)) {
                AlternateAddress = null;
                return errors;
            }

            AlternateAddress = HelperProvider.CapitalizeFirstLetterOfEachWord(AlternateAddress);

            var lenTest = new Regex(@".{1,50}");
            if (!lenTest.IsMatch(AlternateAddress))
                errors.Add(5);

            var rx = new Regex(@"^[A-Za-z\d\-.,'() ]*$");
            if (!rx.IsMatch(AlternateAddress))
                errors.Add(6);

            return errors;
        }

        public List<string> GenerateErrorMessages(List<int> errors) {
            var messages = new List<string>();

            //For BuildingName
            if (errors.Contains(0)) messages.Add("Building Name is too long. Max 50 characters.");
            if (errors.Contains(1)) messages.Add("Building Name con only contain these special characters: -.,'()");
            
            //For StreetAddress
            if (errors.Contains(2)) messages.Add("Street Address is missing. This field is required.");
            if (errors.Contains(3)) messages.Add("Street Address is too long. Max 50 characters.");
            if (errors.Contains(4)) messages.Add("Street Address con only contain these special characters: -.,'()");
            
            //For AlternateAddress
            if (errors.Contains(5)) messages.Add("Alternate Address is too long. Max 50 characters.");
            if (errors.Contains(6)) messages.Add("Alternate Address can only contain these special characters: -.,'()");
            
            //For Suburb
            if (errors.Contains(12)) messages.Add("Suburb is missing. This field is required.");
            if (errors.Contains(13)) messages.Add("Suburb is too long. Max 50 characters.");
            if (errors.Contains(14)) messages.Add("Suburb can only contains thesse special characters: '.-");

            //For Postcode
            if (errors.Contains(15)) messages.Add("Postcode is missing. This field is requried.");
            if (errors.Contains(16)) messages.Add("Postcode is too long. Max 10 characters.");
            if (errors.Contains(17)) messages.Add("Postcode can only contain digits.");

            //For State
            if (errors.Contains(18)) messages.Add("State is missing. This field is required.");
            if (errors.Contains(19)) messages.Add("State is too long. Max 40 characters.");
            if (errors.Contains(20)) messages.Add("State can only contain these special characters: '.-");

            //For Group
            if (errors.Contains(21) || errors.Contains(22)) messages.Add("Group should be a number with max 3 digits.");

            //For Lane
            if (errors.Contains(23)) messages.Add("Lane is too long. Max 10 characters.");
            if (errors.Contains(24)) messages.Add("Lane is NOT allowed to have special characters.");

            //For Quarter
            if (errors.Contains(25)) messages.Add("Quarter is too long. Max 40 characters.");
            if (errors.Contains(26)) messages.Add("Quarter is NOT allowed to have special characetrs.");

            //For Hamlet
            if (errors.Contains(27)) messages.Add("Hamlet is too long. Max 40 characters.");
            if (errors.Contains(28)) messages.Add("Hamlet is NOT allowed to have special characetrs.");

            //For Commute
            if (errors.Contains(29)) messages.Add("Commute is too long. Max 40 characters.");
            if (errors.Contains(30)) messages.Add("Commute is NOT allowed to have special characetrs.");

            //For Ward
            if (errors.Contains(31)) messages.Add("Ward is too long. Max 40 characters.");
            if (errors.Contains(32)) messages.Add("Ward is NOT allowed to have special characetrs.");

            //For District
            if (errors.Contains(33)) messages.Add("District is too long. Max 40 characters.");
            if (errors.Contains(34)) messages.Add("District is NOT allowed to have special characetrs.");

            //For Town
            if (errors.Contains(35)) messages.Add("Town is too long. Max 40 characters.");
            if (errors.Contains(36)) messages.Add("Town is NOT allowed to have special characetrs.");

            //For Province
            if (errors.Contains(37)) messages.Add("Province is too long. Max 40 characters.");
            if (errors.Contains(38)) messages.Add("Province is NOT allowed to have special characetrs.");

            //For City
            if (errors.Contains(39)) messages.Add("City is too long. Max 40 characters.");
            if (errors.Contains(40)) messages.Add("City is NOT allowed to have special characetrs.");

            return messages;
        }
    }
}