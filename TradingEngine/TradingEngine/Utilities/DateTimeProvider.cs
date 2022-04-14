using TradingEngine.Utilities.Interfaces;

namespace TradingEngine.Utilities;

public class DateTimeProvider : IDateTimeProvider
{
	public DateTime Now => DateTime.Now;

	private static IDateTimeProvider? _instance;

	public static IDateTimeProvider? Instance
	{
		get
		{
			_instance ??= new DateTimeProvider();
			return _instance;
		}
		set => _instance = value;
	}
}
