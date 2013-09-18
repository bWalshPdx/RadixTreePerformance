using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using RadixBitSetTree;

namespace RadixTreePerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            BitSetTree bst = new BitSetTree();
            List<long> listOLongs = new List<long>();

            int range = 10000000;

            Console.WriteLine("Started w/ " + range + " elements");

            Stopwatch sw = new Stopwatch();
            
            Console.WriteLine("Adding the elements");

            for (long i = 0; i < range; i++)
                listOLongs.Add(i);

            for (long i = 0; i < range; i++)
                bst.Add(i);

            Console.WriteLine("Finding all the elements...");

            sw.Start();

            for (int i = 0; i < range; i++)
            {
                if (!bst.Contains(i))
                    throw new Exception("Tree Element: " + i + " not found");
            }

            Console.WriteLine("Tree: Elapsed Time: " + sw.Elapsed);

            sw.Stop();

            sw.Reset();

            sw.Start();

            for (int i = 0; i < range; i++)
            {
                if (!listOLongs.Contains(i))
                    throw new Exception("Array Element: " + i + " not found");
            }

            sw.Stop();

            Console.WriteLine("List: Elapsed Time: " + sw.Elapsed);

            sw.Stop();

            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
