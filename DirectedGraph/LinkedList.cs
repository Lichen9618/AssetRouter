using System;
using System.Collections;
using System.Text;

namespace DirectedGraph
{
    /// <summary>
    /// Represents an item in the linked list.
    /// </summary>
    /// <typeparam name="T">The type of the value stored.</typeparam>
    class LinkedListItem<T>
    {
        /// <summary>
        /// The value stored in this item.
        /// </summary>
        public T value;

        /// <summary>
        /// The previous item in the list referenced by this item.
        /// </summary>
        public LinkedListItem<T> previousItem;

        /// <summary>
        /// The next item in the list referenced by this item.
        /// </summary>
        public LinkedListItem<T> nextItem;
    }

    /// <summary>
    /// A linked list.
    /// </summary>
    /// <typeparam name="T">Type of value stored in the linked list.</typeparam>
    public class LinkedList<T> : IEnumerable
    {
        /// <summary>
        /// First item in the linked list.
        /// </summary>
        private LinkedListItem<T> first;

        /// <summary>
        /// Last item in the list.
        /// </summary>
        private LinkedListItem<T> last;

        /// <summary>
        /// Total number of items in the linked list.
        /// </summary>
        private int length;

        /// <summary>
        /// Total number of items in the linked list.
        /// </summary>
        public int Length
        {
            get
            {
                return length;
            }
        }

        /// <summary>
        /// Indexer to access the linked list like an array.
        /// </summary>
        /// <param name="i">Index (zero-based) to access an item in the linked list. 
        /// Throws <see cref="System.IndexOutOfRangeException"/> if index is out of 
        /// range.</param>
        /// <returns>The value stored at the specified index.</returns>
        public T this[int i]
        {
            get {
                LinkedListItem<T> item = Find(i);

                if (item == null)
                    return default(T);

                return Find(i).value;
            }
            set
            {
                LinkedListItem<T> item = Find(i);

                if (item == null)
                    throw new IndexOutOfRangeException();

                item.value = value;
            }
        }

        /// <summary>
        /// Retrieves the <see cref="LinkedListItem"/> at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The item found.</returns>
        private LinkedListItem<T> Find(int index)
        {
            // If last item index, return last
            if (index == length - 1)
                return last;

            if (first == null || index == 0)
                return first;

            // Iterate the list till the specified index is reached
            int i = 0;
            LinkedListItem<T> nextItem = first;
            while (nextItem.nextItem != null && i != index)
            {
                nextItem = nextItem.nextItem;
                i++;
            }
            if (i != index)
            {
                throw new IndexOutOfRangeException();
            }
            return nextItem;
        }

        /// <summary>
        /// Find an item in the linked list.
        /// </summary>
        /// <param name="match">The delegate used for performing the match.</param>
        /// <returns>Item found or else null.</returns>
        public T Find(Predicate<T> match)
        {
            LinkedListItem<T> nextItem = first;
            while (nextItem != null)
            {
                if (match(nextItem.value))
                {
                    return nextItem.value;
                }
                nextItem = nextItem.nextItem;
            }
            return default(T);
        }

        /// <summary>
        /// A simpler version of the Find method. Checks if the object specified 
        /// exists in the linked list based on the Equals method.
        /// </summary>
        /// <param name="obj">Object to search for.</param>
        /// <returns>The matching object found or null.</returns>
        public T Find(T obj)
        {
            return Find(delegate(T match)
            {
                if (obj.Equals(match))
                    return true;
                else
                    return false;
            });
        }

        /// <summary>
        /// Adds the specified object to the end of the linked list.
        /// </summary>
        /// <param name="obj">Object value to add.</param>
        public void Add(T obj)
        {
            LinkedListItem<T> item = new LinkedListItem<T>();
            item.value = obj;
            if (first == null)
            {
                first = item;
                last = item;
            }
            else
            {
                last.nextItem = item;
                item.previousItem = last;
                last = item;
            }
            length++;
        }

        /// <summary>
        /// Removes the specified item from the linked list. 
        /// </summary>
        /// <param name="obj">Index of the item to remove.</param>
        /// <returns>The item to remove.</returns>
        public T Remove(int index)
        {
            LinkedListItem<T> item = Find(index);
            if (item == null)
                throw new IndexOutOfRangeException();

            item.previousItem.nextItem = item.nextItem;
            item.nextItem.previousItem = item.previousItem;
            item.previousItem = null;
            item.nextItem = null;

            return item.value;
        }

        /// <summary>
        /// Pushes an object to the end of the linked list, or the top of the stack.
        /// </summary>
        /// <param name="obj">Object of type T.</param>
        public void Push(T obj)
        {
            Add(obj);
        }

        /// <summary>
        /// Peek at the object at the end of the linked list, or top of the stack.
        /// </summary>
        /// <returns>Object of type T.</returns>
        public T Peek()
        {
            return last.value;
        }

        /// <summary>
        /// Retrieves the top most item in the stack or else null if the stack is empty.
        /// </summary>
        /// <returns>Object of type T.</returns>
        public T Pop()
        {
            if (last == null)
            {
                return default(T);
            }

            T obj = last.value;
            last = last.previousItem;
            if (last != null)
                last.nextItem = null;
            else
                first = null;
            length--;
            return obj;
        }

        /// <summary>
        /// Support for foreach.
        /// </summary>
        /// <returns>The enumerator for this list.</returns>
        public IEnumerator GetEnumerator()
        {
            return new LinkedListEnumerator<T>(this);
        }

        /// <summary>
        /// Creates a copy of this linked list.
        /// </summary>
        /// <returns>A copy of this linked list.</returns>
        public LinkedList<T> Copy()
        {
            LinkedList<T> copy = new LinkedList<T>();

            LinkedListItem<T> nextItem = first;
            while (nextItem != null)
            {
                copy.Add(nextItem.value);
                nextItem = nextItem.nextItem;
            }

            return copy;
        }

        /// <summary>
        /// Override ToString.
        /// </summary>
        /// <returns>Concatenation of result of call to ToString for all objects stored.</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            LinkedListItem<T> nextItem = first;
            while (nextItem != null)
            {
                s.Append(nextItem.value.ToString());
                nextItem = nextItem.nextItem;
            }
            return s.ToString();
        }

        /// <summary>
        /// Compare this linked to another. Lists with the same length and objects are considered equals.
        /// Objects are compared using object.Equals.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if the lists have the same length and objects.</returns>
        public bool Compare(LinkedList<T> other)
        {
            bool equal = true;

            if (Length != other.Length) 
                return false;

            for (int i = 0; i < Length; i++)
            {
                if (!this[i].Equals(other[i])) 
                    return false;
            }

            return equal;
        }
    }

    /// <summary>
    /// Enumerator for <see cref="LinkedList"/>.
    /// </summary>
    /// <typeparam name="T">The type of object stored in <see cref="LinkedList"/>.</typeparam>
    class LinkedListEnumerator<T> : IEnumerator
    {
        LinkedList<T> list;
        int index;

        /// <summary>
        /// Constructor for the enumerator.
        /// </summary>
        /// <param name="list">The linked list being enumerated.</param>
        public LinkedListEnumerator(LinkedList<T> list)
        {
            this.list = list;
            index = -1;
        }

        public object Current
        {
            get
            {
                return list[index];
            }
        }

        public bool MoveNext()
        {
            index++;
            return (index < list.Length);
        }

        public void Reset()
        {
            index = -1;
        }
    }
}
