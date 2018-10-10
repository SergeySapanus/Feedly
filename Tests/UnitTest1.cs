using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Xunit;

namespace MyFeedlyServer.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        [MemberData(nameof(AgeHeightWeightData))]
        public void MyFirstTheory(int value)
        {
            Assert.True(IsOdd(value));
        }

        public static IEnumerable<object[]> AgeHeightWeightData()
        {
            yield return new object[] { 20, 180, 80, "good" };
            yield return new object[] { 20, 180, 50, "bad" };
        }

        bool IsOdd(int value)
        {
            return value % 2 == 1;
        }

        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}

namespace xUnitFixturesLifeCycle
{
    static class Out
    {
        public static void Show(this string text)
        {
            Debug.WriteLine(text);
        }
    }

    public class CollectionFixture : IDisposable
    {
        public CollectionFixture()
        {
            "CollectionFixture : ctor".Show();
        }

        public void Dispose()
        {
            "CollectionFixture : disposed".Show();
        }
    }

    public class ClassFixture : IDisposable
    {
        public ClassFixture()
        {
            "ClassFixture : ctor".Show();
        }

        public void Dispose()
        {
            "ClassFixture : disposed".Show();
        }
    }

    [CollectionDefinition("ContextOne")]
    public class TestCollection : ICollectionFixture<CollectionFixture>
    {
        public TestCollection()
        {
            "TestCollection is never instantiated".Show();
        }
    }

    [Collection("ContextOne")]
    public class TestContainerOne : IClassFixture<ClassFixture>, IDisposable
    {
        //public TestContainerOne()
        //{
        //    "TestContainerOne : ctor".Show();

        //}

        public TestContainerOne(ClassFixture classFixture)
        {
            "TestContainerOne(ClassFixture classFixture) : ctor".Show();
            $"TestContainerOne(ClassFixture classFixture) : {classFixture}".Show();
        }

        [Fact]
        public void TestOne()
        {
            "TestContainerOne : TestOne".Show();
        }

        [Fact]
        public void TestTwo()
        {
            "TestContainerOne : TestTwo".Show();
        }

        public void Dispose()
        {
            "TestContainerOne : disposed".Show();
        }
    }

    [Collection("ContextOne")]
    public class TestContainerTwo : IDisposable
    {
        public TestContainerTwo()
        {
            "TestContainerTwo : ctor".Show();
        }

        [Fact]
        public void TestOne()
        {
            "TestContainerTwo : TestOne".Show();
        }

        public void Dispose()
        {
            "TestContainerTwo : disposed".Show();
        }
    }

    public class TestContainerThree
    {
        [Fact]
        public void TestOne()
        {
            "TestContainerThree : TestOne".Show();
        }

        [Fact]
        public void ShouldGetItemWithSameId()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var facade = fixture.Freeze<Mock<IDataFacade>>();
            facade.Setup(c => c.Get(It.IsAny<int>())).Returns((int i) => new Item { Key = i });

            var sut = fixture.Create<BusinessLogic>();
            var expected = fixture.Create<int>();

            Assert.Equal(expected, sut.Get(expected).Key);
        }

        [Fact]
        public void NameIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();

            fixture.Register(() => "Name");
            var expectedName = fixture.Create<string>();

            var sut = fixture.Create<Pizza>();
            // Exercise system
            string result = sut.Name;
            // Verify outcome
            Assert.Equal(expectedName, result);
            // Teardown
        }

        [Fact]
        public void NameIsCorrect_Freeze()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedName = fixture.Freeze<string>();
            var sut = fixture.Create<Pizza>();
            // Exercise system
            string result = sut.Name;
            // Verify outcome
            Assert.Equal(expectedName, result);
            // Teardown
        }
    }

    public class Pizza
    {
        public string Name { get; set; }
    }

    public class Item
    {
        public Item(int i)
        {
            Key = i;
        }

        public Item()
        {
        }

        public int Key { get; set; }
    }

    public class BusinessLogic : IDataFacade
    {
        public Item Get(int i)
        {
            return new Item(i);
        }
    }

    public interface IDataFacade
    {
        Item Get(int i);
    }
}
