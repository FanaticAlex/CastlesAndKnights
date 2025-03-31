using Carcassone.ApiClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class SavedAuthData
    {
        public string Login { get; set; }
        public string Token { get; set; }

        public SavedAuthData(string login, string token)
        {
            Login = login;
            Token = token;
        }
    }

    internal static class CarcassonePrefs
    {
        public static SavedAuthData GetSavedAuthData()
        {
            var login = PlayerPrefs.GetString("Login");
            var token = PlayerPrefs.GetString("AuthToken");
            var data = new SavedAuthData(login, token);
            return data;
        }

        public static void SetSavedAuthData(string login, string token)
        {
            PlayerPrefs.SetString("AuthToken", token);
            PlayerPrefs.SetString("Login", login);
        }

        public static void DeleteSavedAuthData()
        {
            PlayerPrefs.DeleteKey("AuthToken");
            PlayerPrefs.DeleteKey("Login");
        }
    }
}
