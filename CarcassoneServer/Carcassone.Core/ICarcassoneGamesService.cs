using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carcassone.Core
{
    /// <summary>
    /// Contains functions of handle game rooms.
    /// </summary>
    public interface ICarcassoneGamesService
    {
        GameRoom CreateRoom();
        GameRoom GetRoom(string roomId);
        List<string> GetAvailableRoomsId();
        void DeleteRoom(string roomId);
    }
}
