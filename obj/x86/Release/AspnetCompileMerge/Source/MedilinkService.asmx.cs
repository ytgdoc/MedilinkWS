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
             PMS.ThongSo.ThongSoHeThong ts = new PMS.ThongSo.ThongSoHeThong("", "", 0);
             BenhAnDTSearch dsbn = new BenhAnDTSearch(ts,"");
             dsbn.MaKhoaPhong = khoaphong;
             dsbn.LoaiBA = PMS.Access.LoaiBenhAn.NoiTru;
             dsbn.Done = "1";
             ListBN =  dsbn.LoadDSHienDien(mdbCon);
             return ListBN; 

        }
        [WebMethod]
        public void CloseConnection()
        {
            Database.Close(ref mdbCon);
        }
     }
}
