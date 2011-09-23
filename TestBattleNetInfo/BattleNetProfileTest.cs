using BattleNetInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestBattleNetInfo
{
    
    
    /// <summary>
    ///This is a test class for BattleNetProfileTest and is intended
    ///to contain all BattleNetProfileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BattleNetProfileTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetServer
        ///</summary>
        [TestMethod()]
        public void GetServerTest()
        {
            BattleNetProfile target;
            target = new BattleNetProfile( @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/" ); 
            Assert.AreEqual( "eu", target.GetServer(), true );
         
            
        }

        /// <summary>
        ///A test for GetPlayerName
        ///</summary>
        [TestMethod()]
        public void GetPlayerNameTest()
        {
            BattleNetProfile target;
            target = new BattleNetProfile( @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/" );
            Assert.AreEqual( "Zakk", target.GetPlayerName(), true );
            
        }

        /// <summary>
        ///A test for DownloadProfileContent
        ///</summary>
        [TestMethod()]
        public void DownloadProfileContentTest()
        {
            BattleNetProfile target;
            target = new BattleNetProfile( @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/" );
            Assert.IsTrue( target.DownloadProfileContent() );
            Assert.AreNotEqual( "", target.ProfileContent );
            
        }

        /// <summary>
        ///A test for DownloadLadderContent
        ///</summary>
        [TestMethod()]
        public void DownloadLadderContentTest()
        {
            BattleNetProfile target;
            target = new BattleNetProfile( @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/" );
            Assert.IsTrue( target.DownloadLadderContent() );
            Assert.AreNotEqual( "", target.LadderContent );
            
        }

        /// <summary>
        ///A test for GetAchievementPoints
        ///</summary>
        [TestMethod()]
        public void GetAchievementPointsTest()
        {
            BattleNetProfile target;
            target = new BattleNetProfile( @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/" );
            target.DownloadData();
            Assert.AreEqual( 2390, target.GetAchievementPoints() );
            Assert.AreEqual( 2390, target.AchievementPoints );
            Assert.AreEqual( target.GetAchievementPoints(), target.AchievementPoints );
            
        }

        /// <summary>
        ///A test for GetLeague
        ///</summary>
        [TestMethod()]
        public void GetLeagueTest()
        {
            BattleNetProfile target;
            target = new BattleNetProfile( @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/" );
            target.DownloadData();
            Assert.AreEqual( "diamond_1", target.GetLeague(), true );
        }

        /// <summary>
        ///A test for GetRace
        ///</summary>
        [TestMethod()]
        public void GetRaceTest()
        {
            BattleNetProfile target;
            target = new BattleNetProfile( @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/" );
            target.DownloadData();
            Assert.AreEqual( "terran", target.GetRace(), true );

          
        }

        /// <summary>
        ///A test for GetRank
        ///</summary>
        [TestMethod()]
        public void GetRankTest()
        {
            BattleNetProfile target;
            target = new BattleNetProfile( @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/" );
            target.DownloadData();
            Assert.AreEqual( 57, target.GetRank() );

            

        }

        /// <summary>
        ///A test for GetStats
        ///</summary>
        [TestMethod()]
        public void GetStatsTest()
        {
            BattleNetProfile target;
            target = new BattleNetProfile( @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/" );
            target.DownloadData();
            target.GetStats();
            Assert.AreEqual( 14, target.Points );
            Assert.AreEqual( 2, target.Wins );
        }

        /// <summary>
        ///A test for IsValidUrl
        ///</summary>
        [TestMethod()]
        public void IsValidUrlTest()
        {
            BattleNetProfile target;
            target = new BattleNetProfile( @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/" );
            Assert.IsTrue( target.IsValidUrl() );

            target = new BattleNetProfile( @"http://us.battle.net/sc2/en/profile/289433/1/AlLaboUtyOu/" );
            Assert.IsTrue( target.IsValidUrl() );

            target = new BattleNetProfile( @"http://kr.battle.net/sc2/ko/profile/106432/1/SoundWeRRa/" );
            Assert.IsTrue( target.IsValidUrl() );

            target = new BattleNetProfile( @"http://gmail.com" );
            Assert.IsFalse( target.IsValidUrl() );

            target = new BattleNetProfile( @"http://us.battle.net/sc2/en/profile/2894d33/1/AlLaboUtyO1u/" );
            Assert.IsFalse( target.IsValidUrl() );

            target = new BattleNetProfile( @"" );
            Assert.IsFalse( target.IsValidUrl() );

        }
    }
}
