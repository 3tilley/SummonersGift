﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersGift.Models
{
    public class Utils
    {
        static public DateTime ConvertFromEpochMilliseconds(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }

        //public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>
        //    (this IEnumerable<Tuple<TKey, TValue>>) {
        //    var dt = new System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue>()
    }
}
