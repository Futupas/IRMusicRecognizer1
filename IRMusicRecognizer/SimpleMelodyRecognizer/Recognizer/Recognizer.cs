using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMelodyRecognizer
{
    static class Recognizer
    {
        public static Song[] Dataset { get; } = {
            new() { Name = "The imperial March", StartNote = 0, 
                NoteDeltas = { } },
            new() { Name = "В траве сидел кузнечик", StartNote = 0, 
                NoteDeltas = { } },
            new() { Name = "Державний гімн України", StartNote = 19, 
                NoteDeltas = new int[] { 0, 0, -2, 2, 1, 2, -2, -1, -2, -2, 4, -5, 5, -7, -1, 1, 2, 1, 2, 2, 0, 0, -2, 2, 1, 2, -2, -1, -2, -2, -3, 7, -8, 1, 0 } },
            new() { Name = "Всё идёт по плану", StartNote = 0, 
                NoteDeltas = { } },
            new() { Name = "Wind of Change", StartNote = 0, 
                NoteDeltas = { } },
        };

        /// <summary>If delta between songs is more, than this value, it will be 0. Also used for calculating percentage of overlaping</summary>
        public const uint ZeroPercentEquivalent = 100;

    }
}
