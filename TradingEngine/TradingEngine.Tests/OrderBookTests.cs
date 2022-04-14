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
	public void BuyOrdersAreIncludedInPrintout()
	{
		//Arrange
		var order1 = CreateOrderMock("bb", OrderSide.Buy, 2, 1);
		var order2 = CreateOrderMock("bc", OrderSide.Buy, 1, 1);
		var order3 = CreateOrderMock("dd", OrderSide.Buy, 3, 1);
		var ordersMocks = new List<Mock<IOrder>>{ order1, order2, order3 };

		_orderBook.Add(order1.Object);
		_orderBook.Add(order2.Object);
		_orderBook.Add(order3.Object);

		const string buyLabel = "Buy Orders:";

		//Act
		string result = _orderBook.ToString();

		//Assert
		result
			.Should()
			.ContainAll(buyLabel);

		foreach (var order in ordersMocks)
			order.Verify(o => o.ToString(), Times.Once);
	}

	private Mock<IOrder> CreateOrderMock(string orderId, OrderSide orderSide, int price, int quantity)
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

		return mock;
	}
}
