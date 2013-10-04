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
            return Root.Contains(value, 8);
        }

        public void Add(long value)
        {
            Root.Add(value, 8);
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
        //public Dictionary<int, BitSetNode> Values = new Dictionary<int, BitSetNode>();
        private Byte _leafValue = 0;

        private BitSetNode[] Values;

        public void Add(long input, int chunksLeft)
        {
            if (chunksLeft == 1)
            {
                _leafValue = (Byte)(_leafValue | getByte(input, chunksLeft));
            }
            else
            {
                if (Values == null)
                    Values = new BitSetNode[256];

                int ConvertedCurrentChunk = getByte(input, chunksLeft);

                if (Values[ConvertedCurrentChunk] == null)
                    Values[ConvertedCurrentChunk] = new BitSetNode();

                Values[ConvertedCurrentChunk].Add(input, chunksLeft - 1);        
            }

        }


        public bool Contains(long input, int chunksLeft)
        {
            if (chunksLeft == 1)
            {
                var convertedBooleans = getByte(input, chunksLeft);
                var outputOfCheck = _leafValue & convertedBooleans;

                 if(outputOfCheck == convertedBooleans)
                     return true;
            }

            int ConvertedCurrentChunk = getByte(input, chunksLeft);

            if (chunksLeft != 1)
            {
                if (Values[ConvertedCurrentChunk] != null)
                    return Values[ConvertedCurrentChunk].Contains(input, chunksLeft - 1);
            }

            

            return false;
        }

        public Byte getByte(long input, int ByteIndex)
        {
            //8 is most siginificant byte
            //1 is the least significant byte

            int shiftToLeft = 64 - (ByteIndex * 8);

            //push to left:
            //    get rid of bytes that are more significant

            var postLeftShift = input << shiftToLeft;

            //push to right:
            //    make proper byte least significant

            var postRightShift = postLeftShift >> 56;

            var output = (byte) postRightShift;

            return output;
        }
    }
}
