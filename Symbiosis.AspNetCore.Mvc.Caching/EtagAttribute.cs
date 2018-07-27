using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Cryptography;

namespace Microsoft.AspNetCore.Mvc
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class EtagAttribute
		: Attribute, IFilterFactory, IDisposable
	{
		private HashAlgorithm _hashAlgorithm;
		public bool IsReusable => true;
		public HashAlgorithms HashAlgorithm { get; set; } = HashAlgorithms.SHA512;

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			switch (HashAlgorithm)
			{
				case HashAlgorithms.MD5:
					_hashAlgorithm = MD5.Create();
					break;
				case HashAlgorithms.SHA1:
					_hashAlgorithm = SHA1.Create();
					break;
				case HashAlgorithms.SHA256:
					_hashAlgorithm = SHA256.Create();
					break;
				case HashAlgorithms.SHA384:
					_hashAlgorithm = SHA384.Create();
					break;
				case HashAlgorithms.SHA512:
				default:
					_hashAlgorithm = SHA512.Create();
					break;
			}

			return new EtagHeaderFilter(_hashAlgorithm);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					_hashAlgorithm?.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~EtagAttribute() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
