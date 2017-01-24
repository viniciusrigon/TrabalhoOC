using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


namespace AircraftLanding
{
    public class AircraftLandingParser
    {
        public List<Plane> planes;

        int nPlanes;        
        FileManager fm;

        public AircraftLandingParser(string nomeArquivo)
        {
            planes = new List<Plane>();
            fm = new FileManager(nomeArquivo);
        }

        public void InputAirlandFile()
        {
            List<string> incoming = new List<string>();

            // pega número de aviões.
            incoming = fm.ReadFileLine();
            nPlanes = Convert.ToInt32(incoming[0]);

            //while (!fr.EndOfFile())
            int indice = 1;
            while (planes.Count < nPlanes)
            {
                Plane p = new Plane();
                p.idPlane = indice;
                incoming = fm.ReadFileLine();

                p.ET = Convert.ToInt32(incoming[1]);
                p.TT = Convert.ToInt32(incoming[2]);
                p.LT = Convert.ToInt32(incoming[3]);
                p.pE = Convert.ToDecimal(incoming[4], new CultureInfo("en-US"));
                p.pL = Convert.ToDecimal(incoming[5], new CultureInfo("en-US"));

                while (p.S.Count < nPlanes)
                {
                    incoming = fm.ReadFileLine();
                    p.S.AddRange(incoming.ConvertAll<int>(delegate(string s) { return Convert.ToInt32(s); }));
                }
                planes.Add(p);

                indice++;
            }
        }

        public void OutputAirlandFile()
        {
            foreach (Plane p in planes)
            {
                //Console.WriteLine(p.ToString());                   
            }
        }

    }
}
