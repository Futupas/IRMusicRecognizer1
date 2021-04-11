using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMelodyRecognizer.UI
{
    class PianoKeyboard
    {
        const int BUTTON_HEIGHT = 200;
        const int BUTTON_WIDTH = 50;
        //const int BLACK_BUTTON_WIDTH = 50;
        const int X = 10;
        const int Y = 200;

        PianoButton[] buttons;
        public SortedDictionary<int, PianoButton> NotePianoMap { get; protected set; } = new();

        Form1 _Form;


        /// <param name="start">Included</param>
        /// <param name="end">Excluded</param>
        public PianoKeyboard(int start, int end, Form1 form)
        {
            if (start > end) throw new ArgumentException("End must be greater or equal than start");
            if (form == null) throw new ArgumentNullException("Form must not be null");
            this._Form = form;
            this.buttons = new PianoButton[end - start];
            for (int i = 0; i < this.buttons.Length; i-=-1)
            {
                int currentNote = i + start;
                bool black = IsButtonBlack(currentNote);
                int y = black ? Y : Y + BUTTON_HEIGHT;
                int x = X + i * BUTTON_WIDTH;
                this.buttons[i] = new(x, y, BUTTON_WIDTH, BUTTON_HEIGHT, currentNote, black, form);
                NotePianoMap.Add(currentNote, this.buttons[i]);
                this._Form.Controls.Add(this.buttons[i]);
            }
        }

        private static bool IsButtonBlack(int note)
        {
            switch (note % 12)
            {
                case 0:
                case 2:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                    return false;
                case 1:
                case 4:
                case 6:
                case 9:
                case 11:
                    return true;
            }
            throw new ($"Cannot detect piano button's color: note={note}");
        }
        public static double CalculateFrequencyOfNote(int note, int start = 0)
        {
            note += start;
            const double A3Frequency = 440.0;
            double sqrt12_2 = Math.Pow(2.0, 1.0 / 12);
            if (note > 0)
            {
                return A3Frequency * Math.Pow(sqrt12_2, note);
            }
            else if (note < 0)
            {
                return A3Frequency / Math.Pow(sqrt12_2, -note);
            }
            else
            {
                return A3Frequency;
            }
        }

    }
}
