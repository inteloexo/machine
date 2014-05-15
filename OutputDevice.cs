using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRealMachine
{
    public class OutputDevice
    {
        Queue<MemoryBlock> outputQueue;

        public OutputDevice()
        {
            outputQueue = new Queue<MemoryBlock>();
        }

        public void setOutput(MemoryBlock block)
        {
            outputQueue.Enqueue(block);
        }

        public MemoryBlock getOutput()
        {
            try
            {
                return outputQueue.Dequeue();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Output is empty");
            }
        }

        public bool outputExists()
        {
            if (outputQueue.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
