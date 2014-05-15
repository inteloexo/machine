using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRealMachine
{
    public class MemoryBlock
    {
        private Word[] words;
        public const int WORDS_IN_BLOCK = 10;

        public MemoryBlock()
        {
            words = new Word[WORDS_IN_BLOCK];
            for (int i = 0; i < WORDS_IN_BLOCK; i++)
            {
                words[i] = new Word("0000");
            }
        }

        public Word getBlockWord(int position)
        {
            try
            {
                return words[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("There is no such position in block");
            }        
        }

        public void setBlockWord(int position, Word word)
        {
            try
            {
                words[position] = word;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("There is no such position in block");
            }   
        }
    }
}
