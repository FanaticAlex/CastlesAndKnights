namespace Sockets.Client.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using Sockets.Client.Storage.DataModel;

    /// <summary>
    /// Сервис сохранения сообщений в базе MS SQL Server.
    /// </summary>
    internal class MessageStorageEntityFramework : IMessageStorage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageStorageEntityFramework"/> class.
        /// </summary>
        public MessageStorageEntityFramework()
        {
            using MessagesDbContext context = new ();
            context.SaveChanges();
        }

        /// <inheritdoc/>
        public MessageEntity SaveMessage(string message)
        {
            var messageDbEntity = new MessageEntity() { Value = message, IsSent = false };
            using MessagesDbContext context = new ();
            context.Messages.Add(messageDbEntity);
            context.SaveChanges();
            return messageDbEntity;
        }

        /// <inheritdoc/>
        public void SetAsSent(int messageId)
        {
            using MessagesDbContext context = new ();
            var message = context.Messages.FirstOrDefault(message => message.Id == messageId);
            if (message == null)
            {
                return;
            }

            message.IsSent = true;
            context.SaveChanges();
        }

        /// <inheritdoc/>
        public List<MessageEntity> GetAllNotSentMessages()
        {
            using MessagesDbContext context = new ();
            IQueryable<MessageEntity> query = context.Messages.Where(message => !message.IsSent);
            return query.ToList();
        }
    }
}
