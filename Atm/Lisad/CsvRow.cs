using System;
using System.Collections.Generic;
using System.Text;

namespace Atm.Lisad
{
    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }
}
