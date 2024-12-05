package com.example;


public class Customer {
    private int maKH;
    private String hoTen;
    private String sdt;
    private int diemTichLuy;

    public Customer(int maKH, String hoTen, String sdt, int diemTichLuy) {
        this.maKH = maKH;
        this.hoTen = hoTen;
        this.sdt = sdt;
        this.diemTichLuy = diemTichLuy;
    }

    public int getMaKH() { return maKH; }
    public String getHoTen() { return hoTen; }
    public String getSdt() { return sdt; }
    public int getDiemTichLuy() { return diemTichLuy; }

    @Override
    public String toString() {
        return "Customer{" + "maKH=" + maKH + ", hoTen='" + hoTen + '\'' + ", sdt='" + sdt + '\'' + ", diemTichLuy=" + diemTichLuy + '}';
    }
}


