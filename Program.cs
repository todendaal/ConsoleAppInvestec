using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ConsoleAppInvestec
{

    class Program
    {
        public static List<SWars> SWarsList = new List<SWars>();
        public static SWarsFilm[] SWarsFilmList = new SWarsFilm[20];


        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");



            await ProcessFilms("https://swapi.dev/api/films/");
        }


        private static async Task ProcessFilms(string pURL)
        {
            var streamTask = client.GetStreamAsync(pURL);
            var msg = await streamTask;
            var repositories = JsonSerializer.Deserialize<SWarsFilmHeader>(msg);
            //Get the Film URL Id's
            foreach (var itm in repositories.results)
            {
                string fname = itm.url;
                fname = fname.Replace("https://swapi.dev/api/films/", "");
                fname = fname.Replace("/", "");
                itm.filmLinkID = Int32.Parse(fname);
            }
            SWarsFilmList = repositories.results;
            await ProcessRepositories("https://swapi.dev/api/people");
        }


        private static async Task ProcessRepositories(string pURL)
        {

            for (int i = 1; i <= 9; i++)
            {
                pURL = "https://swapi.dev/api/people/?page=" + i.ToString();
                var streamTask = client.GetStreamAsync(pURL);
                var msg = await streamTask;
                var repositories = JsonSerializer.Deserialize<SWars>(msg);
                SWarsList.Add(repositories);
            }


            var SWarsListCopy = SWarsList;
            foreach (var itmX in SWarsList)
            {
                foreach (var repoA in itmX.results)
                {
                    foreach (var itmY in SWarsList)
                    {
                        foreach (var repoB in itmY.results.Where(x => x.name != repoA.name && x.films.SequenceEqual(repoA.films) && x.films.Count() == repoA.films.Count()))
                        {
                            Console.WriteLine("--------------------------------------------------");
                            Console.WriteLine("(CHAR 1) " + repoA.name + "  ------   (CHAR 2) " + repoB.name);
                            Console.WriteLine(repoB.films.Count() + " Fims in total");
                            foreach (var _itm in repoB.films)
                            {
                                string fname = _itm;
                                fname = fname.Replace("https://swapi.dev/api/films/", "");
                                fname = fname.Replace("/", "");
                                int filmLinkID = Int32.Parse(fname);
                                var filmname = SWarsFilmList.Where(x => x.filmLinkID == filmLinkID).SingleOrDefault();
                                Console.WriteLine("     " + filmname.title);
                            }
                            Console.WriteLine("--------------------------------------------------\n\n");
                        }
                    }
                }
            }

        }


    }
}
