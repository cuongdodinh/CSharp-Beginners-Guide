using System;
using System.Collections.Generic;
using System.Linq;


namespace C3SockNetUtil
{
	public class RandomWrapper
	{
        Random Rand;

        RandomWrapper(int seed)
        {
            Rand = new Random(seed);
        }

        RandomWrapper(Random rand)
        {
            Rand = rand;
        }

        /// <summary>
        /// Return random bool value.
        /// </summary>
        public bool NextBool()
		{
			return Rand.Next(0, 2) == 0;
		}

        public T Next<T>(T item1, T item2)
        {
            return NextBool() ? item1 : item2;
        }

        public T Next<T>(T item1, T item2, T item3)
        {
            int n = Rand.Next(0, 3);
            return n == 0 ? item1 : (n == 1 ? item2 : item3);
        }

        ///// <summary>
        ///// Return random item from array.
        ///// </summary>
        public T NextItem<T>(T[] array)
        {
            return array[Rand.Next(0, array.Length)];
        }

        public T NextItem<T>(List<T> list)
        {
            return list[Rand.Next(0, list.Count)];
        }

        public T NextEnum<T>()
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Rand.Next(0, values.Length));
        }

        public int NextWeightedInd(int[] weights)
        {
            int randomPoint = Rand.Next(0, weights.Sum()) + 1;
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                sum += weights[i];
                if (randomPoint <= sum)
                    return i;
            }
            throw new Exception("Logic error!");
        }

        public List<T> Take<T>(List<T> list, int count)
        {
            List<T> items = new List<T>();
            List<int> remainedIndexes = Enumerable.Range(0, list.Count).ToList();
            for (int i = 0; i < count; i++)
            {
                int selectedIndex = NextItem(remainedIndexes);
                remainedIndexes.Remove(selectedIndex);
                items.Add(list[selectedIndex]);
            }
            return items;
        }

        public void Shuffle<T>(List<T> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                int indRnd = Rand.Next(0, i + 1);
                T temp = list[i];
                list[i] = list[indRnd];
                list[indRnd] = temp;
            }
        }

        public void Shuffle<T>(T[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                int indRnd = Rand.Next(0, i + 1);
                T temp = array[i];
                array[i] = array[indRnd];
                array[indRnd] = temp;
            }
        }

        public bool GetChance(int percentage)
		{
			return Rand.Next(0, 100) + 1 <= percentage;
		}

		
	}
}