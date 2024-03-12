// See https://aka.ms/new-console-template for more information

/*
 * Created by 
 * dotnet new sln -n BinaryTree
 * dotnet new console -n BinaryTreeConsole -o BinaryTreeConsole
 * dotnet sln add --in-root BinaryTreeConsole\BinaryTreeConsole.csproj
 */

using Console1;

Console.WriteLine("Start ...");


BinaryTree tree = new BinaryTree();

tree.Insert(50);
tree.Insert(30);
tree.Insert(20);
tree.Insert(40);
tree.Insert(70);
tree.Insert(60);
tree.Insert(80);

Console.WriteLine("Inorder traversal of the constructed tree is:");
tree.TraverseInOrder();
Console.WriteLine();

tree.TraverseInOrderDescend();
Console.WriteLine();