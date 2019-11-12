using System;
using System.Collections.Generic;
using System.Linq;

namespace SetBoxTV.VideoPlayer.Extensions
{
    public static class LinqExtensions
    {
        public static T FirstOrDefaultFromMany<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector,
            Predicate<T> condition)
        {
            if (source == null || !source.Any())
                return default(T);

            var attempt = source.FirstOrDefault(t => condition(t));
            if (!Equals(attempt, default(T)))
                return attempt;

            return source.SelectMany(childrenSelector).FirstOrDefaultFromMany(childrenSelector, condition);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                return; 

            foreach (var item in source)
                action(item);
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dicSource, Dictionary<TKey, TValue> dicNewSource)
        {
            if (dicNewSource == null) return;
            dicNewSource.ForEach(x => { if (!dicSource.ContainsKey(x.Key)) dicSource.Add(x.Key, x.Value); });
        }
    }
}
