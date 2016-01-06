using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HarryPotter.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class OptimizationTests
    {
        [TestMethod]
        public void KataExampleTest()
        {
            // arrange
            var basket = new List<Book>(new[]
            {
                new Book { Id = 1, Title = "first book" },
                new Book { Id = 1, Title = "first book" },
                new Book { Id = 2, Title = "second book" },
                new Book { Id = 2, Title = "second book" },
                new Book { Id = 3, Title = "third book" },
                new Book { Id = 3, Title = "third book" },
                new Book { Id = 4, Title = "fourth book" },
                new Book { Id = 5, Title = "fifth book" }
            });

            var calc = new Calculator(basket);

            // act
            var total = calc.CalculatePrice();

            // assert
            Assert.AreEqual(51.20m, total);
        }

        [TestMethod]
        public void NullBasketTest()
        {
            // arrange
            var calc = new Calculator(null);

            // act
            var total = calc.CalculatePrice();

            // assert
            Assert.AreEqual(0m, total);
        }

        [TestMethod]
        public void ZeroBooksTest()
        {
            // arrange
            var basket = new List<Book>();

            var calc = new Calculator(basket);

            // act
            var total = calc.CalculatePrice();

            // assert
            Assert.AreEqual(0m, total);
        }

        [TestMethod]
        public void SingleBookTest()
        {
            // arrange
            var basket = new List<Book>(new[]
            {
                new Book() {Id = 1, Title = "first book"},
            });

            var calc = new Calculator(basket);

            // act
            var total = calc.CalculatePrice();

            // assert
            Assert.AreEqual(8m, total);
        }

        [TestMethod]
        public void TwoBookTest()
        {
            // arrange
            var basket = new List<Book>(new[]
            {
                new Book() {Id = 1, Title = "first book"},
                new Book() {Id = 2, Title = "second book"},
            });

            var calc = new Calculator(basket);

            // act
            var total = calc.CalculatePrice();

            // assert
            Assert.AreEqual(16m, total);
        }

        [TestMethod]
        public void IdenticalBooksNotDiscountedTest()
        {
            // arrange
            var basket = new List<Book>(new[]
            {
                new Book() {Id = 1, Title = "first book"},
                new Book() {Id = 1, Title = "first book"},
                new Book() {Id = 1, Title = "first book"},
                new Book() {Id = 1, Title = "first book"},
            });

            var calc = new Calculator(basket);

            // act
            var total = calc.CalculatePrice();

            // assert
            Assert.AreEqual(32m, total);
        }

        // this failed in my initial solution
        [TestMethod]
        public void SetOfThreeBooksDiscountedTest()
        {
            // arrange
            var basket = new List<Book>(new[]
            {
                new Book() {Id = 1, Title = "first book"},
                new Book() {Id = 2, Title = "second book"},
                new Book() {Id = 1, Title = "first book"},
                new Book() {Id = 3, Title = "third book"},
            });

            var calc = new Calculator(basket);

            // act
            var total = calc.CalculatePrice();

            // assert
            Assert.AreEqual(29.6m, total);
        }

        [TestMethod]
        public void TwoSetsOfThreeBooksTest()
        {
            // arrange
            var basket = new List<Book>(new[]
            {
                new Book() {Id = 1, Title = "first book"},
                new Book() {Id = 2, Title = "second book"},
                new Book() {Id = 3, Title = "third book"},
                new Book() {Id = 2, Title = "second book"},
                new Book() {Id = 3, Title = "third book"},
                new Book() {Id = 4, Title = "fourth book"},
            });

            var calc = new Calculator(basket);

            // act
            var total = calc.CalculatePrice();

            // assert
            Assert.AreEqual(43.2m, total);
        }

        [TestMethod]
        public void OneSetOfFiveBooksTest()
        {
            // arrange
            var basket = new List<Book>(new[]
            {
                new Book() {Id = 1, Title = "first book"},
                new Book() {Id = 2, Title = "second book"},
                new Book() {Id = 3, Title = "third book"},
                new Book() {Id = 4, Title = "fourth book"},
                new Book() {Id = 5, Title = "fifth book"},
            });

            var calc = new Calculator(basket);

            // act
            var total = calc.CalculatePrice();

            // assert
            Assert.AreEqual(30m, total);
        }

        // this currently fails with 61.60 in 4 partial sets
        [TestMethod]
        public void OverflowingSetTest()
        {
            // arrange
            var basket = new List<Book>(new[]
            {
                new Book() {Id = 1, Title = "first book"},
                new Book() {Id = 2, Title = "second book"},
                new Book() {Id = 3, Title = "third book"},
                new Book() {Id = 4, Title = "fourth book"},
                new Book() {Id = 5, Title = "fifth book"},
                new Book() {Id = 5, Title = "fifth book"},
                new Book() {Id = 5, Title = "fifth book"},
                new Book() {Id = 5, Title = "fifth book"},
            });

            var calc = new Calculator(basket);

            // act
            var total = calc.CalculatePrice();

            // assert
            Assert.AreEqual(54m, total);
        }
    }
}
