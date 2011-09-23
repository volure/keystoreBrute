using System;
namespace keystoreBrute
{
    using System;
    using System.Collections.Generic;

    // Original code from  Ian Griffiths: http://www.interact-sw.co.uk/iangblog/2004/09/16/permuterate
    public class PermuteUtils
    {
        // Returns an enumeration of enumerators, one for each permutation
        // of the input.
        public static IEnumerable<IEnumerable<T>> Permute<T> (IEnumerable<T> list, int count)
        {
            if (count == 0) {
                yield return new T[0];
            } else {
                int startingElementIndex = 0;
                foreach (T startingElement in list) {
                    IEnumerable<T> remainingItems = AllExcept (list, startingElementIndex);

                    foreach (IEnumerable<T> permutationOfRemainder in Permute (remainingItems, count - 1)) {
                        yield return Concat<T> (new T[] { startingElement }, permutationOfRemainder);
                    }
                    startingElementIndex += 1;
                }
            }
        }

        // Enumerates over contents of both lists.
        public static IEnumerable<T> Concat<T> (IEnumerable<T> a, IEnumerable<T> b)
        {
            foreach (T item in a) {
                yield return item;
            }
            foreach (T item in b) {
                yield return item;
            }
        }

        // Enumerates over all items in the input, skipping over the item
        // with the specified offset.
        public static IEnumerable<T> AllExcept<T> (IEnumerable<T> input, int indexToSkip)
        {
            int index = 0;
            foreach (T item in input) {
                if (index != indexToSkip)
                    yield return item;
                index += 1;
            }
        }
    }
}

