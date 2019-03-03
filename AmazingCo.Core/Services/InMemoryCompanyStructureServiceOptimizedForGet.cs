using System;
using System.Collections.Generic;
using System.Linq;
using AmazingCo.DataLayer;
using AmazingCo.Dtos;

namespace AmazingCo.Services
{
    public class InMemoryCompanyStructureServiceOptimizedForGet : ICompanyStructureService
    {
        private readonly ICompanyStructureDataLayer _companyStructureDataLayer;

        private class Node
        {
            public Node(int id, string title)
            {
                Id = id;
                Title = title;
                Children = new HashSet<Node>();
            }

            public int Id { get; }
            public string Title { get; }
            public Node Parent { get; set; }
            public ISet<Node> Children { get; }
        }

        public InMemoryCompanyStructureServiceOptimizedForGet(ICompanyStructureDataLayer companyStructureDataLayer)
        {
            _companyStructureDataLayer = companyStructureDataLayer ?? throw new ArgumentNullException(nameof(companyStructureDataLayer));

            _dictionary = LoadGraph();
        }

        private readonly Dictionary<int, Node> _dictionary;
        private readonly object _lock = new object();

        public bool TryGetNode(int id, out NodeInformation nodeInformation)
        {
            // The complexity is O(log(n, base)), because we need to find the level by
            // traversing the tree from the node to the root.
            lock (_lock)
            {
                // O(1) complexity for finding the nodes by id
                if (_dictionary.TryGetValue(id, out var node))
                {
                    nodeInformation = CreateNodeInformation(node);

                    return true;
                }
            }

            nodeInformation = null;
            return false;
        }

        public bool TryUpdateNode(
            int id, 
            int parentId, 
            out NodeInformation modifiedNodeInformation
        )
        {
            // TryUpdateNode operation is slow. I assumed the company chart
            // is not going to change that often. Based on my researches, to
            // improve the performance of the updates, instead of file system,
            // I could use an in memory key/value stores that have builtin
            // persistence capabilities to implement ICompanyStructureDataLayer
            // (for example Redis).
            lock (_lock)
            {
                if(id==0 || id== parentId)
                {
                    modifiedNodeInformation = null;
                    return false;
                }
                if (
                    _dictionary.TryGetValue(id, out var node) && 
                    _dictionary.TryGetValue(parentId, out var newParentNode)
                )
                {
                    MoveNode(node, newParentNode);

                    _companyStructureDataLayer.WriteAll(
                        _dictionary.Values.Select(
                            i => new NodeSnapshot(i.Id, i.Title, i.Parent?.Id ?? -1)
                        )    
                    );

                    modifiedNodeInformation = CreateNodeInformation(node);

                    return true;
                }
            }

            modifiedNodeInformation = null;
            return false;
        }

        private static NodeInformation CreateNodeInformation(Node node)
        {
            return new NodeInformation(
                node.Id,
                node.Parent?.Id,
                node.Title,
                FindLevel(node),
                node.Children.Select(ch => ch.Id)
            );
        }

        private static void MoveNode(Node node, Node newParentNode)
        {
            // O(1) complexity for moving a node
            node.Parent.Children.Remove(node);

            node.Parent = newParentNode;

            newParentNode.Children.Add(node);
        }

        private static int FindLevel(Node node)
        {
            // Note 1: Since we are dealing with a three structure, finding the level has a log complexity 
            // Note 2: Keeping track of visited items to prevent circular references and infinite loops
            // Note 3: Returns -1 if there is a loop
            var visited = new HashSet<Node>();
            var level = 0;
            var currentNode = node;
            
            while (currentNode.Parent != null)
            {
                if (!visited.Add(currentNode))
                    return -1;

                currentNode = currentNode.Parent;
                level++;
            }

            return level;
        }

        private Dictionary<int, Node> LoadGraph()
        {
            var graph = new Dictionary<int, Node>();
            foreach (var nodeSnapshot in _companyStructureDataLayer.GetAll())
            {
                if(graph.TryGetValue(nodeSnapshot.Id, out _))
                    throw new Exception(
                        $"Exception in loading the graph. Node id {nodeSnapshot.Id} is duplicated."
                    );

                var newNode = new Node(nodeSnapshot.Id, nodeSnapshot.Title);
                graph[nodeSnapshot.Id] = newNode;
            }

            foreach (
                var nodeSnapshot 
                in _companyStructureDataLayer.GetAll().Where(s =>s.ParentId>-1)
            )
            {
                var node = graph[nodeSnapshot.Id];
                var parent = graph[nodeSnapshot.ParentId];
                node.Parent = parent;
                parent.Children.Add(node);      
            }

            return graph;
        }
    }
}