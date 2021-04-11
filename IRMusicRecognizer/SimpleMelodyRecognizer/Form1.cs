using System;
using System.Collections.Generic;
using System.Linq;
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
            Keyboard = new(0, 37, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Recognizer.Dataset[2].Play(this.Keyboard.NotePianoMap);
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
            this.Text = "Form1";
            this._recording = false;
            return _record.ToArray();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Text = "Record: ";
            this.StartRecording();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var record = this.GetRecord();
            MessageBox.Show($"[{string.Join(", ", record)}]");
        }
    }
}
