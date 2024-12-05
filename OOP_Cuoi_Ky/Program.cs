using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Concurrent;

namespace FastFoodManagement
{
    // Lớp phát đi sự kiện giáng sinh
    class ChristmasEventScheduler
    {
        private static System.Timers.Timer timer;//Đối tượng Timer từ thư viện System.Timers
        private DateTime targetTime; 

        public event EventHandler ChristmasEventTriggered;  

        public void Start()
        {
            timer = new System.Timers.Timer(1000); //chu kì 1000ms sẽ thực hiện checktime 1 lần
            timer.Elapsed += CheckTime;// đăng kí phương thức checktime
            timer.AutoReset = true; 
            timer.Start();
            Console.WriteLine("Timer bắt đầu...");

            
            targetTime = DateTime.Now.AddMinutes(2); // thời điểm sự kiện kích hoạt
        }

        private void CheckTime(object source, ElapsedEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            
            
            if (currentTime >= targetTime)//Nếu đến hoặc vượt thời điểm kích hoạt sự kiện thì kích hoạt sự kiện
            {
                OnChristmasEventTriggered(EventArgs.Empty); //kích hoạt 
                timer.Stop();  // Dừng timer sau khi sự kiện đã được kích hoạt
            }
        }

        protected virtual void OnChristmasEventTriggered(EventArgs e)//phương thức phát đi sự kiện
        {
            ChristmasEventTriggered?.Invoke(this, e); // Kích hoạt sự kiện
            Console.WriteLine("Christmas event đã kích hoạt vào lúc: " + DateTime.Now);
        }
    }
    public class InvalidNumberException : Exception
    {
        public InvalidNumberException(string message) : base(message) { }
    }

    // Customer
    public class Customer
    {
        [Key]
        public int MAKH { get; set; }
        public string HoTen { get; set; }
        public string SDT { get; set; }
        public decimal DiemTichLuy { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Promotion> Promotions { get; set; } 
    }

    // Employee
    public class Employee
    {
        [Key]
        public int MANV { get; set; }
        public string HoTen { get; set; }
        public string ChucVu { get; set; }
        public string SDT { get; set; }
        public DateTime NgayBatDau { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }

    // Product
    public class Product
    {
        [Key]
        public int MASP { get; set; }
        public string TenSP { get; set; }
        public decimal DonGia { get; set; }
        public ProductStatus TinhTrang { get; set; }  
        public string HinhAnh { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }

    // Invoice
    public class Invoice
    {
        [Key]
        public int MAHD { get; set; }
        public int MAKH { get; set; }
        public int MANV { get; set; }
        public DateTime ThoiGianTT { get; set; }
        public decimal TongTien { get; set; }
        public string HinhThucTT { get; set; }
        public decimal SoTienNhan { get; set; }
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }

    // InvoiceDetail
    public class InvoiceDetail
    {
        public int MAHD { get; set; }
        public int MASP { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }
        public Invoice Invoice { get; set; }
        public Product Product { get; set; }
    }

    // Promotion
    public class Promotion
    {
        [Key]
        public int MAKM { get; set; }
        public DateTime ThoiGianHL { get; set; }
        public string DieuKien { get; set; }
        public decimal MucGiamGia { get; set; }
        public int MAKH { get; set; } 
        public bool DaDung { get; set; } 
        public Customer Customer { get; set; }
        
    }

    public enum ProductStatus
    {
        Available,  // Có sẵn
        OutOfStock  // Hết hàng
    }

    public class FastFoodContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Promotion> Promotions { get; set; }

        // Kết nối đến SQL server
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=FastFoodManagement;User Id=sa;Password=YourPassword123;TrustServerCertificate=true;");
        }

