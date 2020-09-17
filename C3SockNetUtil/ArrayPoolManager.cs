using System;
using System.Collections.Generic;
using System.Text;

namespace C3SockNetUtil
{
    public class ArrayPoolManager
    {
        System.Buffers.ArrayPool<byte> Pool;
        int MaxArraySize;
        int BucketCount;

        public void Init(int maxBufferSize, int bucketCount)
        {
            MaxArraySize = maxBufferSize;
            BucketCount = bucketCount;

            Pool = System.Buffers.ArrayPool<byte>.Create(MaxArraySize, BucketCount);
        }

        public byte[] Alloc(int bufferSize)
        {
            var rentBuffer = Pool.Rent(bufferSize);
            return rentBuffer;
        }

        public void FreeBuffer(byte[] buffer)
        {
            Pool.Return(buffer);
        }
    }
}
