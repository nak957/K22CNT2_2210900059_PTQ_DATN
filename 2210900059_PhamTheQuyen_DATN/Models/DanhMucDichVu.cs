using _221090005_PhamTheQuyen_DATN.Models;

public class DanhMucDichVu
{
    public int MaDanhMucDV { get; set; }
    public string TenDanhMuc { get; set; }
    public string Slug { get; set; }
    public string MoTa { get; set; }

    public int? maNguoiTao { get; set; }
    public int? maNguoiCapNhat { get; set; }

    public NguoiDung NguoiTao { get; set; }
    public NguoiDung NguoiCapNhat { get; set; }

    public ICollection<DichVu> DichVus { get; set; }
}
