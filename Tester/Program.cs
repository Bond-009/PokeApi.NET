using System;
using System.IO;
using LitJson;
using PokeAPI.NET;

namespace Tester
{
    static class Program
    {
        static void Main(string[] args)
        {
            // test list parser:
            //Pokedex p = Pokedex.GetInstance();

            //int i = 0; // breakpoint here
            //i++;

            // write dictionary ids:
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
