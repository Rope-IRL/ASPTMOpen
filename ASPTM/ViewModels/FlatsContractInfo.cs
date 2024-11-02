using ASPTM.Models;

namespace ASPTM.ViewModels
{
    public class FlatsContractInfo
    {
        public int Id { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public decimal Cost { get; set; }

        public int FlatId { get; set; }
        public String? FlatAddress { get; set; }
        public String? LesseeName { get; set; }
        public String? LesseeSurName { get; set; }

        public String? LandLordName { get; set; }
        public String? LandLordSurName { get; set; }

    }
}
