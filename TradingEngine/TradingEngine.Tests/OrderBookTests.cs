using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TradingEngine.Enums;
using TradingEngine.Enums.Extensions;
using TradingEngine.Models;
using TradingEngine.Models.Orders;
using TradingEngine.Models.Orders.Interfaces;

namespace TradingEngine.Tests;

public class OrderBookTests
{
	private OrderBook _orderBook = null!;

	private readonly Dictionary<OrderSide, string> _orderSideToPrintoutStartLabelDictionary = new()
	{
		{ OrderSide.Buy, "Buy Orders:" },
		{ OrderSide.Sell, "Sell Orders:" }
	};

	[SetUp]
	public void Setup()
	{
		var buyOrdersEqualityComparer = CreateEqualityAndSequenceComparer();
		var sellOrdersEqualityComparer = CreateEqualityAndSequenceComparer();

		_orderBook = new OrderBook(
			buyOrdersEqualityComparer,
			sellOrdersEqualityComparer);
	}

	private IComparer<IOrder> CreateEqualityAndSequenceComparer()
	{
		var baseOrdersComparer = new Mock<IEqualityComparer<IOrder>>();
		baseOrdersComparer
			.Setup(m => m.Equals(It.IsAny<IOrder>(), It.IsAny<IOrder>()))
			.Returns(() => false);

		var ordersEqualityComparer = baseOrdersComparer.As<IComparer<IOrder>>();
		ordersEqualityComparer
			.Setup(m => m.Compare(It.IsAny<IOrder>(), It.IsAny<IOrder>()))
			.Returns(-1);

		return ordersEqualityComparer.Object;
	}

	[Test]
	public void BuyOrdersAreIncludedInPrintout()
	{
		OrdersOfTargetSideAreIncludedInPrintout(OrderSide.Buy);
	}

	private void OrdersOfTargetSideAreIncludedInPrintout(OrderSide side)
	{
		//Arrange
		string lineStartLabel = _orderSideToPrintoutStartLabelDictionary[side];
		var ordersMocks = AddTestOrdersToOrderBook(side);

		//Act
		var lineWithSpecifiedOrderSide = GetPrintoutForOrderSide(side);


		//Assert
		lineWithSpecifiedOrderSide
			.Should()
			.ContainAll(lineStartLabel);

		foreach (var order in ordersMocks)
			order.Verify(o => o.ToString(), Times.Once);
	}

	private List<Mock<IOrder>> AddTestOrdersToOrderBook(OrderSide side)
	{
		var order1 = CreateOrderMock("bb", side, 2, 1);
		var order2 = CreateOrderMock("bc", side, 1, 1);
		var order3 = CreateOrderMock("dd", side, 3, 1);

		_orderBook.Add(order1.Object);
		_orderBook.Add(order2.Object);
		_orderBook.Add(order3.Object);
		return new List<Mock<IOrder>> { order1, order2, order3 };
	}

	private string GetPrintoutForOrderSide(OrderSide side)
	{
		string lineStartLabel = _orderSideToPrintoutStartLabelDictionary[side];
		string result = _orderBook.ToString();
		string lineWithSpecifiedOrderSide = result
			.Split(Environment.NewLine)
			.Single(m => m.StartsWith(lineStartLabel));

		return lineWithSpecifiedOrderSide;
	}

	[Test]
	public void SellOrdersAreNotIncludedInBuyPrintout()
	{
		OrdersOfOppositeDirectionAreNotIncludedIntoPrintout(OrderSide.Buy);
	}

	private void OrdersOfOppositeDirectionAreNotIncludedIntoPrintout(OrderSide side)
	{
		AddTestOrdersToOrderBook(side);

		string oppositeSideOrderId = "oppositeSideOrder_5555";
		var oppositeSideOrder = CreateOrderMock(oppositeSideOrderId, side.ToOppositeDirection(), 9, 99);
		oppositeSideOrder
			.Setup(m => m.ToString())
			.Returns(oppositeSideOrderId);
		_orderBook.Add(oppositeSideOrder.Object);

		//Act
		var sidePrintout = GetPrintoutForOrderSide(side);

		//Assert
		sidePrintout
			.Should()
			.NotContain(oppositeSideOrderId);
	}

	[Test]
	public void BuyOrdersAreNotIncludedInSellPrintout()
	{
		OrdersOfOppositeDirectionAreNotIncludedIntoPrintout(OrderSide.Sell);
	}

	[Test]
	public void SellOrdersAreIncludedInPrintout()
	{
		OrdersOfTargetSideAreIncludedInPrintout(OrderSide.Sell);
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

	[Test]
	public void LimitOrderIsAddedToOrderBook_WhenThereAreNoOrdersOfOppositeDirection([Values] OrderSide side)
	{
		//Arrange
		var order = new LimitOrder("1", side, 1, 2);

		//Act
		_orderBook.Process(order);

		//Assert
		CheckIfParticularSideForOrderBookContainsTheOrder(order);
	}

	private void CheckIfParticularSideForOrderBookContainsTheOrder(IOrder order)
	{
		var result = _orderBook.GetAll(order.Side);

		//Cannot use "Contains()" without mocking comparer even further
		result
			.Any(m => m.GetHashCode() == order.GetHashCode())
			.Should()
			.BeTrue();
	}

	[Test]
	public void OrdersCanBeRetrievedCorrectlyForTheSideSpecified([Values] OrderSide side)
	{
		//Arrange
		var orderMock = CreateOrderMock("order123", side, 5, 10);
		var order = orderMock.Object;
		_orderBook.Add(order);

		//Act && Assert
		CheckIfParticularSideForOrderBookContainsTheOrder(order);
	}
}
