using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Ink;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading;
[assembly: log4net.Config.DOMConfigurator(Watch = true)] 

namespace InputMethodEdit
{
    public partial class InputMethodEdit : Form
    {
        Recognizers myRecognizers;
        private InkCollector myInkCollector = null;
        RecognizerContext myRecoContext = new RecognizerContext();
        private delegate void FlushClient(RecognizerContextRecognitionWithAlternatesEventArgs e);
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ///Thread t; //定义一线程 
        

        //[DllImport("user32.dll", EntryPoint = "keybd_event")]
        //public static extern void keybd_event(

        //    byte bVk,    //虚拟键值

        //    byte bScan,// 一般为0

        //    int dwFlags,  //这里是整数类型  0 为按下，2为释放

        //    int dwExtraInfo  //这里是整数类型 一般情况下设成为 0

        //);

        //private const int WS_EX_TOOLWINDOW = 0x00000080;
        //private const int WS_EX_NOACTIVATE = 0x08000000;

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= (WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);
        //        cp.Parent = IntPtr.Zero; // Keep this line only if you used UserControl
        //        return cp;
        //    }
        //}

        // 去除鼠标
        //[DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        //public extern static void ShowCursor(int status);

        //[DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        //public static extern int GetKeyboardState(byte[] pbKeyState);
        //public static bool CapsLockStatus
        //{
        //    get
        //    {
        //        byte[] bs = new byte[256];
        //        GetKeyboardState(bs);
        //        return (bs[0x14] == 1);
        //    }
        //}


        public InputMethodEdit()
        {
            InitializeComponent();
            myRecoContext.RecognitionWithAlternates += new RecognizerContextRecognitionWithAlternatesEventHandler(rct_RecognitionWithAlternates);
        }

        private void InputMethodEdit_Load(object sender, EventArgs e)
        {
            //ShowCursor(0);  //去除鼠标
            myRecognizers = new Recognizers();
            // Create a new ink collector that uses the group box handle
            myInkCollector = new InkCollector(gbInkArea.Handle);
            // Turn the ink collector on
            myInkCollector.Enabled = true;
            myRecoContext.Strokes = myInkCollector.Ink.Strokes;
            this.myInkCollector.Stroke += new InkCollectorStrokeEventHandler(ic_Stroke);

            //读取配置文件（Ferdiando,github创建分支的测试内容）
            log.Info("窗口Load...");
            //窗口加载读取拼音和
            Read_pymb_Txt("D:\\BOBRVT\\Version3.3\\Terminal\\InkRecognition\\pymb.txt");
            Read_py_Txt("D:\\BOBRVT\\Version3.3\\Terminal\\InkRecognition\\py.txt");

            //keybd_event((byte)Keys.LWin, 0, 0, 0);
            //keybd_event((byte)Keys.D, 0, 0, 0);
            //keybd_event((byte)Keys.LWin, 0, 2, 0);
            //keybd_event((byte)Keys.D, 0, 2, 0);
            //t = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadTimer));
            //t.Start();
        }

        //private void ThreadTimer()
        //{
        //    while (true)
        //    {
        //        if (this.textBox2.Text == "")
        //        {
        //            //log.Info("2222");
        //            int k = panel2.Controls.Count;
        //            for (int j = k - 1; j >= 0; j--)
        //            {
        //                Control t = panel2.Controls[j];
        //                if (t is Button)
        //                {
        //                    log.Info("123123123123123213");
        //                    this.panel2.Controls.Remove(t);  //移除它
        //                }

        //            }
        //        }
        //        else
        //        {
        //            //log.Info("1111");
        //            int count = 0;
        //            for (int i = 0; i < pymbDic.Count; i++)
        //            {
        //                if (pymbDic.ContainsKey(this.textBox2.Text.Trim()))
        //                {
        //                    for (int j = 0; j < pymbDic[this.textBox2.Text].Length; j++)
        //                    {
        //                        Button btn = new Button();//1 创建一个button
        //                        btn.Name = "N" + count;
        //                        btn.Text = pymbDic[this.textBox2.Text].Substring(j, 1);         //2 设置button的text
        //                        btn.Width = 33; //3 设置button的宽和高
        //                        btn.Height = 33;
        //                        //4 设置btn的字体大小
        //                        btn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        //                        btn.Click += new EventHandler(MoreButton_Click);
        //                        btn.FlatStyle = FlatStyle.Popup;
        //                        //5 设置btn的location（考虑三个按钮后需要换行，显示新的按钮，所以location的y会变化）
        //                        btn.Location = new System.Drawing.Point(7 + (7 * count) + (33 * count), 7);

