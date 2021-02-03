using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectedGraph;

namespace DirectedGraphTest
{
    [TestClass]
    public class DirectedGraphTest
    {
        DirectedGraph<string, int> graph;

        [TestInitialize]
        public void Initialize()
        {
            // add several nodes
            graph = new DirectedGraph<string, int>();
            graph.AddEdge("A", "B", 5);
            graph.AddEdge("B", "C", 4);
            graph.AddEdge("C", "D", 8);
            graph.AddEdge("D", "C", 8);
            graph.AddEdge("D", "E", 6);
            graph.AddEdge("A", "D", 5);
            graph.AddEdge("C", "E", 2);
            graph.AddEdge("E", "B", 3);
            graph.AddEdge("A", "E", 7);
        }

        [TestMethod]
        public void TestAddEdge()
        {            
            // test duplicate
            try
            {
                graph.AddEdge("A", "B", 5);
                Assert.Fail("ArgumentException not thrown.");
            }
            catch(ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestSearchAB()
        {
            LinkedList<LinkedList<Node<string, int>>> results = graph.Search("A", "B", 1);
            printResults("A-B (Depth <= 1)", results);
            Assert.AreEqual(1, results.Length);
            Assert.AreEqual(2, results[0].Length);
            Assert.AreEqual("B", results[0][1].Value);
        }

        [TestMethod]
        public void TestSearchAC()
        {
            LinkedList<LinkedList<Node<string, int>>> results = graph.Search("A", "C", 4);
            printResults("A-C (Depth <= 4)", results);
            Assert.AreEqual(6, results.Length);
        }

        [TestMethod]
        public void TestSearchAD()
        {
            LinkedList<LinkedList<Node<string, int>>> results = graph.Search("A", "D", cycle: false);
            printResults("A-D", results);
            Assert.AreEqual(4, results.Length);
        }

        [TestMethod]
        public void TestSearchBB()
        {
            LinkedList<LinkedList<Node<string, int>>> results = graph.Search("B", "B");
            printResults("B-B", results);
            Assert.AreEqual(2, results.Length);
        }

        [TestMethod]
        public void TestSearchCD()
        {
            LinkedList<LinkedList<Node<string, int>>> results = graph.Search("C", "D", depth: 5);
            printResults("C-D (Depth <= 5)", results);
            Assert.AreEqual(3, results.Length);
        }

        [TestMethod]
        public void TestSearchCC()
        {
            // test search with weight and cycle
            LinkedList<LinkedList<Node<string, int>>> results = graph.Search("C", "C", weight: 30, cycle: true);
            Assert.AreEqual(7, results.Length);
            printResults("C-C (Weight < 30)", results);
        }

        /// <summary>
        /// Helper method to print the results of the search to test output.
        /// Useful debug aid.
        /// </summary>
        /// <param name="text">Any text to output.</param>
        /// <param name="results">The search results.</param>
        void printResults(string text, LinkedList<LinkedList<Node<string, int>>> results)
        {
            Console.WriteLine("--------------------");
            Console.WriteLine(text);
            Console.WriteLine("--------------------");
            foreach (LinkedList<Node<string, int>> result in results)
            {
                int weight = 0;
                foreach (Node<string, int> node in result)
                {
                    weight += node.Weight;
                    Console.Write(node.ToString());
                }
                Console.WriteLine(" (Weight = " + weight + ")");
            }
        }
    }
}
