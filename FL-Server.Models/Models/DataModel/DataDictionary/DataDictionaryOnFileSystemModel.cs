using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL_Server.Models.Models.DataModel
{
    public class DataDictionaryOnFileSystemModel 
    {


        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? FileType { get; set; }
        public string? UsersEngaged { get; set; }
        public Boolean? Status { get; set; }
        public string? Extension { get; set; }
        public string? Description { get; set; }
        public string? DataDictionaryPath { get; set; }
        public DateTime? LastUploadTime { get; set; }
        public DateTime? IntialUploadTime { get; set; }



        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }



        public int? StudyCatalogueId { get; set; }
        public virtual StudyCatalogue StudyCatalogue { get; set; }




        public int? DataDictionaryOnDatabaseModelId { get; set; }
        public virtual DataDictionaryOnDatabaseModel DataDictionaryOnDatabaseModel { get; set; }








    }
}
