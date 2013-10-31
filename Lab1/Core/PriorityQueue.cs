using System;
using System.Collections.Generic;

namespace Lab1.Core
{
    internal class PriorityQueue<T> where T : IComparable<T>
    {
        /**
         *  internal data
         */
        private readonly List<T> _mList = new List<T>();


        /// <summary>Returns the number of elements in the queue.</summary>
        public int Count
        {
            get { return _mList.Count; }
        }

        /// <summary>Returns true if the queue is empty.</summary>
        /// Trying to call Peek() or Next() on an empty queue will throw an exception.
        /// Check using Empty first before calling these methods.
        public bool Empty
        {
            get { return _mList.Count == 0; }
        }

        public void Add(T item)
        {
            int n = _mList.Count;
            _mList.Add(item);
            while (n != 0)
            {
                int p = n/2; // This is the 'parent' of this item
                if (_mList[n].CompareTo(_mList[p]) >= 0)
                    break; // Item >= parent

                T tmp = _mList[n];
                _mList[n] = _mList[p];
                _mList[p] = tmp; // Swap item and parent
                n = p; // And continue
            }
        }


        /// <summary>Allows you to look at the first element waiting in the queue, without removing it.</summary>
        /// This element will be the one that will be returned if you subsequently call Next().
        public T Peek()
        {
            return _mList[0];
        }

        public bool IsMember(T item)
        {
            return _mList.IndexOf(item) >= 0;
        }

        /// <summary>Removes and returns the first element from the queue (least element)</summary>
        /// <returns>The first element in the queue, in ascending order.</returns>
        public T Pop()
        {
            // The element to return is of course the first element in the array, 
            // or the root of the tree. However, this will leave a 'hole' there. We
            // fill up this hole with the last element from the array. This will 
            // break the heap property. So we bubble the element downwards by swapping
            // it with it's lower child until it reaches it's correct level. The lower
            // child (one of the orignal elements with index 1 or 2) will now be at the
            // head of the queue (root of the tree).
            T val = _mList[0];
            int nMax = _mList.Count - 1;
            _mList[0] = _mList[nMax];
            _mList.RemoveAt(nMax); // Move the last element to the top

            int p = 0;
            while (true)
            {
                // c is the child we want to swap with. If there
                // is no child at all, then the heap is balanced
                int c = p*2;
                if (c >= nMax)
                    break;

                // If the second child is smaller than the first, that's the one
                // we want to swap with this parent.
                if (c + 1 < nMax && _mList[c + 1].CompareTo(_mList[c]) < 0)
                    c++;

                // If the parent is already smaller than this smaller child, then
                // we are done
                if (_mList[p].CompareTo(_mList[c]) <= 0)
                    break;

                // Othewise, swap parent and child, and follow down the parent
                T tmp = _mList[p];
                _mList[p] = _mList[c];
                _mList[c] = tmp;
                p = c;
            }
            return val;
        }
    }
}