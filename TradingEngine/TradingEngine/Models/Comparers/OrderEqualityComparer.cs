using TradingEngine.Models.Interfaces;

namespace TradingEngine.Models.Comparers;

public class OrderEqualityComparer : IEqualityComparer<IOrder>
{
	public bool Equals(IOrder? x, IOrder? y) => x?.Id == y?.Id;

	public int GetHashCode(IOrder obj)
	{
		return HashCode.Combine(obj.Id, (int)obj.Side, obj.Price, obj.Quantity);
	}
}
