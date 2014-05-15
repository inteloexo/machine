using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VirtualRealMachine
{
    public class HDDManager
    {
        private readonly string FILE_NAME;
        private readonly int WORDS_IN_BLOCK;
        private readonly int NUMBER_OF_BLOCKS;

        public HDDManager(String fileName, int numberOfBlocks, int wordsInBlockNumber)
        {
            try
            {
                FILE_NAME = fileName;
                WORDS_IN_BLOCK = wordsInBlockNumber;
                NUMBER_OF_BLOCKS = numberOfBlocks;

                if (!File.Exists(fileName))
                {
                    XmlWriterSettings hddCreatorSettings = new XmlWriterSettings();
                    hddCreatorSettings.Indent = true;
                    hddCreatorSettings.IndentChars = "\t";
                    XmlWriter hddCreator = XmlWriter.Create(fileName, hddCreatorSettings);

                    hddCreator.WriteStartDocument();
                    hddCreator.WriteStartElement("HDD");
                    hddCreator.WriteStartElement("Blocks");

                    for (int i = 0; i < NUMBER_OF_BLOCKS; i++)
                    {
                        hddCreator.WriteStartElement("Block");
                        hddCreator.WriteAttributeString("number", i.ToString());

                        for (int j = 0; j < WORDS_IN_BLOCK; j++)
                        {
                            hddCreator.WriteStartElement("Word");
                            hddCreator.WriteAttributeString("number", j.ToString());
                            hddCreator.WriteString("0000");
                            hddCreator.WriteEndElement();
                        }

                        hddCreator.WriteEndElement();
                    }

                    hddCreator.WriteEndDocument();
                    hddCreator.Close();
                }
            }
            catch (Exception)
            {
                throw new Exception("Cannot create HDD");
            }
        }

        public void setWordAtAddress(int address, Word word)
        {
            try
            {
                XmlDocument hddXml = new XmlDocument();
                hddXml.Load(FILE_NAME);

                XmlNode wordNode;
                XmlElement root = hddXml.DocumentElement;

                int blockAddress = address / WORDS_IN_BLOCK;
                int wordNumber = address % WORDS_IN_BLOCK;
 
                wordNode = root.SelectSingleNode("/HDD/Blocks/Block[@number = '" 
                    + blockAddress.ToString() + "']/Word[@number = '" 
                    + wordNumber.ToString() + "']");

                //wordNode = root.SelectSingleNode("//Block[1]/Word[1]");

                wordNode.InnerText = word.ToString();

                hddXml.Save(FILE_NAME);
            }
            catch(Exception)
            {
                throw new Exception("Cannot write into HDD");
            }
        }

        public Word getWordAtAddress(int address)
        {
            try
            {
                XmlDocument hddXml = new XmlDocument();
                hddXml.Load(FILE_NAME);

                XmlNode wordNode;
                XmlElement root = hddXml.DocumentElement;

                int blockAddress = address / WORDS_IN_BLOCK;
                int wordNumber = address % WORDS_IN_BLOCK;

                wordNode = root.SelectSingleNode("/HDD/Blocks/Block[@number = '"
                    + blockAddress.ToString() + "']/Word[@number = '"
                    + wordNumber.ToString() + "']");

                return new Word(wordNode.InnerText);
            }
            catch (Exception)
            {
                throw new Exception("Cannot read from HDD");
            }
        }
    }
}
