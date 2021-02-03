using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectedGraph;

namespace DirectedGraphTest
{
    [TestClass]
    public class LinkedListTest
    {
        /// <summary>
        /// Test LinkedList. 
        /// TODO: refactor this method into separate test methods for easier understanding and maintenance.
        /// </summary>
        [TestMethod]
        public void TestLinkedList()
        {
            // Create a linked list with 100 items
            LinkedList<int> list = new LinkedList<int>();
            for (int i = 0; i < 100; i++) {
                list.Add(i);
            }

            // Test list length
            Assert.AreEqual(100, list.Length);

            // Test enumerator
            int index = 0;
            foreach (int value in list)
            {
                Assert.AreEqual(index, value);
                index++;
            }

            // Test indexer
            for (int i = 0; i < list.Length; i++)
            {
                Assert.AreEqual(i, list[i]);
            }

            // Set last item
            list[99] = 100;
            Assert.AreEqual(100, list[99]);

            // Test Find with Predicate search
            int result = list.Find(delegate(int value)
            {
                if (value == 100) return true;
                else return false;
            });
            Assert.AreEqual(100, result);

            // Test simple Find
            result = list.Find(50);
            Assert.AreEqual(50, result);

            // Test push
            list.Push(101);
            list.Push(102);

            // Test peek
            Assert.AreEqual(102, list.Peek());

            // Test pop
            result = list.Pop();
            Assert.AreEqual(102, result);
            result = list.Pop();
            Assert.AreEqual(101, result);

            // Test IndexOutOfRangeException is thrown
            try
            {
                int i = list[100];
                Assert.Fail("IndexOutOfRangeException not thrown");
            }
            catch (IndexOutOfRangeException)
            {
            }
            try
            {
                int i = list[-1];
                Assert.Fail("IndexOutOfRangeException not thrown");
            }
            catch (IndexOutOfRangeException)
            {
            }

            // Test Copy
            LinkedList<int> copy = list.Copy();
            Assert.AreEqual(100, copy.Length);
        }
    }
}
