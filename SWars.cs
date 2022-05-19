using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleAppInvestec
{
    internal class SWars
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public SWarsPeople[] results { get; set; }
    }

    internal class SWarsPeople
    {
        public string name { get; set; }
        public string[] films { get; set; }
    }

    internal class SWarsFilmHeader
    {
        public int count { get; set; }
        public SWarsFilm[] results { get; set; }
    }

    internal class SWarsFilm
    {
        public string title { get; set; }
        public string url { get; set; }
        public int filmLinkID { get; set; }
    }
}
