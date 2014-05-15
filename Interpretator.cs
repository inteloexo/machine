using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRealMachine
{
    public class Interpretator
    {
        private CPU cpu;
        private Memory ram;
        private Memory supervisorMemory;
        private InputDevice inputDevice;
        private OutputDevice outputDevice;
        private HDDManager hddManager;
        private int[] stackSize = new int[10];
        public bool incIC = true;
        public Memory memory;
        private int decTimerValue = 1;

        public Interpretator(ref CPU cpu, ref Memory ram, ref Memory supervisorMemory,
            ref InputDevice inputDevice, ref OutputDevice outputDevice,
            ref HDDManager hddManager)
        {
            this.cpu = cpu;
            this.ram = ram;
            this.supervisorMemory = supervisorMemory;
            this.inputDevice = inputDevice;
            this.outputDevice = outputDevice;
            this.hddManager = hddManager;
            this.memory = supervisorMemory;
        }

        public void changeInterpretatorMemory()
        {
           if (cpu.MODE.getValue() == 'S')
               this.memory = this.supervisorMemory;
           else
               this.memory = this.ram;
        }

        public int interpretate(Word word)
        {
            decTimerValue = 1;
            char ch1 = word.getWordByte(1);
            char ch2 = word.getWordByte(2);
            char ch3 = word.getWordByte(3);
            char ch4 = word.getWordByte(4);

            switch (ch1)
            {
                case '+': 
                    casePlus(ch2, ch3, ch4);
                    break;
                case '-':
                    caseMinus(ch2, ch3, ch4);
                    break;
                case '*':
                    caseMul(ch2, ch3, ch4);
                    break;
                case '/':
                    caseDiv(ch2, ch3, ch4);
                    break;
                case 'I':
                    caseI(ch2, ch3, ch4);
                    break;
                case 'D':
                    caseD(ch2, ch3, ch4);
                    break;
                case 'L':
                    caseL(ch2, ch3, ch4);
                    break;
                case 'S':
                    caseS(ch2, ch3, ch4);
                    break;
                case 'C':
                    caseC(ch2, ch3, ch4);
                    break;
                case 'J':
                    caseJ(ch2, ch3, ch4);
                    break;
                case 'O':
                    caseO(ch2, ch3, ch4);
                    break;
                case 'P':
                    caseP(ch2, ch3, ch4);
                    break;
                case 'G':
                    caseG(ch2, ch3, ch4);
                    break;
                case 'H':
                    caseH(ch2, ch3, ch4);
                    break;
                case 'M':
                    caseM(ch2, ch3, ch4);
                    break;
                case 'X':
                    caseX(ch2, ch3, ch4);
                    break;
                //case 'T':
                //    caseT(ch2, ch3, ch4);
                //    break;
                default:
                    notFound();
                    break;

            }
            return decTimerValue;
        }

        private bool isAddress(char ch3, char ch4)
        {
            if ((ch3 <= '9') && (ch3 >= '0'))
            {
                if ((ch4 <= '9') && (ch4 >= '0'))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private bool isAddress(string str)
        {
            int address = 0;
            try
            {
                address = Convert.ToInt32(str);
            }
            catch (FormatException)
            {
                notFound();
            }
            if ((address > -1) && (address < 1000))
                return true;
            else
                return false;
        }

        private int interpretateAddress(int address)
        {
            if (cpu.MODE.getValue() == 'S')
            {
                return 100 * cpu.RC.getIntValue() + address;
            }
            else
            {
                return cpu.getRealAddress(ref ram, address);
            }
        }

        private bool isSupervisorMode()
        {
            if (cpu.MODE.getValue() == 'S')
                return true;
            else
                return false;
        }

        private void casePlus(char ch2, char ch3, char ch4)
        {
            //+rx1x2
            if (isAddress(ch3, ch4))
            {
                int address = Convert.ToInt32(String.Concat(ch3, ch4));
                address = interpretateAddress(address);

                if (ch2 == 'A')
                    cpu.addRegisterMemory(ref cpu.A, memory.getWordAtAddress(address));
                else if (ch2 == 'B')
                    cpu.addRegisterMemory(ref cpu.B, memory.getWordAtAddress(address));
                else
                    notFound();
            }
            //+r1r20
            else
            {
                if (ch4 == '0')
                {
                    if (ch2 == 'A')
                    {
                        if (ch3 == 'A')
                        {
                            cpu.addRegisters(ref cpu.A, cpu.A);
                        }
                        else if (ch3 == 'B')
                            cpu.addRegisters(ref cpu.A, cpu.B);
                        else
                            notFound();
                    }
                    else if (ch2 == 'B')
                    {
                        if (ch3 == 'A')
                        {
                            cpu.addRegisters(ref cpu.B, cpu.A);
                        }
                        else if (ch3 == 'B')
                            cpu.addRegisters(ref cpu.B, cpu.B);
                        else
                            notFound();
                    }
                    else
                        notFound();
                }
                else
                    notFound();
            }            
        }

        private void caseMinus(char ch2, char ch3, char ch4)
        {
            //-rx1x2
            if (isAddress(ch3, ch4))
            {
                int address = Convert.ToInt32(String.Concat(ch3, ch4));
                address = interpretateAddress(address);

                if (ch2 == 'A')
                    cpu.subRegisterMemory(ref cpu.A, memory.getWordAtAddress(address));
                else if (ch2 == 'B')
                    cpu.subRegisterMemory(ref cpu.B, memory.getWordAtAddress(address));
                else
                    notFound();
            }
            //-r1r20
            else
            {
                if (ch4 == '0')
                {
                    if (ch2 == 'A')
                    {
                        if (ch3 == 'A')
                        {
                            cpu.subRegisters(ref cpu.A, cpu.A);
                        }
                        else if (ch3 == 'B')
                            cpu.subRegisters(ref cpu.A, cpu.B);
                        else
                            notFound();
                    }
                    else if (ch2 == 'B')
                    {
                        if (ch3 == 'A')
                        {
                            cpu.subRegisters(ref cpu.B, cpu.A);
                        }
                        else if (ch3 == 'B')
                            cpu.subRegisters(ref cpu.B, cpu.B);
                        else
                            notFound();
                    }
                    else
                        notFound();
                }
                else
                    notFound();
            }            
        }

        private void caseMul(char ch2, char ch3, char ch4)
        {
            //*rx1x2
            if (isAddress(ch3, ch4))
            {
                int address = Convert.ToInt32(String.Concat(ch3, ch4));
                address = interpretateAddress(address);

                if (ch2 == 'A')
                    cpu.mulRegisterMemory(ref cpu.A, memory.getWordAtAddress(address));
                else if (ch2 == 'B')
                    cpu.mulRegisterMemory(ref cpu.B, memory.getWordAtAddress(address));
                else
                    notFound();
            }
            //*r1r20
            else
            {
                if (ch4 == '0')
                {
                    if (ch2 == 'A')
                    {
                        if (ch3 == 'A')
                        {
                            cpu.mulRegisters(ref cpu.A, cpu.A);
                        }
                        else if (ch3 == 'B')
                            cpu.mulRegisters(ref cpu.A, cpu.B);
                        else
                            notFound();
                    }
                    else if (ch2 == 'B')
                    {
                        if (ch3 == 'A')
                        {
                            cpu.mulRegisters(ref cpu.B, cpu.A);
                        }
                        else if (ch3 == 'B')
                            cpu.mulRegisters(ref cpu.B, cpu.B);
                        else
                            notFound();
                    }
                    else
                        notFound();
                }
                else
                    notFound();
            }            
        }

        private void caseDiv(char ch2, char ch3, char ch4)
        {
            // /rx1x2
            if (isAddress(ch3, ch4))
            {
                int address = Convert.ToInt32(String.Concat(ch3, ch4));
                address = interpretateAddress(address);

                if (ch2 == 'A')
                    cpu.divRegisterMemory(cpu.A, memory.getWordAtAddress(address));
                else if (ch2 == 'B')
                    cpu.divRegisterMemory(cpu.B, memory.getWordAtAddress(address));
                else
                    notFound();
            }
            // /r1r20
            else
            {
                if (ch4 == '0')
                {
                    if (ch2 == 'A')
                    {
                        if (ch3 == 'A')
                        {
                            cpu.divRegisters(cpu.A, cpu.A);
                        }
                        else if (ch3 == 'B')
                            cpu.divRegisters(cpu.A, cpu.B);
                        else
                            notFound();
                    }
                    else if (ch2 == 'B')
                    {
                        if (ch3 == 'A')
                        {
                            cpu.divRegisters(cpu.B, cpu.A);
                        }
                        else if (ch3 == 'B')
                            cpu.divRegisters(cpu.B, cpu.B);
                        else
                            notFound();
                    }
                    else
                        notFound();
                }
                else
                    notFound();
            }            
        }

        private void caseI(char ch2, char ch3, char ch4)
        {
            if (ch2 == 'N')
            {
                if (ch3 == 'C')
                {
                    try
                    {
                        if (ch4 == 'A')
                        {
                            cpu.A.setValue(new Word((cpu.A.getValue().toInt() + 1).ToString()));
                        }
                        else if (ch4 == 'B')
                        {
                            cpu.B.setValue(new Word((cpu.B.getValue().toInt() + 1).ToString()));
                        }
                        else
                        {
                            notFound();
                        }
                    }
                    catch (Exception)
                    {
                        cpu.PI.setValue('1');
                    }
                }
                else if(isAddress(ch3, ch4))
                {
                    cpu.SI.setValue('1');
                    int address = Convert.ToInt32(String.Concat(ch3, ch4));
                    address = interpretateAddress(address);

                    string addressString = address.ToString();
                    cpu.A.setValue(new Word(String.Concat("I", cpu.MODE.getValue())));
                    cpu.B.setValue(new Word(addressString));
                    decTimerValue = 3;
                }
                else if (ch3 == 'H' && ch4 == 'A' && isSupervisorMode())
                {
                    try
                    {
                        cpu.input(supervisorMemory, hddManager, cpu.B.getValue().toInt() % 1000, cpu.A.getValue().toInt() % 1000);
                        decTimerValue = 3;
                    }
                    catch (Exception)
                    {
                        cpu.PI.setValue('1');
                    }
                }
                else
                {
                    notFound();
                }
            }
            else
            {
                notFound();
            }
        }

        private void caseD(char ch2, char ch3, char ch4)
        {
            if ((ch2 == 'E') && (ch3 == 'C'))
            {
                if (ch4 == 'A')
                    cpu.decRegister(ref cpu.A);
                else if (ch4 == 'B')
                    cpu.decRegister(ref cpu.B);
                else
                    notFound();
            }
            else
                notFound();
        }

        private void caseL(char ch2, char ch3, char ch4)
        {
            if (isAddress(ch3, ch4))
            {
                int address = Convert.ToInt32(String.Concat(ch3, ch4));
                address = interpretateAddress(address);

                if (ch2 == 'A')
                {
                    cpu.loadRegister(ref cpu.A, memory.getWordAtAddress(address));
                }
                else if (ch2 == 'B')
                {
                    cpu.loadRegister(ref cpu.B, memory.getWordAtAddress(address));
                }
                else if (ch2 == 'I')
                {
                    cpu.loadRegister(ref cpu.IC, memory.getWordAtAddress(address));
                }
                else
                    notFound();
            }
            else
                notFound();
        }

        private void caseC(char ch2, char ch3, char ch4)
        {
            switch (ch2)
            {
                case 'P':
                    if (ch3 == 'Y')
                    {
                        if (ch4 == 'A')
                        {
                            cpu.copyRegister(ref cpu.A, cpu.B);
                        }
                        else if (ch4 == 'B')
                        {
                            cpu.copyRegister(ref cpu.B, cpu.A);
                        }
                        else
                            notFound();
                    }
                    else
                        notFound();
                    break;
                case 'A':
                    if (isAddress(ch3, ch4))
                    {
                        int address = Convert.ToInt32(String.Concat(ch3, ch4));
                        address = interpretateAddress(address);

                        cpu.compRegisterMemory(ref cpu.A, memory.getWordAtAddress(address));
                    }
                    else if ((ch3 == 'A') && (ch4 == '0'))
                    {
                        cpu.compRegisters(cpu.A, cpu.A);
                    }
                    else if ((ch3 == 'B') && (ch4 == '0'))
                    {
                        cpu.compRegisters(cpu.A, cpu.B);
                    }
                    else 
                        notFound();
                    break;
                case 'B':
                    if (isAddress(ch3, ch4))
                    {
                        int address = Convert.ToInt32(String.Concat(ch3, ch4));
                        address = interpretateAddress(address);

                        cpu.compRegisterMemory(ref cpu.B, memory.getWordAtAddress(address));
                    }
                    else if ((ch3 == 'A') && (ch4 == '0'))
                    {
                        cpu.compRegisters(cpu.B, cpu.A);
                    }
                    else if ((ch3 == 'B') && (ch4 == '0'))
                    {
                        cpu.compRegisters(cpu.B, cpu.B);
                    }
                    else 
                        notFound();
                    break;                    
                default:
                    notFound();
                    break;
            }
        }

        private void caseS(char ch2, char ch3, char ch4)
        {
            if (ch2 == 'T')
            {
                string valueA = cpu.A.getValue().ToString();
                switch (ch3)
                {
                    case 'C':
                        if ((ch4 == '0') && ((valueA[3] == '0') || (valueA[3] == '1') || (valueA[3] == '2')))
                            cpu.setRegister(ref cpu.C);
                        else
                            notFound();
                        break;
                    case 'I':
                        if ((ch4 == 'C') && (isAddress(valueA)))
                        {
                            cpu.setRegister(ref cpu.IC);
                            incIC = false;
                        }
                        else if ((ch4 == 'O' && isSupervisorMode()) &&
                                ((valueA[3] == '0') || (valueA[3] == '1') || (valueA[3] == '2') ||
                                (valueA[3] == '3') || (valueA[3] == '4') || (valueA[3] == '5') ||
                                (valueA[3] == '6') || (valueA[3] == '7')))
                            cpu.setRegister(ref cpu.IOI);
                        else
                            notFound();
                        break;
                    case 'P':
                        if ((ch4 == 'R' && isSupervisorMode()) && (isAddress(valueA)))
                            cpu.setRegister(ref cpu.PR);
                        else if (ch4 == 'I' && isSupervisorMode() &&
                                ((valueA[3] == '0') || (valueA[3] == '1') || (valueA[3] == '2') ||
                                (valueA[3] == '3') || (valueA[3] == '4') || (valueA[3] == '5')))
                            cpu.setRegister(ref cpu.PI);
                        else
                            notFound();
                        break;
                    case 'S':
                        if ((ch4 == 'P' && isSupervisorMode()) && (isAddress(valueA)))
                            cpu.setRegister(ref cpu.SP);
                        else if (ch4 == 'I' && isSupervisorMode() &&
                                ((valueA[3] == '0') || (valueA[3] == '1') || (valueA[3] == '2') ||
                                (valueA[3] == '3')))
                            cpu.setRegister(ref cpu.SI);
                        else
                            notFound();
                        break;
                    case 'T':
                        if (ch4 == 'I' && isSupervisorMode() && ((valueA[3] == '0') || (valueA[3] == '1')))
                            cpu.setRegister(ref cpu.TI);
                        else if ((ch4 == 'M' && isSupervisorMode()) && (isAddress(valueA[2], valueA[3])))
                            cpu.setRegister(ref cpu.TIMER);
                        else
                            notFound();
                        break;
                    case 'M':
                        if (ch4 == '0' && isSupervisorMode() &&
                                ((valueA[3] == '0') || (valueA[3] == '1') || (valueA[3] == '2') ||
                                (valueA[3] == '3') || (valueA[3] == '4') || (valueA[3] == '5') ||
                                (valueA[3] == '6') || (valueA[3] == '7') || (valueA[3] == '8') ||
                                (valueA[3] == '9')))
                            cpu.setRegister(ref cpu.M);
                        else if (ch4 == 'O' && isSupervisorMode() && ((valueA[3] == 'S') || (valueA[3] == 'V')))
                            cpu.setRegister(ref cpu.MODE);
                        else
                            notFound();
                        break;
                    case 'R':
                        if (ch4 == 'C' && isSupervisorMode() &&
                                ((valueA[3] == '0') || (valueA[3] == '1') || (valueA[3] == '2') ||
                                (valueA[3] == '3') || (valueA[3] == '4') || (valueA[3] == '5') ||
                                (valueA[3] == '6') || (valueA[3] == '7') || (valueA[3] == '8') ||
                                (valueA[3] == '9')))
                            cpu.setRegister(ref cpu.RC);
                        else
                            notFound();
                        break;
                    case 'K':
                        if (ch4 == '1' && isSupervisorMode() && ((valueA[3] == '0') || (valueA[3] == '1')))
                            cpu.setRegister(ref cpu.K1);
                        else if (ch4 == '2' && ((valueA[3] == '0') || (valueA[3] == '1')))
                            cpu.setRegister(ref cpu.K2);
                        else if (ch4 == '3' && ((valueA[3] == '0') || (valueA[3] == '1')))
                            cpu.setRegister(ref cpu.K3);
                        else
                            notFound();
                        break;
                    //case 'L':
                    //    if ((ch3 == 'A') && (ch4 == 'V'))
                    //        cpu.slave(ref memory);
                    //    else
                    //        notFound();
                    //    break;
                    default:
                        notFound();
                        break;
                }
            }
            else if((ch2 == 'A' || ch2 == 'B') && isAddress(ch3, ch4))
            {
                int address = Convert.ToInt32(String.Concat(ch3, ch4));
                address = interpretateAddress(address);

                if (ch2 == 'A')
                {
                    memory.setWordAtAddress(address, cpu.A.getValue());
                }
                else if (ch2 == 'B')
                {
                    memory.setWordAtAddress(address, cpu.B.getValue());
                }
            }
            else if ((ch2 == 'L') && (ch3 == 'A') && (ch4 == 'V') && !isSupervisorMode())
                cpu.slave();
            else
                notFound();
        }

        private void caseJ(char ch2, char ch3, char ch4)
        {
            if (isAddress(ch3, ch4))
            {
                int address = Convert.ToInt32(String.Concat(ch3, ch4));
                if (cpu.MODE.getValue() == 'S')
                {
                    address = 100 * cpu.RC.getIntValue() + address;
                }

                switch (ch2)
                {
                    case 'P':
                        cpu.jump(address);
                        incIC = false;
                        break;
                    case 'E':
                        cpu.conditionalJump(address, ch2);
                        if (cpu.C.getValue() == 0)
                            incIC = false;
                        break;
                    case 'G':
                        cpu.conditionalJump(address, ch2);
                        if (cpu.C.getValue() == 1)
                            incIC = false;
                        break;
                    case 'L':
                        cpu.conditionalJump(address, ch2);
                        if (cpu.C.getValue() == 2)
                            incIC = false;
                        break;
                    default:
                        notFound();
                        break;
                }
            }
            else
                notFound();
        }

        private void caseO(char ch2, char ch3, char ch4)
        {
            if (ch2 == 'U')
            {
                if (isAddress(ch3, ch4))
                {
                    cpu.SI.setValue('2');
                    int address = Convert.ToInt32(String.Concat(ch3, ch4));
                    address = interpretateAddress(address);

                    string addressString = address.ToString();
                    cpu.A.setValue(new Word(String.Concat("O", cpu.MODE.getValue())));
                    cpu.B.setValue(new Word(addressString));
                    decTimerValue = 3;
                }
                else if (ch3 == 'H' && ch4 == 'A' && isSupervisorMode()) 
                {
                    try
                    {
                        cpu.output(supervisorMemory, hddManager, cpu.A.getValue().toInt() % 1000, cpu.B.getValue().toInt() % 1000);
                        decTimerValue = 3;
                    }
                    catch (Exception)
                    {
                        cpu.PI.setValue('1');
                    }
                }
                else
                {
                    notFound();
                }
            }
            else
            {
                notFound();
            }
        }

        private void caseP(char ch2, char ch3, char ch4)
        {
            int machine = cpu.M.getIntValue();
            if ((ch2 == 'U') && !isSupervisorMode())
            {
                switch (ch3)
                {
                    case 'A':
                        if (ch4 == '0')
                            if (stackSize[machine] != 20)
                                cpu.push(cpu.A, ref memory);
                            else
                                cpu.PI.setValue('3');
                        else
                            notFound();
                        stackSize[machine]++;
                        break;
                    case 'B':
                        if (ch4 == '0')
                            if (stackSize[machine] != 20)
                                cpu.push(cpu.B, ref memory);
                            else
                                cpu.PI.setValue('3');
                        else
                            notFound();
                        stackSize[machine]++;
                        break;
                    case 'C':
                        if (ch4 == '0')
                            if (stackSize[machine] != 20)
                                cpu.push(cpu.C, ref memory);
                            else
                                cpu.PI.setValue('3');
                        else
                            notFound();
                        stackSize[machine]++;
                        break;
                    case 'I':
                        if (ch4 == 'C')
                            if (stackSize[machine] != 20)
                                cpu.push(cpu.IC, ref memory);
                            else
                                cpu.PI.setValue('3');
                        else
                            notFound();
                        stackSize[machine]++;
                        break;
                    default:
                        notFound();
                        break;
                }
            }
            else if ((ch2 == 'O') && !isSupervisorMode())
            {
                switch (ch3)
                {
                    case 'A':
                        if (ch4 == '0')
                            if (stackSize[machine] != 0)
                                cpu.pop(ref cpu.A, ref memory);
                            else
                                cpu.PI.setValue('4');
                        else
                            notFound();
                        stackSize[machine]--;
                        break;
                    case 'B':
                        if (ch4 == '0')
                            if (stackSize[machine] != 0)
                                cpu.pop(ref cpu.B, ref memory);
                            else
                                cpu.PI.setValue('4');
                        else
                            notFound();
                        stackSize[machine]--;
                        break;
                    case 'C':
                        if (ch4 == '0')
                            if (stackSize[machine] != 0)
                                cpu.pop(ref cpu.C, ref memory);
                            else
                                cpu.PI.setValue('4');
                        else
                            notFound();
                        stackSize[machine]--;
                        break;
                    case 'I':
                        if (ch4 == 'C')
                            if (stackSize[machine] != 0)
                                cpu.pop(ref cpu.IC, ref memory);
                            else
                                cpu.PI.setValue('4');
                        else
                            notFound();
                        stackSize[machine]--;
                        break;
                    default:
                        notFound();
                        break;
                }
            }
            else
                notFound();
        }

        private void caseG(char ch2, char ch3, char ch4)
        {
            if (ch2 == 'T')
                switch (ch3)
                {
                    case 'I':
                        if (ch4 == 'C')
                            cpu.getRegister(cpu.IC);
                        else if (ch4 == 'O' && isSupervisorMode())
                            cpu.getRegister(cpu.IOI);
                        else
                            notFound();
                        break;
                    case 'P':
                        if (ch4 == 'R' && isSupervisorMode())
                            cpu.getRegister(cpu.PR);
                        else if (ch4 == 'I' && isSupervisorMode())
                            cpu.getRegister(cpu.PI);
                        else
                            notFound();
                        break;
                    case 'S':
                        if (ch4 == 'P' && isSupervisorMode())
                            cpu.getRegister(cpu.SP);
                        else if (ch4 == 'I' && isSupervisorMode())
                            cpu.getRegister(cpu.SI);
                        else
                            notFound();
                        break;
                    case 'T':
                        if (ch4 == 'I' && isSupervisorMode())
                            cpu.getRegister(cpu.TI);
                        else if (ch4 == 'M' && isSupervisorMode())
                            cpu.getRegister(cpu.TIMER);
                        else
                            notFound();
                        break;
                    case 'M':
                        if (ch4 == '0' && isSupervisorMode())
                            cpu.getRegister(cpu.M);
                        else if (ch4 == 'O' && isSupervisorMode())
                            cpu.getRegister(cpu.MODE);
                        else
                            notFound();
                        break;
                    case 'R':
                        if (ch4 == 'C' && isSupervisorMode())
                            cpu.getRegister(cpu.RC);
                        else
                            notFound();
                        break;
                    case 'K':
                        if (ch4 == '1' && isSupervisorMode())
                            cpu.getRegister(cpu.K1);
                        else if (ch4 == '2' && isSupervisorMode())
                            cpu.getRegister(cpu.K2);
                        else if (ch4 == '3' && isSupervisorMode())
                            cpu.getRegister(cpu.K3);
                        else
                            notFound();                        
                        break;
                    case 'C':
                        if (ch4 == '0')
                            cpu.getRegister(cpu.C);
                        else
                            notFound();
                        break;
                    default:
                        notFound();
                        break;
                }
            else 
                notFound();

        }

        private void caseH(char ch2, char ch3, char ch4)
        {
            if ((ch2 == 'A') && (ch3 == 'L') && (ch4 == 'T'))
                cpu.halt();
            else
                notFound();
        }

        private void caseM(char ch2, char ch3, char ch4)
        {
            if (isAddress(ch3, ch4))
            {
                int address = Convert.ToInt32(String.Concat(ch3, ch4));
                address = interpretateAddress(address);

                if (ch2 == 'O' && isSupervisorMode())
                {
                    cpu.changeMode(address, memory);
                }                
                else
                    notFound();
            }
            else
                notFound();
        }
        
        private void caseX(char ch2, char ch3, char ch4)
        {
            try
            {
                if (ch2 == 'C' && ch3 == 'H' && ch4 == 'G' && isSupervisorMode())
                {
                    int blockNumber = 0;
                    try
                    {
                        blockNumber = cpu.B.getValue().toInt() / 10;
                    }
                    catch (Exception)
                    {
                        notFound();
                    }
                    if (cpu.A.getValue().getWordByte(3) == 'I')
                    {
                        if (cpu.A.getValue().getWordByte(4) == 'V')
                        {
                            cpu.input(ram, inputDevice, blockNumber);
                        }
                        else
                        {
                            cpu.input(supervisorMemory, inputDevice, blockNumber);
                        }
                    }
                    else if (cpu.A.getValue().getWordByte(3) == 'O')
                    {
                        if (cpu.A.getValue().getWordByte(4) == 'V')
                        {
                            cpu.output(ram, outputDevice, blockNumber);
                        }
                        else
                        {
                            cpu.output(supervisorMemory, outputDevice, blockNumber);
                        }
                    }
                }
                else
                {
                    notFound();
                }
            }
            catch (Exception)
            {
                cpu.PI.setValue('1');
            }
        }

        private void caseT(char ch2, char ch3, char ch4)
        {
            if (ch2 == 'E' && ch3 == 'S' && ch4 == 'T' && isSupervisorMode())
                cpu.test();
            else
                notFound();
        }

        private void notFound()
        {
            cpu.PI.setValue('1');
        }
    }
}
