using System;
using System.Collections.Generic;
using System.Linq;
using SimpleMelodyRecognizer.Helpers;

namespace SimpleMelodyRecognizer
{
    static class Recognizer
    {
        /// <summary>If delta between songs is more, than this value, it will be 0. Also used for calculating percentage of overlaping</summary>
        public const uint ZERO_PERCENT_DIFFERENCE = 20;
        public const int TOP_SONGS_COUNT = 3;


        public static Song[] Dataset { get; } = {
            new() { Name = "The imperial March", StartNote = 10, 
                NoteDeltas = new[] { 0, 0, -4, 7, -3, -4, 7, -3, 0, 7, 0, 0, 1, -5, -3, -4, 7, -3, 0 } },
            new() { Name = "В траве сидел кузнечик", StartNote = 12, 
                NoteDeltas = new[] { -5, 5, -5, 5, -1, 0, 0, -4, 4, -4, 4, 1, 0 } },
            new() { Name = "Державний гімн України", StartNote = 19, 
                NoteDeltas = new[] { 0, 0, -2, 2, 1, 2, -2, -1, -2, -2, 4, -5, 5, -7, -1, 1, 2, 1, 2, 2, 0, 0, -2, 2, 1, 2, -2, -1, -2, -2, -3, 7, -8, 1, 0 } },
            new() { Name = "Всё идёт по плану", StartNote = 14,
                NoteDeltas = new[] { 1, -1, -2, -5, 5, 2, 1, -1, -2, -5, 5, 2, 1, -1, -2, -5, 5, 2, 1, -1, -2, -5, 5, 2 } },
            new() { Name = "Wind of Change", StartNote = 12,
                NoteDeltas = new[] { 2, 1, -3, 2, 1, 2, -2, -3, 2, 1, -3, 2, 1, 2, -2, 0, 2, 2, 1, -1, -4, 2, -5, 3, -1 } },
        };



        /// <summary>0 is the best</summary>
        public static (Song song, double matchPercents)[] Search(IEnumerable<int> notes)
        {
            if (notes == null) throw new ArgumentNullException("Notes cannot be null");

            var datasetComparation = new List<(Song song, uint diff)>();
            Song ourSong = new() { NoteDeltas = notes.ToArray() };

            foreach (var datasetSong in Dataset)
            {
                uint diff = SongComparator.CompareSongs(ourSong, datasetSong);
                datasetComparation.Add((datasetSong, diff));
            }

            datasetComparation.Sort(new FuncComparer<(Song song, uint diff)>((a, b) => a.diff.CompareTo(b.diff)));

            var top = new (Song song, double matchPercents)[TOP_SONGS_COUNT];
            for (int i = 0; i < TOP_SONGS_COUNT; i -= -1)
            {
                var song = datasetComparation.First();
                datasetComparation.Remove(datasetComparation.First());
                var percents = .0;
                if (song.diff < ZERO_PERCENT_DIFFERENCE)
                {
                    percents = MathFuncs.Map(song.diff, 0, Recognizer.ZERO_PERCENT_DIFFERENCE, 100, 0);
                }
                top[i] = (song.song, percents);
            }

            return top;
        }
    }
}
