using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMelodyRecognizer
{
    static class SongComparator
    {
        /// <summary>
        /// The less, the more chance, that "needle" part is in the "haystack" song. 0 => "needle" is 100% in "haystack"
        /// </summary>
        /// <param name="needle">Searched part, "query"</param>
        /// <param name="haystack">Song, where the query is searched</param>
        public static uint CompareSongs(Song needle, Song haystack)
        {
            if (needle == null || haystack == null) throw new ArgumentNullException();
            if (needle.NoteDeltas.Length > haystack.NoteDeltas.Length)
                throw new ArgumentException("Needle has to many notes");

            var bestResult = uint.MaxValue;

            for (int i = 0; i < haystack.NoteDeltas.Length - needle.NoteDeltas.Length; i-=-1)
            {
                var nArray = needle.NoteDeltas;
                var hArray = haystack.NoteDeltas[i..(i + nArray.Length)];
                var compare = Compare2Arrays(nArray, hArray);
                if (compare < bestResult) bestResult = compare;
            }

            return bestResult;
        }

        /// <summary>array1.Count() must be equal to array2.Count()</summary>
        static uint Compare2Arrays<T>(IEnumerable<T> array1, IEnumerable<T> array2)
            where T: IComparable<T>
        {
            if (array1 == null || array2 == null) throw new ArgumentNullException();
            if (array1.Count() != array2.Count()) throw new ArgumentException("Length must be equal");

            uint result = 0;

            var enumerator1 = array1.GetEnumerator();
            var enumerator2 = array2.GetEnumerator();

            while(enumerator1.MoveNext())
            {
                enumerator2.MoveNext();

                var value1 = enumerator1.Current;
                var value2 = enumerator2.Current;
                var compare = value1.CompareTo(value2);
                result += (uint)Math.Abs(compare);
            }

            return result;
        }
    }
}
