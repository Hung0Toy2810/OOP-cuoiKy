package com.example;
import java.math.BigDecimal;

public class InvoiceDetail {
    private int maHD;
    private int maSP;
    private int soLuong;
    private BigDecimal thanhTien;

    public InvoiceDetail(int maHD, int maSP, int soLuong, BigDecimal thanhTien) {
        this.maHD = maHD;
        this.maSP = maSP;
        this.soLuong = soLuong;
        this.thanhTien = thanhTien;
    }

    public int getMaHD() { return maHD; }
    public int getMaSP() { return maSP; }
    public int getSoLuong() { return soLuong; }
    public BigDecimal getThanhTien() { return thanhTien; }

    @Override
    public String toString() {
        return "InvoiceDetail{" + "maHD=" + maHD + ", maSP=" + maSP + ", soLuong=" + soLuong + ", thanhTien=" + thanhTien + '}';
    }
}



