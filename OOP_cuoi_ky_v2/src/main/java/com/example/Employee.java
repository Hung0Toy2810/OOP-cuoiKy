package com.example;

public class Employee {
    private int maNV;
    private String hoTen;
    private String chucVu;
    private String sdt;
    private String ngayBatDau;

    public Employee(int maNV, String hoTen, String chucVu, String sdt, String ngayBatDau) {
        this.maNV = maNV;
        this.hoTen = hoTen;
        this.chucVu = chucVu;
        this.sdt = sdt;
        this.ngayBatDau = ngayBatDau;
    }

    public int getMaNV() { return maNV; }
    public String getHoTen() { return hoTen; }
    public String getChucVu() { return chucVu; }
    public String getSdt() { return sdt; }
    public String getNgayBatDau() { return ngayBatDau; }

    @Override
    public String toString() {
        return "Employee{" + "maNV=" + maNV + ", hoTen='" + hoTen + '\'' + ", chucVu='" + chucVu + '\'' + ", sdt='" + sdt + '\'' + ", ngayBatDau='" + ngayBatDau + '\'' + '}';
    }
}

