using Microsoft.AspNetCore.Mvc.Rendering;

namespace _2210900059_PhamTheQuyen_Lab03.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public int TotalPage { get; set; }
        public string Sumary { get; set; }
        public List<Book> GetBookList()
        {
            List<Book> list = new List<Book>()
        {
            new Book()
            {
                Id = 1,
                Title = "Chí Phèo",
                AuthorId = 1,
                GenreId = 1,
                Image = "/images/products/b1.jpg",
                Price = 500000,
                Sumary = "Tác phẩm nổi tiếng của Nam Cao.",
                TotalPage = 250
            },

            new Book()
            {
                Id = 2,
                Title = "Lão Hạc",
                AuthorId = 1,
                GenreId = 1,
                Image = "/images/products/b2.jpg",
                Price = 450000,
                Sumary = "Câu chuyện cảm động về số phận người nông dân.",
                TotalPage = 190
            },

            new Book()
            {
                Id = 3,
                Title = "Dế Mèn Phiêu Lưu Ký",
                AuthorId = 2,
                GenreId = 2,
                Image = "/images/products/b3.jpg",
                Price = 300000,
                Sumary = "Tác phẩm thiếu nhi nổi tiếng của Tô Hoài.",
                TotalPage = 210
            },

            new Book()
            {
                Id = 4,
                Title = "Tắt Đèn",
                AuthorId = 3,
                GenreId = 1,
                Image = "/images/products/b4.jpg",
                Price = 520000,
                Sumary = "Tác phẩm hiện thực xuất sắc của Ngô Tất Tố.",
                TotalPage = 320
            },

            new Book()
            {
                Id = 5,
                Title = "Tuổi Trẻ Đáng Giá Bao Nhiêu",
                AuthorId = 4,
                GenreId = 3,
                Image = "/images/products/b5.jpg",
                Price = 150000,
                Sumary = "Sách kỹ năng sống truyền cảm hứng cho giới trẻ.",
                TotalPage = 280
            }
        };

            return list;
        }

        //đọc chi tiết sách
        public Book GetBookById(int id)
        {
            Book book = this.GetBookList().FirstOrDefault(x => x.Id == id);
            return book;
        }

        public List<SelectListItem> Authors { get; } = new List<SelectListItem>()
                 {
                     new SelectListItem { Value = "1", Text = "Nam Cao" },
                     new SelectListItem { Value = "2", Text = "Ngô Tất Tố" },
                     new SelectListItem { Value = "3", Text = "Tô Hoài" },
                     new SelectListItem { Value = "4", Text = "Thiền sư Thích Nhất Hạnh" }
                 };

        public List<SelectListItem> Genres { get; } = new List<SelectListItem>()
                 {
                     new SelectListItem { Value = "1", Text = "Truyện tranh" },
                     new SelectListItem { Value = "2", Text = "Văn học đương đại" },
                     new SelectListItem { Value = "3", Text = "Phật học phổ thông" },
                     new SelectListItem { Value = "4", Text = "Truyện cười" }
                 };

    }    

}
