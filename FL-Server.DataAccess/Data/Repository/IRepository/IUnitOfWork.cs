namespace FL_Server.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {


        ISP_Call SP_Call { get; }

        IUserRepository User { get; }
        void Save();
    }
}
