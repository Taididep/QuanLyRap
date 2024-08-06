using GUI.DTO;
using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;//thư viện để mã hóa mật khẩu
using System.Text;

namespace GUI.DAO
{
    public class AccountDAO
    {
        private AccountDAO() { }


        //private static string PasswordEncryption(string password)
        //{
        //    byte[] salt = new byte[16];
        //    using (var rng = new RNGCryptoServiceProvider())
        //    {
        //        rng.GetBytes(salt);
        //    }

        //    string saltString = BitConverter.ToString(salt).Replace("-", ""); // Chuyển đổi mảng byte thành chuỗi hexa

        //    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, saltString);

        //    return hashedPassword;
        //}

        public static int Login(string userName, string passWord)
        {
            string query = "SELECT Pass FROM TaiKhoan WHERE UserName = @userName";
            DataTable result = DataProvider.ExecuteQuery(query, new object[] { userName });

            if (result == null || result.Rows.Count == 0)
                return -1;

            string passwordFromDatabase = result.Rows[0]["Pass"].ToString();

            if (passWord == passwordFromDatabase)
                return 1;
            else
                return 0;
        }



        public static bool UpdatePasswordForAccount(string userName, string passWord, string newPassWord)
        {

            string oldPass = passWord;
            string newPass = newPassWord;

            int result = DataProvider.ExecuteNonQuery("EXEC USP_UpdatePasswordForAccount @username , @pass , @newPass", new object[] { userName, oldPass, newPass });

            return result > 0;
        }

        public static Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.ExecuteQuery("Select * from TaiKhoan where userName = '" + userName + "'");

            foreach (DataRow row in data.Rows)
            {
                return new Account(row);
            }

            return null;
        }

        public static void DeleteAccountByIdStaff(string idStaff)
        {
            DataProvider.ExecuteQuery("DELETE dbo.TaiKhoan WHERE idNV = '" + idStaff + "'");
        }

		public static DataTable GetAccountList()
		{
			return DataProvider.ExecuteQuery("USP_GetAccountList");
		}

		public static bool InsertAccount(string username, int accountType, string staffID)
		{
			int result = DataProvider.ExecuteNonQuery("EXEC USP_InsertAccount @username , @loaiTK , @idnv ", new object[] { username, accountType, staffID });
			return result > 0;
		}

		public static bool UpdateAccount(string username, int accountType)
		{
			string command = string.Format("USP_UpdateAccount  @username , @loaiTK", new object[] { username, accountType});
			int result = DataProvider.ExecuteNonQuery(command);
			return result > 0;
		}

		public static bool DeleteAccount(string username)
		{
			int result = DataProvider.ExecuteNonQuery("DELETE dbo.TaiKhoan WHERE UserName = N'" + username + "'");
			return result > 0;
		}

		public static DataTable SearchAccountByStaffName(string name)
		{
			return DataProvider.ExecuteQuery("EXEC USP_SearchAccount @hoten ", new object[] { name });
		}

		public static bool ResetPassword(string username)
		{
			int result = DataProvider.ExecuteNonQuery("USP_ResetPasswordtAccount @username", new object[] { username});
			return result > 0;
		}
    }
}
