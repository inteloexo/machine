using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRealMachine
{
    public class CPU
    {        
        public Register4B A = new Register4B();
        public Register4B B = new Register4B();
        public Register4B IC = new Register4B();
        public Register4B SP = new Register4B();
        public Register4B PR = new Register4B();
        public Register2B TIMER = new Register2B();
        public Register1B RC = new Register1B();
        public Register1B M = new Register1B();
        public Register1B PI = new Register1B();
        public Register1B SI = new Register1B();
        public Register1B IOI = new Register1B();
        public Register1B TI = new Register1B();
        public Register1B MODE = new Register1B();
        public Register1B K1 = new Register1B();
        public Register1B K2 = new Register1B();
        public Register1B K3 = new Register1B();
        public Register1B C = new Register1B();
        private Memory supervisorMemory;
        private bool needTest = false;
        public bool stopMachine = false;
        public char tempK1 = '0';
        public char tempK2 = '0';
        public char tempK3 = '0';

        public CPU(ref Memory supervisorMemory)
        {
            this.supervisorMemory = supervisorMemory;
        }

        public void execute(Interpretator interpretator)
        {
            try
            {
                if (!needTest)
                {
                    interpretator.changeInterpretatorMemory();
                    int commandAddress;
                    if (MODE.getValue() == 'V')
                    {
                        commandAddress = IC.getValue().toInt() % 100;
                        commandAddress = getRealAddress(ref interpretator.memory, commandAddress);
                    }
                    else
                    {
                        commandAddress = IC.getValue().toInt();
                    }

                    Word command = interpretator.memory.getWordAtAddress(commandAddress);
                    int decTimerValue = interpretator.interpretate(command);
                    decTIMER(decTimerValue);
                    needTest = true;
                }
                else
                {
                    if (interpretator.incIC)
                    {
                        if (((IC.getValue().toInt() % 100 == 99) && (MODE.getValue() == 'V')) ||
                            ((IC.getValue().toInt() == 399) && (MODE.getValue() == 'S')))
                            PI.setValue('5');
                        incRegister(ref IC);
                    }
                    else
                    {
                        interpretator.incIC = true;
                    }
                    tempK1 = K1.getValue();
                    tempK2 = K2.getValue();
                    tempK3 = K3.getValue();
                    test();
                    needTest = false;
                }
            }
            catch (Exception)
            {
                PI.setValue('1');
            }

        }

        public void addRegisterMemory(ref Register4B register, Word word)
        {
            try
            {
                int op1, op2;
                Word tempWord = new Word("0000");

                op1 = register.getValue().toInt();
                op2 = word.toInt();
                op1 = op1 + op2;
                if (op1 > 9999)
                    PI.setValue('3');
                else
                {
                    tempWord.setWord(op1.ToString());
                    register.setValue(tempWord);
                }
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void addRegisters(ref Register4B register1, Register4B register2)
        {
            try
            {
                int op1, op2;
                Word tempWord = new Word("0000");

                op1 = register1.getValue().toInt();
                op2 = register2.getValue().toInt();
                op1 = op1 + op2;
                if (op1 > 9999)
                    PI.setValue('3');
                else
                {
                    tempWord.setWord(op1.ToString());
                    register1.setValue(tempWord);
                }
            }
            catch (Exception)
            {        
                PI.setValue('1');
            }
        }

        public void subRegisterMemory(ref Register4B register, Word word)
        {
            try
            {
                int op1, op2;
                Word tempWord = new Word("0000");

                op1 = register.getValue().toInt();
                op2 = word.toInt();
                if (op1 >= op2)
                {
                    op1 = op1 - op2;
                    tempWord.setWord(op1.ToString());
                    register.setValue(tempWord);
                }
                else
                    PI.setValue('4');
            }
            catch (Exception)
            {
                PI.setValue('1');
            }               
        }

        public void subRegisters(ref Register4B register1, Register4B register2)
        {
            try
            {
                int op1, op2;
                Word tempWord = new Word("0000");

                op1 = register1.getValue().toInt();
                op2 = register2.getValue().toInt();
                if (op1 >= op2)
                {
                    op1 = op1 - op2;
                    tempWord.setWord(op1.ToString());
                    register1.setValue(tempWord);
                }
                else
                    PI.setValue('4');
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void mulRegisterMemory(ref Register4B register, Word word)
        {
            try
            {
                int op1, op2;
                Word tempWord = new Word("0000");

                op1 = register.getValue().toInt();
                op2 = word.toInt();
                op1 = op1 * op2;
                if (op1 > 9999)
                    PI.setValue('3');
                else
                {
                    tempWord.setWord(op1.ToString());
                    register.setValue(tempWord);
                }
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void mulRegisters(ref Register4B register1, Register4B register2)
        {
            try
            {
                int op1, op2;
                Word tempWord = new Word("0000");

                op1 = register1.getValue().toInt();
                op2 = register2.getValue().toInt();
                op1 = op1 * op2;
                if (op1 > 9999)
                    PI.setValue('3');
                else
                {
                    tempWord.setWord(op1.ToString());
                    register1.setValue(tempWord);
                }
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void divRegisterMemory(Register4B register, Word word)
        {
            try
            {
                int op1, op2, op3;
                Word tempWord = new Word("0000");

                op1 = register.getValue().toInt();
                op2 = word.toInt();
                if (op1 == 0)
                    PI.setValue('2');
                else
                {
                    op3 = op1 % op2;
                    op1 = op1 / op2;
                    tempWord.setWord(op1.ToString());
                    A.setValue(tempWord);
                    tempWord.setWord(op3.ToString());
                    B.setValue(tempWord);
                }
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void divRegisters(Register4B register1, Register4B register2)
        {
            try
            {
                int op1, op2, op3;
                Word tempWord = new Word("0000");

                op1 = register1.getValue().toInt();
                op2 = register2.getValue().toInt();
                if (op1 == 0)
                    PI.setValue('2');
                else
                {
                    op3 = op1 % op2;
                    op1 = op1 / op2;
                    tempWord.setWord(op1.ToString());
                    A.setValue(tempWord);
                    tempWord.setWord(op3.ToString());
                    B.setValue(tempWord);
                }
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        private void incRegister(ref Register4B register)
        {
            try
            {
                Word tempWord = new Word("0000");
                int op = register.getValue().toInt();
                if (op == 9999)
                    PI.setValue('3');
                else
                {
                    op++;
                    tempWord.setWord(op.ToString());
                    register.setValue(tempWord);
                }
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void decRegister(ref Register4B register)
        {
            try
            {
                Word tempWord = new Word("0000");
                int op = register.getValue().toInt();
                if (op == 0)
                    PI.setValue('4');
                else
                {
                    op--;
                    tempWord.setWord(op.ToString());
                    register.setValue(tempWord);
                }
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void loadRegister(ref Register4B register, Word word)
        {
            register.setValue(word);
        }

        private void saveRegister(Register4B register, ref Memory memory, int address)
        {
            memory.setWordAtAddress(address, register.getValue());
        }

        public void copyRegister(ref Register4B register1, Register4B register2)
        {
            register1.setValue(register2.getValue());
        }

        public void compRegisterMemory(ref Register4B register, Word word)
        {
            try
            {
                int op1, op2;

                op1 = register.getValue().toInt();
                op2 = word.toInt();
                if (op1 > op2)
                    C.setValue('1');
                else
                    if (op1 < op2)
                        C.setValue('2');
                    else
                        C.setValue('0');
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
            
        }

        public void compRegisters(Register4B register1, Register4B register2)
        {
            try
            {
                int op1, op2;

                op1 = register1.getValue().toInt();
                op2 = register2.getValue().toInt();
                if (op1 > op2)
                    C.setValue('1');
                else
                    if (op1 < op2)
                        C.setValue('2');
                    else
                        C.setValue('0');
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void jump(int address)
        {
            Word word = new Word(address.ToString());
            IC.setValue(word);
        }

        public void conditionalJump(int address, char ch)
        {
            switch (ch)
            {
                case 'E':
                    if (C.getValue() == '0')
                        jump(address);
                    break;
                case 'G':
                    if (C.getValue() == '1')
                        jump(address);
                    break;
                case 'L':
                    if (C.getValue() == '2')
                        jump(address);
                    break;
            }
        }

        public void input(Memory memory, InputDevice inputDevice, int blockNumber)
        {
            try
            {
                if (exchange(K1) == true)
                {
                    K1.setValue('1');
                    tempK1 = '1';
                    memory.setBlock(blockNumber, inputDevice.getInput());
                    K1.setValue('0');
                }
            }
            catch (Exception)
            {
                IOI.setValue((char)(IOI.getIntValue() + 1 + '0')); 
            }
        }

        public void output(Memory memory, OutputDevice outputDevice, int blockNumber)
        {
            try
            {
                if (exchange(K2) == true)
                {
                    K2.setValue('1');
                    tempK2 = '1';
                    outputDevice.setOutput(memory.getBlock(blockNumber));
                    K2.setValue('0');
                }
            }
            catch(Exception)
            {
                IOI.setValue((char)(IOI.getIntValue() + 2 + '0')); 
            }
        }

        public void input(Memory memory, HDDManager hddManager, int memoryWordAddress, int hddWordAddress)
        {
            try
            {
                if (exchange(K3) == true)
                {
                    K3.setValue('1');
                    tempK3 = '1';
                    memory.setWordAtAddress(memoryWordAddress, hddManager.getWordAtAddress(hddWordAddress));
                    K3.setValue('0');
                }
            }
            catch (Exception)
            {
                IOI.setValue((char)(IOI.getIntValue() + 4 + '0')); 
            }
        }

        public void output(Memory memory, HDDManager hddManager, int memoryWordAddress, int hddWordAddress)
        {
            try
            {
                if (exchange(K3) == true)
                {
                    K3.setValue('1');
                    tempK3 = '1';
                    hddManager.setWordAtAddress(hddWordAddress, memory.getWordAtAddress(memoryWordAddress));
                    K3.setValue('0');
                }
            }
            catch (Exception)
            {
                IOI.setValue((char)(IOI.getIntValue() + 4 + '0')); 
            }
        }

        public void push(Register4B register, ref Memory memory)
        {
            try
            {
                saveRegister(register, ref memory, SP.getValue().toInt());
                incRegister(ref SP);
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void push(Register2B register, ref Memory memory)
        {
            try
            {
                memory.setWordAtAddress(SP.getValue().toInt(), new Word(register.getValue().ToString()));
                incRegister(ref SP);
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void push(Register1B register, ref Memory memory)
        {
            try
            {
                memory.setWordAtAddress(SP.getValue().toInt(), new Word(register.getValue().ToString()));
                incRegister(ref SP);
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void pop(ref Register4B register, ref Memory memory)
        {
            try
            {
                decRegister(ref SP);
                loadRegister(ref register, memory.getWordAtAddress(SP.getValue().toInt()));
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void pop(ref Register2B register, ref Memory memory)
        {
            try
            {
                decRegister(ref SP);
                register.setValue(memory.getWordAtAddress(SP.getValue().toInt()).ToString());
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void pop(ref Register1B register, ref Memory memory)
        {
            try
            {
                decRegister(ref SP);
                register.setValue(memory.getWordAtAddress(SP.getValue().toInt()).getWordByte(4));
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
        }

        public void getRegister(Register4B register)
        {
            A.setValue(register.getValue());
        }

        public void setRegister(ref Register4B register)
        {
            register.setValue(A.getValue());
        }

        public void getRegister(Register2B register)
        {
            Word word = new Word(register.getValue().ToString());
            A.setValue(word);
        }

        public void setRegister(ref Register2B register)
        {
            string str = A.getValue().getWordByte(3).ToString() + A.getValue().getWordByte(4);
            register.setValue(str);

        }

        public void getRegister(Register1B register)
        {
            Word word = new Word(register.getValue().ToString());
            A.setValue(word);
        }

        public void setRegister(ref Register1B register)
        {
            char ch = A.getValue().getWordByte(4);
            register.setValue(ch);
        }

        public void slave()
        {
            supervisorMemory.setWordAtAddress(M.getIntValue() * 10, A.getValue());
            supervisorMemory.setWordAtAddress(M.getIntValue() * 10 + 1, B.getValue());
            supervisorMemory.setWordAtAddress(M.getIntValue() * 10 + 2, new Word(C.getValue().ToString()));
            supervisorMemory.setWordAtAddress(M.getIntValue() * 10 + 3, PR.getValue());
            supervisorMemory.setWordAtAddress(M.getIntValue() * 10 + 4, SP.getValue());
            Word increasedIC = null;
            try
            {
                increasedIC = new Word(IC.getValue().ToString());
            }
            catch (Exception)
            {
                PI.setValue('1');
            }

            supervisorMemory.setWordAtAddress(M.getIntValue() * 10 + 5, increasedIC);
        }

        public void halt()
        {
            if (MODE.getValue() == 'S')
            {
                stopMachine = true;
            }
            else
            {
                SI.setValue('3');
            }
        }

        public void changeMode(int address, Memory memory)
        {
            if (MODE.getValue() == 'S')
                MODE.setValue('V');
            else
                MODE.setValue('S');
            loadRegister(ref IC, memory.getWordAtAddress(address));
        }

        public bool exchange(Register1B channel)
        {
            if (channel.getIntValue() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void test()
        {
            if (PI.getValue() != '0')
            {
                handlePI();
                PI.setValue('0');
            }
            if (SI.getValue() != '0')
            {
                handleSI();
                SI.setValue('0');
            }
            if (IOI.getValue() != '0')
            {
                handleIOI();
                IOI.setValue('0');
            }
            if (TI.getValue() != '0')
            {
                handleTI();
                TI.setValue('0');
            }
        }

        private void handleInterrupt(int address)
        {
            Word tempWord = new Word("0000");
            if (MODE.getValue() != 'S')
            {
                slave();
                MODE.setValue('S');
            }            
            tempWord.setWord(address.ToString());
            IC.setValue(tempWord);            
        }

        private void handlePI()
        {
            char ch = PI.getValue();

            switch (ch)
            {
                case '1':
                    handleInterrupt(140);
                    break;
                case '2':
                    handleInterrupt(150);
                    break;
                case '3':
                    handleInterrupt(160);
                    break;
                case '4':
                    handleInterrupt(170);
                    break;
                case '5':
                    handleInterrupt(180);
                    break;
            }
        }

        private void handleSI()
        {
            Word word = new Word("0000");
            char ch = SI.getValue();

            switch (ch)
            {
                case '1':
                    handleInterrupt(190);
                    break;
                case '2':
                    handleInterrupt(200);
                    break;
                case '3':
                    handleInterrupt(210);
                    break;
            }
        }

        private void handleIOI()
        {
            Word word = new Word("0000");
            char ch = IOI.getValue();

            switch (ch)
            {
                case '1':
                    handleInterrupt(220);
                    break;
                case '2':
                    handleInterrupt(230);
                    break;
                case '3':
                    handleInterrupt(240);
                    break;
                case '4':
                    handleInterrupt(250);
                    break;
                case '5':
                    handleInterrupt(260);
                    break;
                case '6':
                    handleInterrupt(270);
                    break;
                case '7':
                    handleInterrupt(280);
                    break;
            }
        }

        private void handleTI()
        {
            handleInterrupt(240);
        }

        public int getRealAddress(ref Memory memory, int virtualAddress)
        {
            int x1 = 0;
            int x2 = 0;
            int a3 = 0;
            int a4 = 0;
            try
            {
                a3 = PR.getValue().toInt() / 10 % 10;
                a4 = PR.getValue().toInt() % 10;
                x1 = virtualAddress / 10;
                x2 = virtualAddress % 10;                
            }
            catch (Exception)
            {
                PI.setValue('1');
            }
            return 10 * memory.getWordAtAddress(10 * (10 * a3 + a4) + x1).toInt() + x2;
        }

        private void decTIMER(int value)
        {
            int timer = Int32.Parse(new String(TIMER.getValue()));
            if (MODE.getValue() == 'V')
                if (timer > value)
                    timer = timer - value;
                else
                {
                    timer = 0;
                    TI.setValue('1');
                }
            if (timer < 10)
            {
                TIMER.setValue(String.Concat('0', timer.ToString()));
            }
            else
            {
                TIMER.setValue(timer.ToString());
            }
        }
    }
}
