using System;
using System.Numerics;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TradingEngine.Enums;
using TradingEngine.Models;
using TradingEngine.Models.Orders;
using TradingEngine.Utilities;
using TradingEngine.Utilities.Interfaces;

namespace TradingEngine.Tests.Models;

[TestFixture]
public class LimitOrdersTest
{
	[Test]
	public void ArgumentsPassedInConstructorArePassedToProperties()
	{
		//Arrange
		string id = "1";
		OrderSide side = OrderSide.Sell;
		decimal price = 5.3m;
		decimal quantity = 10.1m;

		//Act
		var result = new LimitOrder(id, side, price, quantity);

		//Assert
		result
			.Id
			.Should()
			.Be(id);
		result
			.Side
			.Should()
			.Be(side);
		result
			.Price
			.Should()
			.Be(price);
		result
			.Quantity
			.Should()
			.Be(quantity);
	}

	[Test]
	public void CurrentDateFromDateProviderIsSet_WhenNewOrderIsCreated()
	{
		//Arrange
		var testDate = DateTime.Now.AddDays(-3);
		var dateTimeProviderMock = new Mock<IDateTimeProvider>();

		dateTimeProviderMock
			.Setup(m => m.Now)
			.Returns(testDate);

		DateTimeProvider.Instance = dateTimeProviderMock.Object;

		//Act
		var limitOrder = new LimitOrder("1", OrderSide.Sell, 1, 2);

		//Assert
		limitOrder
			.Created
			.Should()
			.Be(testDate);

	}
}
