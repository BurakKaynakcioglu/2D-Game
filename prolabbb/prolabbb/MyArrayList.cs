using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prolabbb
{
    internal class MyArrayList<T>
    {
        private Object[] array;
        private int SIZE;
        private int CAPACITY = 4;


        public MyArrayList()
        {
            this.SIZE = 0;
            this.array = new Object[CAPACITY];
        }


        public void Add(T Object)
        {
            if (SIZE == array.Length)
            {
                resize();
            }

            this.array[SIZE++] = Object;
        }

        private void resize()
        {
            CAPACITY *= 2;
            Object[] newArray = new Object[CAPACITY];
            Array.Copy(array, 0, newArray, 0, SIZE);
            array = newArray;

        }

        public T get(int index)
        {
            if (index < 0 && index >= this.SIZE)
            {
                throw new Exception();
            }
            return (T)array[index];
        }

        public void set(int index, T deger) 
        {
            array[index] = deger;
        }

        public int size()
        {
            return this.SIZE;
        }

        public bool isEmpty()
        {
            if (SIZE == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
