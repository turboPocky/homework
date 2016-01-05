using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringCalculator.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StringCalculatorTests
    {
        // step 1: an empty string it will return 0
        [TestMethod]
        public void EmptyStringReturnsZero()
        {
            // arrange
            var calc = new StringCalculator();
            var input = string.Empty;

            // act
            var result = calc.Add(input);

            // assert
            Assert.AreEqual(0, result); 
        }

        // step 1: 1 number returns the number
        [TestMethod]
        public void OneNumberReturnsItself()
        {
            // arrange
            var calc = new StringCalculator();
            const string input = "1";

            // act
            var result = calc.Add(input);

            // assert
            Assert.AreEqual(1, result);
        }

        // step 1: 2 numbers get added
        [TestMethod]
        public void TwoCommaSeparatedNumbersAreAdded()
        {
            // arrange
            var calc = new StringCalculator();
            const string input = "1,2";

            // act
            var result = calc.Add(input);

            // assert
            Assert.AreEqual(3, result);
        }

        // step 2: arbitrary number of operands are handled
        [TestMethod]
        public void FourNumbersAreAdded()
        {
            // arrange
            var calc = new StringCalculator();
            const string input = "1,2,3,4";

            // act
            var result = calc.Add(input);

            // assert
            Assert.AreEqual(10, result);
        }

        // step 3: Newlines work as well as commas
        [TestMethod]
        public void NewlinesCanBeUsedAsDelimiters()
        {
            // arrange
            var calc = new StringCalculator();
            const string input = "1\n2,3";

            // act
            var result = calc.Add(input);

            // assert
            Assert.AreEqual(6, result);
        }

        // step 4: support delimiter specification
        [TestMethod]
        public void AllowsSpecifiedOneCharDelimiters()
        {
            // arrange
            var calc = new StringCalculator();
            const string input = "//;\n1;2";

            // act
            var result = calc.Add(input);

            // assert
            Assert.AreEqual(3, result);
        }

        // step 5: negatives throw an exception
        [TestMethod]
        public void NegativeNumbersCauseException()
        {
            // arrange
            var calc = new StringCalculator();
            const string input = "3,4,-10,3,-6";
            InvalidOperationException exception = null;

            try
            {
                // act
                var result = calc.Add(input);
            }
            catch (InvalidOperationException e)
            {
                exception = e;
            }

            // assert
            Assert.IsNotNull(exception);
            Assert.AreEqual("Negative numbers are not allowed: -10, -6", exception.Message);
        }

        // step 6: ignore numbers > 1000
        [TestMethod]
        public void NumbersOver1000AreIgnored()
        {
            // arrange
            var calc = new StringCalculator();
            const string input = "2,1001";

            // act
            var result = calc.Add(input);

            // assert
            Assert.AreEqual(2, result);
        }

        // step 7: Delimiters can be of any length with the following format:  “//[delimiter]\n” 
        [TestMethod]
        public void MulticharDelimitersAreSupported()
        {
            // arrange
            var calc = new StringCalculator();
            const string input = "//[***]\n1***2***3";      

            // act
            var result = calc.Add(input);

            // assert
            Assert.AreEqual(6, result);  // this passed after step 6 even though i didn't add [***] support in the code!!
        }

        // step 8: Allow multiple step 7 type delimiters
        [TestMethod]
        public void MultipleBracketedDelimitersCanBeSpecified()
        {
            // arrange
            var calc = new StringCalculator();
            const string input = "//[*][%]\n1*2%3";

            // act
            var result = calc.Add(input);

            // assert
            Assert.AreEqual(6, result);  // this still passed even though i haven't changed code since step 6!!
        }

        // step 8: Allow multiple step 7 type delimiters
        [TestMethod]
        public void MultipleBracketedMulticharDelimitersCanBeSpecified()
        {
            // arrange
            var calc = new StringCalculator();
            const string input = "//[*][***]\n1*2***3";

            // act
            var result = calc.Add(input);

            // assert
            Assert.AreEqual(6, result);  // this still passed even though i haven't changed code since step 6!!
        }
    }
}
