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

        #region
        public void AddLast(LinkedListNode<T> node)
        {
            ////// if the tail is null, it means the list was never initialized, so we update the tail value(node) to the new node
            ////// else if the tail already has a value(node), we need to create a new node, set its value to the node we passed as params
            ////// we have to update the tail node to the newly instanciated node
            if (tail == null)
                tail = node;
            else
            {
                ///// we update the new node previous to be the next of the current tail
                ///// PROEBLEM, the tail node doesnt have a next since it is last in line
                ///// How do we retrieve the next value?
                ///// We go up the ladder one step, the node before the tail has a reference to the tail (next == tail)
                node.Previous = tail;
                ///// we also need to set the tail.Next to be the new node
                tail.Next = node;
                ///// we keep the next value of the node null since nothing comes after
                tail = node;
            }
            ////// if the head is null, it means the tail is our first node and also means it's equal to the head
            if (head == null)
                head = tail;
        }
        public void AddFirst(LinkedListNode<T> node)
        {
            ////// if the head is null, then the head equals the node pass has params
            ////// else it means that we already have a head which could either be a single node (head/tail) or multiple nodes
            if (head == null)
                head = node;
            else
            {
                ///// in the case where we have a single node (head/tail)
                ///// what do we do?
                ///// head node doesnt have a previous or next since head and tail reference the same node in this situation
                head.Previous = node;
                node.Next = head;
                head = node;
            }
            ////// if the tail was never set, then the tail is set to the head
            if (tail == null)
                tail = head;
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
            ////// Create the Initial Emtpy LinkedList<T>
            LinkedList<int> linkedList = new LinkedList<int>();
            ////// Create a single ListNode<T>
            LinkedListNode<int> ll_node1 = new LinkedListNode<int>(5);
            ////// Add the node to the LinkedList<T>, which initialize the head and the tail since it was empty
            linkedList.AddLast(ll_node1);
            ////// Debug.Log to verify if the head and tail are the same when adding the first node
            Console.WriteLine("Head : {0}, Tail : {1}", linkedList.Head.Value, linkedList.Tail.Value);
            ////// we have to Test that adding another node update the tail value
            ////// TESTED!! WORKED!!
            LinkedListNode<int> ll_node2 = new LinkedListNode<int>(7);
            linkedList.AddLast(ll_node2);
            Console.WriteLine("Head : {0}, Tail : {1}", linkedList.Head.Value, linkedList.Tail.Value);
            ////// Lets Test is the previous node of the current tail has the proper value
            LinkedListNode<int> ll_node3 = new LinkedListNode<int>(21);
            linkedList.AddLast(ll_node3);
            Console.WriteLine("Head : {0}, Tail.Previous {1}, Tail : {2}", linkedList.Head.Value, linkedList.Tail.Previous.Value, linkedList.Tail.Value);
            ////// lets try adding with the AddFirst Method in front of the current Head that holds the value 5
            LinkedListNode<int> ll_node4 = new LinkedListNode<int>(11);
            linkedList.AddFirst(ll_node4);
            Console.WriteLine("Head : {0}, Head.Next : {1}, Tail : {2}", linkedList.Head.Value, linkedList.Head.Next.Value, linkedList.Tail.Value);
        }
    }
}
