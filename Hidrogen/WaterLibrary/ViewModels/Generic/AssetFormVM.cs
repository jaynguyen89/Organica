using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace WaterLibrary.ViewModels {

    public class AssetFormVM {

        public IFormFile File { get; set; }

        public int HidrogenianId { get; set; }

        public string ApiKey { get; set; }

        private static readonly List<string> IMAGE_TYPES = new List<string>
        {
            "image/gif", "image/png", "image/jpg", "image/jpeg"
        };

        private static readonly int MAX_SIZE = 2000000; //2MB

        public List<int> CheckFile() {
            var errors = new List<int>();

            if (File == null) errors.Add(-1);

            if (!IMAGE_TYPES.Contains(File.ContentType)) {
                errors.Add(0);
                return errors;
            }

            if (File.Length == 0) errors.Add(1);
            if (File.Length > MAX_SIZE) errors.Add(2);

            return errors;
        }

        public List<string> GenerateErrorMessages(List<int> errors) {
            var messages = new List<string>();

            if (errors.Contains(-1)) messages.Add("No photo was submitted. Please select a photo");
            if (errors.Contains(0)) messages.Add("The photo is not of expected type. Expected: JPG, PNG, GIF.");
            if (errors.Contains(1)) messages.Add("The photo seems to be blank. Please select another one.");
            if (errors.Contains(2)) messages.Add("Photo size is too large. Max 2MB allowed.");

            return messages;
        }
    }
}