using System;
using System.Security.Cryptography;
using System.Text;

namespace Maanfee.Dashboard.Core
{
#pragma warning disable SYSLIB0001

	//public static class EncryptionExtention
	//{
	//	private static readonly string key = "GordonMcqueen(MF)1982";

	//	public static string Encrypt(this string str)
	//	{
	//		byte[] EnctArray = UTF8Encoding.ASCII.GetBytes(str);
	//		byte[] SrctArray = UTF8Encoding.ASCII.GetBytes(key);

	//		TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
	//		//MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();

	//		//SrctArray = objcrpt.ComputeHash(UTF8Encoding.ASCII.GetBytes(key));
	//		//objcrpt.Clear();

	//		objt.Key = SrctArray;
	//		objt.Mode = CipherMode.ECB;
	//		objt.Padding = PaddingMode.PKCS7;
	//		ICryptoTransform crptotrns = objt.CreateEncryptor();

	//		byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);

	//		objt.Clear();

	//		return Convert.ToBase64String(resArray, 0, resArray.Length).HashData();
	//	}

	//	public static string Decrypt(this string str)
	//	{
	//		byte[] DrctArray = Convert.FromBase64String(str.DehashData());
	//		byte[] SrctArray = UTF8Encoding.UTF8.GetBytes(key);

	//		TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();

	//		//MD5CryptoServiceProvider objmdcript = new MD5CryptoServiceProvider();
	//		//SrctArray = objmdcript.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
	//		//objmdcript.Clear();

	//		objt.Key = SrctArray;
	//		objt.Mode = CipherMode.ECB;
	//		objt.Padding = PaddingMode.PKCS7;
	//		ICryptoTransform crptotrns = objt.CreateDecryptor();

	//		byte[] resArray = crptotrns.TransformFinalBlock(DrctArray, 0, DrctArray.Length);

	//		objt.Clear();

	//		return UTF8Encoding.UTF8.GetString(resArray);
	//	}

	//	// *********************************************************

	//	public static string HashData(this string str)
	//	{
	//		if (string.IsNullOrEmpty(str))
	//			return string.Empty;

	//		// To Hex
	//		Byte[] stringBytes = Encoding.UTF8.GetBytes(str);
	//		var sbBytes = new StringBuilder(stringBytes.Length * 2);
	//		foreach (byte b in stringBytes)
	//		{
	//			sbBytes.AppendFormat("{0:X2}", b);
	//		}

	//		return sbBytes.ToString();
	//	}

	//	public static string DehashData(this string str)
	//	{
	//		int numberChars = str.Length;
	//		byte[] bytes = new byte[numberChars / 2];
	//		for (int i = 0; i < numberChars; i += 2)
	//		{
	//			bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
	//		}

	//		return Encoding.UTF8.GetString(bytes);
	//	}

	//}

#pragma warning restore SYSLIB0001
}
