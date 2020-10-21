﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WombatPanelWindowsForms;


namespace SerialI2CMaster
{
    public partial class Form1 : Form
    {
        SerialPort _serialPort = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bOpenSerial_Click(object sender, EventArgs e)
        {
            SerialPortSelector sps = new SerialPortSelector();
            sps.ShowDialog();
            {
                if (sps.SelectedPort != null)
                {
                    _serialPort = new SerialPort(sps.SelectedPort, 115200, Parity.None, 8, StopBits.One);
                    _serialPort.Open();
                    bSend0.Enabled = true;
                    bSend1.Enabled = true;
                }
            }
        }

        private enum ParseMode
        {
            Normal,
            InDec,
            InHex,
            InString,
            InChar,
            InComment,
            Error
        };

        private List<byte> ParseMsg(string s)
        {
            // This function parses a user string into bytes.  
            // Bytes may be expressed in a number of different ways
            // a number between 0 and 255 is a single byte
            // a hex number between 0x00 and 0xFF is a single byte.
            // A character in single quotes is converted to it's ascii equiv.
            // For example 'A' is converted to 65.
            // A group of characters in double quotes is converted to a series
            // of bytes.  For example, "ABCD" is converted to
            // 65 66 67 68 .
            // anything after an unquoted semicolon 
            // 16 bit values can be entered in hex (0x1234) and count as two bytes

            int bytecounter = 0;
            int stringcounter = 0;
            UInt32 partialval = 0;
            int hexcounter = 0;
          
            string Comment = "";
            List<byte> _data = new List<byte>();
            ParseMode m = ParseMode.Normal;

            try
            {
                while (stringcounter < s.Length)
                {
                    char c = s[stringcounter];
                    switch (m)
                    {
                        case ParseMode.Normal:
                            {
                                switch (c)
                                {
                                    case ' ':  //Space
                                    case '\t': //Tab
                                               // whitespace.  Ignore.
                                        break;

                                    case '\'':  //Single quote
                                        m = ParseMode.InChar;
                                        break;

                                    case '"': //Double quote
                                        m = ParseMode.InString;
                                        break;

                                    case ';': //Comment
                                        m = ParseMode.InComment;
                                        break;
                                    case '0':
                                        partialval = 0;
                                        if (s.Length - 1 > stringcounter)
                                        {
                                            // There's at least one more char after this one
                                            if (s[stringcounter + 1] == 'x' ||
                                                s[stringcounter + 1] == 'X')
                                            {
                                                ++stringcounter; // Absorb x
                                                m = ParseMode.InHex;
                                                hexcounter = 0;
                                            }
                                            else m = ParseMode.InDec;
                                        }
                                        else
                                        {
                                            // Last character
                                            _data.Add( 0);
                                            ++bytecounter;
                                        }
                                        break;
                                    case '1':
                                    case '2':
                                    case '3':
                                    case '4':
                                    case '5':
                                    case '6':
                                    case '7':
                                    case '8':
                                    case '9':
                                        partialval = (uint)((byte)(c) - (byte)'0');
                                        m = ParseMode.InDec;
                                        if (s.Length - 1 == stringcounter)
                                        {
                                            //Last character
                                            _data.Add( (byte)partialval);
                                            ++bytecounter;
                                        }
                                        break;

                                    default:
                                        // Something unexpected or useless.  Do nothing
                                        break;

                                }
                            }
                            break;
                        case ParseMode.InChar:
                            {
                                if (s.Length - 1 > stringcounter)
                                {
                                    // There's at least one more char after this one
                                    if (s[stringcounter + 1] == '\'')
                                    {
                                        _data.Add((byte)c);
                                        ++bytecounter;
                                        ++stringcounter; // Absorb trailing ' 
                                        m = ParseMode.Normal;
                                    }
                                    else
                                    {
                                        m = ParseMode.Error;
                                    }
                                }
                                else
                                {
                                    m = ParseMode.Error;
                                }
                            }
                            break;

                        case ParseMode.InString:
                            {
                                _data.Add((byte)c);
                                ++bytecounter;
                                if (s.Length - 1 > stringcounter)
                                {
                                    // There's at least one more char after this one
                                    if (s[stringcounter + 1] == '"')
                                    {
                                        //Absorb trailing "
                                        ++stringcounter;
                                        m = ParseMode.Normal;
                                    }
                                }
                                else
                                {
                                    m = ParseMode.Error;
                                }
                            }
                            break;

                        case ParseMode.InDec:

                            {
                                switch (c)
                                {
                                    case '0':
                                    case '1':
                                    case '2':
                                    case '3':
                                    case '4':
                                    case '5':
                                    case '6':
                                    case '7':
                                    case '8':
                                    case '9':
                                        {
                                            partialval *= 10;
                                            partialval = (uint)(partialval + ((byte)(c) - (byte)'0'));
                                        }
                                        break;

                                    case ' ':
                                    case '\t':
                                    case ';':
                                        {
                                            if (c == ';')
                                            {
                                                m = ParseMode.InComment;
                                            }
                                            else
                                            {
                                                m = ParseMode.Normal;
                                            }
                                            if (partialval <= 255)
                                            {
                                                _data.Add((byte)partialval);
                                                ++bytecounter;
                                            }
                                            else
                                            {
                                                m = ParseMode.Error;
                                            }
                                        }
                                        break;

                                    default:
                                        m = ParseMode.Error;
                                        break;
                                }
                                if ((s.Length - 1) == stringcounter)
                                {
                                    // Last number in the string.  process it
                                    if (partialval <= 255)
                                    {
                                        _data.Add( (byte)partialval);
                                        ++bytecounter;
                                    }
                                    else
                                    {
                                        m = ParseMode.Error;
                                    }
                                }
                            }
                            break;

                        case ParseMode.InHex:
                            {
                                switch (c)
                                {
                                    case ' ':
                                    case '\t':
                                    case ';':
                                    case ',':
                                        if (c == ';')
                                        {
                                            m = ParseMode.InComment;
                                        }
                                        else
                                        {
                                            m = ParseMode.Normal;
                                        }

                                        if (hexcounter <= 2)
                                        {
                                            _data.Add( (byte)partialval);
                                            ++bytecounter;
                                        }
                                        else if (hexcounter <= 4)
                                        {
                                            _data.Add((byte)(partialval % 256));
                                            ++bytecounter;
                                            _data.Add((byte)(partialval / 256));
                                            ++bytecounter;
                                        }
                                        else
                                        {
                                            m = ParseMode.Error;
                                        }

                                        break;

                                    case char n when (n >= '0' && n <= '9'):
                                        partialval *= 16;
                                        partialval = (uint)(partialval + ((byte)(c) - (byte)'0'));
                                        ++hexcounter;
                                        break;
                                    case char n when (n >= 'a' && n <= 'f'):
                                        partialval *= 16;
                                        partialval = (uint)(partialval + 10 + ((byte)(c) - (byte)'a'));
                                        ++hexcounter;
                                        break;

                                    case char n when (n >= 'A' && n <= 'F'):
                                        partialval *= 16;
                                        partialval = (uint)(partialval + 10 + ((byte)(c) - (byte)'A'));
                                        ++hexcounter;
                                        break;




                                    default:
                                        m = ParseMode.Error;
                                        break;
                                }
                                if ((s.Length - 1) == stringcounter)
                                {
                                    m = ParseMode.Normal;
                                    if (hexcounter <= 2)
                                    {
                                        _data.Add((byte)partialval);
                                        ++bytecounter;
                                    }
                                    else if (hexcounter <= 4)
                                    {
                                        _data.Add((byte)(partialval % 256));
                                        ++bytecounter;
                                        _data.Add((byte)(partialval / 256));
                                        ++bytecounter;
                                    }
                                    else
                                    {
                                        m = ParseMode.Error;
                                    }
                                }
                            }
                            break;

                        case ParseMode.InComment:
                            {
                                Comment += c;
                            }
                            break;


                    }

                    ++stringcounter;
                }
            }
            catch
            {
                m = ParseMode.Error;
                //TODO TxDescription += "Exception: " + e.Message;
            }

            if (m == ParseMode.Error)
            {
                return (null) ;
            }


            return (_data);
        }

        private void bSend0_Click(object sender, EventArgs e)
        {
            if (_serialPort == null) return;
            _serialPort.DiscardInBuffer();
            List<byte> data = ParseMsg(tbSend0.Text);
            if (data != null)
            {
                _serialPort.Write(data.ToArray(),0,data.Count);
            }
            Thread.Sleep(100);
            if (_serialPort.BytesToRead > 0)
            {
                while (_serialPort.BytesToRead > 0)
                {
                    tbLog.AppendText($"{_serialPort.ReadByte():X2}  ");
                }
                tbLog.AppendText(Environment.NewLine);
            }
        }

        private void bSend1_Click(object sender, EventArgs e)
        {
            if (_serialPort == null) return;
            _serialPort.DiscardInBuffer();
            List<byte> data = ParseMsg(tbSend1.Text);
            if (data != null)
            {
                _serialPort.Write(data.ToArray(), 0, data.Count);
            }
            Thread.Sleep(100);
            if (_serialPort.BytesToRead > 0)
            {
                while (_serialPort.BytesToRead > 0)
                {
                    tbLog.AppendText($"{_serialPort.ReadByte():X2}  ");
                }
                tbLog.AppendText(Environment.NewLine);
            }
        }
    }


}
