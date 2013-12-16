using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;	// Needed for DllImport

namespace FileDetails
{
    public class ssdeepWrapper
    {
        [DllImport("fuzzy.dll")]
        public static extern int fuzzy_hash_filename(string fname, StringBuilder result);
        [DllImport("fuzzy.dll")]
        public static extern int fuzzy_compare(string sig1, string sig2);
    }
}