        //                        //panel2.Controls.Add(btn);//将创建好的Radiobutton放入控件集合中
        //                        count++;
        //                    }
        //                    log.Info("count = " + count);
        //                    break;
        //                }
        //            }
        //        }
        //        Thread.Sleep(1);
        //    }
        //}


        //定义拼音map变量

        Dictionary<string, string> pymbDic = new Dictionary<string, string> { };

        private void Read_pymb_Txt(string FilePath)
        {
            String line;
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(FilePath, Encoding.Default);
            int count = 0;

            while (!sr.EndOfStream)
            {
                count++;
                line = sr.ReadLine();
                
                //log.Info("line:" + line + " length : " + line.Length);
                for (int i = 0; i < line.Length; i++)
                {
                    string car = line.Substring(i, 1);
                    //log.Info("car:" + car);
                    if (!Regex.Match(car, "[a-zA-Z]").Success)
                    {
                        pymbDic.Add(line.Substring(0, i), line.Substring(i, line.Length - i));
                        //log.Info("key:" + line.Substring(0, i ) + "   value:" + line.Substring(i, line.Length - i));
                        break;
                    }
                }
                
            }
            log.Info("Read_pymb_Txt, count:" + count.ToString());
        }

        Dictionary<string, string> pyDic = new Dictionary<string, string> { };
        private void Read_py_Txt(string FilePath)
        {
            String line;
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(FilePath, Encoding.Default);
            int count = 0;
            while (!sr.EndOfStream)
            {
                count++;
                line = sr.ReadLine();
                string key = line.Split(',')[0];
                string value = line.Split(',')[1];
                if (pyDic.ContainsKey(key))
                    pyDic[key] = pyDic[key] + "," + value;
                else
                    pyDic.Add(key, value);
            }
            log.Info("Read_py_Txt, count:" + count.ToString());
        }



        public void FormResp(RecognizerContextRecognitionWithAlternatesEventArgs e)
        {
            if (this.InvokeRequired)
            {
                FlushClient fc = new FlushClient(FormResp);
                this.Invoke(fc, e);
            }
            else
            {
                FormResp1(e);
            }
        }

