using System.Collections.Generic;
using System.Linq;
using BBS.RedisExtenders.Extenders;
using BBS.RedisExtenders.Test.Core.Model;
using BBS.RedisExtenders.Test.Core.Types;
using NUnit.Framework;

namespace BBS.RedisExtenders.Test.Tests
{
    [TestFixture]
    public class GenericListTests
    {
        private static WebSite BuildTestSite(long siteId, string name)
        {
            var s = new WebSite
            {
                SiteId = siteId,
                DomainName = name + ".dev.bigblackspider.com",
                Description = $"Unit test website for '{name}'.",
                MailDomainName = name + ".mail.bigblackspider.com"
            };
            s.Details.Add("$SHORT-DESCRIPTION$", "Test web site.");
            s.Details.Add("$MAIN-HEADING$", $"{name} Unit Test Web Site");
            s.Details.Add("$MAIN-TEXT$", $"This is a unit test website for '<b>{name}</b>,'.");
            s.Details.Add("$ABOUT-HEADING$", $"All About {name}'");
            s.Details.Add("$ABOUT-TEXT$",
                $"Some text that describes the about section for domain '<b><i>{s.DomainName}</i></b>'.");
            return s;
        }

        [Test]
        public void GeneralTst()
        {
            //********** Init
            var lis = new List<WebSite>();
            lis.RedisClear();

            //********** Create Test Data
            for (var i = 0; i < 100; i++)
                lis.RedisAdd(BuildTestSite(lis.RedisNextId(), $"WebSite{i:000}"));
            var tst = new List<WebSite>();
            tst.RedisGetAll();
            Assert.AreEqual(lis.Count, tst.Count);
            Assert.AreEqual(100, tst.Count);

            //********** Find 
            tst = new List<WebSite>();
            tst.RedisFind(o => o.DomainName.Contains("7"));
            Assert.AreEqual(19, tst.Count);

            //********** Update 
            foreach (var site in lis.Where(o => o.DomainName.Contains("5")))
                site.Status = TSiteStatus.Closed;
            lis.RedisUpdate();
            tst = new List<WebSite>();
            tst.RedisGetAll();
            Assert.AreEqual(lis.Count, tst.Count);

            //********** Delete 
            foreach (var site in lis.Where(o => o.Status == TSiteStatus.Closed).ToArray())
                lis.RedisRemove(site);
            tst = new List<WebSite>();
            tst.RedisGetAll();
            Assert.AreEqual(lis.Count, tst.Count);
            Assert.AreEqual(81, tst.Count);

            //********** Clear
            lis.RedisClear();
            tst = new List<WebSite>();
            tst.RedisGetAll();
            Assert.AreEqual(lis.Count, tst.Count);
            Assert.AreEqual(0, tst.Count);
        }
    }
}