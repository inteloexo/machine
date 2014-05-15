using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRealMachine
{
    class Machine
    {
        public CPU cpu;
        public Memory supervisorMemory;
        public Memory memory;
        public InputDevice inputDevice;
        public OutputDevice outputDevice;
        public Interpretator interpretator;
        public HDDManager hddManager;


        public Machine()
        {
            supervisorMemory = new Memory(40);
            cpu = new CPU(ref supervisorMemory);
            memory = new Memory(100);
            inputDevice = new InputDevice();
            outputDevice = new OutputDevice();
            hddManager = new HDDManager("hdd.xml", 100, 10);
            interpretator = new Interpretator(ref cpu, ref memory, ref supervisorMemory, ref inputDevice,
                ref outputDevice, ref hddManager);

            initialize();
        }

        private void initialize()
        {
            cpu.MODE.setValue('V');
            cpu.TIMER.setValue("99");
            cpu.PR.setValue(new Word("0010"));
            cpu.IC.setValue(new Word("0020"));
            memory.setWordAtAddress(100, new Word("0000"));
            memory.setWordAtAddress(101, new Word("0001"));
            memory.setWordAtAddress(102, new Word("0002"));
            memory.setWordAtAddress(103, new Word("0003"));
            memory.setWordAtAddress(104, new Word("0004"));
            memory.setWordAtAddress(105, new Word("0005"));
            memory.setWordAtAddress(106, new Word("0006"));
            memory.setWordAtAddress(107, new Word("0007"));
            memory.setWordAtAddress(108, new Word("0008"));
            memory.setWordAtAddress(109, new Word("0009"));

            memory.setWordAtAddress(0, new Word("0008"));
            memory.setWordAtAddress(1, new Word("0005"));

            memory.setWordAtAddress(10, new Word("ATSA"));
            memory.setWordAtAddress(11, new Word("KYMA"));
            memory.setWordAtAddress(12, new Word("S:  "));

            memory.setWordAtAddress(20, new Word("LA00"));
            memory.setWordAtAddress(21, new Word("LB01"));
            memory.setWordAtAddress(22, new Word("+AB0"));
            memory.setWordAtAddress(23, new Word("SB13"));
            memory.setWordAtAddress(24, new Word("OU10"));
            memory.setWordAtAddress(25, new Word("HALT"));

            supervisorMemory.setWordAtAddress(140, new Word("HALT"));
            supervisorMemory.setWordAtAddress(200, new Word("XCHG"));
            supervisorMemory.setWordAtAddress(201, new Word("MO25"));

        }
    }
}
