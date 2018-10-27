using System;
using System.Threading;
using Zaabee.Redis;
using Zaabee.Redis.Abstractions;
using Zaabee.Redis.Protobuf;

namespace ConsoleApp1
{
    class Program
    {      
        private static readonly IZaabeeRedisClient _client =
            new ZaabeeRedisClient(new RedisConfig("wolfapp.cn:6379,abortConnect=true,syncTimeout=3000"),
                new Serializer());
        
        static void Main(string[] args)
        {
            int total = 0;
            for (int t = 0; t < 1000; t++)
            {        
                new Thread(() =>
                {
                    try
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            _client.ListLeftPush("key", "testaaaaaaaa");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }).Start();
                new Thread(() =>
                {
                    try
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            _client.ListRightPop<string>("key");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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