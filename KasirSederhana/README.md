# Simple Cashier (WPF)

## 1. Deskripsi

Simple Cashier adalah aplikasi kasir sederhana untuk mencatat pembelian. Kasir memilih produk, mengisi jumlah, dan menambahkannya ke keranjang. Aplikasi menghitung subtotal, diskon member, dan total secara otomatis. Pembayaran bisa dilakukan secara **Tunai** (dengan perhitungan kembalian) atau **QRIS**.

Aplikasi ini adalah pengembangan dari project latihan *Student Registration App* pada Hands-On Lab WPF.

## 2. Tools & Teknologi

- Visual Studio (workload *.NET desktop development*)
- C#
- WPF pada .NET 8
- XAML

## 3. Fitur Aplikasi

- Memilih produk dari daftar, harga satuan tampil otomatis.
- Menambah item ke keranjang. Jika produk yang sama ditambahkan lagi, jumlahnya digabung.
- Menghapus item dari keranjang dengan konfirmasi **Yes/No**.
- Diskon **member 10%** lewat CheckBox.
- Ringkasan otomatis: jumlah item, subtotal, diskon, dan total.
- Pembayaran **Tunai** (validasi uang cukup, tampil kembalian) atau **QRIS**.
- Struk pembayaran dalam bentuk dialog, lalu keranjang dikosongkan untuk transaksi baru.
- Tombol **Reset** untuk mengosongkan input produk dan jumlah.

Daftar produk contoh:

| Produk | Harga |
|---|---|
| Nasi Goreng | Rp 20.000 |
| Mie Ayam | Rp 15.000 |
| Es Teh | Rp 5.000 |
| Kopi Susu | Rp 12.000 |
| Roti Bakar | Rp 10.000 |

## 4. Pemenuhan Requirement

| Requirement | Pemenuhan |
|---|---|
| Minimal 5 jenis control | `TextBlock`, `TextBox`, `ComboBox`, `RadioButton`, `CheckBox`, `Button`, `ListBox` (7 jenis) |
| Minimal 3 event | `Click` (Tambah, Reset, Hapus, Bayar), `SelectionChanged` (ComboBox), `Checked/Unchecked` (CheckBox dan RadioButton) |
| Validasi input | Produk wajib dipilih, jumlah harus angka bulat lebih dari 0, keranjang tidak boleh kosong, metode bayar wajib dipilih, uang tunai harus angka dan mencukupi |
| Operasi tambah/hapus | `BtnTambah_Click` dan `BtnHapus_Click` |
| Tampilan data | `ListBox` keranjang dan ringkasan jumlah item, subtotal, diskon, total |

## 5. Penjelasan Implementasi

### 5.1 Tata Letak (XAML)

Window dibagi menjadi dua kolom dengan `Grid`:

| Kolom | Isi |
|---|---|
| Kiri | Form input: pilih produk, harga satuan, jumlah, tombol Tambah dan Reset |
| Kanan | Keranjang (`ListBox`), tombol Hapus Item, CheckBox member, ringkasan harga, metode pembayaran, input uang diterima, tombol Bayar |

### 5.2 Model Data

```csharp
public class Produk
{
    public string Nama { get; set; }
    public decimal Harga { get; set; }
}

public class CartItem
{
    public string Nama { get; set; }
    public decimal Harga { get; set; }
    public int Qty { get; set; }
    public decimal Subtotal => Harga * Qty;
}
```

Keranjang disimpan dalam `ObservableCollection<CartItem>` yang dipasang sebagai `ItemsSource` pada `ListBox`, sehingga tampilan ikut berubah saat data ditambah atau dihapus.

### 5.3 Event Handler

