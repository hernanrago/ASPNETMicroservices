using MongoDB.Driver;

namespace Catalog.API.Data
{
    internal class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> products)
        {
            bool existProduct = products.Find(p => true).Any();

            if (!existProduct)
            {
                products.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new Product[]
            {
                new Product()
                {
                    Name = "Iphone X",
                    Category = "",
                    Description = "",
                    ImageFile = "",
                    Price = 950.00M,
                    Summary = ""
                },
                                new Product()
                {
                    Name = "Samsung 10",
                    Category = "",
                    Description = "",
                    ImageFile = "",
                    Price = 840.00M,
                    Summary = ""
                }
            };
        }
    }
}