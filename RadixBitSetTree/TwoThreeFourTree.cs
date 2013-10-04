using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadixTreePerformance
{
    using System.Collections.Generic;
    using System.Linq;

    public class TwoThreeFourNode
    {
        public TwoThreeFourNode Parent;

        public List<int> Values;
        public List<TwoThreeFourNode> Children;

        public TwoThreeFourNode()
        {
            Values = new List<int>();
            Children = new List<TwoThreeFourNode>(4);

            //Use Add not [i]:
            for (int i = 0; i <= 3; i++) Children.Add(null);
        }

        public TwoThreeFourNode AddValue(int value)
        {
            Values.Add(value);
            Values.Sort();

            if (Values.Count() == 4)
                return split();

            if (Values.Count() > 4)
                throw new Exception("ADD VALUES: Too many values!");

            return this;
        }

        private TwoThreeFourNode split()
        {
            if (Parent == null)
                Parent = new TwoThreeFourNode();


            TwoThreeFourNode node1 = new TwoThreeFourNode();
            TwoThreeFourNode node2 = new TwoThreeFourNode();

            node1.AddValue(Values[0]);
            node1.AddValue(Values[1]);
            Parent.AddValue(Values[2]);
            node2.AddValue(Values[3]);




            if (Children[0] != null)
                node1.UpdateChild(Children[0]);
            if (Children[1] != null)
                node1.UpdateChild(Children[1]);

            if (Children[2] != null)
                node2.UpdateChild(Children[2]);
            if (Children[3] != null)
                node2.UpdateChild(Children[3]);


            Parent.UpdateChild(node1);
            Parent.UpdateChild(node2);
            return Parent;
        }


        public TwoThreeFourNode GotoChild(int value)
        {
            /*
            for (int i = 0; i < Children.Count; i++)//The child nodes
            {
                if ((i == 0 || Values[i - 1] < value) && (i == (Children.Count - 1) || value < Values[i]))
                    return Children[i];
            }
            */

            switch (Values.Count)
            {
                case 1:
                    {
                        if (value < Values[0])
                            return Children[0];

                        return Children[1];
                    }
                case 2:
                    {
                        if (value < Values[0])
                            return Children[0];

                        if (Values[0] < value && value < Values[1])
                            return Children[1];

                        if (Values[1] < value)
                            return Children[2];
                        break;
                    }
                case 3:
                    {
                        if (value < Values[0])
                            return Children[0];

                        if (Values[0] < value && value < Values[1])
                            return Children[1];

                        if (Values[1] < value && value < Values[2])
                            return Children[2];

                        if (Values[2] < value)
                            return Children[3];

                        break;
                    }
            }

            throw new Exception("GotoChild: You have broken the data representation");
        }

        public void UpdateChild(TwoThreeFourNode child)
        {
            int value = child.Values.Last();

            /*
            for (int i = 0; i < Children.Count; i++)//The child nodes
            {
                if ((i == 0 || Values[i - 1] < value) && (i == (Children.Count - 1) || value < Values[i]))
                    Children[i] = child;
            }

            throw new Exception("GotoChild: You have broken the data representation");
            */

            switch (Values.Count)
            {
                case 1:
                    {
                        if (value < Values[0])
                            Children[0] = child;
                        else
                            Children[1] = child;
                        break;
                    }
                case 2:
                    {
                        if (value < Values[0])
                            Children[0] = child;
                        else if (Values[0] < value && value < Values[1])
                            Children[1] = child;
                        else if (Values[1] < value)
                            Children[2] = child;
                        break;
                    }
                case 3:
                    {
                        if (value < Values[0])
                            Children[0] = child;
                        else if (Values[0] < value && value < Values[1])
                            Children[1] = child;
                        else if (Values[1] < value && value < Values[2])
                            Children[2] = child;
                        else if (Values[2] < value)
                            Children[3] = child;
                        break;
                    }
                default:
                    throw new Exception("Uh oh, spaghetti o's");
            }


        }
    }

    public class TwoThreeFourTree
    {
        public TwoThreeFourNode RootTwoThreeFourNode = new TwoThreeFourNode();
        public TwoThreeFourTree()
        {

        }

        public TwoThreeFourTree(TwoThreeFourNode rootTwoThreeFourNode)
        {
            this.RootTwoThreeFourNode = rootTwoThreeFourNode;
        }

        public void AddValue(int value)
        {
            var _currentNode = this.RootTwoThreeFourNode;

            //Go to the bottom of the RootTwoThreeFourNode:
            while (true)
            {
                if (_currentNode.Children.All(i => i == null))
                {
                    _currentNode.AddValue(value);
                    break;
                }

                _currentNode = _currentNode.GotoChild(value);
            }

            //Go back up the RootTwoThreeFourNode:
            while (true)
            {

                if (_currentNode.Parent != null)
                {
                    _currentNode = _currentNode.Parent;
                    continue;
                }

                this.RootTwoThreeFourNode = _currentNode;
                break;
            }
        }
    }
}