| Event | Method | Fungsi |
|---|---|---|
| `SelectionChanged` ComboBox | `CmbProduk_SelectionChanged` | Menampilkan harga satuan produk terpilih |
| `Click` Tambah | `BtnTambah_Click` | Validasi, lalu tambah atau gabungkan item di keranjang |
| `Click` Reset | `BtnReset_Click` | Mengosongkan pilihan produk dan jumlah |
| `Click` Hapus Item | `BtnHapus_Click` | Konfirmasi, lalu hapus item terpilih |
| `Checked/Unchecked` CheckBox | `ChkMember_Changed` | Menghitung ulang diskon dan total |
| `Checked` RadioButton | `Metode_Changed` | Mengaktifkan atau menonaktifkan input uang tunai |
| `Click` Bayar | `BtnBayar_Click` | Validasi pembayaran, tampilkan struk, kosongkan keranjang |

### 5.4 Validasi Input

Pola yang dipakai: cek input, jika salah tampilkan `MessageBox`, fokuskan control, lalu `return`.

```csharp
if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
{
    MessageBox.Show("Jumlah harus berupa angka bulat lebih dari 0!", "Validasi",
        MessageBoxButton.OK, MessageBoxImage.Warning);
    txtQty.Focus();
    txtQty.SelectAll();
    return;
}
```

### 5.5 Perhitungan Total

```csharp
Subtotal = jumlah dari (Harga × Qty) seluruh item
Diskon   = Subtotal × 10%   (jika member dicentang)
Total    = Subtotal − Diskon
Kembalian = Uang diterima − Total   (khusus tunai)
```

Method `UpdateTotal()` dipanggil setiap keranjang atau status member berubah agar ringkasan selalu sinkron.

---

## 6. Dokumentasi Tampilan (Screenshot)

### 6.1 Tampilan Awal

<img width="1131" height="860" alt="image" src="https://github.com/user-attachments/assets/7246a393-1826-4a6c-b9a9-319dc548654f" />

_Keterangan: tampilan aplikasi saat pertama kali dibuka dengan keranjang kosong._

### 6.2 Memilih Produk

<img width="1130" height="861" alt="image" src="https://github.com/user-attachments/assets/0f6df8b3-f445-4409-864d-e3be8ff39994" />

_Keterangan: produk dipilih dari ComboBox dan harga satuan tampil otomatis._

### 6.3 Item Ditambahkan ke Keranjang

<img width="1131" height="861" alt="image" src="https://github.com/user-attachments/assets/3c90f0b1-d1cc-4e7b-81f6-c678b5df2ece" />

_Keterangan: beberapa item masuk ke keranjang dan total terhitung otomatis._

### 6.4 Diskon Member

<img width="1130" height="857" alt="image" src="https://github.com/user-attachments/assets/2b36fbad-ff13-4d5d-9ff4-dc3b24f84acb" />

_Keterangan: CheckBox member dicentang, diskon 10% terhitung._

### 6.5 Konfirmasi Hapus Item

<img width="1127" height="857" alt="image" src="https://github.com/user-attachments/assets/1615af6f-e373-481b-a641-72e17083e194" />

_Keterangan: dialog konfirmasi sebelum item dihapus dari keranjang._

### 6.6 Validasi Input

<img width="1126" height="862" alt="image" src="https://github.com/user-attachments/assets/0b5141cf-44e3-4dda-9202-6275e90a371e" />

_Keterangan: pesan validasi saat uang tunai kurang dari total._

### 6.7 Pembayaran Tunai

<img width="1130" height="862" alt="image" src="https://github.com/user-attachments/assets/7ade4567-38fd-46c2-989e-f55278c508bf" />

_Keterangan: struk pembayaran tunai beserta kembalian._

### 6.8 Pembayaran QRIS

<img width="1131" height="861" alt="image" src="https://github.com/user-attachments/assets/cf3adbac-9630-4147-9b69-228482932a75" />

_Keterangan: metode QRIS dipilih, kolom uang diterima nonaktif, pembayaran berhasil._

---

## 7. Kesimpulan

Aplikasi Simple Cashier berhasil dibuat dengan WPF dan memenuhi seluruh requirement: lebih dari 5 jenis control, lebih dari 3 event, validasi input, operasi tambah dan hapus, serta tampilan data. Praktikum ini menunjukkan alur pengembangan aplikasi desktop dari desain UI di XAML, penanganan event, logic C#, hingga validasi dan perhitungan transaksi.
