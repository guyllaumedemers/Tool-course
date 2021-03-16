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
        static char[] psw_lenght = BankOfBitsNBytes.acceptablePasswordChars;
        static bool isRunning = true;
        static int maxIndex = 25;
        static int minIndex = 0;
        static int amountStolen = 0;
        static object myLock = new object();

        public delegate void MyDelegate();

        public static char[] TestPassword(char[] myArr, int indexI, int indexJ, int indexK)
        {
            return new char[] { myArr[indexI], myArr[indexJ], myArr[indexK] };
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
        public static void Forward(int[] indexes)
        {
            indexes[2] = UpdateIndexForward(indexes[2]);
            if (indexes[2] == minIndex)
                indexes[1] = UpdateIndexForward(indexes[1]);
            if (indexes[2] == minIndex && indexes[1] == minIndex)
                indexes[0] = UpdateIndexForward(indexes[0]);
        }

        public static void Backward(int[] indexes)
        {
            if (indexes[2] == minIndex && indexes[1] == minIndex)
                indexes[0] = UpdateIndexBackward(indexes[0]);
            if (indexes[2] == minIndex)
                indexes[1] = UpdateIndexBackward(indexes[1]);
            indexes[2] = UpdateIndexBackward(indexes[2]);
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

        public static void InitializeThreads(Stopwatch stopwatch, BankOfBitsNBytes bbb, int[] f_indexes, int[] b_indexes)
        {
            stopwatch.Start();
            CreateThread(() => { Run(stopwatch, bbb, f_indexes, true); });
            CreateThread(() => { Run(stopwatch, bbb, b_indexes, false); });
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
                int value = bbb.WithdrawMoney(TestPassword(psw_lenght, indexes[0], indexes[1], indexes[2]));
                if (value == -1)
                    isRunning = false;
                lock (myLock)
                {
                    if (value != -1)
                        amountStolen += value;
                }

                if (isForward)
                    Forward(indexes);
                else
                    Backward(indexes);
            }
            stopwatch.Stop();
            Console.WriteLine("Money Stolen : " + amountStolen);
            Console.WriteLine("Time : " + stopwatch.ElapsedMilliseconds + "ms");
        }
        #endregion

        static void Main(string[] args)
        {
            BankOfBitsNBytes bbb = new BankOfBitsNBytes();
            int[] f_indexes = new int[3];
            int[] b_indexes = new int[3];

            f_indexes = InitializeIntArray(f_indexes, minIndex);
            b_indexes = InitializeIntArray(b_indexes, maxIndex);

            Stopwatch stopwatch = new Stopwatch();

            InitializeThreads(stopwatch, bbb, f_indexes, b_indexes);

            Console.ReadLine();
        }
    }
}
