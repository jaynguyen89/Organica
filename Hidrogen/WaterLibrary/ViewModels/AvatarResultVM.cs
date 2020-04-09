namespace WaterLibrary.ViewModels {

    public class AvatarResultVM : WaterResultVM {

        public ResultVM Result { get; set; }
    }

    public class ResultVM {
        
        public string Name { get; set; }
        
        public string Location { get; set; }
    }
}