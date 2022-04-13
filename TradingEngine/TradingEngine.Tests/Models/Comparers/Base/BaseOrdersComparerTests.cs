using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TradingEngine.Models.Interfaces;

namespace TradingEngine.Tests.Models.Comparers.Base;

[TestFixture]
public abstract class BaseOrdersComparerTests
{
	private static object[] TestData =
	{
		new object[] { DateTime.Now.AddHours(-1), DateTime.Now, -1 },
		new object[] { DateTime.Now.AddSeconds(-3), DateTime.Now.AddSeconds(-1), -1 },
		new object[] { DateTime.MinValue, DateTime.MinValue, 0 },
		new object[] { DateTime.Now.AddHours(-1), DateTime.Now.AddDays(-1), 1 },
		new object[] { DateTime.Now.AddSeconds(-1), DateTime.Now.AddSeconds(1), 1 }
	};

	protected IComparer<IOrder> Comparer;

	[TestCase(nameof(TestData))]
	public void OrderCreatedEarlierHasHigherPriority_WhenPricesAreEqual(DateTime createDateForOrder1, DateTime createDateForOrder2, int expectedResult)
	{
		//Arrange
		var order1 = CreateOrderMock(createDateForOrder1);
		var order2 = CreateOrderMock(createDateForOrder2);

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

	private IOrder CreateOrderMock(DateTime createDate)
	{
		var orderMock = new Mock<IOrder>();
		orderMock
			.Setup(m => m.Created)
			.Returns(createDate);

		return orderMock.Object;
	}
}
