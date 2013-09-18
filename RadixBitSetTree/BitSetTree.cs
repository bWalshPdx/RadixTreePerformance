using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RadixBitSetTree
{
    /*
     * 8 bytes in a long
     * 
     * 8 Bits in a Byte
     * 
     * Convert a long into a series of bytes (http://stackoverflow.com/questions/7201972/convert-datatype-long-to-byte-array)
     * 
     * Serialize in a xml file (http://www.codeproject.com/Tips/455861/Serialize-Deserialize-any-object-to-an-XML-file)
     * 
     * Save n longs in a text file vs. a xml file (raxtor tree)
     */

    public class BitSetTree
    {
        public BitSetNode Root;

        public BitSetTree()
        {
            Root = new BitSetNode();
        }

        public bool Contains(long value)
        {
            Byte[] byteArray = BitConverter.GetBytes(value);
            BitArray ba = new BitArray(byteArray);

            bool[] bitArray = new bool[64];
            ba.CopyTo(bitArray, 0);

            return Root.Contains(bitArray);
        }

        public void Add(long value)
        {
            Byte[] byteArray = BitConverter.GetBytes(value);
            BitArray ba = new BitArray(byteArray);

            bool[] bitArray = new bool[64];
            ba.CopyTo(bitArray, 0);

            Root.Add(bitArray);
        }

        public void Serialize(string fileLocation)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileLocation,
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, Root);
            stream.Close();
        }
    }

    [Serializable]
    public class BitSetNode
    {
        public Dictionary<int, BitSetNode> Values = new Dictionary<int, BitSetNode>();
        
        public BitSetNode()
        {
            Values.Add(0, null);
            Values.Add(1, null);
            Values.Add(2, null);
            Values.Add(3, null);
        }

        
        public void Add(bool[] input)
        {
            if (input.Count() == 0)
                return;

            bool[] currentChunk = new bool[2];
            bool[] nextChunk = new bool[input.Count() - 2];

            for (int i = 0; i < (input.Count() - 1); i++)
            {
                if (i <= 1)
                {
                    currentChunk[i] = input[i];
                }
                else
                {
                    nextChunk[i - 2] = input[i];
                }
            }

            int ConvertedCurrentChunk = getInteger(currentChunk);

            if (Values.ContainsKey(ConvertedCurrentChunk))
            {
                if (Values[ConvertedCurrentChunk] == null)
                    Values[ConvertedCurrentChunk] = new BitSetNode();
                
                //TODO: This should be a funciton of some sort since this is the only changed line:
                Values[ConvertedCurrentChunk].Add(nextChunk);
            }
        }

        public bool Contains(bool[] input)
        {
            if (input.Count() == 0)
                return true;

            bool[] currentChunk = new bool[2];
            bool[] nextChunk = new bool[input.Count() - 2];

            for (int i = 0; i < (input.Count() - 1); i++)
            {
                if (i <= 1)
                {
                    currentChunk[i] = input[i];
                }
                else
                {
                    nextChunk[i - 2] = input[i];
                }
            }

            int ConvertedCurrentChunk = getInteger(currentChunk);

            if (Values[ConvertedCurrentChunk] != null)
                    return Values[ConvertedCurrentChunk].Contains(nextChunk);
            
            return false;
        }


        public int getInteger(bool[] input)
        {
            int ConvertedCurrentChunk = 0;
            int value = 2;

            for (int i = 0; i < input.Count(); i++)
            {
                ConvertedCurrentChunk += input[i] == true ? value : 0;
                value--;
            }

            return ConvertedCurrentChunk;
        }
    }
}
