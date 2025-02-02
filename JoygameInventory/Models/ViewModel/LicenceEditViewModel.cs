using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{
    public class LicenceEditViewModel
    {
        public int Id { get; set; }
        public string LicenceName { get; set; }

        public int? SelectedStaffId { get; set; }


        public DateTime LicenceActiveDate { get; set; }
        public DateTime LicenceEndDate { get; set; }

        public List<JoyStaff> JoyStaffs { get; set; }

        public List<LicenceUser> LicenceUser { get; set; }
    }
}
