using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RadixBitSetTree
{
    [TestFixture]
    public class BitSetTree_Test
    {
        BitSetTree bst = new BitSetTree();

        [Test]
        public void Add_IntegrationTest()
        {
            bst.Add(50);

            int stuff = 0;
        }

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
            int valuesRange = 100;

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
        public void SerializeTree()
        {
            int valuesRange = 100;

            for (int i = 0; i < valuesRange; i++)
            {
                bst.Add(i);
            }

            bst.XmlSerialize(@"C:\Users\Brian\Dropbox\.5 - Inbox\TestTree.xml");
        }

    }

    [TestFixture]
    public class BitSetNode_Test
    {
        [Test]
        public void getInteger()
        {
            BitSetNode bsn = new BitSetNode();

            bool[] zero = new bool[2]{false,false};
            bool[] one = new bool[2] {false, true};
            bool[] two = new bool[2] {true, false};
            bool[] three = new bool[2] {true, true};

            Assert.AreEqual(0, bsn.getInteger(zero));
            Assert.AreEqual(1, bsn.getInteger(one));
            Assert.AreEqual(2, bsn.getInteger(two));
            Assert.AreEqual(3, bsn.getInteger(three));
        }

        [Test]
        public void sanityCheck()
        {
            var fifty = 50;
            Byte[] byteArray = BitConverter.GetBytes(fifty);
            BitArray ba = new BitArray(byteArray);

            bool[] fiftyArray = new bool[64];
            ba.CopyTo(fiftyArray, 0);



            var fourtyNine = 49;
            Byte[] byteArray2 = BitConverter.GetBytes(fourtyNine);
            BitArray ba2 = new BitArray(byteArray2);

            bool[] FourtyNineArray = new bool[64];
            ba2.CopyTo(FourtyNineArray, 0);

            var output = byteArray.SequenceEqual(byteArray2);

            Assert.IsFalse(output);
        }

    }

    
}
