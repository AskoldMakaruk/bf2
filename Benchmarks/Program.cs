// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

namespace Benchmarks
{
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> LeftChild { get; set; }
        public Node<T> RightChild { get; set; }
        public Node<T> Next { get; set; }
        public Node<T> Prev { get; set; }

        public Node(T data)
        {
            Data = data;
            LeftChild = null;
            RightChild = null;
            Next = null;
            Prev = null;
        }
    }

    public class AVLTree<T>
    {
        private Node<T> _root;

        public AVLTree()
        {
            _root = null;
        }

        public void Insert(T data)
        {
            _root = Insert(_root, data);
        }

        private Node<T> Insert(Node<T> node, T data)
        {
            if (node == null)
            {
                node = new Node<T>(data);
                return node;
            }

            if (Comparer<T>.Default.Compare(data, node.Data) < 0)
            {
                node.LeftChild = Insert(node.LeftChild, data);
            }
            else if (Comparer<T>.Default.Compare(data, node.Data) > 0)
            {
                node.RightChild = Insert(node.RightChild, data);
            }

            node = Balance(node);

            if (node.Next == null)
            {
                if (node.RightChild != null)
                {
                    node.Next = node.RightChild;
                    node.Next.Prev = node;
                    while (node.Next.LeftChild != null)
                    {
                        node.Next = node.Next.LeftChild;
                        node.Next.Prev = node;
                    }
                }
            }

            if (node.Prev == null)
            {
                if (node.LeftChild != null)
                {
                    node.Prev = node.LeftChild;
                    node.Prev.Next = node;
                    while (node.Prev.RightChild != null)
                    {
                        node.Prev = node.Prev.RightChild;
                        node.Prev.Next = node;
                    }
                }
            }

            return node;
        }

        private Node<T> Balance(Node<T> node)
        {
            if (GetBalanceFactor(node) > 1)
            {
                if (GetBalanceFactor(node.RightChild) < 0)
                {
                    node.RightChild = RotateRight(node.RightChild);
                }

                node = RotateLeft(node);
            }
            else if (GetBalanceFactor(node) < -1)
            {
                if (GetBalanceFactor(node.LeftChild) > 0)
                {
                    node.LeftChild = RotateLeft(node.LeftChild);
                }

                node = RotateRight(node);
            }

            return node;
        }

        private Node<T> RotateLeft(Node<T> node)
        {
            Node<T> temp = node.RightChild;
            node.RightChild = temp.LeftChild;
            temp.LeftChild = node;

            return temp;
        }

        private Node<T> RotateRight(Node<T> node)
        {
            Node<T> temp = node.LeftChild;
            node.LeftChild = temp.RightChild;
            temp.RightChild = node;

            return temp;
        }

        private int GetBalanceFactor(Node<T> node)
        {
            if (node == null)
            {
                return 0;
            }

            return Height(node.RightChild) - Height(node.LeftChild);
        }

        private int Height(Node<T> node)
        {
            if (node == null)
            {
                return -1;
            }

            return 1;
        }
    }
}