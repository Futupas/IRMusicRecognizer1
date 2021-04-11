using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleMelodyRecognizer.UI;

namespace SimpleMelodyRecognizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        PianoKeyboard Keyboard;

        private void Form1_Load(object sender, EventArgs e)
        {
            Keyboard = new(3, 28, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Keyboard.PlaySong(Recognizer.Dataset[2]);
        }

        public bool _recording = false;
        public LinkedList<int> _record = new();
        protected void StartRecording()
        {
            this._recording = true;
            this._record = new();
        }
        protected int[] GetRecord()
        {
            this._recording = false;
            return _record.ToArray();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.StartRecording();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.GetRecord();
        }
    }
}
