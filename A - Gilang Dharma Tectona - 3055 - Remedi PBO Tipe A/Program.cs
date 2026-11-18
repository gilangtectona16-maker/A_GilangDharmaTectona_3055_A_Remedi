using System;
using System.Collections.Generic;

namespace SistemMBG
{
    public class Sekolah
    {
        public string idSekolah;
        public string namaSekolah;
        public int jumlahPaketDiterima;

        public Sekolah(string id, string nama, int jumlahAwal = 0)
        {
            idSekolah = id;
            namaSekolah = nama;
            jumlahPaketDiterima = jumlahAwal;
        }

        public void TampilInfo()
        {
            Console.WriteLine($"ID Sekolah     : {idSekolah}");
            Console.WriteLine($"Nama Sekolah   : {namaSekolah}");
            Console.WriteLine($"Paket Diterima : {jumlahPaketDiterima}");
        }
    }

    public abstract class PaketMBG
    {
        private string idPaket;
        private string namaPaket;
        private int stokDapur;

        protected PaketMBG(string id, string nama, int stokAwal)
        {
            idPaket = id;
            namaPaket = nama;
            stokDapur = stokAwal < 0 ? 0 : stokAwal;
        }

        public string IdPaket
        {
            get { return idPaket; }
            set { idPaket = value; }
        }

        public string NamaPaket
        {
            get { return namaPaket; }
            set { namaPaket = value; }
        }

        public int StokDapur
        {
            get { return stokDapur; }
        }

        public bool KurangiStok(int jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Jumlah pengurangan harus lebih dari 0.");
                return false;
            }
            if (jumlah > stokDapur)
            {
                Console.WriteLine($"Jumlah pengurangan ({jumlah}) melebihi stok tersedia ({stokDapur}).");
                return false;
            }
            stokDapur -= jumlah;
            return true;
        }

        public abstract int HitungBiaya(int jumlah);

        public virtual void TampilInfo()
        {
            Console.WriteLine($"ID Paket   : {idPaket}");
            Console.WriteLine($"Nama Paket : {namaPaket}");
            Console.WriteLine($"Stok Dapur : {stokDapur}");
        }
    }

    public class PaketReguler : PaketMBG
    {
        public const int HargaPerPaket = 15000;

        public PaketReguler(string id, string nama, int stok) : base(id, nama, stok) { }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * HargaPerPaket;
        }

        public override void TampilInfo()
        {
            base.TampilInfo();
            Console.WriteLine("Isi        : Nasi, Ayam, Sayur, Buah");
            Console.WriteLine($"Harga      : Rp{HargaPerPaket:N0} / paket");
        }
    }

    public class PaketKhusus : PaketMBG
    {
        public const int HargaPerPaket = 20000;
        public string jenisDiet;

        public PaketKhusus(string id, string nama, int stok, string diet) : base(id, nama, stok)
        {
            jenisDiet = diet;
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * HargaPerPaket;
        }

        public override void TampilInfo()
        {
            base.TampilInfo();
            Console.WriteLine($"Jenis Diet : {jenisDiet}");
            Console.WriteLine($"Harga      : Rp{HargaPerPaket:N0} / paket");
        }
    }

    public interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    public class DistribusiMBG : DapatDidistribusikan
    {
        private Sekolah sekolahTujuan;
        private PaketMBG paket;
        private int jumlah;

        public DistribusiMBG(Sekolah sekolah, PaketMBG paket, int jumlah)
        {
            this.sekolahTujuan = sekolah;
            this.paket = paket;
            this.jumlah = jumlah;
        }

        public void KirimPaket()
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Distribusi MBG gagal! Jumlah paket harus lebih dari 0.");
                return;
            }
            if (jumlah > paket.StokDapur)
            {
                Console.WriteLine($"Distribusi MBG gagal! Jumlah kiriman ({jumlah}) melebihi stok dapur ({paket.StokDapur}).");
                return;
            }

            paket.KurangiStok(jumlah);
            sekolahTujuan.jumlahPaketDiterima += jumlah;

            Console.WriteLine($"Distribusi MBG berhasil! Sebanyak {jumlah} paket makanan telah dikirim ke sekolah {sekolahTujuan.namaSekolah}");
        }
    }

    public class PengelolaMBG
    {
        private List<Sekolah> daftarSekolah = new List<Sekolah>();
        private List<PaketMBG> daftarPaket = new List<PaketMBG>();

        public void TambahSekolah(Sekolah sekolah)
        {
            daftarSekolah.Add(sekolah);
        }

        public void TambahPaket(PaketMBG paket)
        {
            daftarPaket.Add(paket);
        }

        public void TampilkanDataMBG()
        {
            Console.WriteLine("=== DATA SEKOLAH PENERIMA ===");
            foreach (var s in daftarSekolah)
            {
                s.TampilInfo();
                Console.WriteLine("-----------------------------");
            }

            Console.WriteLine("=== DATA PAKET & STOK DAPUR ===");
            foreach (var p in daftarPaket)
            {
                p.TampilInfo();
                Console.WriteLine("-----------------------------");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Sekolah sekolah1 = new Sekolah("SKL001", "SDN 1 Jember");
            Sekolah sekolah2 = new Sekolah("SKL002", "SMPN 2 Jember");

            Console.WriteLine("=== INFO SEKOLAH ===");
            sekolah1.TampilInfo();
            Console.WriteLine();
            sekolah2.TampilInfo();
            Console.WriteLine();

            PaketMBG reguler = new PaketReguler("PKT001", "Paket Reguler", 100);
            PaketMBG khusus = new PaketKhusus("PKT002", "Paket Khusus", 50, "Rendah Gula");

            Console.WriteLine("=== POLYMORPHISM HitungBiaya ===");
            PaketMBG[] semuaPaket = { reguler, khusus };
            foreach (PaketMBG p in semuaPaket)
            {
                Console.WriteLine($"{p.NamaPaket} x 10 = Rp{p.HitungBiaya(10):N0}");
            }
            Console.WriteLine();

            PengelolaMBG pengelola = new PengelolaMBG();
            pengelola.TambahSekolah(sekolah1);
            pengelola.TambahSekolah(sekolah2);
            pengelola.TambahPaket(reguler);
            pengelola.TambahPaket(khusus);

            Console.WriteLine("=== DISTRIBUSI ===");
            DapatDidistribusikan d1 = new DistribusiMBG(sekolah1, reguler, 40);   // berhasil
            d1.KirimPaket();

            DapatDidistribusikan d2 = new DistribusiMBG(sekolah2, khusus, 80);    // gagal
            d2.KirimPaket();

            DapatDidistribusikan d3 = new DistribusiMBG(sekolah2, khusus, 0);     // gagal
            d3.KirimPaket();

            DapatDidistribusikan d4 = new DistribusiMBG(sekolah2, khusus, 30);    // berhasil
            d4.KirimPaket();
            Console.WriteLine();

            pengelola.TampilkanDataMBG();

            Console.ReadKey();
        }
    }
}