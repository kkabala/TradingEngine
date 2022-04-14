using System;
using System.Collections.Generic;
using System.Linq;
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
		var baseBuyOrdersComparer = new Mock<IEqualityComparer<IOrder>>();
		baseBuyOrdersComparer
			.Setup(m => m.Equals(It.IsAny<IOrder>(), It.IsAny<IOrder>()))
			.Returns(() => false);

		var buyOrdersEqualityComparer = baseBuyOrdersComparer.As<IComparer<IOrder>>();
		buyOrdersEqualityComparer
			.Setup(m => m.Compare(It.IsAny<IOrder>(), It.IsAny<IOrder>()))
			.Returns(-1);

		var baseSellOrdersComparer = new Mock<IEqualityComparer<IOrder>>();
		baseSellOrdersComparer
			.Setup(m => m.Equals(It.IsAny<IOrder>(), It.IsAny<IOrder>()))
			.Returns(false);

		var sellOrdersEqualityComparer = baseSellOrdersComparer.As<IComparer<IOrder>>();

		_orderBook = new OrderBook(
			buyOrdersEqualityComparer.Object,
			sellOrdersEqualityComparer.Object);
	}

	[Test]
	public void BuyOrdersAreIncludedInPrintout()
	{
		OrdersOfTargetSideAreIncludedInPrintout(OrderSide.Buy, "Buy Orders:");
	}

	private void OrdersOfTargetSideAreIncludedInPrintout(OrderSide side, string lineStartLabel)
	{
		//Arrange
		var order1 = CreateOrderMock("bb", side, 2, 1);
		var order2 = CreateOrderMock("bc", side, 1, 1);
		var order3 = CreateOrderMock("dd", side, 3, 1);
		var ordersMocks = new List<Mock<IOrder>>{ order1, order2, order3 };

		_orderBook.Add(order1.Object);
		_orderBook.Add(order2.Object);
		_orderBook.Add(order3.Object);

		//Act
		string result = _orderBook.ToString();
		string lineWithSpecifiedOrderSide = result
			.Split(Environment.NewLine)
			.Single(m => m.StartsWith(lineStartLabel));

		//Assert
		lineWithSpecifiedOrderSide
			.Should()
			.ContainAll(lineStartLabel);

		foreach (var order in ordersMocks)
			order.Verify(o => o.ToString(), Times.Once);
	}

	[Test]
	public void SellOrdersAreNotIncludedInBuyPrintout()
	{
		//Arrange


		//Act


		//Assert

	}

	[Test]
	public void SellOrdersAreIncludedInPrintout()
	{
		OrdersOfTargetSideAreIncludedInPrintout(OrderSide.Sell, "Sell Orders:");
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
