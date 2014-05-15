using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRealMachine
{
    public class Word
    {
        private char[] value;
        public const int WORD_LENGTH = 4;

        public Word(string stringWord)
        {
            value = new char[WORD_LENGTH];
            value = "0000".ToCharArray();
            setWord(stringWord);
        }

        public void setWord(string stringWord)
        {
            if (stringWord.Length <= WORD_LENGTH)
            {
                //value = stringWord.ToCharArray();
                stringWord.CopyTo(0, value, WORD_LENGTH - stringWord.Length, stringWord.Length);
            }
            else
            {
                throw new FormatException("Valid word cannot be created!");
            }
        }

        //byte number from 1 to 4. 1st is eldest byte.
        public char getWordByte(int byteNumber)
        {
            try
            {
                return value[byteNumber-1];
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Byte number must be from 1 to 4!");
            }
        }

        public int toInt()
        {
            try
            {
                int intValue = int.Parse(Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value)));
                return intValue;
            }
            catch
            {
                throw new Exception("Word cannot be converted into integer!");
            }
        }

        public override String ToString()
        {
            return new String(value);
        }
    }
}
