using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadixBitSetTree
{
    using NUnit.Framework;

    [TestFixture]
    public class BitSetExercises
    {
        //Bitwise Operations (Exercises):
        //Read about the following operators on MSDN:  |, &, >>, <<, ~, ^
        //Solve the following using bitwise operators.

        //1.  Write a method that takes a long parameter x and
        //returns the same long but with all but the 5 least significant bits set to 0.

        [TestCase(25)]
        public void One_leftShift(long input)
        {
            long output = input << 5;

            //Largest value to be returned:
            //9223372036854775776
        }


        //What is the maximum value that can be returned from this method?

        //2.  Write a method that takes a long parameter x and returns the same long 
        //but the 5 least significant bits (starting with bit 0) set to 1.


        //http://stackoverflow.com/questions/12416639/how-to-create-mask-with-least-significat-bits-set-to-1-in-c
        [TestCase(25)]
        public void Two_ShiftWithOnes(long input)
        {
            long postShift = input << 5;

            long middleBit = postShift - 1;

            long final = postShift | middleBit;
        }



        //3. Write a method that takes two longs, x and y, and returns a long such that if 
        //a particular bit is set to 1 in x and set to 0 in y, it is set to 1 in the result.  Otherwise it is zero.

        //For example:
        //If x =  00101010
        //And y = 00111000
        //Result =00000010

        //Then the result should be: 00000010
        
        [TestCase(42, 56)]
        public void Three_ShiftWithOnes(long x, long y)
        {
            

            var invertedY = ~ y;

            long output = invertedY & x;

            long expectedOutput = 2;

            Assert.AreEqual(output, expectedOutput);

        }



        

        //4.  Write a method that takes a long and returns a count of the bits that are set to 1.


        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 2)]
        public void Four_GetBitCount(long input, int expectedCount)
        {
            long inputWithShift = input;

            int bitCount = 0;

            while (true)
            {
                if (inputWithShift == 0) 
                    break;

                if (inputWithShift % 2 == 1)
                    bitCount++;

                inputWithShift = inputWithShift >> 1;
            }

            Assert.AreEqual(expectedCount, bitCount);
        }

        //5a.  Write a method that takes a long and returns the most significant byte (as a value of type byte).

        [TestCase(-72057594037927936, 255)]
        public void Five_ShiftWithOnes(long input, int expectedOutput)
        {


            var shiftedInput =  input >> 56;
            var byteoutput = (byte) shiftedInput;

            Assert.AreEqual(byteoutput, expectedOutput);
        }


        //6b.  Generalize the method you wrote in part a to take a second argument n and returns the nth most significant byte.
        //For n == 0, the result should be the same as part a.

        [TestCase(-72057594037927936, 8, 255)]
        [TestCase(71776119061217280, 7, 255)]
        [TestCase(280375465082880, 6, 255)]
        [TestCase(1095216660480, 5, 255)]
        public void Six_ShiftNthByte(long input, int significantByte, long expectedOutput)
        {
            //8 is most siginificant byte
            //1 is the least significant byte
           
            int shiftToLeft = 64 - (significantByte * 8);
            
            //push to left:
            //    get rid of bytes that are more significant

            var postLeftShift = input << shiftToLeft;

            //push to right:
            //    make proper byte least significant

            var postRightShift = postLeftShift >> 56;

            var output = (byte) postRightShift;


            Assert.AreEqual(expectedOutput, output);
        }



        //7.  Write a method that takes a long x and a number from 0 – 63 and returns true if that particular bit is set to 1 and false if it set to 0.
        //Assume that bit 0 is the least significant bit and bit 63 is the most significant bit.

        [TestCase(1, 0, true)]
        [TestCase(2, 0, false)]
        [TestCase(2, 1, true)]
        [TestCase(128, 7, true)]
        public void Seven_ShiftNthByte(long input, int bitIndex, bool expectedOutput)
        {
            //right shift
            //is it even?

            long shiftedValue = input >> bitIndex;

            bool output = (shiftedValue%2 == 1);
            
            Assert.AreEqual(expectedOutput, output);
        }



    }
}
