# EcommerceAPI 🚀

**EcommerceAPI**, .NET 8 ile geliştirilen, ölçeklenebilir bir e-ticaret API projesidir. Katmanlı mimarisi, JWT tabanlı kimlik doğrulaması, cache ve log yönetimiyle profesyonel bir backend altyapısı sunar. AWS ve Azure üzerinde başarıyla deploy edilmiştir.

---

## 🧰 Teknoloji Yığını

* **.NET 8 Web API**
* **Entity Framework Core**
* **SQL Server**
* **JWT Authentication & Authorization**
* **AutoMapper**
* **Logging** 
* **Caching** (In‑Memory Cache desteği)
* **AWS s3**
* **Azure App Service Deployment**
* **Clean Architecture:** 

---

## 🏗️ Proje Yapısı

- **Application** – İş mantığı ve servisler
- **Domain** – Entity ve domain modelleri
- **Infrastructure** – DB, Cache, External Service bağlantıları
- **WebAPI** – API Controller ve giriş noktası

## 🚀 Öne Çıkan Özellikler

* JWT tabanlı kullanıcı doğrulama ve rol/policy bazlı yetkilendirme (RBAC)
* Ürün, kategori ve sipariş yönetimi için RESTful API endpoint’leri
* AutoMapper ile DTO – Entity dönüşümü
* EF Core migration yönetimi
* Pagination desteği ile veri sorgulama optimizasyonu
* Her request için Correlation-Id ile loglama ve izlenebilirlik
* Rate limiting ve timeout yönetimi ile API güvenliği
* In-Memory cache ile performans optimizasyonu
* AWS & Azure üzerinde CI/CD desteği
* Test edilebilir, temiz mimari


  
* ## 🔒 Performans & Güvenlik

* Rate limiting ve timeout yönetimi ile API güvenliği
* In-Memory cache ile veri okuma optimizasyonu
* Soft delete global filtreler ve exception handling



* ## ⚡ CI/CD & Deployment

Proje, GitHub Actions kullanılarak otomatik olarak deploy edilmektedir.  
Her push veya pull request ile:

* Kod derlenir 
* publish package oluşturulur
* Azure App Service  üzerinde otomatik deploy edilir

## 📖 Örnek Request & Response

### 1️⃣ POST /api/Auth/login

**Request:**

```json
{
  "email": "test@example.com",
  "password": "Password123!"
}
Response:

{
  "success": true,
  "message": "Giriş başarılı",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "gecerlilikTarihi": "2025-10-06T18:49:55.9897662Z"
  },
  "errorCodes": null
}

2️⃣ GET /api/v1/products

Request Query Parameters:

pageNumber=1&pageSize=2
Response:

{
  "success": true,
  "message": "Ürünler listelendi",
  "data": {
    "data": [
      {
        "id": 1,
        "sku": 1,
        "name": "Product-00000001",
        "slug": "product-00000001",
        "price": 6939,
        "stok": 135,
        "categoryId": 3,
        "isActive": true
      },
      {
        "id": 6,
        "sku": 6,
        "name": "Product-00000006",
        "slug": "product-00000006",
        "price": 185,
        "stok": 583,
        "categoryId": 15,
        "isActive": true
      }
    ],
    "pageNumber": 1,
    "pageSize": 2,
    "totalRecords": 4998,
    "totalPages": 2499,
    "hasNextPage": true,
    "hasPreviousPage": false
  },
  "errorCodes": null
}

3️⃣ GET /api/v1/user/getAll

Response:

{
  "success": true,
  "message": "Kullanıcı listesi getirildi",
  "data": [
    {
      "id": 1,
      "email": "emre",
      "firstName": "emre",
      "lastName": "emre",
      "roles": [
        {
          "id": 1,
          "name": "Admin",
          "userCount": 1
        },
        {
          "id": 2,
          "name": "Customer",
          "userCount": 1
        }
      ]
    },
    {
      "id": 2,
      "email": "ali",
      "firstName": "ali",
      "lastName": "ali",
      "roles": []
    },
    {
      "id": 3,
      "email": "ayse",
      "firstName": "ayse",
      "lastName": "ayse",
      "roles": []
    }
  ],
  "errorCodes": null
}
