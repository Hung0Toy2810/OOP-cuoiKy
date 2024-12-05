package com.example;
import java.math.BigDecimal;

public class Invoice {
    private int maHD;
    private int maKH;
    private int maNV;
    private String thoiGianTT;
    private BigDecimal tongTien;
    private String hinhThucTT;
    private BigDecimal soTienNhan;

    public Invoice(int maHD, int maKH, int maNV, String thoiGianTT, BigDecimal tongTien, String hinhThucTT, BigDecimal soTienNhan) {
        this.maHD = maHD;
        this.maKH = maKH;
        this.maNV = maNV;
        this.thoiGianTT = thoiGianTT;
        this.tongTien = tongTien;
        this.hinhThucTT = hinhThucTT;
        this.soTienNhan = soTienNhan;
    }

    public int getMaHD() { return maHD; }
    public int getMaKH() { return maKH; }
    public int getMaNV() { return maNV; }
    public String getThoiGianTT() { return thoiGianTT; }
    public BigDecimal getTongTien() { return tongTien; }
    public String getHinhThucTT() { return hinhThucTT; }
    public BigDecimal getSoTienNhan() { return soTienNhan; }

    @Override
    public String toString() {
        return "Invoice{" + "maHD=" + maHD + ", maKH=" + maKH + ", maNV=" + maNV + ", thoiGianTT='" + thoiGianTT + '\'' + ", tongTien=" + tongTien + ", hinhThucTT='" + hinhThucTT + '\'' + ", soTienNhan=" + soTienNhan + '}';
    }
}


