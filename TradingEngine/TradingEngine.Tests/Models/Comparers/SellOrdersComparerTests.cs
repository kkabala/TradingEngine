using FluentAssertions;
using NUnit.Framework;
using TradingEngine.Models.Comparers;
using TradingEngine.Tests.Models.Comparers.Base;

namespace TradingEngine.Tests.Models.Comparers;

[TestFixture]
public class SellOrdersComparerTests : BaseOrdersComparerTests
{
	[SetUp]
	public void SetUp()
	{
		Comparer = new SellOrdersComparer();
	}

	[TestCase(0, 1, -1)]
	[TestCase(1, 2, -1)]
	[TestCase(3, 2, 1)]
	[TestCase(5, 4, 1)]
	public void OrderWithLowerPrice_HasHigherPriority(int priceForOrder1, int priceForOrder2, int expectedResult)
	{
		//Arrange
		var order1 = CreateOrderMock(priceForOrder1);
		var order2 = CreateOrderMock(priceForOrder2);

		//Act
		var result = Comparer.Compare(order1, order2);

		//Assert
		result
			.Should()
			.Be(expectedResult);
	}
}
