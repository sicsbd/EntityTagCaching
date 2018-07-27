using System.Security.Cryptography;

namespace Microsoft.AspNetCore.Mvc
{
	public interface IEtaggable
	{
		string GetEtag(HashAlgorithm algorithm);
	}
}