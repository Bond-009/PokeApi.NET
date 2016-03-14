using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LitJson;
using Xunit;

namespace PokeAPI.Tests
{
    static class XUnitRunnerDoesntWorkSoIllDoItThisWay
    {
        static void Main(string[] args)
        {
            //var pt = DataFetcher.GetApiObject<PokemonSpecies>(1);
            //var p = pt.Result;

            bool hasErrors = false;

            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && !t.ContainsGenericParameters && (t.IsClass || t.IsValueType)))
            {
                object i = null;
                try
                {
                    i = Activator.CreateInstance(t);
                }
                catch when (!Debugger.IsAttached)
                {
                    Console.WriteLine("Skipped type " + t.FullName + ": could not instantiate type.");

                    continue;
                }

                foreach (var m in t.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance).Where(mi => mi.GetParameters().Length == 0 && mi.GetCustomAttribute<FactAttribute>() != null))
                {
                    try
                    {
                        m.Invoke(i, null);
                    }
                    catch (Exception e) when (!Debugger.IsAttached)
                    {
                        Console.Error.WriteLine("Test " + t.FullName + "." + m.Name + " failed: " + e);
                        hasErrors = true;
                    }
                }
            }

            if (hasErrors)
            {
                Console.Beep(220, 500);
                Console.ReadKey(true);
            }
        }
    }
}
