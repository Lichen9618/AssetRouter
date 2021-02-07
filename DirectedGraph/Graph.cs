using System;

namespace DirectedGraph
{
    /// <summary>
    /// Represents a node in a graph.
    /// </summary>
    /// <typeparam name="T">Type of value object.</typeparam>
    /// <typeparam name="W">Type of weigth object.</typeparam>
    public class Node<T, W>
    {
        T value;
        /// <summary>
        /// Sets or retrieves a value object associated with this node.
        /// </summary>
        public T Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        W weight;
        /// <summary>
        /// Sets or retrieves a weight object associated with this node.
        /// </summary>
        public W Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        private LinkedList<Node<T, W>> adjacentNodes = new LinkedList<Node<T, W>>();
        /// <summary>
        /// The list of adjacent nodes of this node.
        /// </summary>
        public LinkedList<Node<T, W>> AdjacentNodes
        {
            get
            {
                return adjacentNodes;
            }
        }

        /// <summary>
        /// Creates a node given the value and weight objects.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="weight"></param>
        public Node(T value, W weight)
        {
            Value = value;
            Weight = weight;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Node<T, W> node = (Node<T, W>)obj;
            return Value.Equals(node.Value);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        // override object.ToString
        public override string ToString()
        {
            return value.ToString();
        }
    }

    /// <summary>
    /// A directed graph. It contains a list of node objects with their adjacent nodes. It is a sparse
    /// structure. Requires Microsoft .NET Framework version 4 or better.
    /// </summary>
    /// <typeparam name="T">The type of value object stored in the nodes of this graph.</typeparam>
    /// <typeparam name="W">The type of weight object stored in the nodes of this graph.</typeparam>
    public class Graph<T, W>
    {
        // List of nodes
        LinkedList<Node<T, W>> nodes = new LinkedList<Node<T, W>>();

        /// <summary>
        /// Adds a edge to this graph.
        /// </summary>
        /// <param name="start">The value object of the starting node.</param>
        /// <param name="end">The value object of the ending node.</param>
        /// <param name="weight">The weight of this edge.</param>
        public void AddEdge(T start, T end, W weight)
        {
            Node<T, W> s = new Node<T, W>(start, default(W));

            Node<T, W> e = new Node<T, W>(end, weight);

            Node<T, W> result = nodes.Find(s); 
            if (result != null)
                s = result;
            else
                nodes.Add(s);

            result = s.AdjacentNodes.Find(e);
            if (result == null)
                s.AdjacentNodes.Add(e);
            else
                throw new ArgumentException("The specified route already exists.");
        }

        /// <summary>
        /// Searches this graph. The search algortihm used is depth-first. Search stops only when 
        /// the specified depth or total weight has been surpassed. The call to this method is
        /// thread-safe, but adding nodes to the graph while it is executing may lead to unexpected 
        /// results.
        /// </summary>
        /// <param name="start">The node from which to start the search.</param>
        /// <param name="end">The node where a search ends.</param>
        /// <param name="depth">The depth of the graph to traverse. If the value is n then the 
        /// result contains up to n nodes after start.</param>
        /// <param name="weight">The total weight of the nodes traversed is restricted to less than weight.</param>
        /// <param name="cycle">Set to true to allow cycles, specify depth or weight otherwise infinite loops may occur.</param>
        /// <returns></returns>
        public LinkedList<LinkedList<Node<T, W>>> Search(T start, T end, int depth = 0, W weight = default(W), bool cycle = false)
        {
            Node<T, W> s = new Node<T, W>(start, default(W));
            Node<T, W> e = new Node<T, W>(end, default(W));

            LinkedList<LinkedList<Node<T, W>>> searchResults = new LinkedList<LinkedList<Node<T, W>>>();
            LinkedList<Node<T, W>> searchResult = new LinkedList<Node<T, W>>();
            searchResult.Push(s); // push start node
            searchResults.Push(searchResult);

            Search(s, e, searchResults, depth, weight, cycle);

            if (searchResults.Peek().Length == 1)
            {
                searchResults.Pop();
            }

            return searchResults;
        }

        /// <summary>
        /// <see cref="DirectedGraph.Search(T start, T end, int depth = 10, W weight, bool cycle)"/>
        /// </summary>
        private void Search(Node<T, W> start, Node<T, W> end, LinkedList<LinkedList<Node<T, W>>> searchResults, int depth, W weight, bool cycle)
        {
            Node<T, W> node = nodes.Find(start);

            if (node == null)
            {
                return;
            }

            foreach (Node<T, W> adjacentNode in node.AdjacentNodes)
            {

                if (searchResults.Peek().Contains(adjacentNode))                 
                {
                    //路过重复节点，跳出循环
                    continue;
                }
                // Add node to search result
                searchResults.Peek().Push(adjacentNode); // add node
                searchResults.Peek()[0].Weight = (dynamic)searchResults.Peek()[0].Weight + adjacentNode.Weight;

                // Continue search
                bool continueSearch = true;
                int length = searchResults.Peek().Length;


                if (depth != 0 && length > depth + 1)
                {
                    // Depth has been provided, and exceeded
                    continueSearch = false;
                }

                if ((dynamic)weight != default(W) && (dynamic)searchResults.Peek()[0].Weight >= weight)
                {
                    // weight has been provided, and exceeded
                    continueSearch = false;
                }

                if (!cycle && length > nodes.Length)
                {
                    // we have a cycle
                    continueSearch = false;
                }

                if (continueSearch)
                {
                    if (end.Equals(adjacentNode))
                    {
                        // Create a copy of the current result to search deeper
                        LinkedList<Node<T, W>> searchResult = searchResults.Peek().Copy();
                        searchResults.Push(searchResult);
                    }

                    // Continue to search deeper
                    Search(adjacentNode, end, searchResults, depth, weight, cycle);

                    if (length == searchResults.Peek().Length)
                    {
                        // No new node added, dead end
                        searchResults.Peek().Pop(); // remove node
                        searchResults.Peek()[0].Weight = (dynamic)searchResults.Peek()[0].Weight - adjacentNode.Weight;
                    }
                }
                else
                {
                    searchResults.Peek().Pop(); // remove node
                    searchResults.Peek()[0].Weight = (dynamic)(searchResults.Peek()[0].Weight) - adjacentNode.Weight;
                    return;
                }
            }
        }
    }
}
