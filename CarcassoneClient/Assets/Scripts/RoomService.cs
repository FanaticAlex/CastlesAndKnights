using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carcassone.ApiClient;

namespace Assets.Scripts
{
    public sealed class RoomService
    {
        private static readonly Lazy<RoomService> instanceHolder =
            new Lazy<RoomService>(() => new RoomService());

        public Client Client { get; set; }
        public string RoomId { get; set; }
        public User User { get; set; }

        private RoomService()
        {
            var httpClient = new System.Net.Http.HttpClient() { Timeout = new TimeSpan(0, 0, 1) };
            Client = new Client(@"http://192.168.1.65:80/", httpClient);
            //Client = new Client(@"https://localhost:44322/", httpClient);
        }

        public static RoomService Instance
        {
            get { return instanceHolder.Value; }
        }
    }
}
