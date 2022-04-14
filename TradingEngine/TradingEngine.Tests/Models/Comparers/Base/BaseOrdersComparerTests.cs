using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TradingEngine.Models.Interfaces;

namespace TradingEngine.Tests.Models.Comparers.Base;

[TestFixture]
public abstract class BaseOrdersComparerTests
{
	protected static object[] TestData =
	{
		new object[] { DateTime.Now.AddHours(-1), DateTime.Now, -1 },
		new object[] { DateTime.Now.AddSeconds(-3), DateTime.Now.AddSeconds(-1), -1 },
		new object[] { DateTime.MinValue, DateTime.MinValue, 0 },
		new object[] { DateTime.Now.AddHours(-1), DateTime.Now.AddDays(-1), 1 },
		new object[] { DateTime.Now.AddSeconds(-1), DateTime.Now.AddSeconds(1), -1 }
	};

	protected IComparer<IOrder> Comparer;

	[TestCaseSource(nameof(TestData))]
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

	protected IOrder CreateOrderMock(decimal price)
	{
		var orderMock = new Mock<IOrder>();
		orderMock
			.Setup(m => m.Price)
			.Returns(price);

		return orderMock.Object;
	}

	private IOrder CreateOrderMockWithConstantPrice(DateTime createDate)
	{
		const decimal price = 2.1m;
		var orderMock = new Mock<IOrder>();
		orderMock
			.Setup(m => m.Created)
			.Returns(createDate);

		orderMock
			.Setup(m => m.Price)
			.Returns(price);

		return orderMock.Object;
	}
}
