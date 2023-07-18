using FL_Server.Models.Models.DataModel.DataCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL_Server.Models.Models.DataModel
{
    public class DataUploadViewModel
    {
        public List<DataDictionaryOnDatabaseModel> DataDictionary { get; set; }
        public List<DataDictionaryOnFileSystemModel> DataDictionaryFile { get; set; }
        public List<DataCleanFunctionOnDatabaseModel> DataCleanFunction { get; set; }
        public List<DataInputModelOnDatabaseModel> DataInputs { get; set; }
        public List<DataCoreFileOnSystemModel> DataCore { get; set; }
        public DataDictionaryOnDatabaseModel dataDictionary { get; set; }
        public DataDictionaryOnFileSystemModel dataDictionaryFile { get; set; }
        public DataCleanFunctionOnDatabaseModel dataCleanFunction { get; set; }
        public DataInputModelOnDatabaseModel dataInputs { get; set; }
        public DataCoreFileOnSystemModel dataCore { get; set; }




        public List<ApplicationUser> ApplicationUser { get; set; }



        public List<StudyCenter> StudyCenter { get; set; }
        public StudyCenter studyCenter { get; set; }

        public StudyCatalogue studyCatalogue { get; set; }
        public List<StudyCatalogue> studyCatalogueList { get; set; }
        public ModelCatalogue modelCatalogue { get; set; }
        public List<ModelCatalogue> modelCatalogueList { get; set; }


    }
}
