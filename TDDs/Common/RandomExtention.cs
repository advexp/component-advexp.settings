using System;
using System.Linq;

namespace TDD
{
    static class RandomExtention
    {
        //------------------------------------------------------------------------------
        public static bool NextBoolean(this Random rng)
        {
            return rng.NextDouble() > 0.5;
        }

        //------------------------------------------------------------------------------
        public static int NextInt32(this Random rng)
        {
            unchecked
            {
                int firstBits = rng.Next(0, 1 << 4) << 28;
                int lastBits = rng.Next(0, 1 << 28);
                return firstBits | lastBits;
            }
        }

        //------------------------------------------------------------------------------
        public static Single NextSingle(this Random rng)
        {
            return (Single)(Single.MaxValue * 2.0 * (rng.NextDouble()-0.5));
        }

        //------------------------------------------------------------------------------
        public static decimal NextDecimal(this Random rng)
        {
            byte scale = (byte) rng.Next(29);
            bool sign = rng.Next(2) == 1;
            return new decimal(rng.NextInt32(), 
                               rng.NextInt32(),
                               rng.NextInt32(),
                               sign,
                               scale);
        }

        //------------------------------------------------------------------------------
        public static DateTime NextDateTime(this Random rng)
        {
            DateTime start = DateTime.MinValue;

            int range = (DateTime.Today - start).Days;
            return start.AddDays(rng.Next(range));
        }

        //------------------------------------------------------------------------------
        public static string NextString(this Random rng, int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(
                Enumerable.Repeat(chars, length)
                .Select(s => s[rng.Next(s.Length)])
                .ToArray());
        }

        //------------------------------------------------------------------------------
        public static char NextChar(this Random rng)
        {
            string chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
            int num = rng.Next(0, chars.Length -1);
            return chars[num];
        }
    }

    class MyRandom : Random
    {
        //------------------------------------------------------------------------------
        public MyRandom() : base((Int32)(DateTime.Now.Ticks & 0x0000FFFF))
        {
        }
    }
}

