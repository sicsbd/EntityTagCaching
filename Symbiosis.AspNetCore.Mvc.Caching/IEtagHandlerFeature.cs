namespace Microsoft.AspNetCore.Mvc
{
	public interface IEtagHandlerFeature
	{
		bool NoneMatch(IEtaggable data);
		bool Match(IEtaggable data);
	}
}