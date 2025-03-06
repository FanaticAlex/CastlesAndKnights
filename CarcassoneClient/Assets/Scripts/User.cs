using Carcassone.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class User
    {
        public string Name { get; set; } = "AnonimousUser";

        public UserInfo GetUserInfo() => new() { Name = this.Name, GamesCount = 0, WinCount = 0 };
    }
}
