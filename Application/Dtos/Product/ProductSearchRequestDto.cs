using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
<<<<<<< HEAD
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Product
{
    
=======

namespace Application.Dtos.Product
{
>>>>>>> DevB-1
    // Catalog arama için query parametrelerini taşıyan DTO.
    // Controller'da [FromQuery] ile bind edilmek için kullanılacak.
    public class ProductSearchRequestDto
    {
<<<<<<< HEAD
      
        // Metin araması: product.Name, product.Slug veya SKU (string olarak) içinde aranır.
        public string? Query { get; set; }

        // Filtrelenecek kategori Id (isteğe bağlı). 
        public int? CategoryId { get; set; }

        
=======
        // Metin araması: product.Name veya SKU içinde aranır.
        public string? Query { get; set; }

        // Slug filtresi (slug guard için kontrol ve arama)
        public string? Slug { get; set; }

        // Filtrelenecek kategori Id (isteğe bağlı). 
        public int? CategoryId { get; set; }

>>>>>>> DevB-1
        // Minimum fiyat (decimal). Nullable; verilmezse uygulanmaz.
        public decimal? MinPrice { get; set; }

        // Maximum fiyat (decimal). Nullable; verilmezse uygulanmaz.
        public decimal? MaxPrice { get; set; }

        // Sadece stokta olanları getir (true -> Stok > 0).
        public bool? InStock { get; set; }

        // Sıralama alanı (örn: "price", "name", "createdDate", "stok").
<<<<<<< HEAD
        // Repository tarafında bu alanın kabul edilen değerleri kontrol edilecek.
=======
>>>>>>> DevB-1
        public string? SortBy { get; set; }

        // "asc" veya "desc". Varsayılan "asc".
        public string? SortOrder { get; set; } = "asc";

        // Sayfa numarası (1-index). Default 1.
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

<<<<<<< HEAD
        // Sayfa büyüklüğü. Default 20. Max değeri validator'da sınırla (ör. 100).
        [Range(1, 200)]
        public int PageSize { get; set; } = 20;
    }
}
=======
        // Sayfa büyüklüğü. Default 20. Max değeri validator'da sınırla (örn. 100).
        [Range(1, 200)]
        public int PageSize { get; set; } = 20;
    }
}
>>>>>>> DevB-1
