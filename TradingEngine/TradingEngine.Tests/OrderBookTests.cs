using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TradingEngine.Enums;
using TradingEngine.Models;
using TradingEngine.Models.Interfaces;

namespace TradingEngine.Tests;

public class OrderBookTests
{
	private OrderBook _orderBook;

	[SetUp]
	public void Setup()
	{
		var orderEqualityComparerMock = new Mock<IEqualityComparer<IOrder>>();
		orderEqualityComparerMock
			.Setup(m => m.Equals(It.IsAny<IOrder>(), It.IsAny<IOrder>()))
			.Returns(false);

		var buyOrdersComparer = new Mock<IComparer<IOrder>>();
		var sellOrdersComparer = new Mock<IComparer<IOrder>>();

		_orderBook = new OrderBook(
			orderEqualityComparerMock.Object,
			buyOrdersComparer.Object,
			sellOrdersComparer.Object);
	}

	[Test]
	public void BuyOrdersWithHigherPriceAreOfHigherPriority()
	{
		//Arrange
		var order1 = CreateOrderMock("bb", OrderSide.Buy, 2, 1);
		var order2 = CreateOrderMock("bc", OrderSide.Buy, 1, 1);
		var order3 = CreateOrderMock("dd", OrderSide.Buy, 3, 1);

		_orderBook.Add(order1);
		_orderBook.Add(order2);
		_orderBook.Add(order3);

		//Act
		string result = _orderBook.GeneratePrintout();

		//Assert
		result
			.Should()
			.Be($"{order3.Id}\n{order1.Id}\n{order2.Id}");
	}

	private IOrder CreateOrderMock(string orderId, OrderSide orderSide, int price, int quantity)
	{
		var mock = new Mock<IOrder>();
		mock
			.Setup(m => m.Id)
			.Returns(orderId);
		mock
			.Setup(m => m.Side)
			.Returns(orderSide);
		mock
			.Setup(m => m.Price)
			.Returns(price);
		mock
			.Setup(m => m.Quantity)
			.Returns(quantity);
		mock
			.Setup(m => m.ToString())
			.Returns(() => mock.Object.Id);

		return mock.Object;
	}
}
