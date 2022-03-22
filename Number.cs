using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Number
    {
        public int Value;

        public int PrimeNumberCheck()
        {
            int n;
            double d = Convert.ToDouble(Value);
            for (n = 2; n <= Value; n++)
                if ((d / n) % 1 == 0)
                {
                    if (n == d) n = 1;
                    break;
                }
            return n;
        }

        public List<int> FormCanon()
        {
            List<int> Canon = new List<int>();
            int save = Value;
            do
            {
                Canon.Add(PrimeNumberCheck());
                Value /= PrimeNumberCheck();
            }
            while (PrimeNumberCheck() != 1);
            Canon.Add(Value);
            Value = save;
            return Canon;
        }

        public int DividerCount()
        {
            int count = 1, total = 1;
            for (int i = 0; i < FormCanon().Count(); i++)
            {
                try
                {
                    if (FormCanon().ElementAt(i) == FormCanon().ElementAt(i + 1))
                        count++;
                    else
                    {
                        total *= count + 1;
                        count = 1;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    total *= count + 1;
                    break;
                }
            }
            return total;
        }
    }
}
