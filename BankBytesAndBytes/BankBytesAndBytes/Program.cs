using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBytesAndBytes
{
    class Program
    {
        static void Main(string[] args)
        {
            BankOfBitsNBytes bbb = new BankOfBitsNBytes();
            for (int i = 0; i < 100; i++)
            {
                int returnedAmt = bbb.WithdrawMoney(BankOfBitsNBytes.GenerateRandomCharArray(bbb.passwordLength));
                //Console.WriteLine("Returned amt: " + returnedAmt);
            }
            Console.ReadLine();
        }
    }
}
