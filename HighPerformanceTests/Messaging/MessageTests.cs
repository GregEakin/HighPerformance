// Greg Eakin
// December 31, 2016
// Copyright (c) 2016

//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Library General Public
//License as published by the Free Software Foundation; either
//version 2 of the License, or(at your option) any later version.

//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU
//Library General Public License for more details.

//You should have received a copy of the GNU Library General Public
//License along with this library; if not, write to the
//Free Software Foundation, Inc., 59 Temple Place - Suite 330,
//Boston, MA  02111-1307, USA.

using HighPerformance.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Messaging
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void EnclodeTest()
        {
            var message = new Message
            {
                Type = 12,
                Tag = 3,
                ["s1"] = "George"
            };

            var data = message.Encode();
            Assert.AreEqual("SMA\t0\t12\t3\t1\ts1\tGeorge\t", data);
        }

        [TestMethod]
        public void DecodeTest()
        {
            var message = new Message();
            message.Decode("SMA\t0\t12\t3\t1\ts1\tGeorge\t");

            Assert.AreEqual(12, message.Type);
            Assert.AreEqual(3, message.Tag);
            Assert.AreEqual("George", message["s1"]);
        }
    }
}