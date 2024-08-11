using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.ChatServer
{
    public class SharedRessource
    {
        private int counter = 0;

        //public void Increment()
        //{
        //    int temp = counter;
        //    Thread.Sleep(500); // Simuler un délai pour accentuer la condition de course
        //    counter = temp + 1;
        //    Console.WriteLine("Global messages counter incremented to: " + counter);
        //}


        private readonly object _lock = new object();
        public void Increment()
        {
            lock (_lock)
            {
                int temp = counter;
                Thread.Sleep(500); // Simuler un délai pour accentuer la condition de course
                counter = temp + 1;
            }
          
            Console.WriteLine("Global messages counter incremented to: " + counter);
        }

        public int GetCounter()
        {
            return counter;
        }
    }
}
