using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace DiscordBanDetector
{
    class Security
    {
        public static string GetMachineGuid()
        {
            string location = @"SOFTWARE\Microsoft\Cryptography";
            string name = "MachineGuid";

            using (RegistryKey localMachineX64View =
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(
                            string.Format("Key Not Found: {0}", location));

                    object machineGuid = rk.GetValue(name);
                    if (machineGuid == null)
                        throw new IndexOutOfRangeException(
                            string.Format("Index Not Found: {0}", name));

                    return machineGuid.ToString();
                }
            }
        }
        public static byte[] Encrypt(string token, string AESKey)
        {
            byte[] Data = Encoding.ASCII.GetBytes(token);
            RijndaelManaged aesEncryption = new RijndaelManaged()
            {
                KeySize = 256,
                BlockSize = 128
            };
            byte[] saltBytes = new byte[] { 6, 9, 4, 2, 0, 9, 1, 1, 6, 9, 4, 2, 0, 5, 8, 1, 0, 5, 0, 7, 1, 5, 1, 3, 0, 6, 9, 4 };
            var key = new Rfc2898DeriveBytes(AESKey, saltBytes, 1000);
            aesEncryption.IV = key.GetBytes(aesEncryption.BlockSize / 8);
            aesEncryption.Key = key.GetBytes(aesEncryption.KeySize / 8);
            ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            return crypto.TransformFinalBlock(Data, 0, Data.Length);
        }
        public static string Decrypt(byte[] token, string AESKey)
        {
            try
            {
                RijndaelManaged aesEncryption = new RijndaelManaged()
                {
                    KeySize = 256,
                    BlockSize = 128
                };
                byte[] saltBytes = new byte[] { 6, 9, 4, 2, 0, 9, 1, 1, 6, 9, 4, 2, 0, 5, 8, 1, 0, 5, 0, 7, 1, 5, 1, 3, 0, 6, 9, 4 };
                var key = new Rfc2898DeriveBytes(AESKey, saltBytes, 1000);
                aesEncryption.IV = key.GetBytes(aesEncryption.BlockSize / 8);
                aesEncryption.Key = key.GetBytes(aesEncryption.KeySize / 8);
                ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
                return Encoding.ASCII.GetString(decrypto.TransformFinalBlock(token, 0, token.Length));
            }
            catch
            {
                return "invalid";
            }
        }

    }
}
