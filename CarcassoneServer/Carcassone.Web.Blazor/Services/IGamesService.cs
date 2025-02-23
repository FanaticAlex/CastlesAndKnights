using Carcassone.Core;
using System.Collections.Generic;

namespace Carcassone.Web.Blazor.Services
{
    /// <summary>
    /// Contains functions of handle game rooms.
    /// </summary>
    public interface IGamesService
    {
        GameRoom CreateRoom();
        GameRoom GetRoom(string roomId);
        List<string> GetAvailableRoomsId();
        void DeleteRoom(string roomId);
    }
}
