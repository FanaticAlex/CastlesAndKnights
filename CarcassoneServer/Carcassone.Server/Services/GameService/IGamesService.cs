using Carcassone.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carcassone.Server.Services
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
