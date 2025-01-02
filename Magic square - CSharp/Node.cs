using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magic2
{
    class Node : IComparable<Node>
    {
        public int[,] square;
        public int[]   check;
        int size;
        public int chance;
        public Node(int Size)
        {
            size   = Size;
            square = new int[size, size];
            check  = new int[2 * size + 2];
        }

  
       public void GuessChance()
        {
            int sum ;
            for (int k = 0; k < size; k++)
            {
                sum = 0;
                for (int j = 0; j < size; j++)
                    sum += square[k, j];
                check[k] = (int)(sum == (size * (size * size + 1)) / 2 ? 1 : 0);
            }

            for (int k = 0; k < size; k++)
            {
                sum = 0;
                for (int j = 0; j < size; j++)
                    sum += square[j, k];
                check[k + size] = (sum == (size * (size * size + 1)) / 2 ? 1 : 0);
            }

            sum = 0;
            for (int i = 0; i < size; i++)
                sum += square[i, i];
            check[2*size] = (sum == (size * (size * size + 1)) / 2 ? 1 : 0);


            sum = 0;
            for (int i = 0; i < size; i++)
                sum += square[size - i - 1, i ];
            check[2 * size+1] = (sum == (size * (size * size + 1)) / 2 ? 1 : 0);


            chance = 0;
                foreach (int c in check)
                    chance += c;
        }

        public bool IsResult
        {
            get
            {
                return (chance == 2*size+2 ? true : false);
            }
        }

        public int CompareTo(Node t)
        {
            if (t.chance == chance) return  0;
            if (t.chance > chance ) return  1;
                                    return -1;
        }

    }
}
