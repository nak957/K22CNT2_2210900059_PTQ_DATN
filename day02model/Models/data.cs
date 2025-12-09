using day02model.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Day02Models.Models
{
    public class Data
    {
        public static List<people> GetPeoples()
        {
            List<people> peoples = new List<people>()
                {
                    new people()
                    {
                        Id = 1,
                        Name = "Đô Rê Mon",
                        Email = "meomay@gmail.com",
                        Phone = "0986421259",
                        Address = "Quảng Châu, Quảng Ngãi",
                        Avatar = "images/avatar/01.jpg",
                        Bio = "Nhân vật trong phim Đô Rê Mon",
                        Birthday = new DateTime(1987, 7, 15),
                        Gender = 1
                    },

                    new people()
                    {
                        Id = 2,
                        Name = "Nobita Nobi",
                        Email = "nobita@gmail.com",
                        Phone = "0905123456",
                        Address = "Tokyo, Nhật Bản",
                        Avatar = "images/avatar/02.jpg",
                        Bio = "Cậu bé hậu đậu nhưng tốt bụng trong truyện Doraemon",
                        Birthday = new DateTime(1989, 3, 8),
                        Gender = 1
                    },

                    new people()
                    {
                        Id = 3,
                        Name = "Shizuka Minamoto",
                        Email = "shizuka@gmail.com",
                        Phone = "0912334455",
                        Address = "Tokyo, Nhật Bản",
                        Avatar = "images/avatar/03.jpg",
                        Bio = "Nhân vật nữ hiền lành, học giỏi trong truyện Doraemon",
                        Birthday = new DateTime(1990, 4, 12),
                        Gender = 0
                    },

                    new people()
                    {
                        Id = 4,
                        Name = "Chaien (Gian)",
                        Email = "chaien@gmail.com",
                        Phone = "0909988899",
                        Address = "Tokyo, Nhật Bản",
                        Avatar = "images/avatar/04.jpg",
                        Bio = "Cậu bé khỏe mạnh, hay bắt nạt Nobita nhưng tốt bụng",
                        Birthday = new DateTime(1988, 10, 5),
                        Gender = 1
                    },
                };

            return peoples;
        }

        public static people GetPeopleById(int id)
        {
            return GetPeoples().FirstOrDefault(x => x.Id == id);
        }
    }
}