using Assets.Scripts.Game;
using System.Collections.Generic;

namespace Assets.Scripts
{
    internal interface IOnlineGame
    {
        void Login(SavedAuthData data); 
        void Login(string login, string password); 
        void Connect(string roomId); 
        List<string> GetRoomsIds(); 
    }
}
