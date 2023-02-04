using System;
using System.Threading;

namespace Maanfee.Web.Core.WebSockets
{
	public class Statistics
	{
		private DateTime _StartTime = DateTime.Now.ToUniversalTime();

		private long _ReceivedBytes;

		private long _ReceivedMessages;

		private long _SentBytes;

		private long _SentMessages;

		public DateTime StartTime => _StartTime;

		public TimeSpan UpTime => DateTime.Now.ToUniversalTime() - _StartTime;

		public long ReceivedBytes => _ReceivedBytes;

		public long ReceivedMessages => _ReceivedMessages;

		public int ReceivedMessageSizeAverage
		{
			get
			{
				if (_ReceivedBytes > 0 && _ReceivedMessages > 0)
				{
					return (int)(_ReceivedBytes / _ReceivedMessages);
				}
				return 0;
			}
		}

		public long SentBytes => _SentBytes;

		public long SentMessages => _SentMessages;

		public decimal SentMessageSizeAverage
		{
			get
			{
				if (_SentBytes > 0 && _SentMessages > 0)
				{
					return (int)(_SentBytes / _SentMessages);
				}
				return 0m;
			}
		}

		public override string ToString()
		{
			return "--- Statistics ---" + Environment.NewLine + "    Started     : " + _StartTime.ToString() + Environment.NewLine + "    Uptime      : " + UpTime.ToString() + Environment.NewLine + "    Received    : " + Environment.NewLine + "       Bytes    : " + ReceivedBytes + Environment.NewLine + "       Messages : " + ReceivedMessages + Environment.NewLine + "       Average  : " + ReceivedMessageSizeAverage + " bytes" + Environment.NewLine + "    Sent        : " + Environment.NewLine + "       Bytes    : " + SentBytes + Environment.NewLine + "       Messages : " + SentMessages + Environment.NewLine + "       Average  : " + SentMessageSizeAverage + " bytes" + Environment.NewLine;
		}

		public void Reset()
		{
			_ReceivedBytes = 0L;
			_ReceivedMessages = 0L;
			_SentBytes = 0L;
			_SentMessages = 0L;
		}

		internal void IncrementReceivedMessages()
		{
			_ReceivedMessages = Interlocked.Increment(ref _ReceivedMessages);
		}

		internal void IncrementSentMessages()
		{
			_SentMessages = Interlocked.Increment(ref _SentMessages);
		}

		internal void AddReceivedBytes(long bytes)
		{
			_ReceivedBytes = Interlocked.Add(ref _ReceivedBytes, bytes);
		}

		internal void AddSentBytes(long bytes)
		{
			_SentBytes = Interlocked.Add(ref _SentBytes, bytes);
		}
	}
}
