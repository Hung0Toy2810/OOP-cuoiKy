package com.example;
import java.math.BigDecimal;

public class Product {
    private int maSP;
    private String tenSP;
    private BigDecimal donGia;
    private String tinhTrang;
    private String hinhAnh;

    public Product(int maSP, String tenSP, BigDecimal donGia, String tinhTrang, String hinhAnh) {
        this.maSP = maSP;
        this.tenSP = tenSP;
        this.donGia = donGia;
        this.tinhTrang = tinhTrang;
        this.hinhAnh = hinhAnh;
    }

    public int getMaSP() { return maSP; }
    public String getTenSP() { return tenSP; }
    public BigDecimal getDonGia() { return donGia; }
    public String getTinhTrang() { return tinhTrang; }
    public String getHinhAnh() { return hinhAnh; }

    @Override
    public String toString() {
        return "Product{" + "maSP=" + maSP + ", tenSP='" + tenSP + '\'' + ", donGia=" + donGia + ", tinhTrang='" + tinhTrang + '\'' + ", hinhAnh='" + hinhAnh + '\'' + '}';
    }
}


