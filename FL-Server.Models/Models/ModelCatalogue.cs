using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL_Server.Models.Models
{
    public class ModelCatalogue
    {


        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Kind { get; set; }
        public string Description { get; set; }
        [Required]
        public string StudyLead { get; set; }

        [Required]
        public bool? Status { get; set; }
        [Required]
        public DateTime StartDate { get; set; }

       


    }
}
