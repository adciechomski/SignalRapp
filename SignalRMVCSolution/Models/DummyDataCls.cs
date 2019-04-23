using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SignalRMVCSolution.Models
{

    public class DummyDataCls
    {
        public long Id { get; set; }

        private string _key;

        public string Key
        {
            get
            {
                if (_key == null)
                {
                    _key = Regex.Replace(Ticker.ToLower(), "[^a-z0-9]", "-");
                }
                return _key;
            }
            set { _key = value; }
        }

        public string Ticker { get; set; }
        public string Benchmark { get; set; }
        public double Price { get; set; }
    }
}
