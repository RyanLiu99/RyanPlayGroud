﻿using NUnit.Framework;
using System.Collections.Generic;
using System;
using Aliyun.Api.LogService;
using Aliyun.Api.LogService.Infrastructure.Protocol.Http;
using Aliyun.Api.LogService.Domain.Log;
using System.Threading.Tasks;

namespace Arli_yunTest
{
    public class Tests
    {
        private const string endpoint = "us-west-1.log.aliyuncs.com";
        private const string projectName = "ryan-log-proj-1";
        private const string logstore = "ryan-logstore-1";
        private const string accessKeyId = "LTAI5tE7pj2TkWZhgQEFcPua";
        private const string accessKey = "ghAwibLAIDIIlbhdvP6siGF54LNQFu";



        [SetUp]
        public void Setup()
        {


        }



        [Test]
        public async Task TestAliyunLogging()
        {
            HttpLogServiceClient client = LogServiceClientBuilders.HttpBuilder
              .Endpoint(endpoint, projectName)
              .Credential(accessKeyId, accessKey)
              .Build();

            var logGroup = new LogGroupInfo() 
            { 
                Topic = "Test Date Time",
                Source = "RyanLogSrc"

            };

            logGroup.LogTags.Add("Reference", "Test DateTime");

            // for Time Semms either way is fine and shown on aliyun as local time of PDT.
            var logInfo = new LogInfo();
            logInfo.Time = DateTime.Now;
            logInfo.Contents.Add("Datetime method", "DateTime.Now");
            logGroup.Logs.Add(logInfo);

            logInfo = new LogInfo();
            logInfo.Time = DateTime.UtcNow;
            logInfo.Contents.Add("Datetime method", "DateTime.UtcNow");
            logGroup.Logs.Add(logInfo);

            logInfo = new LogInfo();

            //logInfo.Time = DateTime.UtcNow;//Strange, this is not working, log will not show up in aliyun DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //logInfo.Contents.Add("Datetime method", "==== DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)");
            //logGroup.Logs.Add(logInfo);

            //Assert.AreEqual(3, logGroup.Logs.Count);

            PostLogsRequest request = new PostLogsRequest(logstore, logGroup)
            {
                HashKey = "1", //（optional）put data with set hash, the data will be send to shard whose range contains the hashKey
                ProjectName = projectName
            };


           var response = await client.PostLogStoreLogsAsync(request);
            Assert.IsNotNull(response);
            response.EnsureSuccess();
            

        }

        [Test]
        public void TestDateTimeOffSet()
        {
            DateTime now = DateTime.Now; //16:12:56 PDT
            DateTime utcNow = DateTime.SpecifyKind(now, DateTimeKind.Utc);
            
            DateTimeOffset of1 = now;       //after assign of1 and now are not equal.  now has no offset

            DateTimeOffset of2 = utcNow;    //but of2 and utcNNow are not equal
            
            DateTimeOffset of3 = new DateTimeOffset(now);
            DateTimeOffset of4 = new DateTimeOffset(utcNow);

            Assert.AreEqual(now, utcNow);
            Assert.IsTrue(now == utcNow);

            Assert.AreEqual(of1, of3);  //use constructor or implicit conversation are same
            Assert.AreEqual(of2, of4);  //use constructor or implicit conversation are same

            //Expected: 2022 - 09 - 21 16:12:56.1187832 - 07:00, of1 is right, loal time
            //But was:  2022 - 09 - 21 16:12:56.1187832 + 00:00
            Assert.IsFalse(of1 == of2);  //they have different offset

            //----

            Assert.IsFalse(of1.LocalDateTime == of2.LocalDateTime);
            Assert.IsFalse(of1.UtcDateTime == of2.UtcDateTime);
            Assert.IsTrue(of1.LocalDateTime == of2.UtcDateTime);
            
            //---

            //Expected: 2022 - 09 - 21 16:16:22.8063179  //local time
            //But was:  2022 - 09 - 21 16:16:22.8063179 - 07:00, but still equal
            Assert.IsTrue(now == of1);

            //Expected: 2022-09-21 16:17:22.6313787 //local time even it is utcnow
            //But was:  2022-09-21 16:17:22.6313787-07:0
            Assert.IsFalse(utcNow == of1); // interesting   utcNow = now = of1, but utcnow != of1

            Assert.IsTrue(of1 == now);  // order of expected, actul does not matter
            Assert.IsTrue(of1 != utcNow);

            //Expected: 2022 - 09 - 21 16:27:02.7653093 + 00:00
            //But was:  2022 - 09 - 21 16:27:02.7653093,      UtcNow has no offset, but equal
            Assert.IsTrue(of2 == utcNow);

        }

        [Test]
        public void TestDateTime()
        {
            var now = DateTime.Now;
            Assert.IsTrue(now.Kind == DateTimeKind.Local);

            var utcNow = DateTime.SpecifyKind(now, DateTimeKind.Local);
            Assert.AreEqual(now.Ticks, utcNow.Ticks);


            var backtoLocal = DateTime.SpecifyKind(utcNow, DateTimeKind.Local);
            Assert.AreEqual(now, backtoLocal);
            Assert.AreEqual(now.Ticks, backtoLocal.Ticks);
        }

        //[Test]
        //public void Test2()
        //{

        //    int shardId = 0;
        //    LogClient client = new LogClient(endpoint, accessKeyId, accessKey);
        //    //init http connection timeout
        //    client.ConnectionTimeout = client.ReadWriteTimeout = 10000;

        //    //put logs
        //    PutLogsRequest putLogsReq = new PutLogsRequest();
        //    putLogsReq.Project = projectName;
        //    putLogsReq.Topic = "ryan_topic";
        //    putLogsReq.Logstore = logstore;
        //    putLogsReq.LogItems = new List<LogItem>();
        //    for (int i = 1; i <= 10; ++i)
        //    {
        //        LogItem logItem = new LogItem();
        //        logItem.Time = DateUtils.TimeSpan();
        //        for (int k = 0; k < 10; ++k)
        //            logItem.PushBack("RyanLogItem_" + i.ToString(), "Ryan log operation");
        //        putLogsReq.LogItems.Add(logItem);
        //    }

        //    PutLogsResponse putLogRes = client.PutLogs(putLogsReq);  //Cause NullReferenceException
        //    Assert.IsNotNull(putLogRes);
        //    Console.WriteLine(putLogRes.ToString());
        //}
    }
}