        public void FormResp1(RecognizerContextRecognitionWithAlternatesEventArgs e)
        {
            string ResultString = "";
            RecognitionAlternates alts;

            if (e.RecognitionStatus == RecognitionStatus.NoError)
            {
                alts = e.Result.GetAlternatesFromSelection();

                List<string> strs = new List<string>();
                foreach (RecognitionAlternate alt in alts)
                {
                    ResultString = ResultString + alt.ToString() + " ";

                    strs.Add(alt.ToString());

                }
                int ks = panel1.Controls.Count;
                this.label1.Text = ks.ToString();
                for (int j = ks - 1; j >= 0; j--)
                {
                    Control t = panel1.Controls[j];
                    if (t is Button)
                    {
                        this.label1.Text = strs[j].ToString();
                        t.Text = strs[j].ToString();
                    }
                }
            }

            Control.CheckForIllegalCrossThreadCalls = false;
            Control.CheckForIllegalCrossThreadCalls = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void rct_RecognitionWithAlternates(object sender, RecognizerContextRecognitionWithAlternatesEventArgs e)
        {
            FormResp(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ic_Stroke(object sender, InkCollectorStrokeEventArgs e)
        {
            myRecoContext.StopBackgroundRecognition();
            myRecoContext.Strokes.Add(e.Stroke);

            //myRecoContext.CharacterAutoCompletion = RecognizerCharacterAutoCompletionMode.Full;
            myRecoContext.BackgroundRecognizeWithAlternates(0);
        }

        private void btnRecognize_Click(object sender, EventArgs e)
        {
            if (0 == myRecognizers.Count)
            {
                MessageBox.Show("There are no handwriting recognizers installed.  You need to have at least one in order to recognize ink.");
            }
            else
            {
                RecognitionStatus status;
                RecognitionResult recoResult;

                myRecoContext.Strokes = myInkCollector.Ink.Strokes;
                int iCount = myRecoContext.Strokes.Count;
                recoResult = myRecoContext.Recognize(out status);
                txtResults.SelectedText = recoResult.TopString;
               
                if (!myInkCollector.CollectingInk)
                {
                    myInkCollector.Ink.DeleteStrokes();

                    gbInkArea.Refresh();
                }
            }
        }

        private void clear_btn_Click(object sender, EventArgs e)
        {
            myInkCollector.Ink.DeleteStrokes();
            // Repaint the drawing area
            gbInkArea.Refresh();
            myRecoContext.Strokes.Clear();
        }

        private void MoreButton_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;

            textBox1.Text += b.Text;

            textBox2.Text = "";
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            //清空当前的button
            int k = panel2.Controls.Count;
            for (int j = k - 1; j >= 0; j--)
            {
                Control t = panel2.Controls[j];
                if (t is Button)
                {
                    this.panel2.Controls.Remove(t);  //移除他
                }
            }

            int count = 0;
            for (int i = 0; i < pymbDic.Count + pyDic.Count; i++)
            {
                if (pymbDic.ContainsKey(this.textBox2.Text.Trim()))
                {
                    //this.label2.Text = pymbDic[this.textBox2.Text];
                    log.Info("字。。。。" + pymbDic.ContainsKey(this.textBox2.Text.Trim()));
                    for (int j = 0; j < pymbDic[this.textBox2.Text].Length; j ++)
                    {
                        Button btn = new Button();//1 创建一个button
                        btn.Name = "N" + count;
                        btn.Text = pymbDic[this.textBox2.Text].Substring(j, 1);         //2 设置button的text
                        btn.Width = 33; //3 设置button的宽和高
                        btn.Height = 33;
                        btn.AutoSize = true;
                        //4 设置btn的字体大小
                        btn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        btn.Click += new EventHandler(MoreButton_Click);
                        btn.FlatStyle = FlatStyle.Popup;
                        //5 设置btn的location（考虑三个按钮后需要换行，显示新的按钮，所以location的y会变化）
                        btn.Location = new System.Drawing.Point(7 + (7 * count) + (33 * count), 7);
                        panel2.Controls.Add(btn);//将创建好的Radiobutton放入控件集合中
                        count++;
                    }
                    break;
                }
                else if (pyDic.ContainsKey(this.textBox2.Text.Trim()))
                {
                    log.Info("词语。。。。" + pyDic.ContainsKey(this.textBox2.Text.Trim()));

                    for (int j = 0; j < pyDic[this.textBox2.Text].Split(',').Length; j++)
                    {
                        Button btn = new Button();//1 创建一个button
                        btn.Name = "N" + count;
                        btn.Text = pyDic[this.textBox2.Text].Split(',')[j];         //2 设置button的text
                        btn.Width = 33; //3 设置button的宽和高
                        btn.Height = 33;
                        btn.AutoSize = true;
                        //4 设置btn的字体大小
                        btn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        btn.Click += new EventHandler(MoreButton_Click);
                        btn.FlatStyle = FlatStyle.Popup;
                        //5 设置btn的location（考虑三个按钮后需要换行，显示新的按钮，所以location的y会变化）
                        btn.Location = new System.Drawing.Point(7 + (7 * count) + (33 * count), 7);
                        panel2.Controls.Add(btn);//将创建好的Radiobutton放入控件集合中
                        count++;
                    }
                    break;
                }
            }
            
        }

        private void button_q_Click(object sender, EventArgs e)
        {
            //keybd_event((byte)Keys.Q, 0, 0, 0);
            //keybd_event((byte)Keys.Q, 0, 2, 0);
            this.textBox2.Text = this.textBox2.Text.Trim() + "q";
        }

        private void button_q_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void button_q_Paint(object sender, PaintEventArgs e)
        {
            //try
            //{
            //    System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
            //    System.Drawing.Rectangle newRectangle = button_q.ClientRectangle;
            //    newRectangle.Inflate(0, 0);
            //    buttonPath = CreateRoundedRectanglePath(newRectangle);
            //    Pen pen = new Pen(Brushes.Black);
            //    pen.Width = 2.0F;
            //    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
            //    e.Graphics.DrawPath(pen, buttonPath);
            //    button_q.Region = new System.Drawing.Region(buttonPath);
            //}
            //catch (Exception err)
            //{
            //    log.Info(err.ToString());
            //}
        }

        private void button_w_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "w";
        }

        private void button_e_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "e";
        }

        private void button_r_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "r";
        }

