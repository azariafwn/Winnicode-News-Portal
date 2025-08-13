# 📰 Portal Berita Winnicode (ASP.NET Core MVC)

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) ![MySQL](https://img.shields.io/badge/mysql-%2300f.svg?style=for-the-badge&logo=mysql&logoColor=white) ![Azure](https://img.shields.io/badge/Microsoft%20Azure-0078D4?style=for-the-badge&logo=microsoftazure&logoColor=white) ![Bootstrap](https://img.shields.io/badge/bootstrap-%238511FA.svg?style=for-the-badge&logo=bootstrap&logoColor=white)

Aplikasi web Portal Berita ini merupakan hasil dari program magang di **PT. Winnicode Garuda Teknologi** dan dibangun sepenuhnya menggunakan ekosistem **ASP.NET Core MVC**.
Proyek ini mencakup seluruh siklus pengembangan *full-stack*, mulai dari perancangan skema database, pengembangan backend dan frontend, hingga proses deployment ke platform cloud Microsoft Azure.

### 🌐 Live Demo

Aplikasi ini telah berhasil di-deploy ke **Microsoft Azure** dan dapat diakses secara publik. Silakan kunjungi link di bawah untuk melihat dan mencoba aplikasi secara langsung.

**[➡️ Kunjungi Website Live: winnicode-webbapp.azurewebsites.net](https://winnicode-webbapp.azurewebsites.net/)**

*Catatan: Mungkin memerlukan beberapa saat untuk proses loading awal jika aplikasi sedang dalam mode 'sleep' (cold start) di platform Azure.*

---

## ✨ Fitur Utama

* **Sistem Multi-Peran (Multi-Role System)**: Aplikasi ini mengimplementasikan sistem otorisasi berbasis peran dengan tiga level akses yang berbeda:
    * **Pembaca (Reader)**: Dapat menjelajahi dan membaca semua berita yang telah dipublikasikan.
    * **Penulis (Author)**: Dapat membuat, mengedit, dan mengelola artikel berita miliknya sendiri melalui dashboard khusus.
    * **Editor/Admin**: Memiliki hak akses penuh untuk mengelola semua artikel dari seluruh penulis, mengelola kategori, dan memvalidasi konten sebelum dipublikasikan.

* **Manajemen Konten (CRUD)**: Fungsionalitas penuh untuk Create, Read, Update, dan Delete pada artikel berita dan kategori.

* **Antarmuka Responsif**: Dibangun menggunakan Bootstrap 5, memastikan tampilan yang optimal di berbagai perangkat, mulai dari desktop hingga mobile.

* **Fitur Interaktif (AJAX)**:
    * **Sistem Reaksi**: Pengguna dapat memberikan reaksi pada artikel (misalnya, suka, tidak suka) tanpa perlu me-refresh halaman.
    * **Tabel Data Dinamis**: Dashboard manajemen menggunakan DataTables.js untuk menyediakan fitur pencarian, pengurutan, dan paginasi yang cepat dan interaktif.

* **Deployment ke Cloud**: Seluruh aplikasi dan database berhasil di-deploy dan dikonfigurasi di **Microsoft Azure**.

---

## 🛠️ Tech Stack

| Kategori | Teknologi |
| :--- | :--- |
| **Backend** | `ASP.NET Core MVC`, `C#`, `Entity Framework Core` (untuk ORM) |
| **Frontend** | `HTML`, `CSS`, `JavaScript`, `Bootstrap 5`, `jQuery`, `AJAX` |
| **Database** | `MySQL` |
| **Platform Cloud** | `Microsoft Azure` (App Service & Azure Database for MySQL) |
| **Development Tools**| `Visual Studio 2022`, `Git`, `CI/CD` |

---

## 🏛️ Arsitektur & Desain

Proyek ini dibangun mengikuti arsitektur **Model-View-Controller (MVC)** yang merupakan standar dalam pengembangan aplikasi ASP.NET Core.

* **Model**: Merepresentasikan data dan logika bisnis. Menggunakan Entity Framework Core untuk memetakan objek C# ke tabel database MySQL.
* **View**: Bertanggung jawab untuk menampilkan data kepada pengguna. Menggunakan Razor Pages untuk membangun antarmuka pengguna yang dinamis.
* **Controller**: Bertindak sebagai perantara yang menangani input dari pengguna, berinteraksi dengan Model, dan memilih View yang akan ditampilkan.

Proses deployment dilakukan melalui **CI/CD (Continuous Integration/Continuous Deployment)**, memungkinkan proses rilis yang otomatis dan andal ke Microsoft Azure.
