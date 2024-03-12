using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console1;

public class BinaryTree
{
    private TreeNode root;

    public BinaryTree()
    {
        root = null;
    }

    public void Insert(int value)
    {
        root = InsertRec(root, value);
    }

    private TreeNode InsertRec(TreeNode root, int value)
    {
        if (root == null)
        {
            root = new TreeNode(value);
            return root;
        }

        if (value < root.Value)
            root.Left = InsertRec(root.Left, value);
        else if (value > root.Value)
            root.Right = InsertRec(root.Right, value);

        return root;
    }

    public void TraverseInOrder()
    {
        TraverseInOrder(root);
    }

    private void TraverseInOrder(TreeNode root)
    {
        if (root != null)
        {
            TraverseInOrder(root.Left);
            Console.Write(root.Value + " ");
            TraverseInOrder(root.Right);
        }
    }


    public void TraverseInOrderDescend()
    {
        TraverseInOrderDescend(root);
    }

    private void TraverseInOrderDescend(TreeNode root)
    {
        if (root != null)
        {
            TraverseInOrderDescend(root.Right);
            Console.Write(root.Value + " ");
            TraverseInOrderDescend(root.Left);
        }
    }

}