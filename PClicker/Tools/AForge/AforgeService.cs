using AForge.Imaging;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace PClicker.Tools.AForge
{
    public class AforgeService
    {
        private static ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.9f);

        /// <summary>
        /// Получение коллекции найденных мест где находится образец
        /// </summary>
        /// <returns>коллекция найденных мест</returns>
        public Task<List<FoundPlace>> GetPlaces(Bitmap orig, Bitmap sample)
        {
            return Task.Run(()=> {
                List<FoundPlace> result = new List<FoundPlace>();
                TemplateMatch[] _matchings = tm.ProcessImage(orig, sample);

                foreach (var match in _matchings)
                {
                    FoundPlace place = new FoundPlace(match);
                    result.Add(place);
                }

                return result;
            });
        }

    }
}
