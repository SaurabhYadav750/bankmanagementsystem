using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bankmanagementsystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bankservices bank=new bankservices();
            bank.start();
            

        }
    }
}
