using System;
using System.Drawing;
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

        public const int BEEP_DURATION = 500; // ms

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

            this.MouseClick += PianoButton_MouseClick;
        }

        private void PianoButton_MouseClick(object sender, MouseEventArgs e)
        {
            this.Play();
        }

        public void Play()
        {
            if (this._Form._recording)
            {
                int lastNote = 0;
                foreach (var recNote in this._Form._record)
                {
                    lastNote += recNote;
                }
                this._Form._record.AddLast(
                    this._Form._record.Count < 1 ? this.Note : this.Note - lastNote);
                this._Form.Text = "Record: " + string.Join(", ", this._Form._record);
            }
            Console.Beep((int)PianoKeyboard.CalculateFrequencyOfNote(this.Note), BEEP_DURATION);
        }
    }
}
