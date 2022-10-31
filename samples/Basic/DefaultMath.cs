using MR.AttributeDI;

namespace Basic;

[AddToServices(Lifetime.Transient)]
[AddToServices(Lifetime.Transient, As = typeof(IMath))]
public class DefaultMath : IMath
{
	public int Add(int x, int y)
	{
		return x + y;
	}
}
