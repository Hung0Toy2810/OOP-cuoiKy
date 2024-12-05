package com.example;
import java.math.BigDecimal;

public class Promotion {
    private int maKM;
    private String thoiGianHL;
    private String dieuKien;
    private BigDecimal mucGiamGia;

    public Promotion(int maKM, String thoiGianHL, String dieuKien, BigDecimal mucGiamGia) {
        this.maKM = maKM;
        this.thoiGianHL = thoiGianHL;
        this.dieuKien = dieuKien;
        this.mucGiamGia = mucGiamGia;
    }

    public int getMaKM() { return maKM; }
    public String getThoiGianHL() { return thoiGianHL; }
    public String getDieuKien() { return dieuKien; }
    public BigDecimal getMucGiamGia() { return mucGiamGia; }

    @Override
    public String toString() {
        return "Promotion{" + "maKM=" + maKM + ", thoiGianHL='" + thoiGianHL + '\'' + ", dieuKien='" + dieuKien + '\'' + ", mucGiamGia=" + mucGiamGia + '}';
    }
}



