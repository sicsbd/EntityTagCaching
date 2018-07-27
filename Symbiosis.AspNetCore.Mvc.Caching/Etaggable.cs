using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.AspNetCore.Mvc
{
	public class Etaggable
		: IEtaggable
	{
		public string GetEtag(HashAlgorithm algorithm)
		{
			var json = JsonConvert.SerializeObject(this);
			var hashBytes = Encoding.UTF8.GetBytes(json);
			var builder = new StringBuilder();
			for (int i = 0; i < hashBytes.Length; i++)
			{
				builder.Append(hashBytes[i].ToString("x2"));
			}
			return builder.ToString();
		}
	}
}
