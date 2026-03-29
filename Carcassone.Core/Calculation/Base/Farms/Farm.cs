using Carcassone.Core.Calculation.Base.Farms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Farms
{
    public class Farm : MergableObject
    {
        public override int GetScore(GameState state)
        {
            List<City> completeCities = GetCompleteCities(state);

            // count all connected completed cities
            // for every connected completed city 3 score point

            var connectedCitys = new List<City>();
            foreach (var FarmPart in Parts)
            {
                var connectedCityParts = ((FarmPart)FarmPart).ConnectedCityParts;

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

        public List<City> GetCompleteCities(GameState state)
        {
            var cities = state.GetLastState().Calculated.OfType<City>();
            return cities.Where(c => c.IsComplete(state)).ToList();
        }

        public Farm Copy()
        {
            var copy = new Farm();
            copy.Parts.AddRange(Parts);
            return copy;
        }
    }
}
