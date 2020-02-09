using System.Collections;
using System.Collections.Generic;

namespace DoublyLinkedList
{
    /// <summary>
    /// A linked list collection capable of basic operations such as
    /// Add, Remove, Find and Enumberable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinkedList<T> :
        ICollection<T>
    {
        /// <summary>
        /// The first node in the list or null if empty
        /// </summary>
        public LinkedListNode<T> Head { get; private set; }

        /// <summary>
        /// The last node in the list or null if empty.
        /// </summary>
        public LinkedListNode<T> Tail { get; private set; }

        #region Add
        /// <summary>
        /// Adds the specified value to the beginning of the linked list
        /// </summary>
        /// <param name="value">The value to add</param>
        public void AddFirst(T value)
        {
            AddFirst(new LinkedListNode<T>(value));
        }

        /// <summary>
        /// Adds the specified node to the beginning of the linked list
        /// </summary>
        /// <param name="node">The node to add </param>
        public void AddFirst(LinkedListNode<T> node)
        {
            // Save off the head node so we do not lose it
            var temp = Head;

            // Point head to the new node
            Head = node;

            // Insert the rest of the list behind the head
            Head.Next = temp;
            
            Count++;

            if (Count == 1)
            {
                // if the list was empty then Head and Tail
                // should both point to the new node.
                Tail = Head;
            }
            else
            {
                temp.Previous = Head;
            }

        }

        /// <summary>
        /// Adds the specified value to the end of the list
        /// </summary>
        /// <param name="value">The value to add</param>
        public void AddLast(T value)
        {
            AddLast(new LinkedListNode<T>(value));
        }

        /// <summary>
        /// Adds the specified node to the end of the list
        /// </summary>
        /// <param name="value">The node to add</param>
        public void AddLast(LinkedListNode<T> node)
        {
            if (Count == 0)
            {
                Head = node;
            }
            else
            {
                Tail.Next = node;

                node.Previous = Tail;
            }

            Tail = node;

            // Reset new node. 
            // Maybe, it's already within other list.
            Tail.Next = null;

            Count++;
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the first node from the list.
        /// </summary>
        public void RemoveFirst()
        {
            if (Count != 0)
            {
                // Before: Head -> 3 -> 5
                // After:  Head ------> 5

                // Head -> 3 -> null
                // Head ------> null
                Head = Head.Next;

                Count--;

                if (Count == 0)
                {
                    Tail = null;
                }
                else
                {
                    // 5. Previous was 3, now null
                    Head.Previous = null;
                }
            }
        }

        /// <summary>
        /// Removes the last node from the list
        /// </summary>
        public void RemoveLast()
        {
            if (Count != 0)
            {
                if (Count == 1)
                {
                    Head = null;
                    Tail = null;
                }
                else
                {
                    // Before: Head --> 3 --> 5 --> 7
                    //         Tail = 7
                    // After:  Head --> 3 --> 5 --> null
                    //         Tail = 5
                    // Null out 5's Next pointer

                    Tail.Previous.Next = null;
                    Tail = Tail.Previous;
                }

                Count--;
            }
        }

        #endregion

        #region ICollection

        /// <summary>
        /// The number of items (nodes) currently in the list.
        /// </summary>
        public int Count { get; private set; }


        /// <summary>
        /// True if the collection is readonly, false otherwise.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Adds the specified value to the front of the list.
        /// </summary>
        /// <param name="item">The value to add</param>
        public void Add(T item)
        {
            AddFirst(item);
        }

        /// <summary>
        /// Removes all the nodes from the list.
        /// </summary>
        public void Clear()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        /// <summary>
        /// Returns true if the lsit contains the specified item, false otherwise.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <returns>True if the item is found, false otherwise</returns>
        public bool Contains(T item)
        {
            var current = Head;

            while (current != null)
            {
                if (current.Value.Equals(item))
                    return true;

                current = current.Next;
            }

            return false;
        }

        /// <summary>
        /// Copies the node values into the specified array.
        /// </summary>
        /// <param name="array">The array to copy the linked list values to</param>
        /// <param name="arrayIndex">The index in the array to start copying at</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            var current = Head;

            while (current != null)
            {
                array[arrayIndex] = current.Value;
                current = current.Next;
            }
        }

        /// <summary>
        /// Removes the first occurance of the item from the list (searching from Head to Tail)
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>True if the item was found and removed, false otherwise</returns>
        public bool Remove(T item)
        {
            LinkedListNode<T> previous = null;
            var current = Head;

            // 1: Empty list - do nothing
            // 2: Single node: (previous is null)
            // 3: Many nodes
            //      a: node to remove is the first node
            //      b: node to remoev is the middle or last
            while (current != null)
            {
                if (current.Value.Equals(item))
                {
                    // it's a node in the middle or end
                    if (previous != null)
                    {
                        // Case 3b

                        // Before: Head -> 3 -> 5 -> null
                        // After:  Head -> 3 ------> null
                        previous.Next = current.Next;

                        if (current.Next == null)
                        {
                            Tail = previous;
                        }
                        else
                        {
                            current.Next.Previous = previous;
                        }

                        Count--;
                    }
                    else
                    {
                        // Case 2 or 3a
                        RemoveFirst();
                    }

                    return true;
                }

                previous = current;
                current = current.Next;
            }

            return false;
        }

        /// <summary>
        /// Enumerates over the linked list values from Head to Tail.
        /// </summary>
        /// <returns>A Head to Tail enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var current = Head;

            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        /// <summary>
        /// Enumerates over the linked list values from Head to Tail.
        /// </summary>
        /// <returns>A Head to Tail enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<T>).GetEnumerator();
        }

        #endregion
    }
}
