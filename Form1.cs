using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualRealMachine
{
    public partial class Form1 : Form
    {
        private Machine machine;

        public Form1()
        {
            InitializeComponent();
        }

        private void updateTextBox()
        {
            AText.Text = machine.cpu.A.getValue().ToString();
            BText.Text = machine.cpu.B.getValue().ToString();
            ICText.Text = machine.cpu.IC.getValue().ToString();
            RCText.Text = machine.cpu.RC.getValue().ToString();
            CText.Text = machine.cpu.C.getValue().ToString();
            SPText.Text = machine.cpu.SP.getValue().ToString();
            MText.Text = machine.cpu.M.getValue().ToString();
            PRText.Text = machine.cpu.PR.getValue().ToString();
            PIText.Text = machine.cpu.PI.getValue().ToString();
            SIText.Text = machine.cpu.SI.getValue().ToString();
            IOIText.Text = machine.cpu.IOI.getValue().ToString();
            TIText.Text = machine.cpu.TI.getValue().ToString();
            MODEText.Text = machine.cpu.MODE.getValue().ToString();
            TIMERText.Text = new string(machine.cpu.TIMER.getValue());
            K1Text.Text = machine.cpu.tempK1.ToString();
            K2Text.Text = machine.cpu.tempK2.ToString();
            K3Text.Text = machine.cpu.tempK3.ToString();
        }

        private void initializeListViews()
        {
            listView1.View = View.Details;
            listView2.View = View.Details;
            listView3.View = View.Details;

            listView1.Columns.Add("Address");
            listView1.Columns.Add("Value");

            listView2.Columns.Add("Address");
            listView2.Columns.Add("Value");

            listView3.Columns.Add("Address");
            listView3.Columns.Add("Value");

            updateListViews();
        }

        private void updateListViews()
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();

            for (int i = 0; i < machine.supervisorMemory.NUMBER_OF_BLOCKS * 10; i++)
            {
                listView1.Items.Add(new ListViewItem(new string[] {i.ToString(),
                    machine.supervisorMemory.getWordAtAddress(i).ToString() }));
            }

            for (int i = 0; i < machine.memory.NUMBER_OF_BLOCKS * 10; i++)
            {
                listView2.Items.Add(new ListViewItem(new string[] {i.ToString(),
                    machine.memory.getWordAtAddress(i).ToString() }));
            }

            for (int i = 0; i < 100; i++)
            {
                listView3.Items.Add(new ListViewItem(new string[] {i.ToString(),
                    machine.memory.getWordAtAddress(machine.cpu.getRealAddress(ref machine.memory, i)).ToString() }));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            machine = new Machine();

            updateTextBox();
            initializeListViews();
            showIC();
        }

        private void getInput()
        {
            string inputString = Microsoft.VisualBasic.Interaction.
                                    InputBox("Enter the input", "Input");
            MemoryBlock inputBlock = new MemoryBlock();
            string tempWordString = "";

            for (int i = 0; i < 40; i = i + 4)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (inputString.Length > i + j)
                    {
                        tempWordString += inputString[i + j];
                    }
                    else
                    {
                        tempWordString += "0";
                    }
                }

                inputBlock.setBlockWord(i / 4, new Word(tempWordString));
                tempWordString = "";
            }

            machine.inputDevice.enqueueInput(inputBlock);
        }

        private void getOutput()
        {
            string outputString = "";
            MemoryBlock outputBlock = machine.outputDevice.getOutput();

            for (int i = 0; i < 10; i++)
            {
                outputString += outputBlock.getBlockWord(i).ToString();
            }

            MessageBox.Show(outputString, "Output", MessageBoxButtons.OK);
        }

        private void showIC()
        {
            ListView listView;
            int address;

            if (machine.cpu.MODE.getValue() == 'S')
            {
                listView = listView1;
                address = machine.cpu.IC.getValue().toInt();
            }
            else
            {
                listView = listView3;
                address = machine.cpu.IC.getValue().toInt() % 100;
            }

            try
            {
                listView.Select();
                listView.Items[address].Selected = true;
                listView.EnsureVisible(address);
            }
            catch (Exception)
            {
                machine.cpu.PI.setValue('1');
            }
        }

        private void executeButton_Click(object sender, EventArgs e)
        {            
            machine.cpu.execute(machine.interpretator);

            if (SIText.Text.ToString() == "1")
            {
                getInput();
            }

            if (machine.outputDevice.outputExists())
            {
                getOutput();
            }

            updateTextBox();
            updateListViews();
            showIC();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            int i = 0;

            while (!machine.cpu.stopMachine && i != 10000)
            {
                machine.cpu.execute(machine.interpretator);

                if (machine.cpu.SI.getValue() == '1')
                {
                    getInput();
                }

                if (machine.outputDevice.outputExists())
                {
                    getOutput();
                }

                i++;
            }
            if (i == 10000)
            {
                MessageBox.Show("Maybe your machine has infinite cycle", "Attention",
                    MessageBoxButtons.OK);
            }

            machine.cpu.stopMachine = false;

            updateTextBox();
            updateListViews();
            showIC();
        }
    }
}
