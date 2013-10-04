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
            string fileLocation = @"C:\Users\bwalsh\Dropbox\.5 - Inbox\PerformanceTest";

            PerformanceTest pt = new PerformanceTest(fileLocation);

            pt.RunTest();
        }

    }

    public class PerformanceTest
    {
        int _start = 10000000;
        int _end             = 100000000;
        private int _interval = 10000000;


        private string _baseDirectory = String.Empty;
        private string _timeStamp = String.Empty;


        public PerformanceTest(string fileLocation)
        {
            _baseDirectory = fileLocation;
        }


        public void RunTest()
        {
            int current = _start;

            while (true)
            {
                if (current > _end)
                    break;

                if (!MemoryStressTest(current))
                    break;

                current = current + _interval;
            }

            Log("Done");
            Console.Read();
        }

        BitSetTree bst = new BitSetTree();
        List<long> listOLongs = new List<long>();

        public bool MemoryStressTest(long current)
        {
                Log("Started w/ " + current + " elements");

                Log("Adding the elements");

                for (long i = 0; i < current; i++)
                    bst.Add(i);


                Log("Contains the elements");

                for (long i = 0; i < current; i++)
                {
                    if (!bst.Contains(i))
                    {
                        this.Log("Not found in tree" + i);
                    }
                }    
                    
                    

                return true;
        }

        
        public void Log(string line = "")
        {
            if (_timeStamp == String.Empty)
                _timeStamp = String.Format("{0}_{1}_{2}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString());

            string absPath = _baseDirectory + "\\" + _timeStamp + ".txt";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(absPath,true))
            {
                Console.WriteLine(line);
                file.WriteLine(line);
            }
        }
    }

}
