using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AddArrayRecursively
{
    //
    // add an array of items recursively 
    // 
    class Program
    {
        static void Main(string[] args)
        {
            new Part1().Run();
            new Part2().Run();
            new Part3().Run();
            
#if DEBUG
            Console.ReadKey();
#endif
        }
    }

    // Part 1
    // You have an array of strings. Write a method that will iterate through them concatenating the strings together.
    internal class Part1
    { 
        public void Run()
        {
            Console.WriteLine("Part 1");
            var input = new[] { "one ", "two ", "three ", "four " };
            Console.WriteLine("input: [{0}]", string.Join(",", input.Select(x => $"\"{x}\"")));

            var output = Concat(input, 0);
            Console.WriteLine("output: \"{0}\"", output);
        }

        public string Concat(string[] input, int index)
        {
            if(index == input.Length)
            {
                return string.Empty; 
            }

            return input[index++] + Concat(input, index); 
        }
    }

    // Part 2
    // Refactor your method to take either an array of strings or an array of int "adding" all the items together.
    internal class Part2
    {
        public void Run()
        {
            Console.WriteLine(Environment.NewLine+"Part 2");
            var input = new[] { "one ", "two ", "three ", "four " };
            Console.WriteLine("input 1: [{0}]", string.Join(",", input.Select(x => $"\"{x}\"")));

            var output = Concat(input, 0);
            Console.WriteLine("output 1: \"{0}\"", output);

            var input2 = new[] { 1, 2, 3, 4 };
            Console.WriteLine("input 2: [{0}]", string.Join(",", input2.Select(x => x)));

            var output2 = Concat(input2, 0);
            Console.WriteLine("output 2: \"{0}\"", output2);
        }

        public T Concat<T>(T[] input, int index) 
        {
            if (index == input.Length)
            {
                return default(T);
            }
            
            return (dynamic)input[index++] + Concat(input, index);
        }
    }

    // Part 3
    // Refactor your method further to accept the expression that you want to perform on all of the items
    internal class Part3
    {
        public void Run()
        {
            Console.WriteLine(Environment.NewLine+"Part 3");
            var input = new[] { "five", "six", "seven", "eight" };
            Console.WriteLine("input: [{0}]", string.Join(",", input.Select(x => $"\"{x}\"")));

            var output = DoExpression(input, (x, y) => x + " and a-"+ y, 0);
            Console.WriteLine("output: \"{0}\"", output);
        }

        public T DoExpression<T>(T[] input, Expression<Func<T, T, T>> expr, int index)
        {
            return index == input.Length 
                ? default(T) 
                : expr.Compile()(input[index++], DoExpression(input, expr, index));
        }
    }
}
