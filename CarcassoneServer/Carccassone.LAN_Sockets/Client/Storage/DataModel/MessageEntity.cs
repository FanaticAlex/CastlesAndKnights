namespace Sockets.Client.Storage.DataModel
{
    using Microsoft.EntityFrameworkCore;

    public class MessageEntity
    {
        public int Id { get; set; }

        required public string Value { get; set; }

        public bool IsSent { get; set; }
    }
}