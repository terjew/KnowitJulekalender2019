using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Common
{
	//http://freshclickmedia.co.uk/2013/11/net-stopwatch-meet-idisposable/
	public sealed class CodeTimer : IDisposable
	{
		private readonly Stopwatch _stopwatch;
		private readonly Action<Stopwatch> _action;

		public CodeTimer(string what, Action<Stopwatch> action = null, int count = 1)
		{
			_action = action ?? (s => Console.WriteLine($"(Kjørte {what} {count} gang{(count > 1 ? "er" : "")} på {s.ElapsedMilliseconds} ms, snitt {(s.Elapsed.TotalMilliseconds / count):0.00} ms per kjøring)"));
			_stopwatch = new Stopwatch();
			_stopwatch.Start();
		}

		public void Dispose()
		{
			_stopwatch.Stop();
			_action(_stopwatch);
		}

		public Stopwatch Watch
		{
			get { return _stopwatch; }
		}
	}
}
