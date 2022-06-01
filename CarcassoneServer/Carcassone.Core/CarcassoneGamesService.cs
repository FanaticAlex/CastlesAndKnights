using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core
{
    public class CarcassoneGamesService : ICarcassoneGamesService
    {
        private readonly List<GameRoom> _rooms = new();

        public GameRoom CreateRoom()
        {
            var server = new GameRoom();
            _rooms.Add(server);
            return server;
        }

        public void DeleteRoom(string roomId)
        {
            var room = _rooms.First(room => room.Id == roomId);
            _rooms.Remove(room);
        }

        public GameRoom GetRoom(string roomId)
        {
            return _rooms.First(room => room.Id == roomId);
        }

        public List<string> GetAvailableRoomsId()
        {
            return _rooms.Where(room => !room.IsStarted).Select(room => room.Id).ToList();
        }
    }
}
