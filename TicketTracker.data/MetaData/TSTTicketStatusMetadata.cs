using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketTracker.data /*.MetaData*/
{
    [MetadataType(typeof(TSTTicketStatusMetadata))]
    public partial class TstTicketStatus
    { }

    class TSTTicketStatusMetadata
    {
        public int StatusID { get; set; }

        [Required(ErrorMessage ="*Required")]
        [StringLength(25, ErrorMessage ="25 character max value")]
        [Display(Name ="Status:")]
        public string StatusName { get; set; }

        [StringLength(500, ErrorMessage ="500 character max value")]
        [Display(Name ="Description:")]
        public string StatusDesc { get; set; }
    }
}
