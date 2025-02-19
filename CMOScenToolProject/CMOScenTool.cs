using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using System.IO;
using Microsoft.IO;
using K4os.Compression.LZ4.Legacy;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Runtime.CompilerServices;
using System;
using System.Xml.Linq;



namespace CMOScenToolProject
{
    public static class RCMS
    {
        public static RecyclableMemoryStreamManager recyclableMemoryStreamManager_0 = new RecyclableMemoryStreamManager();

        internal static void smethod_0()
        {
        }
    }

    public static class Misc
    {
        public static string CleanUpXML_Headers(string string_0)
        {
            string @string = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (string_0.StartsWith(@string))
            {
                string_0 = string_0.Remove(0, @string.Length);
            }
            if (!string_0.StartsWith("<"))
            {
                string_0 = "<" + string_0;
            }
            if (string_0.StartsWith("<?xml"))
            {
                string_0 = Strings.Right(string_0, Strings.Len(string_0) - Strings.InStr(string_0, ">"));
            }
            return string_0;
        }

        public static byte[] CleanUpXML_Headers(byte[] byte_0)
        {
            int num = byte_0.Length - 1;
            int num2 = default(int);
            for (int i = 0; i <= num; i++)
            {
                if (Operators.CompareString(Conversions.ToString(Convert.ToChar(byte_0[i])), ">", TextCompare: false) == 0)
                {
                    num2 = i;
                    break;
                }
            }
            byte[] array = new byte[byte_0.Length - (num2 + 2) + 1];
            byte[] result;

            Array.Copy(byte_0, num2 + 1, array, 0, array.Length);
            byte_0 = array;
            result = byte_0;

            return result;
        }

        public static string CleanUpXML_IllegalCharacters(char[] char_0)
        {
            int toExclusive = char_0.Length;
            char int_;
            Parallel.For(0, toExclusive, [SpecialName] (int i) =>
            {
                int_ = char_0[i];
                if (!smethod_1(int_))
                {
                    char_0[i] = '\0';
                }
            });
            return char_0.ToString();
        }

        public static byte[] CleanUpXML_IllegalCharacters(byte[] byte_0)
        {
            int toExclusive = byte_0.Length;
            char int_;
            Parallel.For(0, toExclusive, [SpecialName] (int i) =>
            {
                int_ = Convert.ToChar(byte_0[i]);
                if (!smethod_1(int_))
                {
                    byte_0[i] = 0;
                }
            });
            return byte_0;
        }

        private static bool smethod_1(int int_0)
        {
            if (int_0 != 9 && int_0 != 10 && int_0 != 13 && (int_0 < 32 || int_0 > 55295) && (int_0 < 57344 || int_0 > 65533))
            {
                if (int_0 >= 65536)
                {
                    return int_0 <= 1114111;
                }
                return false;
            }
            return true;
        }
    }

    // [Serializable]
    public class ScenContainer
    {
        public string ScenTitle;
        public string ScenDescription;
        public int Complexity;
        public int Difficulty;
        public int ScenDate;
        public string string_0;
        public byte[] Scenario_Compressed;
        public int CompressVersion;
        public string BuildNumber;
        public string Version;
        public bool IsCampaiganCheckpoint;
        public long SaveCurrentTime;

        public string GetScenarioObject_AsXML()
        {
            string result = "";

            switch (CompressVersion)
            {
                /*
                case 1:
                    result = method_0();
                    break;
                case 2:
                    result = method_1();
                    break;
                case 3:
                    result = Crypto.DecryptStringAES(method_0(), GameGeneral.string_3);
                    break;
                */
                case 5:
                    result = DecompressScenarioObjectToXML_LZ();
                    break;
            }
            return result;
        }

        public string DecompressScenarioObjectToXML_LZ()
        {
            // using MemoryTributary memoryTributary = new MemoryTributary(Scenario_Compressed);
            using MemoryStream memoryTributary = new MemoryStream(Scenario_Compressed);
            using MemoryStream memoryStream = RCMS.recyclableMemoryStreamManager_0.GetStream();
            RijndaelManaged rijndaelManaged = smethod_0();
            byte[] array = new byte[4];
            memoryTributary.Read(array, 0, array.Length);
            byte[] array2 = new byte[BitConverter.ToInt32(array, 0) - 1 + 1];
            memoryTributary.Read(array2, 0, array2.Length);
            rijndaelManaged.IV = array2;
            ICryptoTransform transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
            using (CryptoStream innerStream = new CryptoStream(memoryTributary, transform, CryptoStreamMode.Read))
            {
                using LZ4Stream lZ4Stream = new LZ4Stream(innerStream, LZ4StreamMode.Decompress, LZ4StreamFlags.IsolateInnerStream);
                lZ4Stream.CopyTo(memoryStream);
            }
            memoryStream.Seek(0L, SeekOrigin.Begin);
            byte[] byte_ = memoryStream.ToArray();
            byte_ = Misc.CleanUpXML_Headers(byte_);
            byte_ = Misc.CleanUpXML_IllegalCharacters(byte_);
            return Encoding.UTF8.GetString(byte_);
        }

