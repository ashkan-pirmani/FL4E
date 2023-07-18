using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FL_Server.Models.Models
{
    public class AnalysisCenter
    {


        [Key]
        public int Id { get; set; }


        public int Status { get; set; }

        public string IP { get; set; }
        public string Port { get; set; }


        public int? StudyCenterID { get; set; }
        public virtual StudyCenter StudyCenter { get; set; }



        public int? ModelCatalogueID { get; set; }
        public virtual ModelCenter ModelCenter { get; set; }

    }
}
