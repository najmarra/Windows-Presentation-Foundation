using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KasirSederhana
{
    // ===== Model data =====
    public class Produk
    {
        public string Nama { get; set; }
        public decimal Harga { get; set; }

        public override string ToString()
        {
            return Nama;
        }
    }

    public class CartItem
    {
        public string Nama { get; set; }
        public decimal Harga { get; set; }
        public int Qty { get; set; }
        public decimal Subtotal => Harga * Qty;

        // Teks yang tampil di ListBox
        public override string ToString()
        {
            return $"{Nama}  x{Qty}  @ Rp {Harga:N0}  =  Rp {Subtotal:N0}";
        }
    }

    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<CartItem> keranjang
            = new ObservableCollection<CartItem>();

        private const decimal DiskonMember = 0.10m;

        public MainWindow()
        {
            InitializeComponent();

            // Data produk contoh
            cmbProduk.ItemsSource = new List<Produk>
            {
                new Produk { Nama = "Nasi Goreng", Harga = 20000 },
                new Produk { Nama = "Mie Ayam",    Harga = 15000 },
                new Produk { Nama = "Es Teh",      Harga = 5000  },
                new Produk { Nama = "Kopi Susu",   Harga = 12000 },
                new Produk { Nama = "Roti Bakar",  Harga = 10000 }
            };

            lstKeranjang.ItemsSource = keranjang;
            rbTunai.IsChecked = true;
            UpdateTotal();
        }

        // ===== EVENT 1: SelectionChanged ComboBox =====
        private void CmbProduk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbProduk.SelectedItem is Produk p)
                txtHarga.Text = $"Rp {p.Harga:N0}";
            else
                txtHarga.Text = "-";
        }

        // ===== EVENT 2: Click Tambah (dengan validasi) =====
        private void BtnTambah_Click(object sender, RoutedEventArgs e)
        {
            if (!(cmbProduk.SelectedItem is Produk produk))
            {
                MessageBox.Show("Pilih produk terlebih dahulu!", "Validasi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQty.Text))
            {
                MessageBox.Show("Jumlah harus diisi!", "Validasi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQty.Focus();
                return;
            }

            if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Jumlah harus berupa angka bulat lebih dari 0!", "Validasi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQty.Focus();
                txtQty.SelectAll();
                return;
            }

            // Jika produk sudah ada di keranjang, tambahkan qty-nya
            int index = -1;
            for (int i = 0; i < keranjang.Count; i++)
            {
                if (keranjang[i].Nama == produk.Nama)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                // Ganti objek agar ListBox ter-refresh
                keranjang[index] = new CartItem
                {
                    Nama = produk.Nama,
                    Harga = produk.Harga,
                    Qty = keranjang[index].Qty + qty
                };
            }
            else
            {
                keranjang.Add(new CartItem
                {
                    Nama = produk.Nama,
                    Harga = produk.Harga,
                    Qty = qty
                });
            }

            UpdateTotal();
            cmbProduk.SelectedIndex = -1;
            txtQty.Text = "1";
        }

        // ===== Click Reset =====
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            cmbProduk.SelectedIndex = -1;
            txtQty.Text = "1";
            cmbProduk.Focus();
        }

        // ===== EVENT 3: Click Hapus (dengan konfirmasi) =====
        private void BtnHapus_Click(object sender, RoutedEventArgs e)
        {
            if (lstKeranjang.SelectedItem is CartItem item)
            {
                var hasil = MessageBox.Show(
                    $"Hapus \"{item.Nama}\" dari keranjang?",
                    "Konfirmasi",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (hasil == MessageBoxResult.Yes)
                {
                    keranjang.Remove(item);
                    UpdateTotal();
                }
            }
            else
            {
                MessageBox.Show("Pilih item yang ingin dihapus!", "Validasi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ===== EVENT 4: Checked/Unchecked CheckBox member =====
        private void ChkMember_Changed(object sender, RoutedEventArgs e)
        {
            UpdateTotal();
        }

        // ===== EVENT 5: Checked RadioButton metode bayar =====
        private void Metode_Changed(object sender, RoutedEventArgs e)
        {
            // Event bisa terpanggil saat InitializeComponent, jadi cek null
            if (txtBayar == null || rbTunai == null) return;

            txtBayar.IsEnabled = rbTunai.IsChecked == true;
            if (!txtBayar.IsEnabled) txtBayar.Clear();
        }

        // ===== EVENT 6: Click Bayar (dengan validasi) =====
        private void BtnBayar_Click(object sender, RoutedEventArgs e)
        {
            if (keranjang.Count == 0)
            {
                MessageBox.Show("Keranjang masih kosong!", "Validasi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (rbTunai.IsChecked != true && rbQris.IsChecked != true)
            {
                MessageBox.Show("Pilih metode pembayaran!", "Validasi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal total = HitungTotal();
            string metode = rbTunai.IsChecked == true ? "Tunai" : "QRIS";
            string infoBayar = "";

            if (rbTunai.IsChecked == true)
            {
                if (!decimal.TryParse(txtBayar.Text, out decimal uang))
                {
                    MessageBox.Show("Masukkan uang diterima berupa angka!", "Validasi",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtBayar.Focus();
                    return;
                }

                if (uang < total)
                {
                    MessageBox.Show(
                        $"Uang kurang Rp {(total - uang):N0}!", "Validasi",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtBayar.Focus();
                    return;
                }

                infoBayar = $"\nUang diterima : Rp {uang:N0}\nKembalian     : Rp {(uang - total):N0}";
            }

            MessageBox.Show(
                $"Pembayaran berhasil!\n\nMetode : {metode}\nTotal  : Rp {total:N0}{infoBayar}",
                "Struk",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            // Transaksi baru
            keranjang.Clear();
            chkMember.IsChecked = false;
            txtBayar.Clear();
            UpdateTotal();
        }

        // ===== Helper =====
        private decimal HitungSubtotal()
        {
            return keranjang.Sum(k => k.Subtotal);
        }

        private decimal HitungDiskon()
        {
            return chkMember.IsChecked == true ? HitungSubtotal() * DiskonMember : 0;
        }

        private decimal HitungTotal()
        {
            return HitungSubtotal() - HitungDiskon();
        }

        private void UpdateTotal()
        {
            if (txtTotal == null || chkMember == null) return;

            txtJumlahItem.Text = $"Jumlah Item: {keranjang.Sum(k => k.Qty)}";
            txtSubtotal.Text = $"Subtotal: Rp {HitungSubtotal():N0}";
            txtDiskon.Text = $"Diskon: Rp {HitungDiskon():N0}";
            txtTotal.Text = $"TOTAL: Rp {HitungTotal():N0}";
        }
    }
}
