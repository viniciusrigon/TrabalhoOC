using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AircraftLanding
{
    public class Plane
    {
        public int idPlane;
        public int ET; // tempo mais cedo para pousar (inicio do intervalo que o avião pode pousar)
        public int TT; // tempo ideal
        public int LT; // tempo mais longo para pousar (final do intervalo que o avião pode pousar)
        public decimal pE; // penalidade antes do tempo inicial
        public decimal pL; // penalidade depois do tempo final
        public List<int> S; // tempo de separação entre pouso do avião i e antes do pouso do proximo avião j

        public Plane()
        {

            S = new List<int>();
        }


    }
}
