
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Dictionary<char, string> MapDic; // dictioanry
        List<char> Allchar;// all char without duplicate
        BinaryTree BT;

        string text = "";

        // read dictionary
        void readDic()
        {
            MapDic = new Dictionary<char, string>();

            string[] Dic = dic.Text.Split();

            for (int i = 0; i < Dic.Length; i += 2)
                MapDic.Add(char.Parse(Dic[i]), Dic[i + 1]);


        }

        // Compression
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (dic.Text.Length == 0)
                MessageBox.Show("You should fill the dictionary");
            else
            {
                readDic(); // get dictionary

                text = screen.Text; // take input from user

                screen.Text = "";

                string asciCd = text;
                Allchar = new List<char>();
                BT = new BinaryTree(MapDic, Allchar);

                for (int i = 0; i < text.Length; i++)
                {

                    if (Allchar.IndexOf(text[i]) == -1)// first appearance
                    {
                        Allchar.Add(text[i]); // add new char
                        BT.FirstAppear(text[i] /*send char*/, MapDic[asciCd[i]]/*send ascii of char*/, 1 /*1 mean this is compression*/);

                    }
                    else // not first appearance

                        BT.NotFirstAppear(text[i], "", 1);

                }
                // print compression result
                for (int i = 0; i < BT.compRslt.Count; i++)
                {
                    if (BT.compRslt.Count == 2 && i == 1)
                        break;

                    screen.Text += BT.compRslt[i] + " ";
                }

            }
        }

        // Decompression
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            if (dic.Text.Length == 0)
                MessageBox.Show("You should fill the dictionary");
            else
            {

                readDic();

                string[] compRslt = screen.Text.Split();


                BT = new BinaryTree(); // empty the binary tree
                text = screen.Text;
                screen.Text = "";
                char c = ' ';
                Allchar = new List<char>();

                for (int i = 0; i < compRslt.Length; i++)
                {
                    if (Allchar.Count != MapDic.Count)
                    {
                        var items = from pair in MapDic select pair;
                        foreach (KeyValuePair<char, string> pair in items) // search in dictionary 
                        {
                            if (compRslt[i] == pair.Value)
                            {
                                c = pair.Key;
                                break;
                            }
                        }
                    }

                    if (Allchar.IndexOf(c) == -1 && c != ' ') // first appearance
                    {
                        Allchar.Add(c);
                        BT.FirstAppear(c, MapDic[c]/*send ascii of char*/, 2 /* 2 mean this is decompression*/);
                        if (Allchar.Count != MapDic.Count)
                            i++;
                    }
                    else // not first appearance
                        BT.NotFirstAppear(c, compRslt[i] /*this for code in binary tree 0 or 1 or ...*/, 2);

                    c = ' ';
                }
                screen.Text = BT.decmprsRslt;
            }
        }

    }
}
