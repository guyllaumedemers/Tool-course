using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBytesAndBytes
{
    class Program
    {
        static char[] psw_lenght = BankOfBitsNBytes.acceptablePasswordChars;
        static bool isRunning = true;
        static int maxIndex = 25;
        static int minIndex = 0;

        public static char[] TestPassword(char[] myArr, int indexI, int indexJ, int indexK)
        {
            Console.WriteLine("{0}, {1}, {2}", myArr[indexI], myArr[indexJ], myArr[indexK]);
            return new char[] { myArr[indexI], myArr[indexJ], myArr[indexK] };
        }

        public static int UpdateIndexForward(int index)
        {
            return index < maxIndex ? ++index : minIndex;
        }

        public static int UpdateIndexBackward(int index)
        {
            return index > minIndex ? --index : maxIndex;
        }

        public static void Forward(ref int indexI, ref int indexJ, ref int indexK)
        {
            indexK = UpdateIndexForward(indexK);
            if (indexK == minIndex)
                indexJ = UpdateIndexForward(indexJ);
            if (indexK == minIndex && indexJ == minIndex)
                indexI = UpdateIndexForward(indexI);
        }

        public static void Backward(ref int indexL, ref int indexM, ref int indexN)
        {
            if (indexN == minIndex && indexM == minIndex)
                indexL = UpdateIndexBackward(indexL);
            if (indexN == minIndex)
                indexM = UpdateIndexBackward(indexM);
            indexN = UpdateIndexBackward(indexN);
        }

        public static int[] InitializeIntArray(int[] myArr, int value)
        {
            for (int i = 0; i < myArr.Length; i++)
            {
                myArr[i] = value;
            }
            return myArr;
        }

        static void Main(string[] args)
        {
            BankOfBitsNBytes bbb = new BankOfBitsNBytes();
            int amountStolen = 0;
            int lastAmount = 0;
            int[] f_indexes = new int[3];
            int[] b_indexes = new int[3];
            //int indexI = 0, indexJ = 0, indexK = 0;
            f_indexes = InitializeIntArray(f_indexes, minIndex);
            b_indexes = InitializeIntArray(b_indexes, maxIndex);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (isRunning)
            {
                lastAmount = amountStolen;
                amountStolen += bbb.WithdrawMoney(TestPassword(psw_lenght, b_indexes[0], b_indexes[1], b_indexes[2]));
                if (lastAmount != amountStolen)
                    Console.WriteLine("Amt Stolen : " + amountStolen);
                ///// Test password and increment the index of the last character in the array, if the character is > 26, increament his neighbor
                ///// Forward version where the Thread is going from {'A','A','A'};
                ///// We are going to also do a version where the Thread is going Backward

                //Forward(ref indexes[0], ref indexes[0], ref indexes[0]);
                //Backward(ref b_indexes[0], ref b_indexes[1], ref b_indexes[2]);

                if (amountStolen >= 5000)
                    isRunning = false;
            }
            stopwatch.Stop();
            Console.WriteLine("Time : " + stopwatch.ElapsedMilliseconds + "ms");
            Console.ReadLine();
        }
    }
}
