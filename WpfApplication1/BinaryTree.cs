using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class BinaryTree : MainWindow
    {

        public Dictionary<char, string> MapDic;//dictoinary
        public List<char> Allchar = new List<char>();
        public List<Node> Nodes = new List<Node>();// all nodes
        public Node head ;	// the first element
 
        public class Node
        {
            public char symbol;
            public Node left, right, prev /*previous node*/;
            public int counter /*num of appearance*/, nodeNum;
            public string code;
            public string ascii;

        }

        public BinaryTree(Dictionary<char, string> MapDic, List<char> Allchar)
        {

            this.MapDic = MapDic;
            this.Allchar = Allchar;
           
        }
        public BinaryTree() { this.head = null; }// defult variable
       

        public Node StartPos = null; // start from this position
        public Node curPos = null; // current position of node
        public int nodeNum = 100; // weight of node
        public List<string> compRslt = new List<string>(); // compression result
        public string decmprsRslt = ""; // decompression result
        
        public void chngcods (Node top) // change codes of char after swap the nodes (code 0 or 1 or ...)  
        {

            if (top.left != null)
            {
                if (top.code[0] != top.left.code[0])  // change code of each nodes 

                    while (top.left != null)
                    {

                        top.right.code = top.code + "1";
                        top.left.code = top.code + "0";
                        top.right.prev = top;
                        top.left.prev = top;
                      
                        
                        if (top.left.symbol >= 'a' && top.left.symbol <= 'z')
                            top = top.left;
                        else if (top.right.symbol >= 'a' && top.right.symbol <= 'z')
                            top = top.right;
                    }
            }
            else if (curPos.left != null)
            {
                top = curPos;
                if (curPos.code[0] != curPos.left.code[0])  // change code of each nodes 

                    while (top.left != null)
                    {

                        top.right.code = top.code + "1";
                        top.left.code = top.code + "0";
                        top.right.prev = top;
                        top.left.prev = top;

                        if (top.left.symbol >= 'a' && top.left.symbol <= 'z')
                            top = top.left;
                        else if (top.right.symbol >= 'a' && top.right.symbol <= 'z')
                            top = top.right;

                    }


            } 
        }

        public List<Node> swapNods(Node top)
        {
            List<Node> tmpNods = new List<Node>();

            Node temp = null;
            temp = new Node();
            Node swpWth = top;

            Node box1 = curPos;
            Node box2 = swpWth;

            Node Prev1 = swpWth.prev;
            Node Prev2 = curPos.prev;

            int nodNum1 = swpWth.nodeNum;
            int nodNum2 = curPos.nodeNum;

            string code1 = swpWth.code;
            string code2 = curPos.code;

            //swap nodes
            temp = swpWth;
            swpWth = curPos;
            curPos = temp;

            swpWth.nodeNum = nodNum1;
            curPos.nodeNum = nodNum2;
            swpWth.code = code1;
            curPos.code = code2;


            if (Prev1.left == box2)
                Prev1.left = swpWth;
            else if (Prev1.right == box2)
                Prev1.right = swpWth;

            if (Prev2.left == box1)
                Prev2.left = curPos;
            else if (Prev2.right == box1)
                Prev2.right = curPos;

            swpWth.prev = Prev1;
            curPos.prev = Prev2;


            tmpNods.Add(curPos);
            tmpNods.Add(swpWth);


            return tmpNods;
        }

        public void FirstAppear(char symbol, string ascii, int type /*compress or decompress*/)
        {
            Node top = new Node();
            Node Left = new Node();
            Node Right = new Node();
            Node Prev = top.prev;
            Node temp = null; 
                 temp = new Node();

                 if (head == null)
                 {
                     // top 
                     top.counter = 0;
                     top.nodeNum = nodeNum;
                     top.prev = null;
                     head = top;

                     Nodes.Add(head);
                 }
                 else 
                   top = StartPos;// start from last position
                
                Left = top.left;
                Right = top.right;


                // right
                temp.symbol = symbol;
                temp.counter = 1; // increment new node
                temp.code = top.code + "1";
                temp.nodeNum = --nodeNum;
                temp.prev = top;
                temp.ascii = ascii;
                Right = temp;
                top.right = Right;

                Nodes.Add(Right);


                // empty temp 
                temp = null;
                temp = new Node();

                // left
                temp.symbol = ' ';
                temp.counter = 0;
                temp.code = top.code + "0"; // split NYT (not transmuted yet)
                temp.nodeNum = --nodeNum;
                temp.prev = top;

                Left = temp;
                top.left = Left;

                Nodes.Add(Left);

                curPos = Right.prev; // get old node
                curPos.counter++; // increment old node


                if (type == 1) // compress 
                {
                    if (Allchar.Count != MapDic.Count){
                        compRslt.Add(Right.ascii); 
                        compRslt.Add(Left.code + ""); // send ascii and NYT
                    
                    }
                    else
                        compRslt.Add(Right.ascii + ""); 

                }
                else  // decompress 

                    decmprsRslt += Right.symbol + "";  // send symbol

                StartPos = Left;
                
            
            UpdateTree(0);
        }
        
        public void NotFirstAppear(char symbol, string code , int type)
        {

            for (int indxNd = 0; indxNd < Nodes.Count; indxNd++) 
            {
                Node top = Nodes[indxNd];
                if (type == 1) // compress
                {

                    if (top.symbol == symbol)
                    {

                        curPos = top; // go to symbol node

                        compRslt.Add(top.code + ""); // send code of symbol
                        StartPos = top; //
                        break;
                    }
                }
                else if (type == 2) // decompress
                {
                    if (top.code == code)
                    {
                        curPos = top; // go to symbol node

                        decmprsRslt += top.symbol + ""; // send symbol

                        StartPos = top;
                        break;
                    }


                }

            }
            UpdateTree(1);

        }

        public void UpdateTree(int check)
        {

            Node top = null;
          
            while (true)
            {
                if (curPos == head) // is it root node ?
                    break;

                if (check == 0)
                    curPos = curPos.prev; // go to parent

                if (check == 1) 
                    check = 0; // don's go to parent in the first time

                for (int indxNd = 0; indxNd < Nodes.Count; indxNd++) // search for nodes need swap
                {
                    top = Nodes[indxNd];
                    if (curPos != head && top.code != null)

                        if (curPos.counter >= top.counter && curPos.nodeNum < top.nodeNum)
                        {

                            if ((curPos.code[0] != top.code[0] || (curPos.symbol >= 'a' && curPos.symbol <= 'z') && (top.symbol >= 'a' && top.symbol <= 'z')))
                            {

                                List<Node> swap = new List<Node>(swapNods(top));

                                curPos = swap[0];
                                top = curPos;
                                curPos = swap[1];
                                chngcods(top);

                                break;


                            }
                        }        
                }
                curPos.counter++; // increment num of appear 
            
            }

        }

    }
}
