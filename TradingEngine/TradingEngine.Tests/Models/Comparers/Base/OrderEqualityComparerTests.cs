using FluentAssertions;
using Moq;
using NUnit.Framework;
using TradingEngine.Models.Comparers;
using TradingEngine.Models.Comparers.Base;
using TradingEngine.Models.Interfaces;
using TradingEngine.Models.Orders.Interfaces;

namespace TradingEngine.Tests.Models.Comparers.Base;

public abstract class OrderEqualityComparerTests
{
	protected OrderEqualityComparer OrderEqualityComparer;

	[Test]
	public void OrdersAreEqual_WhenIdsAreTheSame()
	{
		//Arrange
		const string id = "1";
		var orderMock1 = CreateBasicOrderMock(id);

		var orderMock2 = CreateBasicOrderMock(id);

		//Act
		bool result = OrderEqualityComparer.Equals(orderMock1, orderMock2);

		//Assert
		result
			.Should()
			.BeTrue();
	}

	[Test]
	public void OrdersAreNotEqual_WhenOneOfOrdersIsNull()
	{
		//Arrange
		const string id = "1";
		var orderMock1 = CreateBasicOrderMock(id);

		//Act
		bool result = OrderEqualityComparer.Equals(orderMock1, null);

		//Assert
		result
			.Should()
			.BeFalse();
	}

	[Test]
	public void OrdersAreEqual_WhenBothOfOrdersAreNull()
	{
		//Arrange

		//Act
		bool result = OrderEqualityComparer.Equals(null, null);

		//Assert
		result
			.Should()
			.BeTrue();
	}

	private IOrder CreateBasicOrderMock(string id)
	{
		var orderMock1 = new Mock<IOrder>();
		orderMock1
			.Setup(m => m.Id)
			.Returns(id);

		return orderMock1.Object;
	}
}
