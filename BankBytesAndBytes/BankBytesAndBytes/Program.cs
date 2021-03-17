using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankBytesAndBytes
{
    class Program
    {
        static char[] char_array = BankOfBitsNBytes.acceptablePasswordChars;
        static int psw_lenght = BankOfBitsNBytes.passwordLength;
        static bool isRunning = true;
        static int maxIndex = 25;
        static int minIndex = 0;
        static int amountStolen = 0;
        static object myLock = new object();
        static Mutex mutex = new Mutex();

        public delegate void MyDelegate();

        //public static char[] TestPassword(char[] myArr, int indexI, int indexJ, int indexK)
        //{
        //    return new char[] { myArr[indexI], myArr[indexJ], myArr[indexK] };
        //}

        public static char[] TestPassword(char[] myArr, int[] myIndexes, int pws_lenght)
        {
            string myString = "";
            for (int i = 0; i < pws_lenght; i++)
            {
                myString += myArr[myIndexes[i]];
            }
            //Console.WriteLine(myString);
            return myString.ToCharArray();
        }

        #region HANDLE INDEXES
        public static int UpdateIndexForward(int index)
        {
            return index < maxIndex ? ++index : minIndex;
        }

        public static int UpdateIndexBackward(int index)
        {
            return index > minIndex ? --index : maxIndex;
        }
        #endregion

        #region BRUTE FORCE 
        //public static void Forward(int[] indexes)
        //{
        //    indexes[2] = UpdateIndexForward(indexes[2]);
        //    if (indexes[2] == minIndex)
        //        indexes[1] = UpdateIndexForward(indexes[1]);
        //    if (indexes[2] == minIndex && indexes[1] == minIndex)
        //        indexes[0] = UpdateIndexForward(indexes[0]);
        //}

        //public static void Backward(int[] indexes)
        //{
        //    if (indexes[2] == minIndex && indexes[1] == minIndex)
        //        indexes[0] = UpdateIndexBackward(indexes[0]);
        //    if (indexes[2] == minIndex)
        //        indexes[1] = UpdateIndexBackward(indexes[1]);
        //    indexes[2] = UpdateIndexBackward(indexes[2]);
        //}

        public static void Forward(int[] indexes)
        {
            indexes[indexes.Length - 1] = UpdateIndexForward(indexes[indexes.Length - 1]);
            /////// lets say we have an arr of 4 indexes, lenght -1 is the last index of the array and the one that needs
            /////// to be constantly updateded
            /////// we loop thru the other index and compare with their neighbors
            /////// the only time when we can update the current node is when all its right neighbors == maxIndex <-
            int cpt = indexes.Length - 2;
            do
            {
                bool isMaxedOut = false;
                ///// so for each position, we have to look at ALL the right neighbors
                for (int i = cpt; i < indexes.Length - 1; i++)
                {
                    if (indexes[i + 1] != minIndex)
                    {
                        isMaxedOut = false;
                        break;
                    }
                    isMaxedOut = true;
                }
                if (isMaxedOut)
                    indexes[cpt] = UpdateIndexForward(indexes[cpt]);
                cpt--;
            } while (cpt != -1);
        }

        public static void Backward(int[] indexes)
        {
            indexes[indexes.Length - 1] = UpdateIndexBackward(indexes[indexes.Length - 1]);
            int cpt = indexes.Length - 2;
            do
            {
                bool isMaxedOut = false;
                ///// so for each position, we have to look at ALL the right neighbors
                for (int i = cpt; i < indexes.Length - 1; i++)
                {
                    if (indexes[i + 1] != maxIndex)
                    {
                        isMaxedOut = false;
                        break;
                    }
                    isMaxedOut = true;
                }
                if (isMaxedOut)
                    indexes[cpt] = UpdateIndexBackward(indexes[cpt]);
                cpt--;
            } while (cpt != -1);
        }
        #endregion

        #region INITIALIZATION
        public static int[] InitializeIntArray(int[] myArr, int value)
        {
            for (int i = 0; i < myArr.Length; i++)
            {
                myArr[i] = value;
            }
            return myArr;
        }

        //public static void InitializeThreads(Stopwatch stopwatch, BankOfBitsNBytes bbb, int[] f_indexes, int[] b_indexes)
        //{
        //    stopwatch.Start();
        //    CreateThread(() => { Run(stopwatch, bbb, f_indexes, true); });
        //    CreateThread(() => { Run(stopwatch, bbb, b_indexes, false); });
        //}

        public static void InitializeThreads(Stopwatch stopwatch, BankOfBitsNBytes bbb, List<int[]> all_indexes)
        {
            stopwatch.Start();
            bool dir = false; //// this boolean determine the order in which you can add arrays to the list
            ////// the first array must be a array that goes forward, the second will go backward and so on
            for (int i = 0; i < all_indexes.Count; i++)
            {
                dir = !dir;
                CreateThread(() => { Run(stopwatch, bbb, all_indexes[i - 1], dir); });
            }
        }

        public static void CreateThread(MyDelegate myDel)
        {
            Thread myThread = new Thread(new ThreadStart(myDel));
            myThread.Start();
        }

        public static void Run(Stopwatch stopwatch, BankOfBitsNBytes bbb, int[] indexes, bool isForward)
        {
            while (isRunning)
            {
                int value = bbb.WithdrawMoney(TestPassword(char_array, indexes, psw_lenght));
                lock (myLock)
                {
                    mutex.WaitOne();
                    if (value == -1)
                        isRunning = false;
                    if (value != -1)
                        amountStolen += value;
                    mutex.ReleaseMutex();
                }

                if (isForward)
                    Forward(indexes);
                else
                    Backward(indexes);
            }
            stopwatch.Stop();
            Console.WriteLine("Time : " + stopwatch.ElapsedMilliseconds + "ms");
        }
        #endregion

        static void Main(string[] args)
        {
            BankOfBitsNBytes bbb = new BankOfBitsNBytes();
            List<int[]> list_Array = new List<int[]>();
            int[] f_indexes = new int[psw_lenght];
            int[] b_indexes = new int[psw_lenght];

            f_indexes = InitializeIntArray(f_indexes, minIndex);
            b_indexes = InitializeIntArray(b_indexes, maxIndex);
            //char[] myArr = TestPassword(char_array, b_indexes, 3);
            //foreach (char c in myArr)
            //{
            //    Console.WriteLine(c);
            //}
            list_Array.Add(f_indexes);
            list_Array.Add(b_indexes);

            Stopwatch stopwatch = new Stopwatch();

            //InitializeThreads(stopwatch, bbb, f_indexes, b_indexes);
            InitializeThreads(stopwatch, bbb, list_Array);
            Console.ReadLine();
        }
    }
}
