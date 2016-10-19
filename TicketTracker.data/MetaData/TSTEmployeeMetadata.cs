using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketTracker.data /*.MetaData*/
{
    [MetadataType(typeof(TSTEmployeeMetadata))]
    public partial class TSTEmployee
    {
        [DisplayFormat(NullDisplayText ="No Tech Assigned")]
        public string FullName { get { return fname + " " + lname; } }
    }

    public class TSTEmployeeMetadata
    {
        public int EmpID { get; set; }

        [Required(ErrorMessage ="*Required")]
        [StringLength(20, ErrorMessage = "20 character max value")]
        [Display(Name ="First Name:")]
        public string fname { get; set; }

        [Required(ErrorMessage ="*Required")]
        [StringLength(20, ErrorMessage = "20 character max value")]
        [Display(Name ="Last Name:")]
        public string lname { get; set; }

        [StringLength(150, ErrorMessage ="150 character max value")]
        [Display(Name ="Address:")]
        public string address1 { get; set; }

        [StringLength(50, ErrorMessage ="50 character max value")]
        [Display(Name ="Address 2")]
        public string address2 { get; set; }

        [StringLength(20, ErrorMessage ="20 character max value")]
        [Display(Name = "City:")]
        public string City { get; set; }

        [StringLength(2, ErrorMessage ="2 character max value")]
        [Display(Name ="State:")]
        public string State { get; set; }

        [StringLength(10, ErrorMessage ="10 character max value")]
        [Display(Name ="Zip:")]
        public string zip { get; set; }

        [Required(ErrorMessage ="*Required")]
        [StringLength(15, ErrorMessage ="15 character max value")]
        [Display(Name ="Phone:")]
        public string phone1 { get; set; }

        [StringLength(15, ErrorMessage ="15 character max value")]
        [Display(Name ="Secondary Phone")]
        public string phone2 { get; set; }

        [Required(ErrorMessage ="*Required")]
        [StringLength(65, ErrorMessage ="65 character max value")]
        [Display(Name ="Email:")]
        public string Email { get; set; }

        [Required(ErrorMessage ="*Required")]
        [Display(Name ="Department:")]
        public int DeptID { get; set; }

        [StringLength(100, ErrorMessage ="100 character max value")]
        [Display(Name ="Image:")]
        public string Image { get; set; }

        [Required(ErrorMessage ="*Required")]
        [Display(Name ="DOB:")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime DOB { get; set; }

        [Required(ErrorMessage ="*Required")]
        [Display(Name ="Hire Date:")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime HireDate { get; set; }

        [Display(Name ="Separation Date:")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> SeparationDate { get; set; }

        [Required(ErrorMessage ="*required")]
        [Display(Name ="Is Active:")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage ="*Required")]
        [StringLength(25, ErrorMessage ="25 character max value")]
        [Display(Name ="Job Title:")]
        public string JobTitle { get; set; }

        [UIHint("MultilineText")]
        [Display(Name ="Notes")]
        public string Notes { get; set; }

        [StringLength(100, ErrorMessage ="100 character max value")]
        [Display(Name ="User ID")]
        public string UserID { get; set; }
    }
}
