using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cinemas.Models;

public partial class DbCinemasContext : DbContext
{
    public DbCinemasContext()
    {
    }

    public DbCinemasContext(DbContextOptions<DbCinemasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Ghe> Ghes { get; set; }

    public virtual DbSet<GhePhong> GhePhongs { get; set; }

    public virtual DbSet<GioChieu> GioChieus { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LichChieuPhim> LichChieuPhims { get; set; }

    public virtual DbSet<Phim> Phims { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TblDisable> TblDisables { get; set; }

    public virtual DbSet<TheLoaiPhim> TheLoaiPhims { get; set; }

    public virtual DbSet<TinhTrangPhim> TinhTrangPhims { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-NHH\\SQLEXPRESS;Database=dbCinemas;Integrated Security=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Idaccount);

            entity.Property(e => e.Idaccount).HasColumnName("IDAccount");
            entity.Property(e => e.Avatar).IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_Roles");
        });

        modelBuilder.Entity<Ghe>(entity =>
        {
            entity.HasKey(e => e.Idghe);

            entity.ToTable("Ghe");

            entity.Property(e => e.Idghe).HasColumnName("IDGhe");
            entity.Property(e => e.TenGhe)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GhePhong>(entity =>
        {
            entity.ToTable("Ghe_Phong");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idghe).HasColumnName("IDGhe");
            entity.Property(e => e.Idphong).HasColumnName("IDPhong");

            entity.HasOne(d => d.IdgheNavigation).WithMany(p => p.GhePhongs)
                .HasForeignKey(d => d.Idghe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ghe_Phong_Ghe");

            entity.HasOne(d => d.IdphongNavigation).WithMany(p => p.GhePhongs)
                .HasForeignKey(d => d.Idphong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ghe_Phong_Phong");
        });

        modelBuilder.Entity<GioChieu>(entity =>
        {
            entity.HasKey(e => e.IdgioChieu);

            entity.ToTable("GioChieu");

            entity.Property(e => e.IdgioChieu).HasColumnName("IDGioChieu");
            entity.Property(e => e.GioChieu1)
                .HasPrecision(0)
                .HasColumnName("GioChieu");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.IdhoaDon);

            entity.ToTable("HoaDon");

            entity.Property(e => e.IdhoaDon).HasColumnName("IDHoaDon");
            entity.Property(e => e.Idkh).HasColumnName("IDKH");
            entity.Property(e => e.IdlichChieu).HasColumnName("IDLichChieu");
            entity.Property(e => e.NgayMuaVe).HasColumnType("date");
            entity.Property(e => e.TenGhe)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdkhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.Idkh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDon_KhachHang1");

            entity.HasOne(d => d.IdlichChieuNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.IdlichChieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDon_LichChieuPhim");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.Idkh);

            entity.ToTable("KhachHang");

            entity.Property(e => e.Idkh).HasColumnName("IDKH");
            entity.Property(e => e.Avatar)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("date");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastLogin).HasColumnType("date");
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Salt)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SDT");
            entity.Property(e => e.TenKh)
                .HasMaxLength(150)
                .HasColumnName("TenKH");

            entity.HasOne(d => d.Role).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KhachHang_Roles");
        });

        modelBuilder.Entity<LichChieuPhim>(entity =>
        {
            entity.HasKey(e => e.IdlichChieu);

            entity.ToTable("LichChieuPhim");

            entity.Property(e => e.IdlichChieu).HasColumnName("IDLichChieu");
            entity.Property(e => e.IdgioChieu).HasColumnName("IDGioChieu");
            entity.Property(e => e.Idphim).HasColumnName("IDPhim");
            entity.Property(e => e.Idphong).HasColumnName("IDPhong");
            entity.Property(e => e.NgayChieu).HasColumnType("datetime");

            entity.HasOne(d => d.IdgioChieuNavigation).WithMany(p => p.LichChieuPhims)
                .HasForeignKey(d => d.IdgioChieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichChieuPhim_GioChieu");

            entity.HasOne(d => d.IdphimNavigation).WithMany(p => p.LichChieuPhims)
                .HasForeignKey(d => d.Idphim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichChieuPhim_Phim");

            entity.HasOne(d => d.IdphongNavigation).WithMany(p => p.LichChieuPhims)
                .HasForeignKey(d => d.Idphong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichChieuPhim_Phong");
        });

        modelBuilder.Entity<Phim>(entity =>
        {
            entity.HasKey(e => e.Idphim);

            entity.ToTable("Phim");

            entity.Property(e => e.Idphim).HasColumnName("IDPhim");
            entity.Property(e => e.DaoDien).HasMaxLength(50);
            entity.Property(e => e.DienVien).HasMaxLength(250);
            entity.Property(e => e.Gia).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IdtheLoai).HasColumnName("IDTheLoai");
            entity.Property(e => e.IdtinhTrangPhim).HasColumnName("IDTinhTrangPhim");
            entity.Property(e => e.Img)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.NgayKhoiChieu).HasColumnType("date");
            entity.Property(e => e.TenPhim).HasMaxLength(250);

            entity.HasOne(d => d.IdtheLoaiNavigation).WithMany(p => p.Phims)
                .HasForeignKey(d => d.IdtheLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phim_TheLoaiPhim");

            entity.HasOne(d => d.IdtinhTrangPhimNavigation).WithMany(p => p.Phims)
                .HasForeignKey(d => d.IdtinhTrangPhim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phim_TinhTrangPhim");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.Idphong);

            entity.ToTable("Phong");

            entity.Property(e => e.Idphong).HasColumnName("IDPhong");
            entity.Property(e => e.TenPhong)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblDisable>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblDisable");

            entity.Property(e => e.Iddisable)
                .ValueGeneratedOnAdd()
                .HasColumnName("IDDisable");
            entity.Property(e => e.LogTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<TheLoaiPhim>(entity =>
        {
            entity.HasKey(e => e.IdtheLoai);

            entity.ToTable("TheLoaiPhim");

            entity.Property(e => e.IdtheLoai).HasColumnName("IDTheLoai");
            entity.Property(e => e.TenTheLoai).HasMaxLength(150);
        });

        modelBuilder.Entity<TinhTrangPhim>(entity =>
        {
            entity.HasKey(e => e.IdtinhTrangPhim);

            entity.ToTable("TinhTrangPhim");

            entity.Property(e => e.IdtinhTrangPhim).HasColumnName("IDTinhTrangPhim");
            entity.Property(e => e.TenTinhTrang).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
