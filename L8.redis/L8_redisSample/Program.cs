

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace redisSample
{
    class Program
    {
        static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                EndPoints = { "localhost:6379" }
            });

        static void create_bugs_hset(IDatabase db)
        {
            db.HashSet("bug:12339", new HashEntry[] {   // hset bug:12339
                new HashEntry("priority", 1),
                new HashEntry("severity", 3),
                new HashEntry("details", "{id: 12339, comment: bug with id=12339}") });

            db.HashSet("bug:1382", new HashEntry[] {    // hset bug:1382
                new HashEntry("priority", 2),
                new HashEntry("severity", 2),
                new HashEntry("details", "{id: 1382, comment: bug with id=1382}") });

            db.HashSet("bug:338", new HashEntry[] {    // hset bug:338
                new HashEntry("priority", 3),
                new HashEntry("severity", 5),
                new HashEntry("details", "{id: 338, comment: bug with id=338}") });

            db.HashSet("bug:9338", new HashEntry[] {   // hset bug:9338
                new HashEntry("priority", 2),
                new HashEntry("severity", 4),
                new HashEntry("details", "{id: 9338, comment: bug with id=9338}") });
        }

        static async Task Main(string[] args)
        {
            try
            {
                var db = redis.GetDatabase();

                var pong = await db.PingAsync();
                Console.WriteLine(pong);

                db.StringSet("foo", "foo-value");

                // set/get string (get)
                var foo = db.StringGet("foo");
                Console.WriteLine("foo = " + foo);

                // hset / hgetall
                var p1 = db.HashGetAll("person1").Select(g => new { g.Name, g.Value });
                foreach (var v in p1)
                    Console.WriteLine($"{v.Name} = {v.Value}");

                // sadd / smembers
                var p2 = db.SetMembers("users1").ToList();
                foreach (var v in p2)
                    Console.WriteLine("p = " + v);

                // zadd / zrange
                var p3 = db.SortedSetRangeByScoreWithScores("persons").Select(g => new { g.Element, g.Score });
                foreach (var v in p3)
                    Console.WriteLine("p = " + v);

                // publish message to channel
                //db.Publish("warnings", "This message is from C#");

                // subscribing on messages
                ISubscriber subscriber = redis.GetSubscriber();
                subscriber.Subscribe("warnings", (channel, message) => {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}]: {$"Message {message} received successfully"}");
                });

                create_bugs_hset(db);

                Console.ReadLine();
            }
            catch (Exception e)
            { 
                Console.WriteLine("exception = " + e.ToString());
            }
        }
    }
}
