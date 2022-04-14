using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TradingEngine.Models.Comparers.Base;
using TradingEngine.Models.Interfaces;
using TradingEngine.Models.Orders.Interfaces;

namespace TradingEngine.Tests.Models.Comparers.Base;

[TestFixture]
public abstract class OrderDataSequenceComparerTests : OrderEqualityComparerTests
{
	static int _testOrderId;

	[SetUp]
	public void SetUp()
	{
		OrderEqualityComparer = CreateOrderDataSequenceComparer();
		Comparer = OrderEqualityComparer;
		_testOrderId = 0;
	}

	protected abstract OrderDataSequenceComparer CreateOrderDataSequenceComparer();

	protected static object[] TestDataForOrdersWithEqualPrices =
	{
		new object[] { DateTime.Now.AddHours(-1), DateTime.Now, -1 },
		new object[] { DateTime.Now.AddSeconds(-3), DateTime.Now.AddSeconds(-1), -1 },
		new object[] { DateTime.MinValue, DateTime.MinValue, 1 },
		new object[] { DateTime.Now.AddHours(-1), DateTime.Now.AddDays(-1), 1 },
		new object[] { DateTime.Now.AddSeconds(-1), DateTime.Now.AddSeconds(1), -1 }
	};

	protected IComparer<IOrder> Comparer;

	[TestCaseSource(nameof(TestDataForOrdersWithEqualPrices))]
	public void OrderCreatedEarlierHasHigherPriority_WhenPricesAreEqual(DateTime createDateForOrder1, DateTime createDateForOrder2, int expectedResult)
	{
		//Arrange
		var order1 = CreateOrderMockWithConstantPrice(createDateForOrder1);
		var order2 = CreateOrderMockWithConstantPrice(createDateForOrder2);

		//Act
		int result = Comparer.Compare(order1, order2);

		//Assert
		result
			.Should()
			.Be(expectedResult);
	}

	[Test]
	public void OrdersAreEqual_WhenBothHaveTheSameIds()
	{
		//Arrange
		var equalId = "1";
		var order1 = CreateOrderMockWithConstantPrice(DateTime.Now, equalId);
		var order2 = CreateOrderMockWithConstantPrice(DateTime.Now.AddDays(1), equalId);

		//Act
		int result = Comparer.Compare(order1, order2);

		//Assert
		result
			.Should()
			.Be(0);

	}

	protected IOrder CreateOrderMock(decimal price)
	{
		var orderMock = new Mock<IOrder>();
		orderMock
			.Setup(m => m.Price)
			.Returns(price);

		orderMock
			.Setup(m => m.Id)
			.Returns(GetNextOrderId);

		return orderMock.Object;
	}

	private string GetNextOrderId()
	{
		return _testOrderId++.ToString();
	}

	private IOrder CreateOrderMockWithConstantPrice(DateTime createDate, string id = "")
	{
		const decimal price = 2.1m;
		var orderMock = new Mock<IOrder>();
		orderMock
			.Setup(m => m.Created)
			.Returns(createDate);

		orderMock
			.Setup(m => m.Price)
			.Returns(price);

		var finalMockId = string.IsNullOrEmpty(id)
			? GetNextOrderId()
			: id;

		orderMock
			.Setup(m => m.Id)
			.Returns(finalMockId);

		return orderMock.Object;
	}
}
