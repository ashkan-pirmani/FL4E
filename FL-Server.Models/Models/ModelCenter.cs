using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL_Server.Models.Models
{
    public class ModelCenter
    {

        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }

        [Required]
        public int ModelCatalogueId { get; set; }
        [ForeignKey("ModelCatalogueId")]
        [ValidateNever]
        public ModelCatalogue ModelCatalogue { get; set; }


        [Required]
        public bool ModelType { get; set; }


        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public int? FileModelId { get; set; }
        public virtual FileModel FileModel { get; set; }


        [Required]
        public DateTime LastUpdate { get; set; }


        public bool? DSS { get; set; }

        public int? StudyCenterID { get; set; }
        public virtual StudyCenter StudyCenter { get; set; }
    }
}
