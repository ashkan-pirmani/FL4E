using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL_Server.Models.Models.DataModel.DataCore
{
    public class DataCoreFileOnDatabaseModel 
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Title { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string? Description { get; set; }
        public Boolean? Status { get; set; }
        public string UploadedBy { get; set; }
        public byte[]? DataCore { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? DataDictionaryOnDatabaseModelId { get; set; }
        public virtual DataDictionaryOnDatabaseModel DataDictionaryOnDatabaseModel { get; set; }

        public int? StudyCatalogueId { get; set; }
        public virtual StudyCatalogue StudyCatalogue { get; set; }


    }
}
