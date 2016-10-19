using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketTracker.data /*.MetaData*/
{
    [MetadataType(typeof(TSTDepartmentMetadata))]
    public partial class TSTDepartment
    { }


    public class TSTDepartmentMetadata
    {


        public int DeptID { get; set; }

        [Required(ErrorMessage ="*Required")]
        [Display(Name ="Department Name:")]
        [StringLength(20, ErrorMessage ="20 character max value")]
        public string DeptName { get; set; }

        [Display(Name ="Description:")]
        [StringLength(500, ErrorMessage ="500 character max value")]
        public string DeptDesc { get; set; }

        [Display(Name ="Is Active")]
        public bool IsActive { get; set; }

    }
}
