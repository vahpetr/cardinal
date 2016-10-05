using System;
using System.Collections;
using System.Collections.Generic;
using Cardinal.Lessions.Contract;

namespace Cardinal.Lessions
{
    /// <summary>
    /// Наборы данных
    /// </summary>
    public class Datasets : IDatasets
    {
        private IDataset[] values;
        //private static ThreadLocal<Random> _rnd = new ThreadLocal<Random>(() => new Random());
        private static Random _rnd = new Random();
        private IDataset value;
        private int index = -1, count, length, temp, shuffleIndex/*, count*/;

        public IDataset this [int i] => values[i];

        public IDataset[] Values
        {
            get { return values; }
            set
            {
                values = value;
                length = values.Length;
            }
        }

        /// <summary>
        /// Получить обучающий набор
        /// </summary>
        /// <param name="size">Размер</param>
        /// <returns>Обучающий набор</returns>
        public IEnumerable<IDataset> Get(int size)
        {
            count = 0;

            loop:
            for (index = 0; index < length; index++)
            {
                yield return values[index];
                if (++count >= size) goto exit;
            }

            Shuffle();

            goto loop;

            exit:;
        }

        /// <summary>
        /// Перемешать данные
        /// </summary>
        public void Shuffle()
        {
            shuffleIndex = length;
            while (shuffleIndex > 1)
            {
                //temp = _rnd.Value.Next(shuffleIndex);
                temp = _rnd.Next(shuffleIndex);
                shuffleIndex--;
                value = values[temp];
                values[temp] = values[shuffleIndex];
                values[shuffleIndex] = value;
            }
        }

        public bool HasValues()
        {
            return length > 0;
        }

        public IEnumerator<IDataset> GetEnumerator()
        {
            for (index = 0; index < length; index++)
            {
                yield return values[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //public void Dispose()
        //{
        //    Values = null;
        //}

        //public bool MoveNext()
        //{
        //    if (index == count)
        //    {
        //        Reset();
        //        return false;
        //    }

        //    index++;
        //    return true;
        //}

        //public void Reset()
        //{
        //    index = -1;
        //}

        //public IDataset Current => Values[index];

        //object IEnumerator.Current => Values[index];
    }
}