using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var calc = new StringCalculator();
            var result = calc.Add("1,2");
        }
    }

    public class StringCalculator
    {
        public int Add(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            // default delimiters
            var delimiters = new List<string>(new[] { ",", "\n"});

            // parse specified delimiters
            const string comment = "//";
            if (input.StartsWith(comment) && input.Contains("\n"))
            {
                var customDelimiters = input.Substring(comment.Length, input.IndexOf("\n") - comment.Length); 
                delimiters.AddRange(customDelimiters.ToArray().Select(x => x.ToString()));
                input = input.Substring(input.IndexOf("\n"));
            }

            var operands = input.Split(delimiters.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var result = 0;

            // don't allow negative numbers
            var negatives = operands.Where(x => x < 0);
            if (negatives.Any())
            {
                throw new InvalidOperationException($"Negative numbers are not allowed: {string.Join(", ", negatives)}");
            }

            // add up all numbers unless over 1000 
            operands.ForEach(x =>
            {
                if (x <= 1000)
                    result += x;
            });

            return result; 
        }
    }
}
