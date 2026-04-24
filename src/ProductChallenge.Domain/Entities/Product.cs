using System;
using System.Collections.Generic;
using System.Text;

namespace ProductChallenge.Domain.Entities
{
    public class Product
    {
        private Product()
        {
        }

        public int ProductId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int Status { get; private set; }
        public int Stock { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }

        public Product(int productId, string name, int status, int stock, string description, decimal price)
        {
            Validate(productId, name, status, stock, description, price);

            ProductId = productId;
            Name = name.Trim();
            Status = status;
            Stock = stock;
            Description = description.Trim();
            Price = price;
        }

        public void Update(string name, int status, int stock, string description, decimal price)
        {
            Validate(ProductId, name, status, stock, description, price);

            Name = name.Trim();
            Status = status;
            Stock = stock;
            Description = description.Trim();
            Price = price;
        }

        private static void Validate(int productId, string name, int status, int stock, string description, decimal price)
        {
            if (productId <= 0)
                throw new ArgumentException("El ProductId debe ser mayor que cero.", nameof(productId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre es obligatorio.", nameof(name));

            if (status is not (0 or 1))
                throw new ArgumentException("El estado debe ser 0 o 1.", nameof(status));

            if (stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.", nameof(stock));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Se requiere descripción.", nameof(description));

            if (price < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(price));
        }

    }
}
