using System;

namespace Augur.Data.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] InsertAt<T>(this T[] array, T item, int index)
        {
            if (index > array.Length) throw new IndexOutOfRangeException();

            var newArray = new T[array.Length + 1];

            for (var i = 0; i < newArray.Length; i++)
            {
                if (i == index)
                {
                    newArray[i] = item;
                }
                else
                {
                    var offset = i < index
                        ? 0
                        : 1;

                    newArray[i] = array[i - offset];
                }
            }

            return newArray;
        }
    }
}