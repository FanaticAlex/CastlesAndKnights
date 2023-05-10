namespace Sockets.Client.Storage.DataModel
{
    using Microsoft.EntityFrameworkCore;

    public class MessagesDbContext : DbContext
    {
        public MessagesDbContext()
            : base()
        {
        }

        public virtual DbSet<MessageEntity> Messages => Set<MessageEntity>();
    }
}