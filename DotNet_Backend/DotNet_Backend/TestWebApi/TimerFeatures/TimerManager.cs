namespace TestWebApi.TimerFeatures
{
	/// <summary>
	/// Represents timer used to periodically send random sensor values to the frontend to test the graph when
	/// actual device is not connected.
	/// </summary>
	public class TimerManager
	{
		private Timer? _timer;
		private AutoResetEvent? _autoResetEvent;
		private Action? _action;
		public DateTime TimerStarted { get; set; }
		public bool IsTimerStarted { get; set; }

		/// <summary>
		/// Prepares the timer to execute a specified action at regular intervals.
		/// </summary>
		/// <param name="action">The action to be executed by the timer.</param>
		public void PrepareTimer(Action action)
		{
			_action = action;
			_autoResetEvent = new AutoResetEvent(false);
			_timer = new Timer(Execute, _autoResetEvent, 1000, 1000);
			TimerStarted = DateTime.Now;
			IsTimerStarted = true;
		}

		/// <summary>
		/// Executes the specified action and manages the duration of the timer.
		/// </summary>
		/// <param name="stateInfo">The state information associated with the timer.</param>
		public void Execute(object? stateInfo)
		{
			_action();
			if ((DateTime.Now - TimerStarted).TotalSeconds > 600)
			{
				IsTimerStarted = false;
				_timer.Dispose();
			}
		}
	}
}