        private static RijndaelManaged smethod_0()
        {
            byte[] bytes = Encoding.ASCII.GetBytes("Eg:ù2à[kÝB{ãÞ¬KîâÉ{µ\\µ¥¤4\u00b4J»ãvaWÀó±òÈV:£W-(Èª|cêI¹");
            string password = "âcI}\u00b4EjÆãoµËÛÞwÿë6ØçÌP«4lWT¶-áòêªÓb¶þ×r2Z,¬}¶üÿTYá^¦\u00afH%ÿºÂOð=_Û^&¬oÚýª~ÁtÂRëg{Ñ§kA«Õº½Ë¥PÊ+jbo<_ù\u00a8xKUíTïBGÙøªçäð(tX`íÅuÉù³l(¶WØ$ëèw¬¦ÇJ|Z©*.¼ÏÒ_<·UP=W²üßå3ÂOºÃo_¤«ì8G2¶/R¬mo;>YwsÿJS}W£1ãC?ÍREÚâÙLK¬ä%¡X>ÀZÉUïdp¿o/uÕfé>ÄC©ñ)á6T~åÜ¡9>/ÚÑ«Wl£ÈÄëw{úupU®1ìU%0µõ\u00a8";
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, bytes);
            RijndaelManaged rijndaelManaged = null;
            rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.KeySize = 256;
            rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes((int)Math.Round((double)rijndaelManaged.KeySize / 8.0));
            return rijndaelManaged;
        }
    }

    public class Crypto
    {
        private static byte[] byte_0;

        private static string string_0;

        public static string string_1;

        private static object object_0;

        static Crypto()
        {
            byte_0 = Encoding.ASCII.GetBytes("o6806642kbM7c5");
            string_0 = "6b887c5ac993e7ae98c4c08e19f56429fdeb440755a4701b-7e80-4e57-9b96-3e9bee544da1942448a6-3112-4975-a68c-68782c80c0af";
            string_1 = "EBA66B7C-B09A-4EE0E860AE-410C-410D-8E7E-0AC92423D79F8F-8CD3-4BC7C54842BD8CC4DF32-BAAC-4C5F-8120-FD02B6131532";
            object_0 = RuntimeHelpers.GetObjectValue(new object());
        }

        private static byte[] smethod_0(Stream stream_0)
        {
            byte[] array = new byte[4];
            if (stream_0.Read(array, 0, array.Length) != array.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }
            byte[] array2 = new byte[BitConverter.ToInt32(array, 0) - 1 + 1];
            if (stream_0.Read(array2, 0, array2.Length) != array2.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }
            return array2;
        }

        public static string DecryptStringAES(string cipherText, string sharedKey = "")
        {
            if (!string.IsNullOrEmpty(cipherText))
            {
                if (string.IsNullOrEmpty(sharedKey) && string.IsNullOrEmpty(sharedKey))
                {
                    sharedKey = string_0;
                }
                RijndaelManaged rijndaelManaged = null;
                string result = null;
                try
                {
                    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(sharedKey, byte_0);
                    byte[] source = Convert.FromBase64String(cipherText);
                    // using MemoryTributary memoryTributary = new MemoryTributary(source);
                    using MemoryStream memoryTributary = new MemoryStream(source);
                    rijndaelManaged = new RijndaelManaged();
                    rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes((int)Math.Round((double)rijndaelManaged.KeySize / 8.0));
                    rijndaelManaged.IV = smethod_0(memoryTributary);
                    ICryptoTransform transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                    using CryptoStream stream = new CryptoStream(memoryTributary, transform, CryptoStreamMode.Read);
                    using StreamReader streamReader = new StreamReader(stream);
                    result = streamReader.ReadToEnd();
                }
                catch (Exception projectError)
                {
                    ProjectData.SetProjectError(projectError);
                    _ = Debugger.IsAttached;
                    ProjectData.ClearProjectError();
                }
                finally
                {
                    rijndaelManaged?.Clear();
                }
                return result;
            }
            throw new ArgumentNullException("cipherText");
        }
    }

    public class CMOScenTool
    {
        public static ScenContainer LoadScenContainer(string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ScenContainer));
            StringReader stringReader = new StringReader(xml);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            ScenContainer obj = (ScenContainer)xmlSerializer.Deserialize(xmlTextReader);
            xmlTextReader.Close();
            stringReader.Close();

            return obj;
        }

        public static string GetDecryptedDescription(string xml)
        {
            XDocument xmlDoc = XDocument.Parse(xml);
            var descriptionEncryptedElements = xmlDoc.Descendants("Description_Encrypted");
            var sv = descriptionEncryptedElements.First().Value;

            return Crypto.DecryptStringAES(sv, "DaltonTrumbo");
        }

        public static string GetDecryptedDescription(ScenContainer scenContainer) => GetDecryptedDescription(scenContainer.GetScenarioObject_AsXML());
    }
}
