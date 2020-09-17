/** Copyright 2010-2012 Twitter, Inc.*/

/**
 * An object that generates IDs.
 * This is broken into a separate class in case
 * we ever want to support multiple worker threads
 * per process
 */

using System;

namespace C3SockNetUtil.Snowflake
{
    public class IdWorker
    {
        public const long Twepoch = 1288834974657L; // base 시간

        const int WorkerIdBits = 5;
        const int DatacenterIdBits = 5;
        const int SequenceBits = 12;
        const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);

        private const int WorkerIdShift = SequenceBits;
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        public const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;
        private const long SequenceMask = -1L ^ (-1L << SequenceBits); // 1밀리세컨드 당 최대 생성 개수. 4095

        private long _sequence = 0L;
        private long _lastTimestamp = -1L;
        

        public IdWorker(long workerId, long datacenterId, long sequence = 0L) 
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            _sequence = sequence;
		
            // sanity check for workerId
            if (workerId > MaxWorkerId || workerId < 0) 
            {
                throw new ArgumentException( String.Format("worker Id can't be greater than {0} or less than 0", MaxWorkerId) );
            }

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException( String.Format("datacenter Id can't be greater than {0} or less than 0", MaxDatacenterId));
            }

            //log.info(
            //    String.Format("worker starting. timestamp left shift {0}, datacenter id bits {1}, worker id bits {2}, sequence bits {3}, workerid {4}",
            //                  TimestampLeftShift, DatacenterIdBits, WorkerIdBits, SequenceBits, workerId)
            //    );	
        }
	
        public long WorkerId {get; protected set;}
        public long DatacenterId {get; protected set;}

        //public long Sequence
        //{
        //    get { return _sequence; }
        //    internal set { _sequence = value; }
        //}

        // def get_timestamp() = System.currentTimeMillis

        readonly object _lock = new Object();
	
        public virtual long NextId() 
        {
            long newId = 0;

            lock(_lock) 
            {
                var timestamp = TimeGen();

                // 컴퓨터의 시간이 뒤로 간 상태이다. 서버의 시간 동기화 모드 중 step으로 하면 이럴 수 있다. 꼭 slew 모드로 하는 것이 좋다.                
                if (timestamp < _lastTimestamp) 
                {
                    // 일단 여기서는 시간이 뒤로 간 상태에서 1밀리세컨드 당 만들 수 있는 Id 수에 여유가 있다면 바로 앞에 사용한 시간과 _sequence를 사용하여 만든다.
                    if ( _sequence < (SequenceMask-1))
                    {
                        _sequence = (_sequence + 1) & SequenceMask;
                        newId = GenerateId(_lastTimestamp, _sequence);
                        return newId;
                    }

                    //exceptionCounter.incr(1);
                    //log.Error("clock is moving backwards.  Rejecting requests until %d.", _lastTimestamp);
                    var diffStamp = _lastTimestamp - timestamp;
                    throw new InvalidSystemClock($"Clock moved backwards.  Refusing to generate id for {diffStamp} milliseconds");
                }

                if (_lastTimestamp == timestamp) 
                {
                    _sequence = (_sequence + 1) & SequenceMask;
                    if (_sequence == 0) 
                    {
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }

                _lastTimestamp = timestamp;
                newId = GenerateId(timestamp, _sequence);                 
                return newId;
            }
        }

        long GenerateId(long timestamp, long sequence)
        {
            var id = ((timestamp - Twepoch) << TimestampLeftShift) |
                     (DatacenterId << DatacenterIdShift) |
                     (WorkerId << WorkerIdShift) | _sequence;
            return id;
        }

        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp) 
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        protected virtual long TimeGen()
        {
            return System.CurrentTimeMillis();
        }      
    }


    public class DisposableAction : IDisposable
    {
        readonly Action _action;

        public DisposableAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }


    public class InvalidSystemClock : Exception
    {
        public InvalidSystemClock(string message) : base(message) { }
    }


    public static class System
    {
        public static Func<long> currentTimeFunc = InternalCurrentTimeMillis;

        public static long CurrentTimeMillis()
        {
            return currentTimeFunc();
        }

        public static IDisposable StubCurrentTime(Func<long> func)
        {
            currentTimeFunc = func;
            return new DisposableAction(() =>
            {
                currentTimeFunc = InternalCurrentTimeMillis;
            });
        }

        public static IDisposable StubCurrentTime(long millis)
        {
            currentTimeFunc = () => millis;
            return new DisposableAction(() =>
            {
                currentTimeFunc = InternalCurrentTimeMillis;
            });
        }

        private static readonly DateTime Jan1st1970 = new DateTime
           (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long InternalCurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}