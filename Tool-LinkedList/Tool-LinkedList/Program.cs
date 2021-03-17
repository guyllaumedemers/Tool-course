using System;
using System.Collections.Generic;

namespace Tool_LinkedList
{

    public class LinkedList<T>
    {
        int count;
        LinkedListNode<T> head;
        LinkedListNode<T> tail;

        public LinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void AddBefore(LinkedListNode<T> nodeToAdd, LinkedListNode<T> positionAt)
        {
            ////// if the 'positionAt' at which we want to add a node BEFORE is null, it means that their is no BEFORE
            ////// what we could do is allow the user to still add the node but at the 'positionAt' placement
            if (positionAt == null)
                head = tail = nodeToAdd;
            else
            {
                ///// else if the spot placement is valid, it means that their is a previous node OR the previous node could be null
                ///// in the case where the positionAt is the head
                ///// if the positionAt is head, we take the new node and update the head
                if (positionAt == head && positionAt == tail)
                {
                    ///// if my positionAt equals head AND tail, we have to set the previous node of tail to be the new node
                    ///// we also have to set the new node next to be the tail
                    ///// we have to set the head to be the new node
                    tail.Previous = nodeToAdd;
                    nodeToAdd.Next = tail;
                    head = nodeToAdd;
                }
                else if (positionAt == head)
                {
                    head.Previous = nodeToAdd;
                    nodeToAdd.Next = head;
                    head = nodeToAdd;
                }
                else
                {
                    ////// else if they are not equal, it means that we have to update the positionAt previous node to be the one we add
                    ////// but also the positionAt previous node needs to update its next node to be the nodeToAdd
                    ////// DONT FORGET to set the nodeToAdd previous and next
                    nodeToAdd.Previous = positionAt.Previous;
                    nodeToAdd.Next = positionAt;
                    positionAt.Previous.Next = nodeToAdd;
                    positionAt.Previous = nodeToAdd;
                }
            }
            count++;
        }

        public void AddAfter(LinkedListNode<T> nodeToAdd, LinkedListNode<T> positionAt)
        {
            if (positionAt == null)
                head = tail = nodeToAdd;
            else
            {
                if (positionAt == head && positionAt == tail)
                {
                    head.Next = nodeToAdd;
                    nodeToAdd.Previous = head;
                    tail = nodeToAdd;
                }
                else if (positionAt == tail)
                {
                    ///// if the positionAt is the tail, we set the next of the tail to be the nodeToAdd and set the nodeToAdd previous to be
                    ///// be the tail, then we set the new tail to be the nodeToAdd
                    tail.Next = nodeToAdd;
                    nodeToAdd.Previous = tail;
                    tail = nodeToAdd;
                }
                ///// if the positionAt is not null, it means that the next node exist IF the positionAt is NOT the tail node
                else
                {
                    nodeToAdd.Previous = positionAt;
                    nodeToAdd.Next = positionAt.Next;
                    positionAt.Next.Previous = nodeToAdd;
                    positionAt.Next = nodeToAdd;
                }
            }
            count++;
        }

        #region
        public void AddLast(LinkedListNode<T> nodeToAdd)
        {
            ////// if the tail is null, it means the list was never initialized, so we update the tail value(node) to the new node
            ////// else if the tail already has a value(node), we need to create a new node, set its value to the node we passed as params
            ////// we have to update the tail node to the newly instanciated node
            if (tail == null)
                tail = nodeToAdd;
            else
            {
                ///// we update the new node previous to be the next of the current tail
                ///// PROEBLEM, the tail node doesnt have a next since it is last in line
                ///// How do we retrieve the next value?
                ///// We go up the ladder one step, the node before the tail has a reference to the tail (next == tail)
                nodeToAdd.Previous = tail;
                ///// we also need to set the tail.Next to be the new node
                tail.Next = nodeToAdd;
                ///// we keep the next value of the node null since nothing comes after
                tail = nodeToAdd;
            }
            ////// if the head is null, it means the tail is our first node and also means it's equal to the head
            if (head == null)
                head = tail;
            count++;
        }
        public void AddFirst(LinkedListNode<T> nodeToAdd)
        {
            ////// if the head is null, then the head equals the node pass has params
            ////// else it means that we already have a head which could either be a single node (head/tail) or multiple nodes
            if (head == null)
                head = nodeToAdd;
            else
            {
                ///// in the case where we have a single node (head/tail)
                ///// what do we do?
                ///// head node doesnt have a previous or next since head and tail reference the same node in this situation
                head.Previous = nodeToAdd;
                nodeToAdd.Next = head;
                head = nodeToAdd;
            }
            ////// if the tail was never set, then the tail is set to the head
            if (tail == null)
                tail = head;
            count++;
        }

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }
        #endregion

        #region Properties
        public int GetCount { get => count; }
        public LinkedListNode<T> Head { get => head; set { head = value; } }
        public LinkedListNode<T> Tail { get => tail; set { tail = value; } }
        #endregion
    }

    public sealed class LinkedListNode<T>
    {
        T value;
        LinkedListNode<T> next;
        LinkedListNode<T> previous;

        public LinkedListNode(T node)
        {
            previous = null;
            next = null;
            value = node;
        }

        public LinkedListNode<T> Next { get => next; set { next = value; } }
        public LinkedListNode<T> Previous { get => previous; set { previous = value; } }
        public T Value { get => value; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            LinkedList<int> linkedList = new LinkedList<int>();

            LinkedListNode<int> node1 = new LinkedListNode<int>(5);
            LinkedListNode<int> node2 = new LinkedListNode<int>(7);
            LinkedListNode<int> node3 = new LinkedListNode<int>(8);

            //linkedList.AddAfter(node1, linkedList.Head);
            //Console.WriteLine("Head {0}, Tail {1}", linkedList.Head?.Value, linkedList.Tail?.Value);
            //linkedList.AddAfter(node2, linkedList.Head);
            //Console.WriteLine("Head {0}, Tail {1}", linkedList.Head?.Value, linkedList.Tail?.Value);
            //linkedList.AddAfter(node3, linkedList.Tail);
            //Console.WriteLine("Head {0}, Tail {1}", linkedList.Head?.Value, linkedList.Tail?.Value);
        }
    }
}
