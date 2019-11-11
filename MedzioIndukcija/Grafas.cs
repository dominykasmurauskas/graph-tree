using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedzioIndukcija
{

    /// <summary>
    /// Klasė grafo objektui saugoti
    /// </summary>
    class Grafas
    {
        /// <summary>
        /// Grafo viršūnių skaičius
        /// </summary>
        public int n = 0;
        /// <summary>
        /// Grafo gretimumo struktūra
        /// </summary>
        public Dictionary<int, List<int>> gretimumoStruktura;
        /// <summary>
        /// Grafo jungumo požymios
        /// </summary>
        public bool arJungus = false;
        /// <summary>
        /// Grafo kraštinės (briaunos)
        /// </summary>
        public List<KeyValuePair<int, int>> krastines = new List<KeyValuePair<int, int>>();
        /// <summary>
        /// Grafo ciklai
        /// </summary>
        public List<List<int>> ciklai = new List<List<int>>();
        /// <summary>
        /// DFS pagalbinis kintamasis
        /// </summary>
        public int DFSNr = 0;
        /// <summary>
        /// Konstruktorius be parametrų
        /// </summary>
        public Grafas()
        {
            this.n = 0;
            this.gretimumoStruktura = new Dictionary<int, List<int>>();
        }
        /// <summary>
        /// Į gretimumo struktūrą įdedama nauja eilutė
        /// </summary>
        /// <param name="virsune">Viršunė</param>
        /// <param name="kaimynes">Kitos viršūnės, pasiekiamos iš pagrindinės</param>
        public void GretimumoStrukturaAdd(int virsune, List<int> kaimynes)
        {
            n++;
            gretimumoStruktura.Add(virsune, kaimynes);
        }
        /// <summary>
        /// Metodas briaunos pridėjimui į grafą
        /// </summary>
        /// <param name="pradzia">Briaunos pradžios viršūnė</param>
        /// <param name="pabaiga">Briaunos pabaigos viršūnė</param>
        public void PridetiBriauna(int pradzia, int pabaiga)
        {
            if (gretimumoStruktura.ContainsKey(pradzia))
                gretimumoStruktura[pradzia].Add(pabaiga);
            else
                gretimumoStruktura.Add(pradzia, new List<int>() { pabaiga });
        }
        /// <summary>
        /// Metodas, kuris suranda visas grafo kraštines (briaunas)
        /// </summary>
        public void SurastiKrastines()
        {
            foreach (var rysys in this.gretimumoStruktura)
                foreach (var item in rysys.Value)
                {
                    var krastineA = new KeyValuePair<int, int>(rysys.Key, item);
                    var krastineB = new KeyValuePair<int, int>(item, rysys.Key);
                    if (!krastines.Contains(krastineA) && !krastines.Contains(krastineB))
                        krastines.Add(krastineA);
                }
        }
        /// <summary>
        /// Metodas, kuris pašalina pasirinktą kraštinę
        /// </summary>
        /// <param name="pradzia">Kraštinės pradžia</param>
        /// <param name="pabaiga">Kraštinės pabaiga</param>
        public void PasalintiKrastine(int pradzia, int pabaiga)
        {
            if (gretimumoStruktura.ContainsKey(pradzia))
                gretimumoStruktura[pradzia].Remove(pabaiga);
            if (gretimumoStruktura.ContainsKey(pabaiga))
                gretimumoStruktura[pabaiga].Remove(pradzia);
        }
        /// <summary>
        /// Metodas, kuris prideda naują ciklą į sąrašą
        /// </summary>
        /// <param name="v">Antro ciklo viršūnė</param>
        /// <param name="sekantiVirsune">Pirma ciklo viršūnė</param>
        /// <param name="prec">PREC masyvas</param>
        internal void PridetiCikla(int v, int sekantiVirsune, Dictionary<int, int> prec)
        {
            List<int> ciklas = new List<int>() { sekantiVirsune };
            while(prec[v] != sekantiVirsune)
            {
                ciklas.Add(v);
                v = prec[v];
            }

            ciklai.Add(ciklas);
        }
    }
}
