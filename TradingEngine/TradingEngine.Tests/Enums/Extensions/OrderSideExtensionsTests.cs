using FluentAssertions;
using NUnit.Framework;
using TradingEngine.Enums;
using TradingEngine.Enums.Extensions;

namespace TradingEngine.Tests.Enums.Extensions;

[TestFixture]
public class OrderSideExtensionsTests
{
	[TestCase(OrderSide.Buy, OrderSide.Sell)]
	[TestCase(OrderSide.Sell, OrderSide.Buy)]
	public void OrderSideCanBeCorrectlyTransformedToTheOppositeDirection(OrderSide orderSide, OrderSide expectedTransformationResult)
	{
		orderSide
			.ToOppositeDirection()
			.Should()
			.Be(expectedTransformationResult);
	}
}
