using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayMax2
{
    class Avatar
    {
        private int apetito, diversión, energía;

        public int Apetito { get => apetito; set => apetito = value; }
        public int Diversión { get => diversión; set => diversión = value; }
        public int Energía { get => energía; set => energía = value; }

        public Avatar (int a, int d, int e)
        {
            this.apetito = a;
            this.diversión = d;
            this.energía = e;
        }
    }
}
