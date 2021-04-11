using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace SimpleMelodyRecognizer.UI
{
    class PianoButton: Button
    {
        public int Note { get; init; }
        public bool IsBlack { get; init; }
        public readonly Color ACTIVE_COLOR = Color.LightGreen;
        protected readonly Form1 _Form;

        public bool IsPlaying { get; protected set; } = false;

        public const int BEEP_DURATION = 200; // ms

        public PianoButton(int x, int y, int width, int height, int note, bool isBlack, Form1 form): base()
        {
            this.Note = note;
            this.IsBlack = isBlack;
            this._Form = form;

            this.Text = string.Empty;
            this.BackColor = isBlack ? Color.Black : Color.White;
            this.Width = width;
            this.Height = height;
            this.Location = new Point(x, y);

            this.MouseDown += PianoButton_MouseDown;
            this.MouseUp += PianoButton_MouseUp;
        }

        private void PianoButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.StopPlaying();
        }

        private void PianoButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (this._Form._recording)
            {
                this._Form._record.AddLast(this.Note);
                this._Form.Text = string.Join(", ", this._Form._record);
            }
            this.StartPlaying();
        }

        public void StartPlaying()
        {
            this.IsPlaying = true;
            //new Thread(() => {
                Console.Beep((int)PianoKeyboard.CalculateFrequencyOfNote(this.Note), BEEP_DURATION);
            //}).Start();
            this.BackColor = ACTIVE_COLOR;
        }
        public void StopPlaying()
        {
            this.IsPlaying = false;
            this.BackColor = this.IsBlack ? Color.Black : Color.White;
        }
        public void Play()
        {
            this.StartPlaying();
            this.StopPlaying();
        }
    }
}
