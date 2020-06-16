using System;
using System.IO;
using System.Security.Cryptography;

namespace Framework.Utils
{
	public class MD5Util
	{
		public static string Get(FileStream fileStream)
		{
			string md5 = BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(fileStream))
				.Replace("-", "");
		
			fileStream.Close();
		
			return md5;
		}

		public static string Get(string filePath)
		{
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			catch (Exception e)
			{
				BuglyAgent.ReportException("Md5 Get:", "filePath->"+filePath, e.StackTrace);
				fileStream?.Close();
				return "";
			}
			
			return Get(fileStream);
		}
	
	}
}

