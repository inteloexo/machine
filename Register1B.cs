using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRealMachine
{
    public class Register1B
    {
        private char value;

        public Register1B()
        {
            setValue('0');
        }

        public void setValue(char c)
        {
            value = c;
        }

        public char getValue()
        {
            return value;
        }

        public int getIntValue()
        {
            int intValue = (int) Char.GetNumericValue(value);
            if (intValue != -1)
            {
                return intValue;
            }
            else
            {
                throw new FormatException("Value of register is not a number");
            }
        }
    }
}
