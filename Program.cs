using System;
using System.Collections.Generic;
using System.Linq;

namespace RSA
{
    class Program
    {
        unsafe public static void Main(string[] args)
        {
            
            int p = 1, q = 1, n = 1, fi, e, d = 3;

            #region ГЕНЕРАЦИЯ КЛЮЧЕЙ

            while (n < 256)
            {
                Generate(&p, &q);
                n = p * q;
            }

            fi = (p - 1) * (q - 1);

            List<int> Buff = new List<int>();
            Number E = new Number();

            int NOD(int a, int b)
            {
                if (a == b)
                    return a;
                else
                    if (a > b)
                    return NOD(a - b, b);
                else
                    return NOD(b - a, a);
            }

            for (int i = 3; i < fi; i++)
            {
                E.Value = i;
                if (E.PrimeNumberCheck() == 1 && NOD(E.Value, fi) == 1)
                    Buff.Add(i);
            }

            e = Buff[new Random().Next(0, Buff.Count())];
            Buff.Clear();

            int[] OpenKey = { e, n };

            while ((d * e) % fi != 1 || d == e)
                d++;

            int[] PrivateKey = { d, n };

            #endregion

            Console.Clear();
            Console.Write($"КЛЮЧИ СГЕНЕРИРОВАНЫ! СЕССИЯ ШИФРОВАНИЯ ЗАПУЩЕНА. ОТКРЫТЫЙ КЛЮЧ: [{OpenKey[0]}, {OpenKey[1]}] ");
            //Console.Write($"ЗАКРЫТЫЙ КЛЮЧ: [{PrivateKey[0]}, {PrivateKey[1]}]");
            for (;;)
                if (InterFace(OpenKey, PrivateKey) == 0) break;


        }

        unsafe public static void Generate(int* p, int* q)
        {
            var rand = new Random();
            Number P = new Number(),
                   Q = new Number();
            while (P.PrimeNumberCheck() != 1)
                P.Value = rand.Next(3, 200);
            *p = P.Value;

            while (Q.PrimeNumberCheck() != 1 || Q.Value == P.Value)
                Q.Value = rand.Next(3, 200);
            *q = Q.Value;
        }

        public static int ModularPow(int PE, int ed, int n)
        {
            long R = 1;
            for (int i = 0; i < ed; i++)
                R = (R * PE) % n;
            return Convert.ToInt32(R);
        }

        public static List<int> Cipher(string Str, int ed, int n)
        {
            List<int> Ciph = new List<int>();
            var rand = new Random();
            int extra = rand.Next(5, 50);
            for (int i = 0; i < extra; i++)
                Ciph.Add(ModularPow(rand.Next(32, 256), ed, n));

            foreach (char c in Str)
                Ciph.Add(ModularPow((int)c, ed, n));

            Ciph.Add(ModularPow(extra, ed, n));

            return Ciph;
        }

        public static string DeCipher(List<int> Message, int ed, int n)
        {
            string Str2 = "";
            int cc;
            var extra = Message.ElementAt(Message.Count() - 1);
            for (int i = ModularPow(extra, ed, n); i < Message.Count()-1; i++)
            {
                cc = ModularPow(Message.ElementAt(i), ed, n);
                Str2 += (char)cc;
            }

            return Str2;
        }

        public static short InterFace(int[] OpenKey, int[] PrivateKey)
        {
            string message;
            Console.WriteLine("\nВведите сообщение для зашифровки, или 0 для выхода: ");
            message = Console.ReadLine();
            if (message == "0")
                return 0;
            List<int> Message = new List<int>();
            Message = Cipher(message, OpenKey[0], OpenKey[1]);
            Console.Write("Зашифрованное сообщение: [");
            foreach (int cc in Message)
                Console.Write(Convert.ToString(cc, 16));
            Console.Write("]");
            Console.WriteLine($"\nДешифрованное сообщение: {DeCipher(Message, PrivateKey[0], PrivateKey[1])}");

            return 1;
        }

        
    }
}
