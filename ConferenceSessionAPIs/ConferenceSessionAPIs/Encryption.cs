/*Editor Jack Gallop
 * This class handles all encryption and decryption in the program
 * Refrences - Administrator, 2011,Simple C#.Net Encryption and Decryption for String,Tech Example Retrived 20/04/2016
 *             http://www.techexample.com/simple-c-net-encryption-and-decryption-for-string/
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;


namespace ConferenceSessionAPIs
{
    public class Encryption
    {
        private static string Passphrase = "PleaseHideMe"; // create salt passcode used for encryption and decryption 

        #region Encrypt

        public static string encrypt(string input) //encrypt the passed in input
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase)); //create the hash value 

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider(); //create a new TDESAlgorithm object

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(input);

            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results); //return the byte results as a string
        }

        #endregion

        #region decrypt
        public static string decrypt(string input) //decrypt the passed in input
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase)); //create the hash value 

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider(); //create a new TDESAlgorithm object 

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToDecrypt = Convert.FromBase64String(input); //convert the byte results from its hash value to byte plain text

            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return UTF8.GetString(Results); //return the decrypted string

        } //(Administrator, 2011)
        #endregion

    }
}