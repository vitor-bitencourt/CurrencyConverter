using System;
using System.Collections.Generic;
using System.Text;

namespace conversor_moedas.Models
{
    public class ExchangeRates
    {
        public string disclaimer { get; set; }
        public string license { get; set; }
        public string timestamp { get; set; }
        public string @base { get; set; }
        public Rates rates { get; set; }

    }
}
