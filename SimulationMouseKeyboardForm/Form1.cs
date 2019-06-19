using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimulationMouseKeyboardForm
{
    public partial class Form1Name : Form
    {
        public Form1Name()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "你自动点击了按钮..";
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("click label3");
        }

        private void Form1Name_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("你点击了窗体");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("label2···");
        }

        private void ccPanel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void ccPanel_Click(object sender, EventArgs e)
        {
            Point formPoint = this.PointToClient(Control.MousePosition);
            var point = Control.MousePosition;
            this.textBox1.Text += $"\r\n(point.x:{point.X}, point.y:{point.Y})-" +
                                  $"-point.x-formPoint.x={point.X-formPoint.X} ; point.y-formPoint.y={point.Y-formPoint.Y}";
            this.textBox1.Text += $"(formPoint.x : {formPoint.X}, formPoint.y : {formPoint.Y})\r\n";

            //MessageBox.Show($"PanelClick--> (x : {formPoint.X}, y : {formPoint.Y})");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form2 = new Form2();
            form2.ShowDialog();//这种方式必须关闭form2,才能返回form1
            //form2.Show();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                MessageBox.Show(treeView1.SelectedNode.Text);
            }
        }
    }
}
