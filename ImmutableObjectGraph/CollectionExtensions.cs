using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;

namespace ImmutableObjectGraph {

    public static class CollectionExtensions {
        public static ImmutableSortedSet<T> ResetContents<T>(ImmutableSortedSet<T> set, IEnumerable<T> values) {
            return set.SetEquals(values) ? set : set.Clear().Union(values);
        }

        public static ImmutableHashSet<T> ResetContents<T>(ImmutableHashSet<T> set, IEnumerable<T> values) {
            return set.SetEquals(values) ? set : set.Clear().Union(values);
        }

        public static ImmutableSortedSet<T> AddRange<T>(ImmutableSortedSet<T> set, IEnumerable<T> values) {
            return set.Union(values);
        }

        public static ImmutableHashSet<T> AddRange<T>(ImmutableHashSet<T> set, IEnumerable<T> values) {
            return set.Union(values);
        }

        public static ImmutableList<T> AddRange<T>(ImmutableList<T> list, IEnumerable<T> values)
        {
            return list.AddRange(values);
        }

        public static ImmutableDictionary<TKey, TValue> AddRange<TKey, TValue>(ImmutableDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            return dictionary.AddRange(pairs);
        }

        public static ImmutableSortedDictionary<TKey, TValue> AddRange<TKey, TValue>(ImmutableSortedDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            return dictionary.AddRange(pairs);
        }

        public static ImmutableSortedSet<T> RemoveRange<T>(ImmutableSortedSet<T> set, IEnumerable<T> values) {
            return set.Except(values);
        }

        public static ImmutableHashSet<T> RemoveRange<T>(ImmutableHashSet<T> set, IEnumerable<T> values) {
            return set.Except(values);
        }

        public static ImmutableList<T> RemoveRange<T>(ImmutableList<T> list, IEnumerable<T> values)
        {
            return list.RemoveRange(values);
        }

        public static ImmutableDictionary<TKey, TValue> RemoveRange<TKey, TValue>(ImmutableDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        {
            return dictionary.RemoveRange(keys);
        }

        public static ImmutableSortedDictionary<TKey, TValue> RemoveRange<TKey, TValue>(ImmutableSortedDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        {
            return dictionary.RemoveRange(keys);
        }

        public static ImmutableList<T> ResetContents<T>(ImmutableList<T> list, IEnumerable<T> values) {
            return EnumerableV20.SequenceEqual(list, values) ? list : list.Clear().AddRange(values);
        }

        public static ImmutableSortedSet<T> Replace<T>(ImmutableSortedSet<T> set, T oldValue, T newValue) {
            var alteredSet = set.Remove(oldValue);
            return alteredSet != set ? alteredSet.Add(newValue) : set;
        }
    }
}
