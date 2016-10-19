using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketTracker.data /*.MetaData*/
{
    [MetadataType(typeof(TSTTicketMetadata))]
    public partial class TSTTicket
    { }

    public class TSTTicketMetadata
    {
        public int TicketID { get; set; }

        [Required(ErrorMessage ="*Required")]
        [Display(Name ="Status ID:")]
        public int StatusID { get; set; }

        [Required(ErrorMessage ="*Required")]
        [Display(Name ="Submission Date:")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime SubmissionDate { get; set; }

        [Required(ErrorMessage ="*Required")]
        [Display(Name ="Submitted By:")]
        public int SubmittedByID { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name ="Completed Date:")]
        public Nullable<System.DateTime> CompletedDate { get; set; }

        [Required(ErrorMessage ="*Required")]
        [Display(Name ="Ticket Description:")]
        [UIHint("MultilineText")]
        [StringLength(500, ErrorMessage ="500 character max value")]
        public string TicketDesc { get; set; }

        [Display(Name ="Assigned Tech:")]
        public Nullable<int> AssignedTechID { get; set; }
    }
}
