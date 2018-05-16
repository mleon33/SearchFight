using SearchFight.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchFight
{
    class Result
    {
        public string Word = "";
        public float Google = 0;
        public float Bing = 0;
        public float Yandex = 0;
    }
    class Program
    {
        /*---------------------------------------------------------------------------------------------------
         * Created by: Marco Antonio García León
         * Country : Perú
         * Date of build: 2018.05.16-0216hrs
         * Notes: This is a coding assesstment test for a job.
         *---------------------------------------------------------------------------------------------------*/

        static void Main(string[] args)
        {
            List<Result> lstResults = new List<Result>();

            Console.WriteLine("SearchFight: Utility to compare results in the three most popular search engines.");

            string sResultOk = "\n";
            sResultOk = "┌──────────────────────────────┬────────────────────┬────────────────────┬────────────────────┐\n";
            sResultOk += "│ Programming language         │  Google Results    │ Bing Results       │ Yandex Results     │\n";
            sResultOk += "├──────────────────────────────┼────────────────────┼────────────────────┼────────────────────┤\n";

            for (int i = 0; i < args.Count(); i++)
            {
                string sParameter = args[i].Trim();
                Console.WriteLine("Querying for: " + sParameter);

                float iGoogleResult = SearchEngines.SearchUsingGoogle(sParameter);
                if(iGoogleResult < 0)
                {
                    Console.WriteLine("Error found in Google search: " + SearchEngines.LastErrorMessage);
                }

                float iBingResult = SearchEngines.SearchUsingBing(sParameter);
                if(iBingResult < 0)
                {
                    Console.WriteLine("Error found in Bing search: " + SearchEngines.LastErrorMessage);
                }

                float iYandexResult = SearchEngines.SearchUsingYandex(sParameter);
                if(iYandexResult < 0)
                {
                    Console.WriteLine("Error found in Yandex search: " + SearchEngines.LastErrorMessage);
                }

                lstResults.Add(new Result { Word = sParameter, Google = iGoogleResult, Bing = iBingResult, Yandex = iYandexResult });

                sResultOk += "│" + sParameter.PadRight(30, ' ') +
                             "│" + iGoogleResult.ToString("###,###,###,##0").PadLeft(20, ' ') +
                             "│" + iBingResult.ToString("###,###,###,##0").PadLeft(20, ' ') +
                             "│" + iYandexResult.ToString("###,###,###,##0").PadLeft(20, ' ') + "│\n";

                Console.WriteLine("\n");
            }

            // Now, query to see the best one for each column
            Console.WriteLine("Searching for the best results");
            string sBestForGoogle = lstResults.Where(w=> w.Google > 0).OrderByDescending(o=> o.Google).Select(s=> s.Word).FirstOrDefault();
            string sBestForBing = lstResults.Where(w=> w.Bing > 0).OrderByDescending(o => o.Bing).Select(s => s.Word).FirstOrDefault();
            string sBestForYandex = lstResults.Where(w=> w.Yandex > 0).OrderByDescending(o => o.Yandex).Select(s => s.Word).FirstOrDefault();

            sBestForGoogle = sBestForGoogle ?? "### ERROR ###".PadLeft(20, ' ');
            sBestForBing = sBestForBing ?? "### ERROR ###".PadLeft(20, ' ');
            sBestForYandex = sBestForYandex ?? "### ERRROR ###".PadLeft(20, ' ');

            sResultOk += "├──────────────────────────────┼────────────────────┼────────────────────┼────────────────────┤\n";
            sResultOk += "| Best result by search engine │" + sBestForGoogle.PadRight(20, ' ') + "│" + sBestForBing.PadRight(20, ' ') + "│" + sBestForYandex.PadRight(20, ' ') + "│\n";
            sResultOk += "└──────────────────────────────┴────────────────────┴────────────────────┴────────────────────┘\n";

            Console.WriteLine(sResultOk);
            Console.WriteLine("Press [ENTER] to exit");
            Console.ReadLine();
        }
    }
}