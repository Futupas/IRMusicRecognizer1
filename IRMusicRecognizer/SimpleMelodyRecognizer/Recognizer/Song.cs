using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
