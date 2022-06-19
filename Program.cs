﻿using System;

namespace aplikasi_struk
{
    class Program
    {
        // variables
        static Page page;
        static readonly PaymentMethod[] paymentMethod = PaymentMethods.Default;

        class Handler
        {
            static void HandleAddOn(AddOn addOn, Order.OrderItem orderItem)
            {

            }
            static void HandleDish(Dish dish, Order.OrderItem orderItem)
            {
                orderItem.DishList.Add(dish);

                Console.WriteLine();
                Console.WriteLine(dish.Name + " telah ditambahkan ke pesanan ke-" + orderItem.DishList.ToArray().Length + "!");

                if (dish.GetType().IsAssignableFrom((new Dish(0, "", 0)).GetType()))
                {
                    bool repeat = true;
                    do
                    {
                    } while (!repeat);
                }


                Console.Write("Tekan Enter untuk melanjutkan");
                Console.ReadLine();

            }
            public static void HandleCategory(Dish[] dishes, Order.OrderItem orderItem)
            {
                Console.WriteLine("Pilihan yang tersedia: ");
                page.CreateDishList(dishes);
                int dishIndex = int.Parse(page.AskInput("Masukkan pilihan", dishes.Length)) - 1;
                HandleDish(dishes[dishIndex], orderItem);

            }

            public static void HandlePayment(PaymentMethod paymentMethod, Order.OrderItem orderItem)
            {
                orderItem.paymentMethod = paymentMethod;
                Console.WriteLine(paymentMethod.Name + " telah dipilih sebagai metode pembayaran!");
                Console.Write("Tekan Enter untuk melanjutkan");
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
        mulai:
            page = new Page(Store.Name, 50);
            page.WriteTitle(Store.Name);
            Customer customer = new(page.AskInput("Silakan masukkan nama pelanggan: "));

        pesan:
            page.Clear();

            Order order = new();
            Order.OrderItem orderItem = order.NewItem();

            // config
            bool fired = false;
            int i = 1;
            string msg = "Pilih kategori pesanan ke-" + i.ToString();
            bool repeat = true;
            do
            {
                page.Clear();
                Console.WriteLine(msg);
                if (fired) Console.WriteLine("0. Selesai");
                page.CreateCategoryList(Menu.Categories);
                int categoryId = int.Parse(page.AskInput("Masukkan pilihan", Menu.Categories.Length)) - 1;
                Console.WriteLine();
                if (categoryId == -1) repeat = false;
                else Handler.HandleCategory(Menu.Categories[categoryId].Dishes, orderItem);

                // config            
                i++;
                msg = "Pilih kategori pesanan ke-" + i;
                fired = true;
            } while (repeat);

        konfirmasi:
            page.Clear();

            Console.WriteLine("Daftar pesanan: ");
            page.CreateDishList(orderItem.DishList.ToArray());
            Console.Write("Lanjutkan? [y/n]: ");
            string lanjut = Console.ReadLine().ToLower();

            // Tudabes: handle inputan `confirmed` selain "y"

            if (lanjut == "y")
            {
                Console.Write("Sedang memproses pesanan Anda");
                page.Loading();
            }
            else if (lanjut == "n")
            {
                Console.Write("Ingin mengulang pemesanan? [y/n] : ");
                string ulang = Console.ReadLine().ToLower();
                if (ulang == "y")
                {
                    Console.Write("Sedang mengulang pemesanan anda");
                    page.Loading();
                    goto pesan;
                }
                else if (ulang == "n")
                {
                    Console.Write("Terima kasih");
                    page.Loading();
                    Console.Clear();
                    goto mulai;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("INPUTAN ANDA TIDAK VALID!\nSILAHKAN KONFIRMASI ULANG");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("Tekan Enter untuk melanjutkan");
                    Console.WriteLine();
                    Console.ReadKey();
                    goto konfirmasi;
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("INPUTAN ANDA TIDAK VALID!\nSILAHKAN KONFIRMASI ULANG");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Tekan Enter untuk melanjutkan");
                Console.ReadKey();
                goto konfirmasi;
            }

            page.Clear();
            Console.WriteLine("Pilih metode pembayaran:");
            page.CreatePMList(paymentMethod);
            int pmId = int.Parse(page.AskInput("Masukkan pilihan", paymentMethod.Length)) - 1;
            Handler.HandlePayment(paymentMethod[pmId], orderItem);


            // JQ: Bikin output pesanan (lanjutan kodingan ini)
            // FGS: Input Menu di `Menu.cs` (sekarang itu asal2an)
            // C: Bikin fitur catatan hehehe

            Console.ReadLine();
            Console.ReadKey();
        }
    }
}
