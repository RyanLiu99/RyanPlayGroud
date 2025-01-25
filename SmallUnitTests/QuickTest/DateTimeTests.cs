using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickTest
{
    internal class DateTimeTests
    {
        [Test]
        public void TicketsTest()  //PASS
        {
            DateTime dt1 = new DateTime(2023, 11, 4, 12, 0, 0, DateTimeKind.Utc);
            DateTime dt2 = new DateTime(2023, 11, 4, 12, 0, 0, DateTimeKind.Local);

            Assert.IsTrue(dt1.Ticks == dt2.Ticks); 
            Assert.IsTrue(dt1 == dt2);   //Only care about ticks

            var min = DateTime.MinValue;  //base for ticks
            Assert.IsTrue(min.Kind == DateTimeKind.Unspecified);//ticks’ base is unspecified

            Assert.AreEqual(0, min.Ticks);
            
            Assert.AreEqual(dt1.Ticks, dt1.Ticks-min.Ticks);
            Assert.AreEqual(dt2.Ticks, dt2.Ticks - min.Ticks);


            DateTime dt3 = DateTime.SpecifyKind(dt2, DateTimeKind.Utc);
            Assert.IsTrue(dt3.Ticks == dt2.Ticks);
            Assert.IsTrue(dt3 == dt2); 

            DateTime dt4 = dt2.ToUniversalTime();  // This change ticks
            Assert.IsTrue(dt4.Ticks != dt2.Ticks);
            Assert.IsTrue(dt4 != dt2);
        }

        [Test]
        public void TestTimeZone() //PASS
        {
            DateTime dt5 = new DateTime(2023, 11, 4, 8, 0, 0, DateTimeKind.Unspecified);  //	"11/4/2023 8:00:00 AM"	
            var dt5Local = dt5.ToLocalTime(); //	"11/4/2023 1:00:00 AM", consider dt5 is UTC, -7 to get this local time -7
            var dt5Utc = dt5.ToUniversalTime(); // "11/4/2023 3:00:00 PM", consider dt5 is local time (Pacific) , +7 to get this UTC time 
            Assert.IsTrue(dt5 != dt5Local);
            Assert.IsTrue(dt5 !=  dt5Utc);


            DateTime dt6 = new DateTime(2023, 11, 4, 8, 0, 0, DateTimeKind.Utc); //11/4/2023 8AM
            var dt6Local = dt6.ToLocalTime();  //11/4/23  1AM, right, 8-7 to Pacific time
            var dt6Utc = dt6.ToUniversalTime(); //11/4/23 8AM, alrady UTC
            Assert.IsTrue(dt6 != dt6Local); //change timezone,change ticks and pointing to same point in history
            Assert.IsTrue(dt6 == dt6Utc); //since both UTC


            DateTime dt7 = new DateTime(2023, 11, 4, 8, 0, 0, DateTimeKind.Local); //11/4/2023 8AM
            var dt7Local = dt7.ToLocalTime();  //11/4/23  8AM, local to local, no change
            var dt7Utc = dt7.ToUniversalTime(); //11/4/23 3PM, +7 to UTC
            Assert.IsTrue(dt7 == dt7Local); //both local
            Assert.IsTrue(dt7 != dt7Utc); //same point of time in history, but different time zone.
        }

    }
}