        private void button_t_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "t";
        }

        private void button_y_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "y";
        }

        private void button_u_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "u";
        }

        private void button_i_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "i";
        }

        private void button_o_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "o";
        }

        private void button_p_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "p";
        }

        private void button_a_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "a";
        }

        private void button_s_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "s";
        }

        private void button_d_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "d";
        }

        private void button_f_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "f";
        }

        private void button_g_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "g";
        }

        private void button_h_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "h";
        }

        private void button_j_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "j";
        }

        private void button_k_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "k";
        }

        private void button_l_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "l";
        }

        private void button_z_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "z";
        }

        private void button_x_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "x";
        }

        private void button_c_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "c";
        }

        private void button_v_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "v";
        }

        private void button_b_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "b";
        }

        private void button_n_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "n";
        }

        private void button_m_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox2.Text.Trim() + "m";
        }

        private void button_backspace_Click(object sender, EventArgs e)
        {
            if(this.textBox2.Text != "")
            {
                this.textBox2.Text = this.textBox2.Text.Trim().Substring(0, this.textBox2.Text.Trim().Length - 1);
            }
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //清空当前的button
            int k = panel2.Controls.Count;
            for (int j = k - 1; j >= 0; j--)
            {
                Control t = panel2.Controls[j];
                if (t is Button)
                {
                    this.panel2.Controls.Remove(t);  //移除他
                }
            }
            if (pymbDic.ContainsKey(this.textBox2.Text.Trim()))
            {
                for (int j = 0; j < pymbDic[this.textBox2.Text].Length; j++)
                {
                    Button btn = new Button();//1 创建一个button
                    btn.Name = "N" + j;
                    btn.Text = pymbDic[this.textBox2.Text].Substring(j, 1);         //2 设置button的text
                    btn.Width = 33; //3 设置button的宽和高
                    btn.Height = 33;
                    btn.AutoSize = true;
                    //4 设置btn的字体大小
                    btn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    btn.Click += new EventHandler(MoreButton_Click);
                    btn.FlatStyle = FlatStyle.Popup;
                    //5 设置btn的location（考虑三个按钮后需要换行，显示新的按钮，所以location的y会变化）
                    btn.Location = new System.Drawing.Point(7 + (7 * j) + (33 * j), 7);
                    panel2.Controls.Add(btn);//将创建好的Radiobutton放入控件集合中
                }
            }
            if (pyDic.ContainsKey(this.textBox2.Text.Trim()))
            {
                //log.Info("词语。。。。" + pyDic.ContainsKey(this.textBox2.Text.Trim()));

                for (int j = 0; j < pyDic[this.textBox2.Text].Split(',').Length; j++)
                {
                    Button btn = new Button();//1 创建一个button
                    btn.Name = "N" + j;
                    btn.Text = pyDic[this.textBox2.Text].Split(',')[j];         //2 设置button的text
                    btn.Width = 33; //3 设置button的宽和高
                    btn.Height = 33;
                    btn.AutoSize = true;
                    //4 设置btn的字体大小
                    btn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    btn.Click += new EventHandler(MoreButton_Click);
                    btn.FlatStyle = FlatStyle.Popup;
                    //5 设置btn的location（考虑三个按钮后需要换行，显示新的按钮，所以location的y会变化）
                    btn.Location = new System.Drawing.Point(7 + (7 * j) + (33 * j), 7);
                    panel2.Controls.Add(btn);//将创建好的Radiobutton放入控件集合中
                }
            }
        }

    

        private void button12_Click_1(object sender, EventArgs e)
        {
            pal_PinYin.Visible = false;
            pal_ShouXie.Visible = true;
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            pal_PinYin.Visible = true;
            pal_ShouXie.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            txtResults.Text += button11.Text;
            clear_btn_Click(null,null);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtResults.Text += button10.Text;
            clear_btn_Click(null, null);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtResults.Text += button9.Text;
            clear_btn_Click(null, null);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txtResults.Text += button8.Text;
            clear_btn_Click(null, null);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtResults.Text += button7.Text;
            clear_btn_Click(null, null);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtResults.Text += button6.Text;
            clear_btn_Click(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtResults.Text += button5.Text;
            clear_btn_Click(null, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtResults.Text += button4.Text;
            clear_btn_Click(null, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtResults.Text += button3.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtResults.Text += button2.Text;
            clear_btn_Click(null, null);
        }

        private void button41_Click(object sender, EventArgs e)
        {

        }

        private void button42_Click(object sender, EventArgs e)
        {

        }

        private void gb_pinyin_Enter(object sender, EventArgs e)
        {

        }

  


        
    }
}
