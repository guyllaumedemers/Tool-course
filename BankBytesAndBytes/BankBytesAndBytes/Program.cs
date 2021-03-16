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
            return new char[] { myArr[indexI], myArr[indexJ], myArr[indexK] };
        }

        public static int UpdateIndex(int index)
        {
            return index < maxIndex ? ++index : 0;
        }

        static void Main(string[] args)
        {
            BankOfBitsNBytes bbb = new BankOfBitsNBytes();
            int amountStolen = 0;
            int lastAmount = 0;
            int indexI = 0, indexJ = 0, indexK = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (isRunning)
            {
                lastAmount = amountStolen;
                amountStolen += bbb.WithdrawMoney(TestPassword(psw_lenght, indexI, indexJ, indexK));
                if (lastAmount != amountStolen)
                    Console.WriteLine("Amt Stolen : " + amountStolen);
                ///// Test password and increment the index of the last character in the array, if the character is > 26, increament his neighbor
                ///// Forward version where the Thread is going from {'A','A','A'};
                ///// We are going to also do a version where the Thread is going Backward
                indexK = UpdateIndex(indexK);
                if (indexK == minIndex)
                    indexJ = UpdateIndex(indexJ);
                if (indexK == minIndex && indexJ == minIndex)
                    indexI = UpdateIndex(indexI);

                if (amountStolen >= 5000)
                    isRunning = false;
            }
            stopwatch.Stop();
            Console.WriteLine("Time : " + stopwatch.ElapsedMilliseconds + "ms");
            Console.ReadLine();
        }
    }
}
