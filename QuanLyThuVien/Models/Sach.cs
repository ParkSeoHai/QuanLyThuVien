﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class Sach
    {
        private int _ID_Sach;
        private string _TenSach;
        private DateTime _NgayNhap;
        private double _GiaTien;
        private int _SoLuong;
        private string _UrlImg;

        private int _ID_TheLoai;
        private TheLoai _TheLoai;

        private int _ID_TacGia;
        private TacGia _TacGia;

        private ICollection<CTPhieuMuon> _CTPhieuMuon;

        [Key]
        public int ID_Sach { get => _ID_Sach; set => _ID_Sach = value; }
        [Required]
        public string TenSach { get => _TenSach; set => _TenSach = value; }
        public DateTime NgayNhap { get => _NgayNhap; set => _NgayNhap = value; }
        public double GiaTien { get => _GiaTien; set => _GiaTien = value; }
        public int SoLuong { get => _SoLuong; set => _SoLuong = value; }
        public string UrlImg { get => _UrlImg; set => _UrlImg = value; }

        public int ID_TheLoai { get => _ID_TheLoai; set => _ID_TheLoai = value; }
        [ForeignKey("ID_TheLoai")]
        public TheLoai TheLoai { get => _TheLoai; set => _TheLoai = value; }

        public int ID_TacGia { get => _ID_TacGia; set => _ID_TacGia = value; }
        [ForeignKey("ID_TacGia")]
        public TacGia TacGia { get => _TacGia; set => _TacGia = value; }

        public ICollection<CTPhieuMuon> CTPhieuMuon { get => _CTPhieuMuon; set => _CTPhieuMuon = value; }
    }
}
