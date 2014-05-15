using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRealMachine
{
    public class InputDevice
    {
        Queue<MemoryBlock> inputQueue;

        public InputDevice()
        {
            inputQueue = new Queue<MemoryBlock>();
        }

        public MemoryBlock getInput()
        {
            try
            {
                return inputQueue.Dequeue();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("There is no input in queue");
            }
        }

        public void enqueueInput(MemoryBlock block)
        {
            inputQueue.Enqueue(block);
        }
    }
}
