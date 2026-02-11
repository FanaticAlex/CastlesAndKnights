using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Farms
{
    public class Farm : MergableObject
    {
        private CitiesManager _citiesManager;

        public void SetCitiesManager(CitiesManager citiesManager)
        {
            _citiesManager = citiesManager;
        }

        public override int GetScore()
        {
            List<City> completeCities = _citiesManager.GetCompleteCities();

            // count all connected completed cities
            // for every connected completed city 3 score point

            var connectedCitys = new List<City>();
            foreach (var FarmPart in Parts)
            {
                var connectedCityParts = FarmPart.Tile.GetConnectedCityParts((FarmPart)FarmPart);

                foreach (var city in completeCities)
                {
                    var commonParts = city.Parts.Intersect(connectedCityParts);
                    if (commonParts.Any())
                        connectedCitys.Add(city);
                }
            }

            var completedConnectedCitys = connectedCitys.Distinct();
            return 3 * completedConnectedCitys.Count();
        }
    }
}
