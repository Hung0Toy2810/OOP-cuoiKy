//mvn exec:java
package com.example;
import java.math.BigDecimal;
import java.util.*;
import java.util.stream.Collectors;

public class App {

    public static void main(String[] args) {
        // Khởi tạo dữ liệu mẫu mở rộng
        List<Customer> customers = Arrays.asList(
            new Customer(1, "Nguyen A", "1234567890", 1500),
            new Customer(2, "Tran B", "0987654321", 500),
            new Customer(3, "Hoang C", "0971234567", 1200),
            new Customer(4, "Pham D", "0967654321", 800)
        );

        List<Employee> employees = Arrays.asList(
            new Employee(1, "Le C", "Manager", "0988888888", "2019-05-01"),
            new Employee(2, "Pham D", "Staff", "0977777777", "2020-01-01"),
            new Employee(3, "Vu E", "Staff", "0909123456", "2021-03-15"),
            new Employee(4, "Nguyen F", "Manager", "0912345678", "2018-09-10")
        );

        List<Product> products = Arrays.asList(
            new Product(1, "Product A", new BigDecimal("100"), "Active", "imageA.jpg"),
            new Product(2, "Product B", new BigDecimal("200"), "Inactive", "imageB.jpg"),
            new Product(3, "Product C", new BigDecimal("150"), "Active", "imageC.jpg"),
            new Product(4, "Product D", new BigDecimal("250"), "Active", "imageD.jpg"),
            new Product(5, "Product E", new BigDecimal("300"), "Inactive", "imageE.jpg")
        );

        List<Invoice> invoices = Arrays.asList(
            new Invoice(1, 1, 1, "2024-12-05", new BigDecimal("500"), "Cash", new BigDecimal("500")),
            new Invoice(2, 2, 2, "2024-12-06", new BigDecimal("1000"), "Card", new BigDecimal("1000")),
            new Invoice(3, 3, 3, "2024-12-07", new BigDecimal("450"), "Cash", new BigDecimal("450")),
            new Invoice(4, 4, 1, "2024-12-08", new BigDecimal("750"), "Card", new BigDecimal("750"))
        );

        List<InvoiceDetail> invoiceDetailsList = Arrays.asList(
            new InvoiceDetail(1, 1, 2, new BigDecimal("200")),
            new InvoiceDetail(1, 2, 1, new BigDecimal("300")),
            new InvoiceDetail(2, 1, 3, new BigDecimal("300")),
            new InvoiceDetail(2, 3, 2, new BigDecimal("700")),
            new InvoiceDetail(3, 3, 1, new BigDecimal("150")),
            new InvoiceDetail(3, 4, 2, new BigDecimal("300")),
            new InvoiceDetail(4, 4, 3, new BigDecimal("400")),
            new InvoiceDetail(4, 5, 1, new BigDecimal("350"))
        );

        List<Promotion> promotions = Arrays.asList(
            new Promotion(1, "2024-12-01", "Mua 1 tặng 1 miễn phí", new BigDecimal("20")),
            new Promotion(2, "2024-12-01", "Giảm giá 10%", new BigDecimal("10")),
            new Promotion(3, "2024-12-10", "Giảm giá 50% cho sản phẩm C", new BigDecimal("50")),
            new Promotion(4, "2024-12-15", "Giảm giá 30% khi thanh toán bằng thẻ", new BigDecimal("30"))
        );


        // 1. Lấy tất cả khách hàng
        System.out.println("Tất cả khách hàng:");
        customers.forEach(System.out::println);

        // 2. Lấy khách hàng có điểm tích lũy trên 1000
        System.out.println("\nKhách hàng có điểm tích lũy trên 1000:");
        customers.stream()
            .filter(c -> c.getDiemTichLuy() > 1000)
            .forEach(System.out::println);

        // 3. Lấy nhân viên có chức vụ là 'Manager'
        System.out.println("\nNhân viên có chức vụ 'Manager':");
        employees.stream()
            .filter(e -> e.getChucVu().equalsIgnoreCase("Manager"))
            .forEach(System.out::println);

        // 4. Lấy sản phẩm có giá trên 100
        System.out.println("\nSản phẩm có giá trên 100:");
        products.stream()
            .filter(p -> p.getDonGia().compareTo(new BigDecimal("100")) > 0)
            .forEach(System.out::println);

        // 5. Tìm khách hàng theo số điện thoại
        String phone = "1234567890";
        System.out.println("\nKhách hàng có số điện thoại " + phone + ":");
        customers.stream()
            .filter(c -> c.getSdt().equals(phone))
            .findFirst()
            .ifPresent(System.out::println);

        // 6. Lấy hóa đơn của một khách hàng cụ thể
        int customerId = 1;
        System.out.println("\nHóa đơn của khách hàng " + customerId + ":");
        invoices.stream()
            .filter(i -> i.getMaKH() == customerId)
            .forEach(System.out::println);

        // 7. Tìm tổng số tiền của mỗi hóa đơn
        System.out.println("\nTổng số tiền của mỗi hóa đơn:");
        invoices.stream()
            .collect(Collectors.toMap(Invoice::getMaHD, Invoice::getTongTien))
            .forEach((key, value) -> System.out.println("Hóa đơn " + key + ": " + value));

        // 8. Lấy tất cả chi tiết hóa đơn của một hóa đơn cụ thể
        int invoiceId = 1;
        System.out.println("\nChi tiết hóa đơn cho hóa đơn " + invoiceId + ":");
        invoiceDetailsList.stream()
            .filter(id -> id.getMaHD() == invoiceId)
            .forEach(System.out::println);

        // 9. Lấy sản phẩm có trong một hóa đơn cụ thể
        System.out.println("\nSản phẩm trong hóa đơn " + invoiceId + ":");
        invoiceDetailsList.stream()
            .filter(id -> id.getMaHD() == invoiceId)
            .map(id -> products.stream().filter(p -> p.getMaSP() == id.getMaSP()).findFirst().orElse(null))
            .forEach(System.out::println);

        // 10. Lấy nhân viên có ngày bắt đầu sau một ngày nhất định
        String startDate = "2020-01-01";
        System.out.println("\nNhân viên bắt đầu sau ngày " + startDate + ":");
        employees.stream()
            .filter(e -> e.getNgayBatDau().compareTo(startDate) > 0)
            .forEach(System.out::println);

        // 11. Lấy tất cả các chương trình khuyến mãi có mức giảm giá trên 10
        System.out.println("\nChương trình khuyến mãi có mức giảm giá trên 10:");
        promotions.stream()
            .filter(p -> p.getMucGiamGia().compareTo(new BigDecimal("10")) > 0)
            .forEach(System.out::println);

        // 12. Lấy sản phẩm đắt nhất
        System.out.println("\nSản phẩm đắt nhất:");
        products.stream()
            .max(Comparator.comparing(Product::getDonGia))
            .ifPresent(System.out::println);

        // 13. Lấy tổng số sản phẩm đã bán cho một sản phẩm cụ thể
        int productId = 1;
        System.out.println("\nTổng số sản phẩm đã bán cho sản phẩm " + productId + ":");
        int totalItemsSold = invoiceDetailsList.stream()
            .filter(id -> id.getMaSP() == productId)
            .mapToInt(InvoiceDetail::getSoLuong)
            .sum();
        System.out.println(totalItemsSold);

        // 14. Tìm tất cả sản phẩm đang hoạt động
        System.out.println("\nSản phẩm đang hoạt động:");
        products.stream()
            .filter(p -> p.getTinhTrang().equalsIgnoreCase("Active"))
            .forEach(System.out::println);

        // 15. Lấy tất cả hóa đơn có tổng số tiền trên 500
        System.out.println("\nHóa đơn có tổng số tiền trên 500:");
        invoices.stream()
            .filter(i -> i.getTongTien().compareTo(new BigDecimal("500")) > 0)
            .forEach(System.out::println);
        // 16. Lấy khách hàng đủ điều kiện tham gia khuyến mãi
        System.out.println("\nKhách hàng đủ điều kiện tham gia khuyến mãi:");

        // Lọc trước các chương trình khuyến mãi có mức giảm giá trên 10
        BigDecimal discountThreshold = new BigDecimal("10");
        List<Promotion> eligiblePromotions = promotions.stream()
            .filter(p -> p.getMucGiamGia().compareTo(discountThreshold) > 0)
            .collect(Collectors.toList());
        
        // Lọc khách hàng đủ điều kiện tham gia khuyến mãi
        customers.stream()
            .filter(c -> !eligiblePromotions.isEmpty()) // Kiểm tra xem có chương trình khuyến mãi nào đủ điều kiện không
            .forEach(System.out::println);
            
        // 17. Lấy nhân viên đã xử lý hóa đơn của một khách hàng cụ thể
        System.out.println("\nNhân viên đã xử lý hóa đơn cho khách hàng " + customerId + ":");
        invoices.stream()
            .filter(i -> i.getMaKH() == customerId)
            .map(i -> employees.stream().filter(e -> e.getMaNV() == i.getMaNV()).findFirst().orElse(null))
            .forEach(System.out::println);

        // 18. Lấy tất cả sản phẩm đã có trong bất kỳ hóa đơn nào
        System.out.println("\nSản phẩm đã có trong bất kỳ hóa đơn nào:");
        invoiceDetailsList.stream()
            .map(id -> products.stream().filter(p -> p.getMaSP() == id.getMaSP()).findFirst().orElse(null))
            .distinct()
            .forEach(System.out::println);

        // 19. Lấy tổng doanh thu theo sản phẩm
        System.out.println("\nTổng doanh thu theo sản phẩm:");
        invoiceDetailsList.stream()
            .collect(Collectors.groupingBy(id -> products.stream().filter(p -> p.getMaSP() == id.getMaSP()).findFirst().orElse(null),
                    Collectors.mapping(InvoiceDetail::getThanhTien, Collectors.reducing(BigDecimal.ZERO, BigDecimal::add))))
            .forEach((product, total) -> System.out.println(product.getTenSP() + ": " + total));

        // 20. Lấy sản phẩm bán chạy nhất dựa trên tổng doanh thu
        System.out.println("\nSản phẩm bán chạy nhất dựa trên tổng doanh thu:");
        invoiceDetailsList.stream()
            .collect(Collectors.groupingBy(
                id -> products.stream().filter(p -> p.getMaSP() == id.getMaSP()).findFirst().orElse(null),
                Collectors.reducing(BigDecimal.ZERO, InvoiceDetail::getThanhTien, BigDecimal::add)))
            .entrySet().stream()
            .max(Map.Entry.comparingByValue())
            .map(Map.Entry::getKey)
            .ifPresent(System.out::println);
    }
}

