using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SimpleMelodyRecognizer.UI;
using SimpleMelodyRecognizer.Helpers;

namespace SimpleMelodyRecognizer
{
    public partial class Form1 : Form
    {
        PianoKeyboard Keyboard;
        public bool _recording = false;
        public LinkedList<int> _record = new();

        public Form1()
        {
            InitializeComponent();
            this.Keyboard = new(0, 37, this);
        }


        #region Start/stop recording

        /// <summary>Start recording</summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Text = "Record: ";
            this._recording = true;
            this._record = new();
        }
        
        /// <summary>Stop recording</summary>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Text = "Form1";
            this._recording = false;
            var similarSongs = Recognizer.Search(this._record);
            MessageBox.Show($"Top {Recognizer.TOP_SONGS_COUNT} similar songs: \r\n" +
                string.Join("\r\n", similarSongs.Select(
                    el => $"{el.song.Name}. Math: {MathFuncs.Round(el.matchPercents, 2)}%")) );
        }

        #endregion

        #region Play songs buttons

        private void button4_Click(object sender, EventArgs e)
        {
            Recognizer.Dataset[0].Play(this.Keyboard.NotePianoMap);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Recognizer.Dataset[1].Play(this.Keyboard.NotePianoMap);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Recognizer.Dataset[2].Play(this.Keyboard.NotePianoMap);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Recognizer.Dataset[3].Play(this.Keyboard.NotePianoMap);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Recognizer.Dataset[4].Play(this.Keyboard.NotePianoMap);
        }

        #endregion

    }
}
