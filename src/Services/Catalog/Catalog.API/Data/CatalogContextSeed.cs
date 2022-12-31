using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Catalog.API.Data
{
    public static class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool isProductExist = productCollection.Find(p => true).Any();
            if (!isProductExist)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "IPhone X",
                    Summary = "This phone is the company's biggest change to it's flagship smartphone in years.",
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusantium eius quis dicta, quam fuga sunt vero veritatis repellendus. Enim, atque!",
                    ImageFile = "product-1.png",
                    Price = 950.00M,
                    Category = "Smart Phone"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Samsung 10",
                    Summary = "This phone is the company's biggest change to it's flagship smartphone in years.",
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusantium eius quis dicta, quam fuga sunt vero veritatis repellendus. Enim, atque!",
                    ImageFile = "product-2.png",
                    Price = 840.00M,
                    Category = "Smart Phone"
                },
                 new Product()
                {
                    Id = "602d2149e773f2a3990b47f7",
                    Name = "Huawei Plus",
                    Summary = "This phone is the company's biggest change to it's flagship smartphone in years.",
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusantium eius quis dicta, quam fuga sunt vero veritatis repellendus. Enim, atque!",
                    ImageFile = "product-3.png",
                    Price = 650.00M,
                    Category = "White Appliances"
                },
                  new Product()
                {
                    Id = "602d2149e773f2a3990b47f8",
                    Name = "Xiaomi Mi 9",
                    Summary = "This phone is the company's biggest change to it's flagship smartphone in years.",
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusantium eius quis dicta, quam fuga sunt vero veritatis repellendus. Enim, atque!",
                    ImageFile = "product-4.png",
                    Price = 470.00M,
                    Category = "White Appliances"
                },
                   new Product()
                {
                    Id = "602d2149e773f2a3990b47f9",
                    Name = "HTC U11+ Plus",
                    Summary = "This phone is the company's biggest change to it's flagship smartphone in years.",
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusantium eius quis dicta, quam fuga sunt vero veritatis repellendus. Enim, atque!",
                    ImageFile = "product-5.png",
                    Price = 380.00M,
                    Category = "Smart Phone"
                },
                      new Product()
                {
                    Id = "602d2149e773f2a3990b47fa",
                    Name = "LG G7 ThinQ",
                    Summary = "This phone is the company's biggest change to it's flagship smartphone in years.",
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusantium eius quis dicta, quam fuga sunt vero veritatis repellendus. Enim, atque!",
                    ImageFile = "product-6.png",
                    Price = 240.00M,
                    Category = "Home Kitchen"
                }
            };
        }
    }
}
