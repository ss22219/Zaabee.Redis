using System;
using System.Linq;
using System.Threading;
using Xunit;
using Zaabee.Redis;
using Zaabee.Redis.Abstractions;
using Zaabee.Redis.Protobuf;

namespace UnitTest
{
    public class MultiThreadTest
    {
        private readonly IZaabeeRedisClient _client =
            new ZaabeeRedisClient(new RedisConfig("localhost:32768,abortConnect=true,syncTimeout=3000"),
                new Serializer());

        [Fact]
        public void Test()
        {
            int total = 0;
            for (int t = 0; t < 100; t++)
            {
                new Thread(() =>
                {
                    try
                    {
                        for (int i = 0; i < 10000; i++)
                        {
                            _client.ListLeftPush("key", "testaaaaaaaa");
                            _client.ListRightPop<string>("key");
                        }
                    }
                    catch (Exception e)
                    {
                        Assert.Null(e);
                        Console.WriteLine(e);
                        throw;
                    }
                    Interlocked.Increment(ref total);
                }).Start();
            }

            while (total < 100)
            {
                Thread.Sleep(200);
            }
        }
    }
}