namespace AndBank.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
