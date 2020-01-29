using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueLayer_NitinJain
{
    public static class CommonHelper
    {

        /// <summary>
        /// Decode a Base64 to string.
        /// </summary>
        internal  static string DecodeBase64ToString(string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);

            return decodedString;
        }
    }
}
