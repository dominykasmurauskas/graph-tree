using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
/*
 * B17.	Sudaryti algoritmą ir programą, kuri nustatytų, ar duotame grafe nurodytas viršūnių poaibis indukuoja medį.
 */
namespace MedzioIndukcija
{
    class Program
    {
        static void Main()
        {
            // Grafas duomenys = duomenuIvedimas();
            Grafas duomenys = HardcodedIvedimas();
            GrafoSpausdinimas(duomenys, "Pradinis grafas");

            //Įvedamas poaibis
            List<int> poaibis = IvestiPoaibi();

            //Suformuojamas naujas poaibis ir atspausdinamas
            Grafas suformuotas = FormuotiNaujaGrafa(duomenys, poaibis);
            GrafoSpausdinimas(suformuotas, "Grafas, suformuotas is nurodyto " +
                                           "virsuniu poaibio");

            ////Surandamos grafos kraštinės
            //suformuotas.SurastiKrastines();

            Console.WriteLine("Iveskite savybe, kuria norite tikrinti [1, 2, 3, 4, 5, 6]: ");
            int savybe = int.Parse(Console.ReadLine());

            if (MedzioPatikra(suformuotas, savybe))
                Console.WriteLine("Indukuotasis grafas - medis");
            else
                Console.WriteLine("Indukuotasis grafas - ne medis");

  
        }
        /// <summary>
        /// Metodas, kuriame duotasis grafas patikrinamas pagal pasirinktą medžio savybę
        /// </summary>
        /// <param name="graph">Grafas</param>
        /// <param name="metodas">Pasirinkta medžio savybė</param>
        /// <returns>True, jei grafas - medis; kitu atveju False</returns>
        static bool MedzioPatikra(Grafas graph, int metodas)
        {
            var timer = new Stopwatch();
            bool value = false;
            switch (metodas)
            {
                case 1:
                    timer.Start();
                    value = Savybe1(graph);
                    timer.Stop();
                    break;
                case 2:
                    timer.Start();
                    value = Savybe2(graph);
                    timer.Stop();
                    break;
                case 3:
                    timer.Start();
                    value = Savybe3(graph);
                    timer.Stop();
                    break;
                case 4:
                    timer.Start();
                    value = Savybe4(graph);
                    timer.Stop();
                    break;
                case 5:
                    timer.Start();
                    value = Savybe5(graph);
                    timer.Stop();
                    break;
                case 6:
                    timer.Start();
                    value = Savybe6(graph);
                    timer.Stop();
                    break;
                default:
                    value = false;
                    break;
            }
            Console.WriteLine("Nustatyti, ar grafas yra medis, uztruko: {0}", timer.Elapsed);
            return value;
        }
        /// <summary>
        /// 1 savybė - jungusis ir neturi ciklų
        /// </summary>
        /// <returns>True | False</returns>
        static bool Savybe1(Grafas graph)
        {
            DFSPradzia(graph, true);

            if (graph.arJungus && graph.ciklai.Count() == 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 2 savybė - jungusis ir , m = n – 1
        /// </summary>
        /// <returns>True | False</returns>
        static bool Savybe2(Grafas graph)
        {
            DFSPradzia(graph, false);
            graph.SurastiKrastines();

            if (graph.arJungus && graph.krastines.Count() == graph.n - 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 3 savybė -  neturi ciklų ir , m = n – 1
        /// </summary>
        /// <returns>True | False</returns>
        static bool Savybe3(Grafas graph)
        {
            DFSPradzia(graph, true);
            graph.SurastiKrastines();
            if (graph.ciklai.Count() == 0 & graph.krastines.Count() == graph.n - 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Metodas, kuris patikrina ketvirtą medžio savybę
        /// </summary>
        /// <param name="graph">Indukuotasis grafas</param>
        /// <returns>True | False</returns>
        static bool Savybe4(Grafas graph)
        {
            DFSPradzia(graph, true);
            if (graph.ciklai.Count() == 0)
            {
                Console.WriteLine("\r\nIveskite nauja briauna, jungiancia ne gretimas virsunes: ");
                string input = Console.ReadLine();
                string[] parts = input.Split(' ');
                Grafas naujas = KopijuotiGrafa(graph);
                naujas.PridetiBriauna(int.Parse(parts[0]), int.Parse(parts[1]));
                DFSPradzia(naujas, true);
                return !(naujas.ciklai.Count() == 1);
            }
            return false;
        }

        /// <summary>
        /// Metodas, kuris patikrina 5 medžio savybę
        /// </summary>
        /// <param name="graph">Indukuotasis grafas</param>
        /// <returns>True | False</returns>
        static bool Savybe5(Grafas graph)
        {
            DFSPradzia(graph, false);
            if (graph.arJungus)
            {
                Grafas naujas = KopijuotiGrafa(graph);
                naujas.SurastiKrastines();
                return DFSSalinantBriauna(naujas);
            }
            else
                return false;
            
        }
        /// <summary>
        /// Metodas, kuris patikrina 6 medžio egzistavimo sąlygos teisingumą
        /// </summary>
        /// <param name="grafas">Duotas grafas</param>
        /// <returns>True, jeigu sąlyga tenkinama</returns>
        static bool Savybe6(Grafas grafas)
        {
            bool galioja = true;
            List<int> pasikartojimuList = new List<int>();

            foreach (var rysys in grafas.gretimumoStruktura)
            {
                int virsune = rysys.Key;
                int pasikartojimu = KiekPasikartoja(grafas, virsune);

                //jei į viršūnę galima ateiti iš daugiau nei 2 kitų viršūnių, galime 
                //teigti, kad grafas nėra medis ir daugiau galime nebetikrinti
                if (pasikartojimu != 1 && pasikartojimu != 2)
                {
                    galioja = false;
                    break;
                }

                if(!pasikartojimuList.Contains(pasikartojimu))
                    pasikartojimuList.Add(pasikartojimu);
            }

            if (pasikartojimuList.Count() == 1)
                galioja = false;

            return galioja;
        }
        /// <summary>
        /// Metodas, kuris vykdo DFS tiek kartų, kiek grafas turi viršūnių,
        /// kiekvieną kartą pašalindamas vis skirtingą viršūnę. Jeigu
        /// pašalinus kurią nors viršūnę ir atlikus DFS paaiškėja, kad grafas
        /// tapo nebe jungus, reiškia, kad grafas yra medis. (5 savybė)
        /// </summary>
        /// <param name="graph">Duotasis grafas</param>
        /// <returns>True arba False</returns>
        static bool DFSSalinantBriauna(Grafas graph)
        {
            bool rezultatas = false;
            Grafas kopija;
            foreach(var briauna in graph.krastines)
            {
                kopija = KopijuotiGrafa(graph);
                kopija.PasalintiKrastine(briauna.Key, briauna.Value);
                DFSPradzia(kopija, false);
                if (!kopija.arJungus)
                {
                    rezultatas = true;
                } else
                {
                    break;
                }
            }
            return rezultatas;
        }
        /// <summary>
        /// Metodas, sukuriantis naują identišką grafą
        /// </summary>
        /// <param name="originalus">Originalus grafas</param>
        /// <returns>Originalaus grafo kopija</returns>
        static Grafas KopijuotiGrafa(Grafas originalus)
        {
            Grafas kopija = new Grafas();
            foreach (var rysys in originalus.gretimumoStruktura)
                kopija.GretimumoStrukturaAdd(rysys.Key, rysys.Value);
            return kopija;
        }
        /// <summary>
        /// Duomenu ivedimas is konsoles
        /// </summary>
        /// <returns>Suformuotas grafas</returns>
        static Grafas DuomenuIvedimas()
        {
            Grafas duomenys = new Grafas();
            Console.WriteLine("Iveskite grafo virsuniu sk.: ");
            int n = int.Parse(Console.ReadLine());

            for(int i = 0; i < n; i++)
            {
                Console.WriteLine("Iveskite {0} virsunes kaimynes, atskirdami " +
                                  "jas tarpo simboliu: ", i + 1);

                var kaimynes = Console.ReadLine();
                string[] parts = kaimynes.Split(' ');
                List<int> gretimosVirsunes = new List<int>();

                foreach(var part in parts)
                    gretimosVirsunes.Add(int.Parse(part));

                duomenys.GretimumoStrukturaAdd(i + 1, gretimosVirsunes);
            }

            return duomenys;
        }
        /// <summary>
        /// Įvedimas kode testavimui supaprastinti
        /// </summary>
        /// <returns>Suformuotas grafas</returns>
        static Grafas HardcodedIvedimas()
        {
            Grafas duomenys = new Grafas();

            duomenys.GretimumoStrukturaAdd(1, new List<int>() { 3, 5, 2 });
            duomenys.GretimumoStrukturaAdd(2, new List<int>() { 1, 6 });
            duomenys.GretimumoStrukturaAdd(3, new List<int>() { 1, 4, 6 });
            duomenys.GretimumoStrukturaAdd(4, new List<int>() { 3, 6 });
            duomenys.GretimumoStrukturaAdd(5, new List<int>() { 1 });
            duomenys.GretimumoStrukturaAdd(6, new List<int>() { 2, 3, 4 });

            return duomenys;

        }
        /// <summary>
        /// Grafo atspausdinimo gretimumo struktura metodas
        /// </summary>
        /// <param name="grafas">Grafas</param>
        /// <param name="antr">Antraste</param>
        static void GrafoSpausdinimas(Grafas grafas, string antr)
        {
            Console.Clear();
            Console.WriteLine(antr);
            foreach (var virsune in grafas.gretimumoStruktura)
            {
                Console.Write("{0}: ", virsune.Key);
                foreach (var rysys in virsune.Value)
                {
                    Console.Write("{0} ", rysys);
                }
                Console.WriteLine("");
            }
        }
        /// <summary>
        /// Metodas, kuris suskaičiuoja, kiek kitų viršūnių turi ryšį su pasirinkta
        /// </summary>
        /// <param name="grafas">Grafas</param>
        /// <param name="virsune">Pasirinkta viršūnė</param>
        /// <returns>Viršūnių, turinčių ryšį su pasirinkta, skaičius</returns>
        static int KiekPasikartoja(Grafas grafas, int virsune)
        {
            int k = 0;
            foreach (var rysys in grafas.gretimumoStruktura)
            {
                if (rysys.Value.Contains(virsune))
                    k++;
            }
            return k;
        }
        /// <summary>
        /// Įvedamas indukuojančių viršūnių poaibis konsolėje
        /// </summary>
        /// <returns>Indukuojančių viršūnių sąrašas</returns>
        static List<int> IvestiPoaibi()
        {
            List<int> poaibis = new List<int>();

            Console.WriteLine("\r\nIveskite pasirinkta virsuniu poaibi, " +
                              "atskirdami virsune tarpo simboliu: ");
            
            string input = Console.ReadLine();
            string[] parts = input.Split(' ');

            foreach (var part in parts)
                poaibis.Add(int.Parse(part));

            return poaibis;
        }
        /// <summary>
        /// Metodas, suformuojantis indukuotąjį grafą
        /// </summary>
        /// <param name="originalus">Originalus grafas</param>
        /// <param name="virsuniuPoaibis">Indukuojančių viršūnių poaibis</param>
        /// <returns>Indukuotasis grafas</returns>
        static Grafas FormuotiNaujaGrafa(Grafas originalus, List<int> virsuniuPoaibis)
        {
            Grafas naujas = new Grafas();

            foreach(var rysys in originalus.gretimumoStruktura)
            {
                if(virsuniuPoaibis.Contains(rysys.Key))
                {
                    naujas.GretimumoStrukturaAdd(rysys.Key, 
                                                RastiGretimasVirsunes(rysys.Value, 
                                                                      virsuniuPoaibis));
                }
            }
            return naujas;
        }
        /// <summary>
        /// Metodas, kuris iš visų pateiktos viršūnės ryšių atrenka tik tas,
        /// kurios yra indukuojančių viršūnių aibėje
        /// </summary>
        /// <param name="originalGretimosVirsunes">Pateiktos viršūnės gretimos viršūnės</param>
        /// <param name="virsuniuPoaibis">Indukuojančių viršūnių poaibis</param>
        /// <returns>Prafiltruotos pateiktos viršūnės gretimos viršūnės</returns>
        static List<int> RastiGretimasVirsunes(List<int> originalGretimosVirsunes, 
                                               List<int> virsuniuPoaibis)
        {
            List<int> gretimosVirsunes = new List<int>();
            foreach (var virsune in originalGretimosVirsunes)
            {
                if (virsuniuPoaibis.Contains(virsune))
                    gretimosVirsunes.Add(virsune);
            }
            return gretimosVirsunes;
        }
        /// <summary>
        /// Metodas, kuriame pasiruošiame DFS
        /// </summary>
        /// <param name="grafas">Grafas, kuriame vykdysime DFS</param>
        static void DFSPradzia(Grafas grafas, bool skaiciuotiCiklus)
        {
            var prec = FormuotiPrec(grafas);
            var nr = FormuotiNr(grafas);

            int pradzia = prec.First().Key; //DFSPradzia pradzios virsune

            
            if(skaiciuotiCiklus)
                VykdytiDFS(pradzia, prec, nr, grafas, pradzia, true); //pradeda veikti
            else
                VykdytiDFS(pradzia, prec, nr, grafas, pradzia, false);

            //remiantis DFS rezultatais, nustato grafo jungumą. Jeigų būtų nejungus,
            //DFS neaplankytų visų grafo viršūnių ir lygybė būtų neteisinga.
            grafas.arJungus = KiekAplankytuVirsuniu(prec) == grafas.n; 
        }
        /// <summary>
        /// Metodas, formuojantis aplankymo eilės numerio masyvą
        /// </summary>
        /// <param name="grafas">Grafas</param>
        /// <returns>Tuščias viršūnių aplankymų eilės numerių masyvas</returns>
        static Dictionary<int, int> FormuotiNr(Grafas grafas)
        {
            var nr = new Dictionary<int, int>();
            foreach (var rysys in grafas.gretimumoStruktura)
            {
                nr.Add(rysys.Key, 0);
            }
            return nr;
        }
        /// <summary>
        /// Metodas, suskaičiuojantys, kiek viršūnių aplankyta
        /// </summary>
        /// <param name="prec">PREC masyvas</param>
        /// <returns>Aplankytų viršūnių skaičius</returns>
        static int KiekAplankytuVirsuniu(Dictionary<int, int> prec)
        {
            int k = 0;
            foreach(var item in prec)
            {
                if (item.Value != 0)
                    k++;
            }
                
            return k;
        }
        /// <summary>
        /// Metodas, kuris suformuoja tuščią PREC masyvą
        /// </summary>
        /// <param name="graph">Duotas grafas</param>
        /// <returns>Tuščias PREC masyvas</returns>
        static Dictionary<int, int> FormuotiPrec(Grafas graph)
        {
            var prec = new Dictionary<int, int>();
            foreach(var rysys in graph.gretimumoStruktura)
            {
                prec.Add(rysys.Key, 0);
            }
            return prec;
        }
        /// <summary>
        /// DFS algoritmo logikos metodas
        /// </summary>
        /// <param name="v">Viršūnė, kurioje esame</param>
        /// <param name="prec">PREC masyvas</param>
        /// <param name="nr">Aplankymo eilės numerių masyvas</param>
        /// <param name="graph">Grafas</param>
        /// <param name="ateitaIs">Viršūnė, iš kurios atėjome į esamą</param>
        /// <param name="skaiciuotiCiklus">Ar tikrinti atvirkštines kraštines ir skaičiuoti ciklus</param>
        static void VykdytiDFS(int v, Dictionary<int, int> prec,
                               Dictionary<int, int> nr, Grafas graph,
                               int ateitaIs, bool skaiciuotiCiklus)
        {

            //Kelintą kartą kreipiamės į DFS (gylis)
            graph.DFSNr++;

            if (nr[v] == 0)
                nr[v] = graph.DFSNr;
            if (prec[v] == 0)
                prec[v] = ateitaIs;

            //surandame viršūnes, kurias galime pasiekti iš esamos
            List<int> rysiai = graph.gretimumoStruktura[v];

            //Gretimos viršūnės, į kurią planuojame eiti toliau, indeksas
            int x = 0;
            //Didiname indeksą tol, kol surandame nelankytą viršūnę, tačiau 
            //neišeiname iš ribų, jeigu tokia neegzistuoja
            while (x < rysiai.Count() - 1 && prec[rysiai[x]] != 0)
            {
                //Tikriname atvirkštinės briaunos egzistavimą
                if (skaiciuotiCiklus)
                    if (ArAtvirkstineBriauna(prec, nr, v, rysiai[x]))
                        graph.PridetiCikla(v, rysiai[x], prec);

                x++;
            }
            //Jeigu visos gretimos viršūnės jau lankytos
            if (rysiai.Count() == 0 || prec[rysiai[x]] != 0)
                x = -1;

            //Jeigu suradome viršūnę, kuri dar nelankyta, kreipiamės į
            //metodą su naujais parametrais
            if (x != -1 && rysiai.Count() > 0 && prec[rysiai[x]] == 0)
            {
                int kitaVirsune = rysiai[x];
                VykdytiDFS(kitaVirsune, prec, nr, graph, v, skaiciuotiCiklus);
            }
            //kitu atveju grįžtame į viršūnę, iš kurios atėjome
            else if (x == -1 && ateitaIs != v)
            {
                VykdytiDFS(ateitaIs, prec, nr, graph, prec[ateitaIs], skaiciuotiCiklus);
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// Nustato, ar nurodyta krašinė yra atvirkštinė
        /// </summary>
        /// <param name="prec">PREC masyvas</param>
        /// <param name="nr">NR masyvas</param>
        /// <param name="current">Kraštinės pradžia</param>
        /// <param name="next">Kraštinės pabaiga</param>
        /// <returns>TRUE | FALSE</returns>
        static bool ArAtvirkstineBriauna(Dictionary<int, int> prec, Dictionary<int, int> nr, 
                                         int current, int next)
        {
            if (nr[next] < nr[current] && prec[next] != 0 && prec[current] != next)
                return true;
            else
                return false;
        }
    }
}
