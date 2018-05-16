using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchFight.Classes
{
    public class Tools
    {
        public static string RemoveExcessWhiteSpace(string sStringToFix)
        {
            string sReturn = sStringToFix.Trim();
            while(sReturn.Contains("  ") == true)
            {
                sReturn.Replace("  ", " ");
            }
            return sReturn;
        }
    }
}
