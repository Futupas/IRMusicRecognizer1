using SimpleMelodyRecognizer.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Threading;

namespace SimpleMelodyRecognizer
{
    class Song
    {
        public string Name { get; init; }
        /// <summary>0 is A4</summary>
        public int? StartNote { get; set; } = null; 
        public int[] NoteDeltas { get; init; }

        public int[] RealNotes
        {
            get
            {
                var realNotes = new int[this.NoteDeltas.Length + 1];
                realNotes[0] = this.StartNote ?? 0;
                for (int i = 1; i < realNotes.Length; i -= -1)
                {
                    realNotes[i] = realNotes[i - 1] + this.NoteDeltas[i - 1];
                }
                return realNotes;
            }
        }
        public int[] RealNotesNoRepeats
        {
            get
            {
                var result = new LinkedList<int>();
                result.AddLast(this.StartNote ?? 0);
                foreach (var note in this.RealNotes)
                {
                    if (note != result.Last()) result.AddLast(note);
                }
                return result.ToArray();
            }
        }

        public void Play(SortedDictionary<int, PianoButton> notePianoMap)
        {
            foreach (var note in this.RealNotes)
            {
                PianoButton noteBtn;
                if (notePianoMap == null || !notePianoMap.TryGetValue(note, out noteBtn))
                {
                    Console.Beep((int)PianoKeyboard.CalculateFrequencyOfNote(note), PianoButton.BEEP_DURATION);
                }
                else
                {
                    noteBtn.Play();
                }
                Application.DoEvents();
                Thread.Sleep(PianoButton.BEEP_DURATION);
                Application.DoEvents();
            }
        }
    }
}
