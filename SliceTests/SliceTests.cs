using System.Collections;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace SliceTests
{
    public class SliceTests
    {
        private static IEnumerable Words()
        {
            using (var reader = new StreamReader("./testdata/words.txt"))
            {
                while (reader.Peek() >= 0)
                {
                    var word = reader.ReadLine();
                    yield return new TestCaseData(word).SetName("{m}_" + word);
                }
            }
        }

        private ChromeDriver _webDriver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArguments("--headless", "--no-sandbox", "--disable-dev-shm-usage");

            _webDriver = new ChromeDriver(Directory.GetCurrentDirectory(), options);
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }

        [Test]
        [TestCaseSource(nameof(Words))]
        public void SearchTest_ByWord_ContainsInTitle(string word)
        {
            // Act
            _webDriver.Url = $"https://www.google.co.jp/search?q={word}";

            // Assert
            var title = _webDriver.Title;
            title.Should().Contain(word).And.Contain("Google");
        }
    }
}