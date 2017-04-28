namespace PrimordialOoze
{
	public interface IInjectable
	{
		bool CanBeInjectedBy(Microbe injector);
		void CompleteInjection(Microbe injector);
	}
}
