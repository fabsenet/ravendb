using System.Linq;
using FastTests;
using Raven.Client.Indexes;
using Xunit;

namespace SlowTests.Bugs.Indexing
{
    public class CanHaveAnIndexNameThatStartsWithDynamic : RavenTestBase
    {
        private class SomeDoc
        {
            public string MyStringProp { get; set; }
        }

        private class SomeOtherDoc
        {
            public int MyIntProp { get; set; }
        }

        private class Result
        {
            public string MyStringProp { get; set; }
            public int Count { get; set; }
        }

        private class DynamicIndex : AbstractIndexCreationTask<SomeDoc, Result>
        {
            public DynamicIndex()
            {
                Map = docs => from doc in docs
                              select new { MyStringProp = doc.MyStringProp, Count = 1 };

                Reduce = results => from result in results
                                    group result by result.MyStringProp
                                        into g
                                        select new { MyStringProp = g.Key, Count = g.Sum(x => x.Count) };
            }
        }

        [Fact]
        public void CanHaveAnIndexWithANameThatStartsWithTheWordDynamic()
        {
            using (var store = GetDocumentStore())
            {
                using (var s = store.OpenSession())
                {
                    var one = new SomeDoc { MyStringProp = "Test" };
                    var two = new SomeDoc { MyStringProp = "two" };
                    var other = new SomeOtherDoc { MyIntProp = 1 };
                    s.Store(one);
                    s.Store(two);
                    s.Store(other);
                    s.SaveChanges();
                    new DynamicIndex().Execute(store);
                    var list = s.Query<Result, DynamicIndex>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).ToList();
                    Assert.NotEqual(0, list.Count());
                }
            }
        }
    }
}
