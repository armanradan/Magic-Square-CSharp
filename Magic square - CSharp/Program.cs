using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;
using System.Management;

namespace magic2
{
    class Program
    {
        static Thread[] shaker = {
            new(new ParameterizedThreadStart(Watch!)),
            new(new ParameterizedThreadStart(Watch!)),
            new(new ParameterizedThreadStart(Watch!)),
            new(new ParameterizedThreadStart(Watch!)),
            new(new ParameterizedThreadStart(Watch!)),
            new(new ParameterizedThreadStart(Watch!))
        };

        static List<Node> squares = [];
        static int size;
        static int chance = 0;
        static readonly Random gen = new();
        static bool found = false;
        static bool NeedShake = false;
        static int delay = 800;


        static void Main()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            size = Convert.ToInt32(Console.ReadLine());
            delay = (100000) * (size * size / 5);

            Fill(2);

            int limit = 190 + 10 * size * size, dif = size * size / 2 + 5 * size;
            //  filter(50, 2000);
            foreach (Thread t in shaker) t.Start(gen.Next(10));

            while (!found)
            {
                //  fill(20);
                found = jump(false, 5);
                //   filter(50, 20);
                chance = 0;
                foreach (Node n in squares)
                    chance += n.chance;
                //    Console.WriteLine(chance);
                if (chance > limit && !found)
                {
                    found = jump(true, 2);
                    limit += dif;
                    dif /= 2;
                }
                if (NeedShake && !found)
                {
                    found = jump(true, 2);
                    NeedShake = false;
                }

            }
            squares = [];
            foreach (Thread t in shaker) t.Interrupt();
            stopwatch.Stop();
            Console.WriteLine($"Time taken: {stopwatch.Elapsed.TotalSeconds}");
            Console.Read();

        }


        static bool jump(bool insist, int loop)
        {
            int temp;
            int i, j, k, l;
            int[,] test;
            for (int g = 0; g < loop; g++)
                foreach (Node element in squares)
                {
                    if (element.IsResult) { show(element, size); return true; }
                    i = gen.Next(0, size);
                    j = gen.Next(0, size);
                    k = gen.Next(0, size);
                    l = gen.Next(0, size);
                    test = new int[size, size];

                    for (int m = 0; m < size; m++)
                        for (int n = 0; n < size; n++)
                            test[m, n] = element.square[m, n];
                    temp = test[k, l];
                    test[k, l] = test[i, j];
                    test[i, j] = temp;
                    Node newnode = new Node(size);
                    newnode.square = test;
                    newnode.GuessChance();
                    if (newnode.chance >= element.chance || insist)
                    {
                        element.square = newnode.square;
                        element.GuessChance();
                    }
                }
            return false;
        }


        static void Fill(int count)
        {
            int[] buffer;
            int temp;
            Node n = new Node(size);
            for (int i = 0; i < count; i++)
            {
                buffer = new int[size * size];
                for (int j = 0; j < size; j++)
                    for (int k = 0; k < size; k++)
                    {
                        while (true)
                        {
                            temp = gen.Next(1, size * size + 1);
                            if (!buffer.Any(element => element == temp))
                            {
                                buffer[size * j + k] = temp;
                                break;
                            }
                        }
                        n.square[j, k] = temp;
                    }

                n.GuessChance();
                squares.Add(n);
            }
        }

        static void filter(int index, int count)
        {

            squares.Sort();
            squares.RemoveRange(index, count);
        }


        static void show(Node n, int size)
        {
            Console.WriteLine();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    Console.Write(String.Format("{0,-7}", n.square[i, j]));
                Console.WriteLine();
                Console.WriteLine();
            }
        }




        static void Shake(int span)
        {
            int n;

            Thread.Sleep(delay / 7 * span);
            while (true)
            {
                n = chance;
                Thread.Sleep(delay);
                if (n == chance) { NeedShake = true; delay += 5 * size * size; }
            }
        }

        static void Watch(object obj)
        {
            int delayShift = (int)obj;
            Shake(delayShift);
        }

        static public uint CPUSpeed()
        {
            ManagementObject Mo = new("Win32_Processor.DeviceID='CPU0'");
            uint sp = (uint)Mo["CurrentClockSpeed"];
            Mo.Dispose();
            return sp;
        }
    }
}
