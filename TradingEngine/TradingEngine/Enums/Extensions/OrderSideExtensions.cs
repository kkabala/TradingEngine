namespace TradingEngine.Enums.Extensions;

public static class OrderSideExtensions
{
	public static OrderSide ToOppositeDirection(this OrderSide side)
		=> side == OrderSide.Buy ? OrderSide.Sell : OrderSide.Buy;
}
