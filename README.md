# Windows-Presentation-Foundation

# Student Registration App (WPF)

## 1. Deskripsi

Student Registration App adalah aplikasi desktop sederhana untuk mendaftarkan data mahasiswa. Pengguna mengisi form (NIM, nama, program studi, jenis kelamin), lalu data yang disimpan ditampilkan pada daftar di sisi kanan. Aplikasi dibuat sebagai latihan memahami alur pengembangan WPF:

```
Design UI → XAML → Event → C# → Validation → CRUD
```

## 2. Tools & Teknologi

- Visual Studio (workload *.NET desktop development*)
- Bahasa pemrograman C#
- WPF (Windows Presentation Foundation) pada .NET 8
- XAML untuk desain antarmuka

## 3. Fitur Aplikasi

- **Simpan** data mahasiswa ke daftar (Create).
- **Tampil** data mahasiswa pada `ListBox` (Read).
- **Hapus** data yang dipilih dari daftar (Delete).
- **Reset** form untuk mengosongkan semua input.
- **Validasi input**: NIM, nama, program studi, dan jenis kelamin wajib diisi.

## 4. Penjelasan Implementasi

### 4.1 Tata Letak (XAML)

Window dibagi menjadi dua kolom memakai `Grid`:

| Kolom | Isi |
|---|---|
| Kiri | Form input: NIM, Nama, Program Studi, Jenis Kelamin, tombol Simpan & Reset |
| Kanan | Daftar data mahasiswa (`ListBox`) dan tombol Hapus |

Control yang digunakan:

| Control | Nama | Fungsi |
|---|---|---|
| `TextBox` | `txtNim`, `txtNama` | Input NIM dan nama |
| `ComboBox` | `cmbProdi` | Memilih program studi |
| `RadioButton` | `rbLaki`, `rbPerempuan` | Memilih jenis kelamin |
| `Button` | Simpan, Reset, Hapus | Memicu aksi |
| `ListBox` | `lstMahasiswa` | Menampilkan data tersimpan |

### 4.2 Event Handler (C#)

| Event | Method | Fungsi |
|---|---|---|
| `Click` tombol Simpan | `BtnSimpan_Click` | Validasi, lalu tambah data ke `ListBox` |
| `Click` tombol Reset | `BtnReset_Click` | Mengosongkan form |
| `Click` tombol Hapus | `BtnHapus_Click` | Menghapus item terpilih |

### 4.3 Validasi Input

Setiap input diperiksa secara berurutan. Jika tidak valid, aplikasi menampilkan `MessageBox`, memfokuskan control terkait, lalu menghentikan proses dengan `return`.

```csharp
if (string.IsNullOrWhiteSpace(txtNim.Text))
{
    MessageBox.Show("NIM harus diisi!");
    txtNim.Focus();
    return;
}
```

Validasi yang diterapkan: NIM kosong, nama kosong, program studi belum dipilih, dan jenis kelamin belum dipilih.

### 4.4 Penyimpanan & Penghapusan Data

```csharp
string data = $"{nim} | {nama} | {prodi} | {jenisKelamin}";
lstMahasiswa.Items.Add(data);
```

```csharp
if (lstMahasiswa.SelectedItem != null)
    lstMahasiswa.Items.Remove(lstMahasiswa.SelectedItem);
else
    MessageBox.Show("Pilih data yang ingin dihapus!");
```

Data pada tahap ini hanya disimpan di memori (hilang saat aplikasi ditutup).

---

## 5. Dokumentasi Tampilan (Screenshot)

### 5.1 Tampilan Awal

<img width="980" height="736" alt="Screenshot 2026-10-21 152421" src="https://github.com/user-attachments/assets/59a67b03-b50f-4358-8b55-aa70ae3d6c39" />

_Keterangan: tampilan form dan daftar data saat aplikasi pertama kali dibuka._

### 5.2 Data Berhasil Disimpan

<img width="980" height="737" alt="Screenshot 2026-10-21 152450" src="https://github.com/user-attachments/assets/9b5987cd-2c6b-42cd-bc4a-7d327f8ec2b0" />

_Keterangan: pesan "Data mahasiswa berhasil disimpan!" dan data muncul di panel DATA MAHASISWA._

### 5.3 Validasi Input

<img width="980" height="737" alt="Screenshot 2026-10-21 154629" src="https://github.com/user-attachments/assets/18146169-f165-483a-a180-10d2dbbf19f2" />

_Keterangan: pesan validasi saat program studi belum dipilih._

### 5.4 Reset Form

<img width="980" height="738" alt="Screenshot 2026-10-21 152518" src="https://github.com/user-attachments/assets/d79eb9da-df77-4078-a2f9-b8339a8faf20" />

_Keterangan: form kembali kosong setelah tombol Reset diklik._

### 5.5 Hapus Data

<img width="978" height="732" alt="Screenshot 2026-10-21 152939" src="https://github.com/user-attachments/assets/82fc3d5c-3031-4b5d-8a52-3cd6fb0fa8b6" />

_Keterangan: data terpilih berhasil dihapus dari daftar._

---

## 6. Kesimpulan

Melalui praktikum ini berhasil dibuat aplikasi desktop sederhana berbasis WPF yang menghubungkan antarmuka XAML dengan logic C# melalui event `Click`. Aplikasi mendukung operasi tambah, tampil, dan hapus data, dilengkapi validasi input dan fitur reset. Praktikum ini memberi gambaran alur pengembangan nyata: desain UI, XAML, event, C#, validasi, hingga CRUD dasar.
