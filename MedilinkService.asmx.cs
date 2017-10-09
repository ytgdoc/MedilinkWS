using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MDB;
using MedilinkDatabases;
using PMS.DanhMuc.Library;
using PMS.DanhMuc;
using System.Data;
using PMS.BaoCao.Library;
using PMS.ChiDinhDichVu.Library;


namespace MedilinkWS
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://www.LinkSoft.com/MedilinkWS")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[System.Web.Script.Services.ScriptService]
    public class MedilinkService : System.Web.Services.WebService
    {
        MDBConnection mdbCon = null;
        public DataTable ListBN;
        PMS.ThongSo.ThongSoHeThong ts = new PMS.ThongSo.ThongSoHeThong("", "", 0);
        public MedilinkService()
        {
            try {
                Database.Open(ref mdbCon);
            }
            catch(Exception ex)
            {
                string s = ex.ToString();
            }
            
        }
        [WebMethod]
        public bool bIsValidUser(string s_UserName,string s_Password)
        {
            PMS.DanhMuc.Library.NguoiDung user = new PMS.DanhMuc.Library.NguoiDung("", "", PMS.DanhMuc.Library.LoaiChucNangPhanMem.BenhNhan);
            return user.IsValidUser(mdbCon, s_UserName,s_Password);
        }
        [WebMethod]
        public string List_IDDepartmentsOfUser(string s_UserName)
        {
            NguoiDung userOfKP = new NguoiDung("", s_UserName, LoaiChucNangPhanMem.BenhNhan);
            userOfKP.Ma = s_UserName;
            userOfKP.Load(mdbCon);
            string khoaphong = userOfKP.ListKhoaPhong;
           // neu user admin thi listkhoaphong ="" . no se lay het tat ca ds khoa phong
            return khoaphong;
        }
        [WebMethod]
        public DataTable List_DepartmentsOfUser(string khoaphong, string s_UserName)
        {
            ThuVienDanhMuc khbv = new ThuVienDanhMuc("");
            DataTable listkp = khbv.LoadKhoaLamSang(mdbCon, khoaphong);
            return listkp;
        }
        [WebMethod]
        public DataTable ListPatientOfDepartment(string khoaphong)
        {
             
             BenhAnDTSearch dsbn = new BenhAnDTSearch(ts,"");
             dsbn.MaKhoaPhong = khoaphong;
             dsbn.LoaiBA = PMS.Access.LoaiBenhAn.NoiTru;
             dsbn.Done = "1";
             ListBN =  dsbn.LoadDSHienDien(mdbCon);
             return ListBN; 

        }
        [WebMethod]
        public DataTable GiaVienPhi(string s_UserName)
        {
            PMS.DanhMuc.Library.GiaVienPhi vienphi = new PMS.DanhMuc.Library.GiaVienPhi(ts, "", s_UserName);
          DataTable dt_Chidinh= vienphi.Load(mdbCon);
          return dt_Chidinh;

        }
        [WebMethod]
        public bool InsertChiDinh(string s_UserName,int Id_Computer)
        {
           PMS.DanhMuc.Library.DoiTuong Lis_doituong = new PMS.DanhMuc.Library.DoiTuong(ts, "", s_UserName);
           DataTable dt_DoiTuong= Lis_doituong.Load(mdbCon);
           PMS.ChiDinhDichVu.Library.ChiDinh cd = new PMS.ChiDinhDichVu.Library.ChiDinh(ts, "", s_UserName, Id_Computer, dt_DoiTuong);
           return false;
        }

        [WebMethod]
        public void CloseConnection()
        {
            Database.Close(ref mdbCon);
        }
     }
}
