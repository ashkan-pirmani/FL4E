using FL_Server.Models.Models;
using FL_Server.Models.Models.DataModel.DataCore;

namespace FL_Server.Models
{
    public class HomeViewModel
    {


        public ApplicationUser? ApplicationUser { get; set; }
        public StudyCenter? StudyCenter { get; set; }
        public StudyCatalogue? StudyCatalogue { get; set; }
        public ModelCatalogue? ModelCatalogue { get; set; }
        public ModelCenter? ModelCenter { get; set; }
        public AnalysisCenter? AnalysisCenter { get; set; }
        public DataCoreFileOnSystemModel? DataCoreFileOnSystemModel { get; set; }


        public IEnumerable<ApplicationUser>? UserList { get; set; }
        public IEnumerable<StudyCenter>? studyCenterList { get; set; }
        public IEnumerable<StudyCatalogue>? studyCatalogueList { get; set; }
        public IEnumerable<ModelCatalogue>? modelCatalogueList { get; set; }
        public IEnumerable<ModelCenter>? modelCenterList { get; set; }
        public IEnumerable<AnalysisCenter>? analysisCenterList { get; set; }
        public IEnumerable<DataCoreFileOnSystemModel>? dataCoreFileOnSystemModel { get; set; }









    }
}
