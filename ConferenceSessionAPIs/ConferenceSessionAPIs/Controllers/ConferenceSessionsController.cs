using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConferenceSessionAPIs.Models;
using System.Data.Sql;
using System.Data.SqlClient;

namespace ConferenceSessionAPIs.Controllers
{
    public class ConferenceSessionsController : ApiController
    {

        public SqlConnection Conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDBFilename='|DataDirectory|\AccountsDB.mdf'; Integrated Security=True");
        public SqlDataReader rdr = null;


        #region GetAll
        [Route("api/conferencesessions/{UserName}")]
        public List<Session> GetCreateList(string UserName)
        {
            SqlCommand getAccounts = new SqlCommand("SELECT * FROM UserAccounts WHERE UserAccount = '" + Encryption.encrypt(UserName) + "'", Conn);
            Conn.Open();
            rdr = getAccounts.ExecuteReader();


            List<Session> sqldata = new List<Session>();

            while (rdr.Read())
            {
                sqldata.Add(new Session { AccountName = Encryption.decrypt(rdr[2].ToString()), Username = Encryption.decrypt(rdr[3].ToString()), Password = Encryption.decrypt(rdr[4].ToString()) });
            }

            List<Session> Temp = new List<Session>(sqldata);



            Conn.Close();

            return Temp;
        }
        #endregion


        #region AddAccount
        [Route("api/conferencesessions/{UserAccount}/{AccountName}/{UserName}/{Password}")]
        public bool GetAccount(string UserAccount, string AccountName, string UserName, string Password)
        {
            bool error = false;
            try
            {
                UserAccount = Encryption.encrypt(UserAccount);
                AccountName = Encryption.encrypt(AccountName);
                UserName = Encryption.encrypt(UserName);
                Password = Encryption.encrypt(Password);

                //check if a Account Already Exists
                SqlCommand TestAccount = new SqlCommand("SELECT AccountName FROM UserAccounts WHERE AccountName = '" + AccountName + "' AND UserAccount = '"+ UserAccount +"'", Conn);

                Conn.Open();
                rdr = TestAccount.ExecuteReader();

                while(rdr.Read())
                {
                    if (rdr[0].ToString() != "")
                    {
                        error = true;
                    }
                }

                if (error == false)
                {
                    Conn.Close();



                    SqlCommand AddAccount = new SqlCommand("INSERT INTO UserAccounts (UserAccount, AccountName, UserName, Password) VALUES('" + UserAccount + "', '" + AccountName + "', '" + UserName + "', '" + Password + "'); ", Conn);

                    Conn.Open();

                    AddAccount.ExecuteNonQuery();

                    Conn.Close();
                }
            }
            catch
            {
                error = true;
            }

            return error;
        }
        #endregion

        #region Register
        [Route("api/conferencesessions/{Email}/{UserName}/{Password}")]
        public bool GetRegister(string Email, string UserName, string Password)
        {
            Email = Encryption.encrypt(Email);
            UserName = Encryption.encrypt(UserName);
            Password = Encryption.encrypt(Password);

            //Check if username / email exist
            bool error = false;
            SqlCommand CheckUserName = new SqlCommand("SELECT * FROM AppAccounts WHERE Email = '" + Email + "' OR UserName = '" + UserName + "'", Conn);
            Conn.Open();
            rdr = CheckUserName.ExecuteReader();
            int count = 0;
            while (rdr.Read())
            {
                count++;
            }

            if (count > 0)
            {
                error = true;
            }
            Conn.Close();

            if (error == false)
            {
                SqlCommand AddAccount = new SqlCommand("INSERT INTO AppAccounts (Email, UserName, PassWord) VALUES('" + Email + "', '" + UserName + "', '" + Password + "'); ", Conn);

                Conn.Open();

                AddAccount.ExecuteNonQuery();

                Conn.Close();
            }

            return error;
        }
        #endregion


        #region login
        [Route("api/conferencesessions/getLogin/{UserName}/{Password}")]
        public bool GetLogin(string UserName, string Password)
        {
            bool Success = false;
            UserName = Encryption.encrypt(UserName);
            Password = Encryption.encrypt(Password);

            //Check if username / email exist
            SqlCommand AddAccount = new SqlCommand("SELECT UserName FROM AppAccounts WHERE UserName = '" + UserName + "' AND  PassWord  = '" + Password + "'", Conn);

            Conn.Open();
            rdr = AddAccount.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr[0].ToString() != "")
                {
                    Success = true;
                }
            }
            Conn.Close();

            return Success;

        }

        #endregion

        #region Delete Account
        [Route("api/conferencesessions/GetDelete/{UserAccount}/{AccountName}/")]
        public bool GetAccountDelete(string UserAccount, string AccountName)
        {
            bool error = false;
            AccountName = Encryption.encrypt(AccountName);
            UserAccount = Encryption.encrypt(UserAccount);

            SqlCommand AddAccount = new SqlCommand("DELETE FROM UserAccounts WHERE AccountName = '" + AccountName + "' AND UserAccount = '" + UserAccount + "'", Conn);

            Conn.Open();

            AddAccount.ExecuteNonQuery();

            Conn.Close();
            return error;
        }
        #endregion

        #region Update
        [Route("api/conferencesessions/GetUpdate/{UserAccount}/{AccountName}/{UserName}/{Password}")]
        public bool GetUpdate(string UserAccount, string AccountName, string UserName, string Password)
        {
            bool error = false;
            UserAccount = Encryption.encrypt(UserAccount);
            AccountName = Encryption.encrypt(AccountName);
            UserName = Encryption.encrypt(UserName);
            Password = Encryption.encrypt(Password);

            SqlCommand AddAccount = new SqlCommand("UPDATE UserAccounts SET UserName = '" + UserName + "', password = '" + Password + "' where UserAccount = '" + UserAccount + "' and AccountName ='" + AccountName + "'", Conn);

            Conn.Open();

            AddAccount.ExecuteNonQuery();

            Conn.Close();
            return error;
        }
        #endregion


    }


}
