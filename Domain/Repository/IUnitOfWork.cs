using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IUnitOfWork
    {
        IArticleRepository Articles { get; }
        IUserRepository Users { get; }
        ICommentRepository Comments { get; }
        Task CommitAsync();
        void DiscardChanges();
    }
}
