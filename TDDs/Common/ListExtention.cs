using System.Collections.Generic;

namespace TDD
{
    static class ListExtention
    {
        //------------------------------------------------------------------------------
        public static void Shuffle<T>(this List<T> list)
        {
            MyRandom rng = new MyRandom();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

