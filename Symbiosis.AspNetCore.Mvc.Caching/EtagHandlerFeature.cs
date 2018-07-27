using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Microsoft.AspNetCore.Mvc.Filters
{
	internal class EtagHandlerFeature
		: IEtagHandlerFeature, IDisposable
	{
		private readonly HashAlgorithm _hashAlgorithm;
		private readonly IHeaderDictionary _headers;

		public EtagHandlerFeature(
			HashAlgorithm hashAlgorithm,
			IHeaderDictionary headers)
		{
			_hashAlgorithm = hashAlgorithm;
			_headers = headers;
		}

		private bool CheckRequestHeader(IEtaggable data, CacheRequestHeaders header)
		{
			var headerName = header == CacheRequestHeaders.IfMatch ? "If-Match" : "If-None-Match";

			var eTag = StringValues.Empty;

			var headerHasValue = _headers.TryGetValue(headerName, out eTag);

			var entityEtag = data.GetEtag(_hashAlgorithm);

			switch (header)
			{
				case CacheRequestHeaders.IfMatch:
					if (!headerHasValue)
						return false;

					return eTag.Contains(entityEtag, StringComparer.OrdinalIgnoreCase);
				default:
					if (!headerHasValue)
						return true;
					return !eTag.Contains(entityEtag, StringComparer.OrdinalIgnoreCase);
			}

		}

		public bool Match(IEtaggable data)
			=> CheckRequestHeader(data, CacheRequestHeaders.IfMatch);

		public bool NoneMatch(IEtaggable data)
			=> CheckRequestHeader(data, CacheRequestHeaders.IfNoneMatch);

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					_hashAlgorithm.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~EtagHandlerFeature() {
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