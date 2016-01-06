using System;
using System.Collections.Generic;
using System.Linq;

namespace HarryPotter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var basket = new List<Book>();

            basket.AddRange(new[]
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

            var calculator = new Calculator(basket);
            var result = calculator.CalculatePrice();
            Console.WriteLine($"Best discounted price: {result}");
#if DEBUG
            Console.ReadKey();
#endif
        }
    }

    public class Discount
    {
        public int Quantity { get; set; }
        public decimal AdjustedPrice { get; set; }
    }

    public class Calculator
    {
        private readonly List<Book> _basket;
        private List<Discount> Discounts { get; }

        public Calculator(List<Book> basket)
        {
            _basket = basket;

            Discounts = new List<Discount>(new[]
            {
                new Discount { Quantity = 3, AdjustedPrice = 0.90m },
                new Discount { Quantity = 4, AdjustedPrice = 0.80m },
                new Discount { Quantity = 5, AdjustedPrice = 0.75m },
            });
        }

        public decimal CalculatePrice()
        {
            // different distributions of 2-2-2-1-1

            // this is "most obvious" (5-3) 

            //var sets = new List<List<Book>>(new[] { new List<Book>(), });

            //foreach (var book in _basket.OrderBy(x => x.Id))
            //{
            //    var set = sets.FirstOrDefault(x => x.All(y => y.Id != book.Id));

            //    if (set == null)
            //    {
            //        set = new List<Book>();
            //        sets.Add(set);
            //    }

            //    set.Add(book);
            //}

            //// now try "max spread" (5-3 => 4-4)

            //sets = new List<List<Book>>(new[] { new List<Book>() });

            //foreach (var book in _basket.OrderBy(x => x.Id))
            //{
            //    var set = sets.FirstOrDefault(x => x.All(y => y.Id != book.Id));

            //    if (set == null)
            //    {
            //        set = new List<Book>();
            //        sets.Add(set);
            //    }

            //    set.Add(book);
            //}

            //for (var i = 0; i < sets.Count; i++)
            //{
            //    Console.WriteLine($"Set {i}: {sets[i].Count} books");
            //}

            // how? 
            // group by qty
            // # of sets will == max qty 
            // then ... ??? 

            if (_basket == null || !_basket.Any())
            {
                return 0m; 
            }
            
            //Console.WriteLine("distribution approach: ");
            var bookGroups = _basket.GroupBy(x => x.Id).ToList();

            var maxGroupQuantity = bookGroups
                .Select(x => new { Id = x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .First()
                .Count; 

            var bookSets = new List<List<Book>>();

            for (var i = 0; i < maxGroupQuantity; i++)
            {
                bookSets.Add(new List<Book>());
            }

            foreach (var group in bookGroups)
            {
                foreach (var book in group)
                {
                    var minQuantityBucket = bookSets
                        .OrderBy(x => x.Count)
                        .FirstOrDefault(x => x.Count < 3 && x.All(y => y.Id != book.Id));

                    if (minQuantityBucket == null)
                    {
                        minQuantityBucket = new List<Book>();
                        bookSets.Add(minQuantityBucket);
                    }

                    minQuantityBucket.Add(book);
                }
            }
            
            // attempt to balance/redistribute... 


            
            // get all buckets that have < 3
            // add each book to first bucket that doesn't have that book and has < 5

            var partialSets = bookSets.Where(x => x.Count >= 3 && x.Count < 5).ToList();
            var nonSets = bookSets.Where(x => x.Count > 0 && x.Count < 3).ToList();

            foreach (var nonSet in nonSets)
            {
                var toRemove = new List<Book>(); 

                foreach (var book in nonSet)
                {
                    //var partialSet = partialSets
                    var partialSet = bookSets
                        .OrderBy(x => x.Count)
                        .FirstOrDefault(x => x.Count < 5 && x.All(y => y.Id != book.Id));

                    if (partialSet != null)
                    {
                        partialSet.Add(book);
                        toRemove.Add(book);
                    }
                }

                nonSet.RemoveAll(x => toRemove.Contains(x)); 
            }

            var total = 0m;

            for (var i = 0; i < bookSets.Count(x => x.Count > 0); i++)
            {
                Console.WriteLine($"Bucket {i}");

                foreach (var book in bookSets[i])
                {
                    Console.WriteLine($"\t{book.Title}");
                }

                var subTotal = 0m;
                bookSets[i].ForEach(x => subTotal += x.Price);

                var discount = Discounts.FirstOrDefault(x => x.Quantity == bookSets[i].Count);

                if (discount != null)
                {
                    subTotal *= discount.AdjustedPrice;
                }

                Console.WriteLine($"\t== {subTotal}");
                total += subTotal;
            }

            Console.WriteLine($"total after optimizing: {total}\n");

            return total;
        }
    }

    public class Book
    {
        public Book()
        {
            Price = 8.00m;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; private set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
