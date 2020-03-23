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
    }
}