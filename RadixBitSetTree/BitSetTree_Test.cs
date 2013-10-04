using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RadixBitSetTree
{
    using System.Diagnostics;

    using RadixTreePerformance;

    [TestFixture]
    public class BitSetTree_Test
    {
        BitSetTree bst = new BitSetTree();

        [Test]
        public void SingleValueWorks()
        {
            bst.Add(50);

            var shouldBeTrue = bst.Contains(50);
            var shouldBeFalse = bst.Contains(49);

            Assert.IsTrue(shouldBeTrue);
            Assert.IsFalse(shouldBeFalse);
        }

        [Test]
        public void RangeOfValuesWork()
        {
            int valuesRange = 10000;

            for (int i = 0; i < valuesRange; i++)
            {
                bst.Add(i);
            }

            for (int i = 0; i < valuesRange; i++)
            {
                if (!bst.Contains(i))
                    throw new Exception(i + ": is not in the tree");
            }
        }

        [Test]
        public void BitSetTree_ValuesNotInTree()
        {
            BitSetTree bst1 = new BitSetTree();
            int ceiling = 100000;
            bool output = true;

            for (int i = 0; i < ceiling; i++)
            {
                if (i % 2 == 0)
                    bst1.Add(i);

                if (i % 2 == 1)
                {
                    if (!bst1.Contains(i))
                    {
                        output = false;
                    }
                }
            }

            Assert.IsFalse(output);
        }


    }

    [TestFixture]
    public class BitSetNode_Test
    {
        
        [TestCase(-72057594037927936, 8, 255)]
        [TestCase(47850746040811520, 7, 170)]
        public void getByte_ActuallyWorks(long input, int significantByte, long expectedOutput)
        {
            BitSetNode bsn = new BitSetNode();
            
            var convertedBooleans = bsn.getByte(input, significantByte);

            Assert.AreEqual(convertedBooleans, expectedOutput);
        }


        [TestCase(5, 5, true)]
        public void FinalBitCheck(int input, int leafValue, bool expectedOutput)
        {
            BitSetNode bsn = new BitSetNode();


            Byte[] byteArray = BitConverter.GetBytes(input);
            BitArray ba = new BitArray(byteArray);

            bool[] bitArray = new bool[64];
            ba.CopyTo(bitArray, 0);

            var convertedBooleans = bsn.getByte(input, 1);
            var outputOfCheck = leafValue & convertedBooleans;

            bool output = outputOfCheck == convertedBooleans;

            Assert.AreEqual(expectedOutput, output);
        }

    }




    [TestFixture]
    public class PerformanceTests
    {
        BitSetTree bst1 = new BitSetTree();
        HashSet<long> hs = new HashSet<long>();

        [TestCase(10000000)]
        public void ByteTreeVsHashSet(int ceiling)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (long i = 0; i < ceiling; i++)
                bst1.Add(i);

            for (long i = 0; i < ceiling; i++)
                bst1.Contains(i);

            sw.Stop();
            var treeTime =  sw.ElapsedMilliseconds;

            sw.Restart();

            for (long i = 0; i < ceiling; i++)
               hs.Add(i);

            for (long i = 0; i < ceiling; i++)
                hs.Contains(i);

            sw.Stop();
            var hashTime = sw.ElapsedMilliseconds;

            Debug.WriteLine("Byte Tree: " + treeTime);
            Debug.WriteLine("Hash Set: " + hashTime);

            if (treeTime < hashTime)
            {
                Debug.WriteLine("Byte Tree is faster");
            }
            else
            {
                Debug.WriteLine("Hash Set is faster");
            }
        }

        [TestCase(10000000)]
        public void ByteTreeVs234Tree(int ceiling)
        {
            Stopwatch sw = new Stopwatch();
            TwoThreeFourTree tree = new TwoThreeFourTree();

            sw.Start();
            for (int i = 0; i < ceiling; i++)
                bst1.Add(i);

            sw.Stop();
            var ByteTreeTime = sw.ElapsedMilliseconds;

            sw.Restart();

            for (int i = 0; i < ceiling; i++)
                tree.AddValue(i);

            sw.Stop();
            var TwoThreeFourTime = sw.ElapsedMilliseconds;

            Debug.WriteLine("Byte Tree: " + ByteTreeTime);
            Debug.WriteLine("234 Tree: " + TwoThreeFourTime);

            if (ByteTreeTime < TwoThreeFourTime)
            {
                Debug.WriteLine("Byte Tree is faster");
            }
            else
            {
                Debug.WriteLine("234 Tree is faster");
            }
        }


        [TestCase(10000000)]
        public void ByteTreeVsList(int ceiling)
        {
            Stopwatch sw = new Stopwatch();
            List<long> list = new List<long>();
            BitSetTree byteTree = new BitSetTree();

            sw.Start();
            for (long i = 0; i < ceiling; i++)
                byteTree.Add(i);

            for (long i = 0; i < ceiling; i++)
                byteTree.Contains(i);

            sw.Stop();
            var ByteTreeTime = sw.ElapsedMilliseconds;

            sw.Restart();

            for (long i = 0; i < ceiling; i++)
                list.Add(i);

            for (long i = 0; i < ceiling; i++)
                list.Contains(i);

            sw.Stop();
            var ListTime = sw.ElapsedMilliseconds;

            Debug.WriteLine("Byte Tree: " + ByteTreeTime);
            Debug.WriteLine("234 Tree: " + ListTime);

            if (ByteTreeTime < ListTime)
            {
                Debug.WriteLine("Byte Tree is faster");
            }
            else
            {
                Debug.WriteLine("List is faster");
            }
        }




    }

    [TestFixture]
    public class PerformanceGraph
    {
        private string _fileLocation = @"C:\Users\bwalsh\Dropbox\.5 - Inbox\PerfGraph\";

        [TestCase(10000000, 1000000)]
        public void ByteSetVsHashSet(long ceiling, long interval)
        {
            BitSetTree bst1 = new BitSetTree();
            HashSet<long> hs = new HashSet<long>();

            string absPath = _fileLocation + @"ByteSetVsHashSet.txt";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(absPath))
            {
                
                string treeAddTime = "";
                string hashAddTime = "";

                string treeContainsTime = "";
                string hashContainsTime = "";

                string line = "elements" + "\t" + "treeAddTime" + "\t" + "hashAddTime" + "\t" + "treeContainsTime" + "\t" + "hashContainsTime";

                file.WriteLine(line);

                for (long j = 0; j < ceiling; j = j + interval)
                {
                        
                    Stopwatch sw = new Stopwatch();

                    sw.Start();
                    for (long i = 0; i < j; i++)
                        bst1.Add(i);
                        
                    sw.Stop();
                    treeAddTime = sw.ElapsedMilliseconds.ToString();

                    sw.Restart();

                    for (long i = 0; i < j; i++)
                        bst1.Contains(i);

                    treeContainsTime = sw.ElapsedMilliseconds.ToString();

                    sw.Restart();

                    for (long i = 0; i < j; i++)
                        hs.Add(i);

                    sw.Stop();
                    hashAddTime = sw.ElapsedMilliseconds.ToString();

                    sw.Restart();

                    for (long i = 0; i < j; i++)
                        hs.Contains(i);

                    sw.Stop();
                    hashContainsTime = sw.ElapsedMilliseconds.ToString();

                    Console.WriteLine("Iteration Complete");

                    line = j +"\t" + treeAddTime + "\t" + hashAddTime + "\t" + treeContainsTime + "\t" + hashContainsTime;

                    file.WriteLine(line);
                }
                
            }
   
        }

        [TestCase(10000000, 1000000)]
        public void ByteSetVs234Tree(long ceiling, long interval)
        {
            BitSetTree bst1 = new BitSetTree();
            TwoThreeFourTree threeFourTree = new TwoThreeFourTree();

            string absPath = _fileLocation + @"ByteSetVs234Tree.txt";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(absPath))
            {

                string treeAddTime = "";
                string twoThreeFourAddTime = "";

                string line = "elements" + "\t" + "treeAddTime" + "\t" + "twoThreeFourAddTime";

                file.WriteLine(line);

                for (long j = 0; j < ceiling; j = j + interval)
                {

                    Stopwatch sw = new Stopwatch();

                    sw.Start();
                    for (long i = 0; i < j; i++)
                        bst1.Add(i);

                    sw.Stop();
                    treeAddTime = sw.ElapsedMilliseconds.ToString();

                    sw.Restart();

                    for (int i = 0; i < j; i++)
                        threeFourTree.AddValue(i);

                    sw.Stop();
                    twoThreeFourAddTime = sw.ElapsedMilliseconds.ToString();

                    Console.WriteLine("Iteration Complete");

                    line = j + "\t" + treeAddTime + "\t" + twoThreeFourAddTime;

                    file.WriteLine(line);
                }

            }

        }
        

    }

    
}
