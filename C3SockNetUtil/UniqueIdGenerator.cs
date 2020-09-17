using System;
using System.Collections.Generic;
using System.Text;

namespace C3SockNetUtil
{
    public interface IUniqueIdGenerator
    {
        UInt64 NextId();
    }


    public class UniqueIdGenSimple : IUniqueIdGenerator
    {
        Int64 Sequence = 0;

        public UInt64 NextId()
        {
            var id = System.Threading.Interlocked.Increment(ref Sequence);
            return (UInt64)id;
        }
    }



    public class UniqueIdGenSnowflake : IUniqueIdGenerator
    {
        C3SockNetUtil.Snowflake.IdWorker Generator;

        public void Init(long workId, long datacenterId)
        {
            Generator = new C3SockNetUtil.Snowflake.IdWorker(workId, datacenterId);
        }

        public UInt64 NextId()
        {
            return (UInt64)Generator.NextId();
        }
    }
}
