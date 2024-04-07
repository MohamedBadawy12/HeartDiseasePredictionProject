using Database.Entities;

namespace HeartDiseasePrediction.ViewModel
{
    public class NewLabDropDownViewMode
    {
        public NewLabDropDownViewMode()
        {
            labs = new List<Lab>();
        }
        public List<Lab> labs { get; set; }
    }
}
