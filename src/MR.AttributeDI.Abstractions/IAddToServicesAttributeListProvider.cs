namespace MR.AttributeDI;

public interface IAddToServicesAttributeListProvider
{
	List<(Type Implementation, IEnumerable<AddToServicesAttribute> Attributes)> GetAttributes();
}
