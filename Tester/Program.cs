using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LitJson;
using PokeAPI.NET;

namespace Tester
{
    static class Program
    {
        static void Main(string[] args)
        {
            //string[] lines = File.ReadAllLines("pkmn.txt");
            //string txt = String.Empty;

            //foreach (string s in lines)
            //{
            //    string kvp = s.Substring(1, s.Length - 3);
            //    string key = kvp.Split(',')[0].Trim(); key = key.Substring(1, key.Length - 2);
            //    string val = kvp.Split(',')[1].Trim();

            //    int id = Int32.Parse(val);

            //    txt += "{ \"" + key + "\", " + (id + 1) + " }," + Environment.NewLine;
            //}

            //File.WriteAllText("pkmn2.txt", txt);

            // ----------------------------------------------------------

            //string txt = String.Empty;

            //for (int i = 1; i <= 721; i++)
            //{
            //    try
            //    {
            //        JsonData d = DataFetcher.GetPokemon(i);

            //        txt += "{ \"" + d["name"].ToString().ToLower() + "\", " + i + " }," + Environment.NewLine;

            //        Console.WriteLine(i);
            //    }
            //    catch { }
            //}

            //File.WriteAllText("pkmn.txt", txt);

            // ----------------------------------------------------------

            Pokemon p = Pokemon.GetInstance("Lucario");
            var tids = PokemonType.Combine(p.Types.FillNew(nup => (PokemonType)nup.GetResource())).AnalyzeIDs();
            int baseHp = p.HP;

            // ----------------------------------------------------------

            //Pokedex p = Pokedex.GetInstance();

            //int i = 0; // breakpoint here
            //i++;

            // ----------------------------------------------------------

            //WriteDictionarySnippets();
        }

        static void WriteDictionarySnippets()
        {
            using (FileStream fs = new FileStream("allSprites.txt", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs))
                {
                    DataFetcher.CacheAllSprites();

                    w.WriteLine("{ ");

                    int i = 0;
                    foreach (JsonData data in DataFetcher.SpriteData.Values)
                    {
                        if (i >= DataFetcher.SpriteData.Count)
                            break;
                        string name = (string)data["name"];
                        if (name.EndsWith("_auto"))
                            name = name.Substring(0, name.Length - 5);
                        w.Write("{\"");
                        w.Write(name);
                        w.Write("\", ");
                        w.Write(((int)data["id"]).ToString());
                        w.Write("}");
                        Console.WriteLine(data["name"]);

                        if (i++ < DataFetcher.SpriteData.Count - 1)
                            w.Write(",\n");
                    }

                    w.WriteLine(" };");

                    w.Close();
                }

                fs.Close();
            }
        }
    }
}