        // Cấu hình các mối quan hệ và ràng buộc
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thiết lập khoá chính
            modelBuilder.Entity<Customer>().HasKey(c => c.MAKH);
            modelBuilder.Entity<Employee>().HasKey(e => e.MANV);
            modelBuilder.Entity<Product>().HasKey(p => p.MASP);
            modelBuilder.Entity<Invoice>().HasKey(i => i.MAHD);
            modelBuilder.Entity<Promotion>().HasKey(p => p.MAKM);
            modelBuilder.Entity<InvoiceDetail>().HasKey(id => new { id.MAHD, id.MASP });

            // Thiết lập các mối quan hệ (1-nhiều)
            //(1-nhiều)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.MAKH)
                .OnDelete(DeleteBehavior.Restrict);  
            //(1-nhiều)
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Invoices)
                .WithOne(i => i.Employee)
                .HasForeignKey(i => i.MANV)
                .OnDelete(DeleteBehavior.Restrict);
            //(1-nhiều)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.InvoiceDetails)
                .WithOne(id => id.Product)
                .HasForeignKey(id => id.MASP)
                .OnDelete(DeleteBehavior.Restrict);
            //(1-nhiều)
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.InvoiceDetails)
                .WithOne(id => id.Invoice)
                .HasForeignKey(id => id.MAHD)
                .OnDelete(DeleteBehavior.Cascade);
            //(1-nhiều)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Promotions)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.MAKH)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    public class QueryPrinter
    {
        public static void PrintQuery<T>(IEnumerable<T> queryResults)
        {
            if (!queryResults.Any())
            {
                Console.WriteLine("Không có kết quả.");
                return;
            }
            var properties = typeof(T).GetProperties();
            if (!properties.Any())
            {
                Console.WriteLine("Không thể hiển thị kết quả cho kiểu này.");
                return;
            }
            foreach (var prop in properties)
            {
                Console.Write($"{prop.Name,-20}"); 
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', properties.Length * 20));
            foreach (var item in queryResults)
            {
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item) ?? "NULL";
                    Console.Write($"{value,-20}");
                }
                Console.WriteLine();
            }
        }
    }
    public class Program
    {
        static bool isProcessingEvent = false; // Cờ hiệu để kiểm tra xem sự kiện có đang được xử lí hay không (Việc này ở dữ liệu nhỏ thì gần như tức thời nhưng dữ liệu lớn sẽ tốn thời gian)
        static bool isOrderInProgress = false; // Cờ hiệu để theo dõi xem quá trình đặt hàng có đang được diễn ra hay không
        static object lockObject = new object(); // Đối tượng khóa để đồng bộ hóa các luồng (threads).
        static ConcurrentQueue<Action> eventQueue = new ConcurrentQueue<Action>(); // Queue để chứa các hành động 
        public static void Main(string[] args)
        {           
            using (var context = new FastFoodContext())
            {
                //Truy_Van_DB(context);
                RunProgram(context);
            }
        }
        public static void Truy_Van_DB(FastFoodContext context)
        {
                // 1. In tất cả khách hàng
            Console.WriteLine("1. Tất cả khách hàng:");
            QueryPrinter.PrintQuery(context.Customers.ToList());
            Console.WriteLine();

            // 2. Tìm khách hàng theo ID (Khách hàng có MAKH == 1)
            Console.WriteLine("2. Khách hàng có ID 1:");
            QueryPrinter.PrintQuery(context.Customers.Where(c => c.MAKH == 1).ToList());
            Console.WriteLine();

            // 3. Lấy tất cả nhân viên là quản lý
            Console.WriteLine("3. Nhân viên là quản lý:");
            QueryPrinter.PrintQuery(context.Employees.Where(e => e.ChucVu == "Quan ly").ToList());
            Console.WriteLine();

            // 4. Danh sách sản phẩm hết hàng
            Console.WriteLine("4. Sản phẩm hết hàng:");
            QueryPrinter.PrintQuery(context.Products.Where(p => p.TinhTrang == ProductStatus.OutOfStock).ToList());
            Console.WriteLine();

            // 5. Lấy tất cả hóa đơn của một khách hàng cụ thể
            Console.WriteLine("5. Tất cả hóa đơn của khách hàng có MAKH = 1:");
            QueryPrinter.PrintQuery(context.Invoices.Where(i => i.MAKH == 1).ToList());
            Console.WriteLine();

            // 6. Tính tổng số lượng sản phẩm đã bán
            Console.WriteLine("6. Tổng số lượng sản phẩm đã bán:");
            var totalProductsSold = context.InvoiceDetails.Sum(id => id.SoLuong);
            Console.WriteLine(totalProductsSold);
            Console.WriteLine();

            // 7. Lấy khách hàng có điểm tích lũy trên 1000
            Console.WriteLine("7. Khách hàng có điểm tích lũy trên 1000:");
            QueryPrinter.PrintQuery(context.Customers.Where(c => c.DiemTichLuy > 1000).ToList());
            Console.WriteLine();

            // 8. Danh sách các chương trình khuyến mãi đang có hiệu lực
            Console.WriteLine("8. Chương trình khuyến mãi đang có hiệu lực:");
            QueryPrinter.PrintQuery(context.Promotions.Where(p => p.ThoiGianHL > DateTime.Now && !p.DaDung).ToList());
            Console.WriteLine();

            // 9. Lấy 5 sản phẩm đắt nhất
            Console.WriteLine("9. 5 sản phẩm đắt nhất:");
            QueryPrinter.PrintQuery(context.Products.OrderByDescending(p => p.DonGia).Take(5).ToList());
            Console.WriteLine();

            // 10. Tính tổng doanh thu từ tất cả các hóa đơn
            Console.WriteLine("10. Tổng doanh thu từ tất cả các hóa đơn:");
            var totalRevenue = context.Invoices.Sum(i => i.TongTien);
            Console.WriteLine(totalRevenue);
            Console.WriteLine();

            // 11. Tìm khách hàng chi tiêu nhiều nhất
            Console.WriteLine("11. Khách hàng chi tiêu nhiều nhất:");
            var topSpender = context.Customers
                .OrderByDescending(c => c.Invoices.Sum(i => i.TongTien))
                .Select(c => new { c.HoTen, TotalSpent = c.Invoices.Sum(i => i.TongTien) })
                .Take(1)
                .ToList();
            QueryPrinter.PrintQuery(topSpender);
            Console.WriteLine();

            // 12. Lấy các nhân viên chưa xử lý bất kỳ hóa đơn nào
            Console.WriteLine("12. Nhân viên chưa xử lý hóa đơn nào:");
            QueryPrinter.PrintQuery(context.Employees.Where(e => !e.Invoices.Any()).ToList());
            Console.WriteLine();

            // 13. Tìm sản phẩm có doanh thu cao nhất
            Console.WriteLine("13. Sản phẩm có doanh thu cao nhất:");
            var topProduct = context.Products
                .OrderByDescending(p => p.InvoiceDetails.Sum(id => id.ThanhTien))
                .FirstOrDefault();
            QueryPrinter.PrintQuery(new List<Product> { topProduct });
            Console.WriteLine();

            // 14. Tính giá trị trung bình của các đơn hàng
            Console.WriteLine("14. Giá trị trung bình của các đơn hàng:");
            var averageOrderValue = context.Invoices.Average(i => i.TongTien);
            Console.WriteLine(averageOrderValue);
            Console.WriteLine();

            // 15. Lấy khách hàng và số tiền họ đã chi tiêu
            Console.WriteLine("15. Khách hàng và số tiền họ đã chi tiêu:");
            var customerSpending = context.Customers
                .Select(c => new 
                {
                    CustomerName = c.HoTen,
                    TotalSpent = c.Invoices.Sum(i => i.TongTien)
                })
                .ToList();
            QueryPrinter.PrintQuery(customerSpending);
            Console.WriteLine();

            // 16. Lấy hóa đơn gần nhất của một khách hàng
            Console.WriteLine("16. Hóa đơn gần nhất của khách hàng có MAKH = 1:");
            var recentInvoice = context.Invoices
                .Where(i => i.MAKH == 1)
                .OrderByDescending(i => i.ThoiGianTT)
                .FirstOrDefault();
            QueryPrinter.PrintQuery(new List<Invoice> { recentInvoice });
            Console.WriteLine();

            // 17. Lấy nhân viên và số lượng hóa đơn họ đã xử lý
            Console.WriteLine("17. Nhân viên và số lượng hóa đơn họ đã xử lý:");
            var employeeInvoices = context.Employees
                .Select(e => new 
                {
                    EmployeeName = e.HoTen,
                    InvoiceCount = e.Invoices.Count
                })
                .ToList();
            QueryPrinter.PrintQuery(employeeInvoices);
            Console.WriteLine();

            // 18. Lấy chi tiết hóa đơn, bao gồm sản phẩm và số lượng
            Console.WriteLine("18. Chi tiết hóa đơn cho MAHD = 1:");
            var invoiceDetails = context.InvoiceDetails
                .Where(id => id.MAHD == 1)
                .Select(id => new 
                {
                    ProductName = id.Product.TenSP,
                    Quantity = id.SoLuong,
                    TotalPrice = id.ThanhTien
                })
                .ToList();
            QueryPrinter.PrintQuery(invoiceDetails);
            Console.WriteLine();

            // 19. Tìm khách hàng thường xuyên nhất (theo số lượng hóa đơn)
            Console.WriteLine("19. Khách hàng thường xuyên nhất (theo số lượng hóa đơn):");
            var frequentCustomer = context.Customers
                .OrderByDescending(c => c.Invoices.Count)
                .FirstOrDefault();
            QueryPrinter.PrintQuery(new List<Customer> { frequentCustomer });
            Console.WriteLine();

            // 20. Tìm các sản phẩm có ít nhất một chi tiết hóa đơn
            Console.WriteLine("20. Sản phẩm có ít nhất một chi tiết hóa đơn:");
            QueryPrinter.PrintQuery(context.Products
                .Where(p => p.InvoiceDetails.Any())
                .ToList());
        }
        public static void RunProgram(FastFoodContext context)
        {
            // THÊM DỮ LIỆU ĐỂ CỬA HÀNG CÓ THỂ ĐI VÀO HOẠT ĐỘNG
            // Thêm dữ liệu thực đơn
            var products = new List<Product>
            {
                new Product { TenSP = "Ga ran", DonGia = 80000, TinhTrang = ProductStatus.Available, HinhAnh = "ga_ran.jpg" },
                new Product { TenSP = "Com cuon", DonGia = 70000, TinhTrang = ProductStatus.Available, HinhAnh = "com_cuon.jpg" },
                new Product { TenSP = "Nuoc ngot", DonGia = 20000, TinhTrang = ProductStatus.Available, HinhAnh = "nuoc_ngot.jpg" },
                new Product { TenSP = "Khoai tay chien", DonGia = 30000, TinhTrang = ProductStatus.Available, HinhAnh = "khoai_tay_chien.jpg" },
                new Product { TenSP = "Burger", DonGia = 50000, TinhTrang = ProductStatus.Available, HinhAnh = "burger.jpg" }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
            // Thêm dữ liệu nhân viên đang hoạt động trong cửa hàng
            var employees = new List<Employee>
            {
                new Employee { HoTen = "Nguyen Thi H", ChucVu = "Nhan vien phuc vu", SDT = "0989012345", NgayBatDau = DateTime.Now },
                new Employee { HoTen = "Tran Thi I", ChucVu = "Quan ly", SDT = "0990123456", NgayBatDau = DateTime.Now },
                new Employee { HoTen = "Le Thi J", ChucVu = "Nhan vien vien phuc vu", SDT = "0901234567", NgayBatDau = DateTime.Now },
                new Employee { HoTen = "Pham Thi K", ChucVu = "Nhan vien phuc vu", SDT = "0912345678", NgayBatDau = DateTime.Now }
            };
            context.Employees.AddRange(employees);
            context.SaveChanges();

            ChristmasEventScheduler scheduler = new ChristmasEventScheduler();//Tạo một phiên bản của ChristmasEventScheduler và đăng ký xử lý sự kiện Giáng Sinh bằng HandleChristmasEvent.
            scheduler.ChristmasEventTriggered += HandleChristmasEvent;//đăng kí
            scheduler.Start();//Khởi động timer

            
            Task.Run(() => HandleCustomerOrders(context));  //Chạy một luồng riêng (Task.Run) để xử lý đơn đặt hàng (HandleCustomerOrders).
            while (true)
            {
                lock (lockObject)//giúp đảm bảo rằng chỉ một luồng duy nhất có thể thực thi đoạn mã bên trong khối
                {
                    if (isProcessingEvent && !isOrderInProgress)//thoả
                    {
                        Console.WriteLine("Đang xử lý ưu đãi Giáng Sinh...");
                        PromotionForChristmas(context);//Gọi phương thức PromotionForChristmas(context) để áp dụng ưu đãi.
                        Console.WriteLine("Hoàn tất!");
                        isProcessingEvent = false;
                    }
                }
                if (eventQueue.TryDequeue(out var queuedEvent))//Kiểm tra xem có sự kiện nào trong hàng đợi (eventQueue) cần xử lý không.TryDequeue: Lấy sự kiện đầu tiên trong hàng đợi (nếu có) và gán cho biến queuedEvent.
                {
                    queuedEvent();
                }

                Thread.Sleep(500);
            }
        }
        public static void HandleCustomerOrders(FastFoodContext context)
        {
            while (true) // Liên tục kiểm tra và xử lý các đơn đặt hàng.
            {
                Console.WriteLine("Nhấn '1' rồi Enter để bắt đầu đặt hàng...");

                // Đợi người dùng nhập "1"
                string input = Console.ReadLine();
                if (input == "1")
                {
                    lock (lockObject)
                    {
                        if (!isProcessingEvent && !isOrderInProgress)
                        {
                            Console.WriteLine("Đang trong quá trình đặt hàng...");
                            isOrderInProgress = true;
                            HandleCustomerOrder(context); // Gọi hàm xử lý đơn hàng
                            isOrderInProgress = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Lệnh không hợp lệ. Vui lòng nhấn '1' rồi Enter để tiếp tục.");
                }

                Thread.Sleep(500); // Tránh vòng lặp chạy liên tục không cần thiết
            }
        }
        public static void HandleCustomerOrder(FastFoodContext context)
        {
            while (true)
            {
                try
                {
                    // Nhập số điện thoại khách hàng
                    Console.Write("Nhập số điện thoại khách hàng: ");
                    string phone = Console.ReadLine();
                    var customer = context.Customers.FirstOrDefault(c => c.SDT == phone);//Tìm
                    if (customer == null)//Nếu chưa tồn tại(chưa từng mua hàng) thì thêm vào
                    {
                        Console.Write("Khách hàng chưa tồn tại. Vui lòng nhập tên: ");
                        string name = Console.ReadLine();
                        customer = new Customer { HoTen = name, SDT = phone, DiemTichLuy = 0 };
                        context.Customers.Add(customer);
                        context.SaveChanges();
                    }
                    // Xác thực có nhân viên đang hoạt động hay không (nằm trong list employees)
                    List<Employee> employees = context.Employees.ToList();
                    if (employees.Count == 0)
                    {
                        Console.WriteLine("Không có nhân viên nào trong hệ thống.");
                        return;
                    }
                    // Tạo hoá đơn
                    Random random = new Random();
                    int MANV = random.Next(0, 4);
                    var invoice = new Invoice
                    {
                        MAKH = customer.MAKH,
                        MANV = employees[MANV].MANV,//random nhân viên phục vụ
                        ThoiGianTT = DateTime.Now,
                        TongTien = 0,
                        HinhThucTT = "",
                        SoTienNhan = 0,
                        InvoiceDetails = new List<InvoiceDetail>()
                    };
                    context.Invoices.Add(invoice);
                    context.SaveChanges();
                    // Xác thực có sản phẩm để phục vụ hay không
                    List<Product> products = context.Products.Where(p => p.TinhTrang == ProductStatus.Available).ToList();
                    if (products.Count == 0)
                    {
                        Console.WriteLine("Hiện tại không có sản phẩm nào.");
                        return;
                    }

                    decimal tongTien = 0;
                    //Lặp qua menu để khách hàng chọn số lượng
                    foreach (var product in products)
                    {
                        Console.WriteLine($"{product.MASP}. {product.TenSP} - {product.DonGia} VND");
                        Console.Write($"Nhập số lượng cho {product.TenSP} (tối đa 20): ");

                        bool isValid = false;

                        do
                        {
                            try
                            {
                                int quantity = int.Parse(Console.ReadLine());

                                if (quantity >= 0 && quantity <= 20)
                                {
                                    if (quantity > 0)
                                    {
                                        //Tạo chi tiết đơn hàng
                                        var invoiceDetail = new InvoiceDetail
                                        {
                                            MAHD = invoice.MAHD,
                                            MASP = product.MASP,
                                            SoLuong = quantity,
                                            ThanhTien = product.DonGia * quantity
                                        };
                                        tongTien += invoiceDetail.ThanhTien;// Tổng tiền được thêm vào
                                        invoice.InvoiceDetails.Add(invoiceDetail);
                                    }
                                    isValid = true;
                                }
                                else
                                {
                                    throw new ArgumentOutOfRangeException($"Invalid number: {quantity}. Số lượng phải >= 0 và <= 20.");
                                }
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Đầu vào không hợp lệ. Vui lòng nhập một số nguyên.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi xảy ra: {ex.Message}");
                            }
                        } while (!isValid);
                    }

                    invoice.TongTien = tongTien;//Cập nhật tổng tiền vào hoá đơn
                    customer.DiemTichLuy += (tongTien / 1000);//Cập nhật điểm tích luỹ
                    //Tạo khuyễn mãi mới nếu số điểm vượt 1000
                    var promotion = new Promotion
                    {
                        ThoiGianHL = DateTime.Now.AddDays(21),
                        DieuKien = "Còn trong thời gian khuyến mãi",
                        MucGiamGia = 1,
                        MAKH = customer.MAKH,
                        DaDung = false
                    };
                    if (customer.DiemTichLuy >= 1000 && customer.DiemTichLuy<2000)//Có thể thêm nhiều tuỳ chỉnh cho điều kiện nhận được khuyến mãi
                    {
                        context.Promotions.Add(promotion);
                        context.SaveChanges();
                        customer.DiemTichLuy -= 1000;
                    }
                    else if (customer.DiemTichLuy>=2000)
                    {
                        promotion.MucGiamGia=2;
                        context.Promotions.Add(promotion);
                        context.SaveChanges();
                        customer.DiemTichLuy -= 2000;
                    }
                    context.SaveChanges();//Lưu thay đổi

                    var activePromotion = context.Promotions.FirstOrDefault(p => p.MAKH == customer.MAKH && p.ThoiGianHL >= DateTime.Now && !p.DaDung);//Tìm kiếm khuyễn mãi dùng được
                    //Áp dụng khuyến mãi cho đơn hàng hiện tại
                    if (activePromotion != null)
                    {
                        decimal discount = 0;
                        decimal discountRate = 0;

                        if (activePromotion.MucGiamGia == 1)
                        {
                            discountRate = invoice.TongTien <= 500000 ? 0.30m : 
                                        invoice.TongTien <= 2000000 ? 0.10m : 
                                        0.05m; 
                        }
                        else if (activePromotion.MucGiamGia == 2)
                        {
                            discountRate = invoice.TongTien <= 500000 ? 0.35m : 
                                        invoice.TongTien <= 2000000 ? 0.15m : 
                                        0.10m; 
                        }
                        else if (activePromotion.MucGiamGia == 3)
                        {
                            discountRate = invoice.TongTien <= 500000 ? 0.40m : 
                                        invoice.TongTien <= 2000000 ? 0.20m : 
                                        0.15m; 
                        }
                        discount = invoice.TongTien * discountRate;
                        invoice.TongTien -= discount;

                        Console.WriteLine($"Áp dụng khuyến mãi: Số tiền giảm: {discount:N2} VND.");
                        activePromotion.DaDung = true;
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Bạn không có khuyến mãi nào.");
                    }

                    Console.WriteLine("Chọn hình thức thanh toán: 1. Tiền mặt, 2. Thẻ tín dụng, 3. Chuyển khoản");
                    string choice = Console.ReadLine();
                    invoice.HinhThucTT = choice == "1" ? "TienMat" : choice == "2" ? "TheTinDung" : "ChuyenKhoan";

                    // Thanh toán
                    invoice.SoTienNhan = invoice.TongTien;
                    context.SaveChanges();

                    Console.WriteLine($"Đặt hàng thành công! Tổng tiền: {invoice.TongTien:N2} VND.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
                }
            }
        }
        // Nhân dịp giáng sinh cửa hàng đưa ra ưu đãi tặng khuyến mãi cho khách hàng có tổng mức chi tiêu trong 3 tháng gần nhất vừa qua tại cửa hàng theo từng mức
                // (500-1tr): mức 1
                // (1tr-2tr): mức 2
                // (trên 2tr): mức 3
                // Phát hành khuyến mãi
        public static void PromotionForChristmas(FastFoodContext context)
        {
            DateTime threeMonthsAgo = DateTime.Now.AddMonths(-3);
            var customerSpendings = context.Invoices
                .Where(i => i.ThoiGianTT >= threeMonthsAgo) 
                .GroupBy(i => i.MAKH) 
                .Select(group => new
                {
                    MaKH = group.Key, 
                    TotalSpending = group.Sum(i => i.TongTien)
                })
                .ToList();
            foreach (var spending in customerSpendings)
            {
                int promotionLevel = 0;

                if (spending.TotalSpending >= 500000 && spending.TotalSpending <= 1000000)
                {
                    promotionLevel = 1;
                }
                else if (spending.TotalSpending > 1000000 && spending.TotalSpending <= 2000000)
                {
                    promotionLevel = 2;
                }
                else if (spending.TotalSpending > 2000000)
                {
                    promotionLevel = 3; 
                }
                if (promotionLevel > 0)
                {
                    var promotion = new Promotion
                    {
                        MAKH = spending.MaKH,
                        ThoiGianHL = DateTime.Now.AddMonths(1), 
                        DieuKien = "Christmas promotion chi dung duoc trong vong 1 thang ke tu ngay phat hanh ",
                        MucGiamGia = promotionLevel,
                        DaDung = false 
                    };

                    context.Promotions.Add(promotion);
                }
            }
            context.SaveChanges();
        }
        static void HandleChristmasEvent(object sender, EventArgs e)
        {
            Console.WriteLine("Sự kiện Giáng Sinh đã được kích hoạt!, Việc thêm ưu đãi sẽ được tiến hành sau khi quá trình đặt hàng kết thúc");

            if (isOrderInProgress)//Nếu đang có đơn hàng (isOrderInProgress), sự kiện được thêm vào hàng đợi để xử lý sau.
            {
                eventQueue.Enqueue(() =>
                {
                    isProcessingEvent = true;
                });
            }
            else//Nếu không, tiến hành xử lý ngay lập tức.
            {
                isProcessingEvent = true;
                Console.WriteLine("Thêm ưu đãi được thực hiện ngay lúc này.");
            }
        }
    }
}