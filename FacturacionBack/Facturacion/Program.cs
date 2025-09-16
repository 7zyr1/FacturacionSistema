using System;
using System.Collections.Generic;
using System.Linq;
using Facturacion.data.Repositories;
using Facturacion.data.interfaces;
using Facturacion.domain;

namespace Facturacion
{
    internal class Program
    {
        private static IClientRepository _clientRepository;
        private static IPaymentRepository _paymentRepository;
        private static IProductRepository _productRepository;
        private static IDetailRepository _detailRepository;
        private static IBillRepository _billRepository;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            
            InitializeRepositories();

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== SISTEMA DE FACTURACIÓN ===");
                Console.WriteLine("1. Ingresar nueva factura");
                Console.WriteLine("2. Listar facturas");
                Console.WriteLine("3. Salir");
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        IngresarFactura();
                        break;
                    case "2":
                        ListarFacturas();
                        break;
                    case "3":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void InitializeRepositories()
        {
            _clientRepository = new ClientRepository();
            _paymentRepository = new PaymentRepository();
            _productRepository = new ProductRepository();
            _detailRepository = new DetailRepository();
            _billRepository = new BillRepository();

            // Configurar dependencias
            if (_detailRepository is DetailRepository detailRepo)
            {
                detailRepo._productRepository = _productRepository;
            }

            if (_billRepository is BillRepository billRepo)
            {
                billRepo._clientRepository = _clientRepository;
                billRepo._paymentRepository = _paymentRepository;
                billRepo._detailRepository = _detailRepository;
            }
        }

        private static void IngresarFactura()
        {
            Console.Clear();
            Console.WriteLine("=== INGRESAR NUEVA FACTURA ===");

            try
            {
                var cliente = SeleccionarCliente();
                if (cliente == null) return;

                var formaPago = SeleccionarFormaPago();
                if (formaPago == null) return;

                var detalles = ObtenerDetallesFactura();
                if (detalles == null || !detalles.Any()) return;

                var factura = new Bill
                {
                    Client = cliente,
                    Payment = formaPago,
                    dateTime = DateTime.Now,
                    Details = detalles
                };

                bool resultado = _billRepository.SaveBill(factura);

                if (resultado)
                {
                    Console.WriteLine("¡Factura guardada correctamente!");
                    //Console.WriteLine($"Total: {CalcularTotal(detalles):C}");
                }
                else
                {
                    Console.WriteLine("Error al guardar la factura.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static Client SeleccionarCliente()
        {
            Console.WriteLine("\n--- Selección de Cliente ---");
            Console.Write("Ingrese el ID del cliente: ");

            if (int.TryParse(Console.ReadLine(), out int clienteId))
            {
                var cliente = _clientRepository.GetClientById(clienteId);
                if (cliente != null)
                {
                    Console.WriteLine($"Cliente seleccionado: {cliente.name}");
                    return cliente;
                }
                Console.WriteLine("Cliente no encontrado.");
            }
            else
            {
                Console.WriteLine("ID de cliente no válido.");
            }
            return null;
        }

        private static Payment SeleccionarFormaPago()
        {
            Console.WriteLine("\n--- Selección de Forma de Pago ---");
            Console.WriteLine("1. Efectivo");
            Console.WriteLine("2. Tarjeta de Crédito");
            Console.WriteLine("3. Transferencia");
            Console.Write("Seleccione la forma de pago (1-3): ");

            var opcion = Console.ReadLine();
            int paymentId = opcion switch
            {
                "1" => 1,
                "2" => 2,
                "3" => 3,
                _ => 1
            };

            var pago = _paymentRepository.GetPaymentById(paymentId);
            if (pago != null)
            {
                Console.WriteLine($"Forma de pago seleccionada: {pago.Method}");
                return pago;
            }

            Console.WriteLine("Forma de pago no encontrada, usando Efectivo por defecto.");
            return new Payment { Id = 1, Method = "Efectivo" };
        }

        private static List<Detail> ObtenerDetallesFactura()
        {
            var detalles = new List<Detail>();
            bool agregarMasProductos = true;

            Console.WriteLine("\n--- Detalles de la Factura ---");

            while (agregarMasProductos)
            {
                var productos = _productRepository.GetAllProducts();

                if (productos == null || !productos.Any())
                {
                    Console.WriteLine("No hay productos disponibles.");
                    return detalles;
                }

                Console.WriteLine("\nProductos disponibles:");
                foreach (var prod in productos)
                {
                    Console.WriteLine($"{prod.Id}. {prod.Name} - {prod.Price:C} - Stock: {prod.Stock}");
                }

                Console.Write("\nIngrese el ID del producto: ");
                if (!int.TryParse(Console.ReadLine(), out int productoId))
                {
                    Console.WriteLine("ID no válido.");
                    continue;
                }

                var producto = productos.FirstOrDefault(p => p.Id == productoId);
                if (producto == null)
                {
                    Console.WriteLine("Producto no encontrado.");
                    continue;
                }

                Console.Write($"Cantidad de '{producto.Name}': ");
                if (!int.TryParse(Console.ReadLine(), out int cantidad) || cantidad <= 0)
                {
                    Console.WriteLine("Cantidad no válida.");
                    continue;
                }

                if (cantidad > producto.Stock)
                {
                    Console.WriteLine($"Stock insuficiente. Disponible: {producto.Stock}");
                    continue;
                }

                var detalle = new Detail
                {
                    Product = producto,
                    Quantity = cantidad,
                    Price = producto.Price * cantidad
                };

                detalles.Add(detalle);
                Console.WriteLine($"✓ Agregado: {cantidad} x {producto.Name} = {detalle.Price:C}");

                Console.Write("¿Agregar otro producto? (s/n): ");
                agregarMasProductos = Console.ReadLine().ToLower() == "s";
            }

            return detalles;
        }

        //private static decimal CalcularTotal(List<Detail> detalles)
        //{
        //    return detalles.Sum(d => d.Price);
        //}

        private static void ListarFacturas()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE FACTURAS ===");

            try
            {
                var facturas = _billRepository.GetAllBill();

                if (facturas == null || !facturas.Any())
                {
                    Console.WriteLine("No hay facturas registradas.");
                }
                else
                {
                    foreach (var factura in facturas)
                    {
                        Console.WriteLine($"\nFactura #{factura.Id} - {factura.dateTime:dd/MM/yyyy}");
                        Console.WriteLine($"Cliente: {factura.Client?.name}");
                        Console.WriteLine($"Pago: {factura.Payment?.Method}");
                        Console.WriteLine("Productos:");

                        foreach (var detalle in factura.Details)
                        {
                            Console.WriteLine($"  {detalle.Quantity} x {detalle.Product?.Name} = {detalle.Price:C}");
                        }

                        Console.WriteLine($"TOTAL: {factura.Details.Sum(d => d.Price):C}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}