using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Pub;
using System.Collections;
using System.Threading;
using System.Drawing.Drawing2D;

namespace SoftKeyBoard
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event")]

        public static extern void keybd_event(

            byte bVk,    //虚拟键值

            byte bScan,// 一般为0

            int dwFlags,  //这里是整数类型  0 为按下，2为释放

            int dwExtraInfo  //这里是整数类型 一般情况下设成为 0

        );

        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Boolean bCapaLock = true;
        private Queue qInput = new Queue();
        private Thread tInput;
        public static System.Media.SoundPlayer sp = new System.Media.SoundPlayer();
        public delegate void  DeputeFormInfo(String lpData);

        protected override CreateParams CreateParams
        {
            get
            {
                //CreateParams ret = base.CreateParams;
                //ret.Style = (int)WindowStyles.WS_THICKFRAME | (int)WindowStyles.WS_CHILD;
                //ret.ExStyle |= (int)WindowStyles.WS_EX_NOACTIVATE | (int)WindowStyles.WS_EX_TOOLWINDOW;
                //ret.X = this.Location.X;
                //ret.Y = this.Location.Y;
                //return ret;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= (WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);
                cp.Parent = IntPtr.Zero; // Keep this line only if you used UserControl
                return cp;

                //return base.CreateParams;
            }
        }

        public static bool CapsLockStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x14] == 1);
            }
        }


        public Form1()
        {
            InitializeComponent();
            sp.SoundLocation = "D:\\BOBRVT\\Version3.3\\key.wav";
        }
                               

        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            string message;
            switch (m.Msg)
            {
                case PublicFunction.WM_COPYDATA:
                    PublicFunction.COPYDATASTRUCT mystr = new PublicFunction.COPYDATASTRUCT();
                    Type mytype = mystr.GetType();
                    mystr = (PublicFunction.COPYDATASTRUCT)m.GetLParam(mytype);
                    log.Info("Form1.cs -> DefWndProc _ mystr.lpData=" + mystr.lpData);
                    message =mystr.lpData;
                    qInput.Enqueue(message);
                    break;
                default:
                    base.DefWndProc(ref m);//调用基类函数处理非自定义消息。
                    break;
            }
        }

        private void DMessage(string str)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    DeputeFormInfo fc = new DeputeFormInfo(DMessage);
                    this.Invoke(fc, str);
                }
                else
                {
                    DealMessage(str);
                }

            }
            catch (Exception err)
            {
                log.Info("" + err.ToString());
            }
        }

        private void DealMessage(string message)
        {
            try
            {
                try
                {
                    log.Info("ShowFlag:" + message.Split('|')[0] + " ,Point:" + message.Split('|')[1] + "  ,EnglishOrNum:" + message.Split('|')[2]);
                    if (message.Split('|')[0] == "1")
                    {
                        this.WindowState = FormWindowState.Normal;
                        this.TopMost = true;
                        int iX = Convert.ToInt32(message.Split('|')[1].Split(',')[0]);
                        int iY = Convert.ToInt32(message.Split('|')[1].Split(',')[1]);
                        this.Location = new Point(iX, iY);
                        if (message.Split('|')[2] == "English")
                        {
                            panel3.Visible = true;
                            panel1.Visible = false;
                            button29.BackgroundImage = SoftKeyBoard.Properties.Resources.英文_激活;
                            button28.BackgroundImage = SoftKeyBoard.Properties.Resources.数字符号_标准;
                            if (!CapsLockStatus)
                            {
                                keybd_event((byte)Keys.CapsLock, 0, 0, 0);
                                keybd_event((byte)Keys.CapsLock, 0, 2, 0);
                            }

                            bCapaLock = true;
                            buttonA.BackgroundImage = SoftKeyBoard.Properties.Resources.A_大写标准;
                            buttonB.BackgroundImage = SoftKeyBoard.Properties.Resources.B_大写标准;
                            buttonC.BackgroundImage = SoftKeyBoard.Properties.Resources.C_大写标准;
                            buttonD.BackgroundImage = SoftKeyBoard.Properties.Resources.D_大写标准;
                            buttonE.BackgroundImage = SoftKeyBoard.Properties.Resources.E_大写标准;
                            buttonF.BackgroundImage = SoftKeyBoard.Properties.Resources.F_大写标准;
                            buttonG.BackgroundImage = SoftKeyBoard.Properties.Resources.G_大写标准;
                            buttonH.BackgroundImage = SoftKeyBoard.Properties.Resources.H_大写标准;
                            buttonI.BackgroundImage = SoftKeyBoard.Properties.Resources.I_大写标准;
                            buttonJ.BackgroundImage = SoftKeyBoard.Properties.Resources.J_大写标准;
                            buttonK.BackgroundImage = SoftKeyBoard.Properties.Resources.K_大写标准;
                            buttonL.BackgroundImage = SoftKeyBoard.Properties.Resources.L_大写标准;
                            buttonM.BackgroundImage = SoftKeyBoard.Properties.Resources.M_大写标准;
                            buttonN.BackgroundImage = SoftKeyBoard.Properties.Resources.N_大写标准;
                            buttonO.BackgroundImage = SoftKeyBoard.Properties.Resources.O_大写标准;
                            buttonP.BackgroundImage = SoftKeyBoard.Properties.Resources.P_大写标准;
                            buttonQ.BackgroundImage = SoftKeyBoard.Properties.Resources.Q_大写标准;
                            buttonR.BackgroundImage = SoftKeyBoard.Properties.Resources.R_大写标准;
                            buttonS.BackgroundImage = SoftKeyBoard.Properties.Resources.S_大写标准;
                            buttonT.BackgroundImage = SoftKeyBoard.Properties.Resources.T_大写标准;
                            buttonU.BackgroundImage = SoftKeyBoard.Properties.Resources.U_大写标准;
                            buttonV.BackgroundImage = SoftKeyBoard.Properties.Resources.V_大写标准;
                            buttonW.BackgroundImage = SoftKeyBoard.Properties.Resources.W_大写标准;
                            buttonX.BackgroundImage = SoftKeyBoard.Properties.Resources.X_大写标准;
                            buttonY.BackgroundImage = SoftKeyBoard.Properties.Resources.Y_大写标准;
                            buttonZ.BackgroundImage = SoftKeyBoard.Properties.Resources.Z_大写标准;

                        }
                        else
                        {
                            panel1.Visible = true;
                            panel3.Visible = false;
                            button29.BackgroundImage = SoftKeyBoard.Properties.Resources.英文_标准;
                            button28.BackgroundImage = SoftKeyBoard.Properties.Resources.数字符号_激活;
                        }
                        this.Show();
                    }
                    else
                    {
                        this.Hide();
                    }

                }
                catch (System.Exception ex)
                {
                    log.InfoFormat("[Form1.cs] -> DefWndProc().. 异常 catch Exception ex={0}", ex);
                }


            }
            catch (Exception err)
            {
                log.Info("DealMessage Exception :" + err.ToString());
            }
        }

        private void AddQueItem()
        {
            while (true)
            {
                try
                {
                    if (qInput != null && qInput.Count != 0)
                    {
                        object obj = qInput.Dequeue();
                        if (obj == null)
                        {
                            log.Info("    数据为空");
                        }
                        else
                        {
                            string str = obj.ToString();
                            DMessage(str);

                        }
                    }
                }
                catch (Exception err)
                {
                    log.ErrorFormat("[AgentModuleTZLC] -> AddQueItem() Exception err:{0}", err.ToString());
                }
                Thread.Sleep(1);
            }
        }

        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        public extern static void ShowCursor(int status);

        /*
        * 窗口加载查配置文件改变分辨率大小Form窗口
        * Fernando 2017-3-13
        * */
        private void ChangeFormSize()
        {
            try
            {

                this.Size = new Size(Program.ConvertInt(1530), Program.ConvertInt(528));
                log.Info("************ONE*************" + this.Size + "*************************" + this.Controls.Count);
                int count = 0;

                foreach (Control ct in this.Controls)
                {
                    if (ct is Panel)
                    {
                        ct.Size = new Size(Program.ConvertInt(1528), Program.ConvertInt(390));// new Size(1290, 390);
                        log.Info("********  " + ct.Name + "****************" + ct.Size + "*************************");
                        ct.Location = new Point(Program.ConvertInt(ct.Location.X), Program.ConvertInt(ct.Location.Y));
                        foreach (Control c in ct.Controls)
                        {
                            c.Location = new Point(Program.ConvertInt(c.Location.X), Program.ConvertInt(c.Location.Y));
                            c.Size = new Size(Program.ConvertInt(c.Size.Width), Program.ConvertInt(c.Size.Height));
                            log.Info("---panel----" + c.Name + "------location:" + c.Location.ToString() + "-----size:" + c.Size + "------" + ct.Controls.Count);
                        }
                    }
                    else
                    {
                        ct.Location = new Point(Program.ConvertInt(ct.Location.X), Program.ConvertInt(ct.Location.Y));
                        ct.Size = new Size(Program.ConvertInt(ct.Size.Width), Program.ConvertInt(ct.Size.Height));

                    }

                    ct.Font = new Font(ct.Font.FontFamily, Program.ConvertFloat(ct.Font.Size));
                    count++;
                    log.Info("---form----" + ct.Name + "-------COUNT:" + count + "-----location:" + ct.Location.ToString() + "-----size:" + ct.Size + "------");

                }
                log.Info("Count:" + count.ToString());
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ShowCursor(0);
                //this.Location = new System.Drawing.Point(75, 0);
                panel1.Visible = true;
                panel3.Visible = false;
                button29.BackgroundImage = SoftKeyBoard.Properties.Resources.英文_标准;
                button28.BackgroundImage = SoftKeyBoard.Properties.Resources.数字符号_激活;
                keybd_event((byte)Keys.LWin, 0, 0, 0);
                keybd_event((byte)Keys.D, 0, 0, 0);
                keybd_event((byte)Keys.LWin, 0, 2, 0);   //释放LWIN
                keybd_event((byte)Keys.D, 0, 2, 0);
                bCapaLock = true;
              
                if (!CapsLockStatus)
                {
                    keybd_event((byte)Keys.CapsLock, 0, 0, 0);
                    keybd_event((byte)Keys.CapsLock, 0, 2, 0);
                }
                this.Location = new Point(1920, 0);
                //this.WindowState = FormWindowState.Minimized;
                //this.Hide();
                tInput = new Thread(new ThreadStart(AddQueItem));
                tInput.IsBackground = true;
                tInput.Start();

                ChangeFormSize();// Fernando
            }
            catch (Exception err)
            {
                log.Info("Form1_Load Exception :"+err.ToString ());
            }
        }

        private void buttonQ_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Q, 0, 0, 0);
            keybd_event((byte)Keys.Q, 0, 2, 0);
        }

        private void buttonW_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.W, 0, 0, 0);
            keybd_event((byte)Keys.W, 0, 2, 0);
        }

        private void buttonE_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.E, 0, 0, 0);
            keybd_event((byte)Keys.E, 0, 2, 0);
        }

        private void buttonR_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.R, 0, 0, 0);
            keybd_event((byte)Keys.R, 0, 2, 0);
        }

        private void buttonT_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.T, 0, 0, 0);
            keybd_event((byte)Keys.T, 0, 2, 0);
        }

        private void buttonY_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Y, 0, 0, 0);
            keybd_event((byte)Keys.Y, 0, 2, 0);
        }

        private void buttonU_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.U, 0, 0, 0);
            keybd_event((byte)Keys.U, 0, 2, 0);
        }

        private void buttonI_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.I, 0, 0, 0);
            keybd_event((byte)Keys.I, 0, 2, 0);
        }

        private void buttonO_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.O, 0, 0, 0);
            keybd_event((byte)Keys.O, 0, 2, 0);
        }

        private void buttonP_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.P, 0, 0, 0);
            keybd_event((byte)Keys.P, 0, 2, 0);
        }

        private void buttonA_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.A, 0, 0, 0);
            keybd_event((byte)Keys.A, 0, 2, 0);
        }

        private void buttonS_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.S, 0, 0, 0);
            keybd_event((byte)Keys.S, 0, 2, 0);
        }

        private void buttonD_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D, 0, 0, 0);
            keybd_event((byte)Keys.D, 0, 2, 0);
        }

        private void buttonF_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.F, 0, 0, 0);
            keybd_event((byte)Keys.F, 0, 2, 0);
        }

        private void buttonG_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.G, 0, 0, 0);
            keybd_event((byte)Keys.G, 0, 2, 0);
        }

        private void buttonH_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.H, 0, 0, 0);
            keybd_event((byte)Keys.H, 0, 2, 0);
        }

        private void buttonJ_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.J, 0, 0, 0);
            keybd_event((byte)Keys.J, 0, 2, 0);
        }

        private void buttonK_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.K, 0, 0, 0);
            keybd_event((byte)Keys.K, 0, 2, 0);
        }

        private void buttonL_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.L, 0, 0, 0);
            keybd_event((byte)Keys.L, 0, 2, 0);
        }

        private void buttonZ_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Z, 0, 0, 0);
            keybd_event((byte)Keys.Z, 0, 2, 0);
        }

        private void buttonX_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.X, 0, 0, 0);
            keybd_event((byte)Keys.X, 0, 2, 0);
        }

        private void buttonC_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.C, 0, 0, 0);
            keybd_event((byte)Keys.C, 0, 2, 0);
        }

        private void buttonV_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.V, 0, 0, 0);
            keybd_event((byte)Keys.V, 0, 2, 0);
        }

        private void buttonB_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.B, 0, 0, 0);
            keybd_event((byte)Keys.B, 0, 2, 0);
        }

        private void buttonN_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.N, 0, 0, 0);
            keybd_event((byte)Keys.N, 0, 2, 0);
        }

        private void buttonM_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.M, 0, 0, 0);
            keybd_event((byte)Keys.M, 0, 2, 0);
        }

        private void buttonDEL_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Back, 0, 0, 0);
            keybd_event((byte)Keys.Back, 0, 2, 0);
        }

        private void buttonCOMPELET_Click(object sender, EventArgs e)
        {
            sp.Play();
            this.Hide();
        }

        

        private void buttonLOCK_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.CapsLock, 0, 0, 0);
            keybd_event((byte)Keys.CapsLock, 0, 2, 0);
            if (bCapaLock)
            {
                bCapaLock = false;
                buttonA.BackgroundImage = SoftKeyBoard.Properties.Resources.a_小写标准;
                buttonB.BackgroundImage = SoftKeyBoard.Properties.Resources.b_小写标准;
                buttonC.BackgroundImage = SoftKeyBoard.Properties.Resources.c_小写标准;
                buttonD.BackgroundImage = SoftKeyBoard.Properties.Resources.d_小写标准;
                buttonE.BackgroundImage = SoftKeyBoard.Properties.Resources.e_小写标准;
                buttonF.BackgroundImage = SoftKeyBoard.Properties.Resources.f_小写标准;
                buttonG.BackgroundImage = SoftKeyBoard.Properties.Resources.g_小写标准;
                buttonH.BackgroundImage = SoftKeyBoard.Properties.Resources.h_小写标准;
                buttonI.BackgroundImage = SoftKeyBoard.Properties.Resources.i_小写标准;
                buttonJ.BackgroundImage = SoftKeyBoard.Properties.Resources.j_小写标准;
                buttonK.BackgroundImage = SoftKeyBoard.Properties.Resources.k_小写标准;
                buttonL.BackgroundImage = SoftKeyBoard.Properties.Resources.l_小写标准;
                buttonM.BackgroundImage = SoftKeyBoard.Properties.Resources.m_小写标准;
                buttonN.BackgroundImage = SoftKeyBoard.Properties.Resources.n_小写标准;
                buttonO.BackgroundImage = SoftKeyBoard.Properties.Resources.o_小写标准;
                buttonP.BackgroundImage = SoftKeyBoard.Properties.Resources.p_小写标准;
                buttonQ.BackgroundImage = SoftKeyBoard.Properties.Resources.q_小写标准;
                buttonR.BackgroundImage = SoftKeyBoard.Properties.Resources.r_小写标准;
                buttonS.BackgroundImage = SoftKeyBoard.Properties.Resources.s_小写标准;
                buttonT.BackgroundImage = SoftKeyBoard.Properties.Resources.t_小写标准;
                buttonU.BackgroundImage = SoftKeyBoard.Properties.Resources.u_小写标准;
                buttonV.BackgroundImage = SoftKeyBoard.Properties.Resources.v_小写标准;
                buttonW.BackgroundImage = SoftKeyBoard.Properties.Resources.w_小写标准;
                buttonX.BackgroundImage = SoftKeyBoard.Properties.Resources.x_小写标准;
                buttonY.BackgroundImage = SoftKeyBoard.Properties.Resources.y_小写标准;
                buttonZ.BackgroundImage = SoftKeyBoard.Properties.Resources.z_小写标准;
            }
            else
            {
                bCapaLock = true;
                buttonA.BackgroundImage = SoftKeyBoard.Properties.Resources.A_大写标准;
                buttonB.BackgroundImage = SoftKeyBoard.Properties.Resources.B_大写标准;
                buttonC.BackgroundImage = SoftKeyBoard.Properties.Resources.C_大写标准;
                buttonD.BackgroundImage = SoftKeyBoard.Properties.Resources.D_大写标准;
                buttonE.BackgroundImage = SoftKeyBoard.Properties.Resources.E_大写标准;
                buttonF.BackgroundImage = SoftKeyBoard.Properties.Resources.F_大写标准;
                buttonG.BackgroundImage = SoftKeyBoard.Properties.Resources.G_大写标准;
                buttonH.BackgroundImage = SoftKeyBoard.Properties.Resources.H_大写标准;
                buttonI.BackgroundImage = SoftKeyBoard.Properties.Resources.I_大写标准;
                buttonJ.BackgroundImage = SoftKeyBoard.Properties.Resources.J_大写标准;
                buttonK.BackgroundImage = SoftKeyBoard.Properties.Resources.K_大写标准;
                buttonL.BackgroundImage = SoftKeyBoard.Properties.Resources.L_大写标准;
                buttonM.BackgroundImage = SoftKeyBoard.Properties.Resources.M_大写标准;
                buttonN.BackgroundImage = SoftKeyBoard.Properties.Resources.N_大写标准;
                buttonO.BackgroundImage = SoftKeyBoard.Properties.Resources.O_大写标准;
                buttonP.BackgroundImage = SoftKeyBoard.Properties.Resources.P_大写标准;
                buttonQ.BackgroundImage = SoftKeyBoard.Properties.Resources.Q_大写标准;
                buttonR.BackgroundImage = SoftKeyBoard.Properties.Resources.R_大写标准;
                buttonS.BackgroundImage = SoftKeyBoard.Properties.Resources.S_大写标准;
                buttonT.BackgroundImage = SoftKeyBoard.Properties.Resources.T_大写标准;
                buttonU.BackgroundImage = SoftKeyBoard.Properties.Resources.U_大写标准;
                buttonV.BackgroundImage = SoftKeyBoard.Properties.Resources.V_大写标准;
                buttonW.BackgroundImage = SoftKeyBoard.Properties.Resources.W_大写标准;
                buttonX.BackgroundImage = SoftKeyBoard.Properties.Resources.X_大写标准;
                buttonY.BackgroundImage = SoftKeyBoard.Properties.Resources.Y_大写标准;
                buttonZ.BackgroundImage = SoftKeyBoard.Properties.Resources.Z_大写标准;
            }
        }

        private void button41_Click(object sender, EventArgs e)
        {
            sp.Play();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D1, 0, 0, 0);
            keybd_event((byte)Keys.D1, 0, 2, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D2, 0, 0, 0);
            keybd_event((byte)Keys.D2, 0, 2, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D3, 0, 0, 0);
            keybd_event((byte)Keys.D3, 0, 2, 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D4, 0, 0, 0);
            keybd_event((byte)Keys.D4, 0, 2, 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D5, 0, 0, 0);
            keybd_event((byte)Keys.D5, 0, 2, 0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D6, 0, 0, 0);
            keybd_event((byte)Keys.D6, 0, 2, 0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D7, 0, 0, 0);
            keybd_event((byte)Keys.D7, 0, 2, 0);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D8, 0, 0, 0);
            keybd_event((byte)Keys.D8, 0, 2, 0);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D9, 0, 0, 0);
            keybd_event((byte)Keys.D9, 0, 2, 0);
        }

        private void button0_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D0, 0, 0, 0);
            keybd_event((byte)Keys.D0, 0, 2, 0);
        }

        private void button00_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.D0, 0, 0, 0);
            keybd_event((byte)Keys.D0, 0, 2, 0);
            keybd_event((byte)Keys.D0, 0, 0, 0);
            keybd_event((byte)Keys.D0, 0, 2, 0);
        }

        private void buttonDEL1_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Back, 0, 0, 0);
            keybd_event((byte)Keys.Back, 0, 2, 0);
        }

        private void buttonMultiply_Click(object sender, EventArgs e)
        {
            sp.Play();
            if (!CapsLockStatus)
            {
                keybd_event((byte)Keys.ShiftKey, 0, 0, 0);
                keybd_event((byte)Keys.X, 0, 0, 0);
                keybd_event((byte)Keys.ShiftKey, 0, 2, 0);
                keybd_event((byte)Keys.X, 0, 2, 0);
            }
            else
            {
                keybd_event((byte)Keys.X, 0, 0, 0);
                keybd_event((byte)Keys.X, 0, 2, 0);
            }
        }

        private void buttonDecimal_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Decimal, 0, 0, 0);
            keybd_event((byte)Keys.Decimal, 0, 2, 0);
        }

        private void buttonDivide_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Divide, 0, 0, 0);
            keybd_event((byte)Keys.Divide, 0, 2, 0);
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Left, 0, 0, 0);
            keybd_event((byte)Keys.Left, 0, 2, 0);
        }

        private void buttonRigth_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Right, 0, 0, 0);
            keybd_event((byte)Keys.Right, 0, 2, 0);
        }

        private void buttonRight1_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Right, 0, 0, 0);
            keybd_event((byte)Keys.Right, 0, 2, 0);
        }

        private void buttonLeft1_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Left, 0, 0, 0);
            keybd_event((byte)Keys.Left, 0, 2, 0);
        }

        private void buttonSpace_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Space, 0, 0, 0);
            keybd_event((byte)Keys.Space, 0, 2, 0);
        }

        private void buttonSubtract_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Subtract, 0, 0, 0);
            keybd_event((byte)Keys.Subtract, 0, 2, 0);
        }

        private void buttonSHIFT2_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.ShiftKey, 0, 0, 0);
            keybd_event((byte)Keys.D2, 0, 0, 0);
            keybd_event((byte)Keys.ShiftKey, 0, 2, 0);
            keybd_event((byte)Keys.D2, 0, 2, 0);
        }

        private void buttonSHIFT7_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.ShiftKey, 0, 0, 0);
            keybd_event((byte)Keys.D7, 0, 0, 0);
            keybd_event((byte)Keys.ShiftKey, 0, 2, 0);
            keybd_event((byte)Keys.D7, 0, 2, 0);
        }

        private void buttoncomma_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.Oemcomma, 0, 0, 0);
            keybd_event((byte)Keys.Oemcomma, 0, 2, 0);
        }

        private void buttonSHIFT3_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.ShiftKey, 0, 0, 0);
            keybd_event((byte)Keys.D3, 0, 0, 0);
            keybd_event((byte)Keys.ShiftKey, 0, 2, 0);
            keybd_event((byte)Keys.D3, 0, 2, 0);
        }

        private void buttonSHIFT9_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.ShiftKey, 0, 0, 0);
            keybd_event((byte)Keys.D9, 0, 0, 0);
            keybd_event((byte)Keys.ShiftKey, 0, 2, 0);
            keybd_event((byte)Keys.D9, 0, 2, 0);
        }

        private void buttonSHIFT0_Click(object sender, EventArgs e)
        {
            sp.Play();
            keybd_event((byte)Keys.ShiftKey, 0, 0, 0);
            keybd_event((byte)Keys.D0, 0, 0, 0);
            keybd_event((byte)Keys.ShiftKey, 0, 2, 0);
            keybd_event((byte)Keys.D0, 0, 2, 0);
        }

        private void button29_Click(object sender, EventArgs e)
        {
            sp.Play();
            panel3.Visible = true;
            panel1.Visible = false;
            try
            {
                log.Info("buttonEnglish_Click");
                button29.BackgroundImage = SoftKeyBoard.Properties.Resources.英文_激活;
                button28.BackgroundImage = SoftKeyBoard.Properties.Resources.数字符号_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {

            sp.Play();

        }

        private void button28_Click(object sender, EventArgs e)
        {
            sp.Play();
            panel1.Visible = true;
            panel3.Visible = false;
            try
            {
                log.Info("buttonEnglish_Click");
                button29.BackgroundImage = SoftKeyBoard.Properties.Resources.英文_标准;
                button28.BackgroundImage = SoftKeyBoard.Properties.Resources.数字符号_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
            
        }

        private void button19_Click(object sender, EventArgs e)
        {

            sp.Play();
        }

        private void button25_Click(object sender, EventArgs e)
        {

            sp.Play();
        }

        private void button26_Click(object sender, EventArgs e)
        {

            sp.Play();
        }

        private void button36_Click(object sender, EventArgs e)
        {

            sp.Play();
        }

        private void button37_Click(object sender, EventArgs e)
        {

            sp.Play();
        }

        private void button38_Click(object sender, EventArgs e)
        {

            sp.Play();
        }

        private void button39_Click(object sender, EventArgs e)
        {

            sp.Play();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonA_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonA_MouseDown");
                if (bCapaLock)
                {
                    buttonA.BackgroundImage = SoftKeyBoard.Properties.Resources.A_大写激活;
                }
                else
                {
                    buttonA.BackgroundImage = SoftKeyBoard.Properties.Resources.a_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonA_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonA_MouseUp");
                if (bCapaLock)
                {
                    buttonA.BackgroundImage = SoftKeyBoard.Properties.Resources.A_大写标准;
                }
                else
                {
                    buttonA.BackgroundImage = SoftKeyBoard.Properties.Resources.a_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }


        private void buttonB_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonB_MouseDown");
                if (bCapaLock)
                {
                    buttonB.BackgroundImage = SoftKeyBoard.Properties.Resources.B_大写激活;
                }
                else
                {
                    buttonB.BackgroundImage = SoftKeyBoard.Properties.Resources.b_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonB_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonB_MouseUp");
                if (bCapaLock)
                {
                    buttonB.BackgroundImage = SoftKeyBoard.Properties.Resources.B_大写标准;
                }
                else
                {
                    buttonB.BackgroundImage = SoftKeyBoard.Properties.Resources.b_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonC_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonC_MouseUp");
                if (bCapaLock)
                {
                    buttonC.BackgroundImage = SoftKeyBoard.Properties.Resources.C_大写标准;
                }
                else
                {
                    buttonC.BackgroundImage = SoftKeyBoard.Properties.Resources.c_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonC_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonC_MouseDown");
                if (bCapaLock)
                {
                    buttonC.BackgroundImage = SoftKeyBoard.Properties.Resources.C_大写激活;
                }
                else
                {
                    buttonC.BackgroundImage = SoftKeyBoard.Properties.Resources.c_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonD_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonD_MouseDown");
                if (bCapaLock)
                {
                    buttonD.BackgroundImage = SoftKeyBoard.Properties.Resources.D_大写激活;
                }
                else
                {
                    buttonD.BackgroundImage = SoftKeyBoard.Properties.Resources.d_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonD_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonDMouseUp");
                if (bCapaLock)
                {
                    buttonD.BackgroundImage = SoftKeyBoard.Properties.Resources.D_大写标准;
                }
                else
                {
                    buttonD.BackgroundImage = SoftKeyBoard.Properties.Resources.d_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonE_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonE_MouseDown");
                if (bCapaLock)
                {
                    buttonE.BackgroundImage = SoftKeyBoard.Properties.Resources.E_大写激活;
                }
                else
                {
                    buttonE.BackgroundImage = SoftKeyBoard.Properties.Resources.e_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonE_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonE_MouseUp");
                if (bCapaLock)
                {
                    buttonE.BackgroundImage = SoftKeyBoard.Properties.Resources.E_大写标准;
                }
                else
                {
                    buttonE.BackgroundImage = SoftKeyBoard.Properties.Resources.e_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonF_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonF_MouseDown");
                if (bCapaLock)
                {
                    buttonF.BackgroundImage = SoftKeyBoard.Properties.Resources.F_大写激活;
                }
                else
                {
                    buttonF.BackgroundImage = SoftKeyBoard.Properties.Resources.f_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonF_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonF_MouseUp");
                if (bCapaLock)
                {
                    buttonF.BackgroundImage = SoftKeyBoard.Properties.Resources.F_大写标准;
                }
                else
                {
                    buttonF.BackgroundImage = SoftKeyBoard.Properties.Resources.f_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonG_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonG_MouseDown");
                if (bCapaLock)
                {
                    buttonG.BackgroundImage = SoftKeyBoard.Properties.Resources.G_大写激活;
                }
                else
                {
                    buttonG.BackgroundImage = SoftKeyBoard.Properties.Resources.g_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonG_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonG_MouseUp");
                if (bCapaLock)
                {
                    buttonG.BackgroundImage = SoftKeyBoard.Properties.Resources.G_大写标准;
                }
                else
                {
                    buttonG.BackgroundImage = SoftKeyBoard.Properties.Resources.g_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonH_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonH_MouseDown");
                if (bCapaLock)
                {
                    buttonH.BackgroundImage = SoftKeyBoard.Properties.Resources.H_大写激活;
                }
                else
                {
                    buttonH.BackgroundImage = SoftKeyBoard.Properties.Resources.h_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonH_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonH_MouseUp");
                if (bCapaLock)
                {
                    buttonH.BackgroundImage = SoftKeyBoard.Properties.Resources.H_大写标准;
                }
                else
                {
                    buttonH.BackgroundImage = SoftKeyBoard.Properties.Resources.h_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonI_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonI_MouseDown");
                if (bCapaLock)
                {
                    buttonI.BackgroundImage = SoftKeyBoard.Properties.Resources.I_大写激活;
                }
                else
                {
                    buttonI.BackgroundImage = SoftKeyBoard.Properties.Resources.i_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonI_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonI_MouseUp");
                if (bCapaLock)
                {
                    buttonI.BackgroundImage = SoftKeyBoard.Properties.Resources.I_大写标准;
                }
                else
                {
                    buttonI.BackgroundImage = SoftKeyBoard.Properties.Resources.i_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonJ_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonJ_MouseDown");
                if (bCapaLock)
                {
                    buttonJ.BackgroundImage = SoftKeyBoard.Properties.Resources.J_大写激活;
                }
                else
                {
                    buttonJ.BackgroundImage = SoftKeyBoard.Properties.Resources.j_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonJ_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonJ_MouseUp");
                if (bCapaLock)
                {
                    buttonJ.BackgroundImage = SoftKeyBoard.Properties.Resources.J_大写标准;
                }
                else
                {
                    buttonJ.BackgroundImage = SoftKeyBoard.Properties.Resources.j_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonK_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonK_MouseDown");
                if (bCapaLock)
                {
                    buttonK.BackgroundImage = SoftKeyBoard.Properties.Resources.K_大写激活;
                }
                else
                {
                    buttonK.BackgroundImage = SoftKeyBoard.Properties.Resources.k_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonK_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonKMouseUp");
                if (bCapaLock)
                {
                    buttonK.BackgroundImage = SoftKeyBoard.Properties.Resources.K_大写标准;
                }
                else
                {
                    buttonK.BackgroundImage = SoftKeyBoard.Properties.Resources.k_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonL_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonL_MouseDown");
                if (bCapaLock)
                {
                    buttonL.BackgroundImage = SoftKeyBoard.Properties.Resources.L_大写激活;
                }
                else
                {
                    buttonL.BackgroundImage = SoftKeyBoard.Properties.Resources.l_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonL_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonL_MouseUp");
                if (bCapaLock)
                {
                    buttonL.BackgroundImage = SoftKeyBoard.Properties.Resources.L_大写标准;
                }
                else
                {
                    buttonL.BackgroundImage = SoftKeyBoard.Properties.Resources.l_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonM_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonM_MouseDown");
                if (bCapaLock)
                {
                    buttonM.BackgroundImage = SoftKeyBoard.Properties.Resources.M_大写激活;
                }
                else
                {
                    buttonM.BackgroundImage = SoftKeyBoard.Properties.Resources.m_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonM_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonM_MouseUp");
                if (bCapaLock)
                {
                    buttonM.BackgroundImage = SoftKeyBoard.Properties.Resources.M_大写标准;
                }
                else
                {
                    buttonM.BackgroundImage = SoftKeyBoard.Properties.Resources.m_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonN_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonN_MouseDown");
                if (bCapaLock)
                {
                    buttonN.BackgroundImage = SoftKeyBoard.Properties.Resources.N_大写激活;
                }
                else
                {
                    buttonN.BackgroundImage = SoftKeyBoard.Properties.Resources.n_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonN_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonN_MouseUp");
                if (bCapaLock)
                {
                    buttonN.BackgroundImage = SoftKeyBoard.Properties.Resources.N_大写标准;
                }
                else
                {
                    buttonN.BackgroundImage = SoftKeyBoard.Properties.Resources.n_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonO_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonO_MouseDown");
                if (bCapaLock)
                {
                    buttonO.BackgroundImage = SoftKeyBoard.Properties.Resources.O_大写激活;
                }
                else
                {
                    buttonO.BackgroundImage = SoftKeyBoard.Properties.Resources.o_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonO_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonO_MouseUp");
                if (bCapaLock)
                {
                    buttonO.BackgroundImage = SoftKeyBoard.Properties.Resources.O_大写标准;
                }
                else
                {
                    buttonO.BackgroundImage = SoftKeyBoard.Properties.Resources.o_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonP_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonP_MouseDown");
                if (bCapaLock)
                {
                    buttonP.BackgroundImage = SoftKeyBoard.Properties.Resources.P_大写激活;
                }
                else
                {
                    buttonP.BackgroundImage = SoftKeyBoard.Properties.Resources.p_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonP_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonP_MouseUp");
                if (bCapaLock)
                {
                    buttonP.BackgroundImage = SoftKeyBoard.Properties.Resources.P_大写标准;
                }
                else
                {
                    buttonP.BackgroundImage = SoftKeyBoard.Properties.Resources.p_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonQ_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonQ_MouseUp");
                if (bCapaLock)
                {
                    buttonQ.BackgroundImage = SoftKeyBoard.Properties.Resources.Q_大写标准;
                }
                else
                {
                    buttonQ.BackgroundImage = SoftKeyBoard.Properties.Resources.q_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonQ_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonQ_MouseDown");
                if (bCapaLock)
                {
                    buttonQ.BackgroundImage = SoftKeyBoard.Properties.Resources.Q_大写激活;
                }
                else
                {
                    buttonQ.BackgroundImage = SoftKeyBoard.Properties.Resources.q_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonR_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonR_MouseUp");
                if (bCapaLock)
                {
                    buttonR.BackgroundImage = SoftKeyBoard.Properties.Resources.R_大写标准;
                }
                else
                {
                    buttonR.BackgroundImage = SoftKeyBoard.Properties.Resources.r_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonR_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonR_MouseDown");
                if (bCapaLock)
                {
                    buttonR.BackgroundImage = SoftKeyBoard.Properties.Resources.R_大写激活;
                }
                else
                {
                    buttonR.BackgroundImage = SoftKeyBoard.Properties.Resources.r_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonS_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonS_MouseDown");
                if (bCapaLock)
                {
                    buttonS.BackgroundImage = SoftKeyBoard.Properties.Resources.S_大写激活;
                }
                else
                {
                    buttonS.BackgroundImage = SoftKeyBoard.Properties.Resources.s_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonS_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonS_MouseUp");
                if (bCapaLock)
                {
                    buttonS.BackgroundImage = SoftKeyBoard.Properties.Resources.S_大写标准;
                }
                else
                {
                    buttonS.BackgroundImage = SoftKeyBoard.Properties.Resources.s_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonT_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonT_MouseDown");
                if (bCapaLock)
                {
                    buttonT.BackgroundImage = SoftKeyBoard.Properties.Resources.T_大写激活;
                }
                else
                {
                    buttonT.BackgroundImage = SoftKeyBoard.Properties.Resources.t_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonT_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonT_MouseUp");
                if (bCapaLock)
                {
                    buttonT.BackgroundImage = SoftKeyBoard.Properties.Resources.T_大写标准;
                }
                else
                {
                    buttonT.BackgroundImage = SoftKeyBoard.Properties.Resources.t_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonU_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonU_MouseDown");
                if (bCapaLock)
                {
                    buttonU.BackgroundImage = SoftKeyBoard.Properties.Resources.U_大写激活;
                }
                else
                {
                    buttonU.BackgroundImage = SoftKeyBoard.Properties.Resources.u_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonU_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonU_MouseUp");
                if (bCapaLock)
                {
                    buttonU.BackgroundImage = SoftKeyBoard.Properties.Resources.U_大写标准;
                }
                else
                {
                    buttonU.BackgroundImage = SoftKeyBoard.Properties.Resources.u_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }


        private void buttonV_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonV_MouseDown");
                if (bCapaLock)
                {
                    buttonV.BackgroundImage = SoftKeyBoard.Properties.Resources.V_大写激活;
                }
                else
                {
                    buttonV.BackgroundImage = SoftKeyBoard.Properties.Resources.v_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonV_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonV_MouseUp");
                if (bCapaLock)
                {
                    buttonV.BackgroundImage = SoftKeyBoard.Properties.Resources.V_大写标准;
                }
                else
                {
                    buttonV.BackgroundImage = SoftKeyBoard.Properties.Resources.v_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonW_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonW_MouseDown");
                if (bCapaLock)
                {
                    buttonW.BackgroundImage = SoftKeyBoard.Properties.Resources.W_大写激活;
                }
                else
                {
                    buttonW.BackgroundImage = SoftKeyBoard.Properties.Resources.w_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonW_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonW_MouseUp");
                if (bCapaLock)
                {
                    buttonW.BackgroundImage = SoftKeyBoard.Properties.Resources.W_大写标准;
                }
                else
                {
                    buttonW.BackgroundImage = SoftKeyBoard.Properties.Resources.w_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonX_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonX_MouseDown");
                if (bCapaLock)
                {
                    buttonX.BackgroundImage = SoftKeyBoard.Properties.Resources.X_大写激活;
                }
                else
                {
                    buttonX.BackgroundImage = SoftKeyBoard.Properties.Resources.x_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonX_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonJ_MouseUp");
                if (bCapaLock)
                {
                    buttonX.BackgroundImage = SoftKeyBoard.Properties.Resources.X_大写标准;
                }
                else
                {
                    buttonX.BackgroundImage = SoftKeyBoard.Properties.Resources.x_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonY_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonY_MouseDown");
                if (bCapaLock)
                {
                    buttonY.BackgroundImage = SoftKeyBoard.Properties.Resources.Y_大写激活;
                }
                else
                {
                    buttonY.BackgroundImage = SoftKeyBoard.Properties.Resources.y_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonY_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonY_MouseUp");
                if (bCapaLock)
                {
                    buttonY.BackgroundImage = SoftKeyBoard.Properties.Resources.Y_大写标准;
                }
                else
                {
                    buttonY.BackgroundImage = SoftKeyBoard.Properties.Resources.y_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonZ_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonZ_MouseDown");
                if (bCapaLock)
                {
                    buttonZ.BackgroundImage = SoftKeyBoard.Properties.Resources.Z_大写激活;
                }
                else
                {
                    buttonZ.BackgroundImage = SoftKeyBoard.Properties.Resources.z_小写激活;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonZ_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonZ_MouseUp");
                if (bCapaLock)
                {
                    buttonZ.BackgroundImage = SoftKeyBoard.Properties.Resources.Z_大写标准;
                }
                else
                {
                    buttonZ.BackgroundImage = SoftKeyBoard.Properties.Resources.z_小写标准;
                }
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }


        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button1_MouseDown");
                button1.BackgroundImage = SoftKeyBoard.Properties.Resources._1_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button1_MouseUp");
                button1.BackgroundImage = SoftKeyBoard.Properties.Resources._1_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button2_MouseDown");
                button2.BackgroundImage = SoftKeyBoard.Properties.Resources._2_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button1_MouseUp");
                button2.BackgroundImage = SoftKeyBoard.Properties.Resources._2_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button3_MouseDown");
                button3.BackgroundImage = SoftKeyBoard.Properties.Resources._3_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button3_MouseUp");
                button3.BackgroundImage = SoftKeyBoard.Properties.Resources._3_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button4_MouseDown");
                button4.BackgroundImage = SoftKeyBoard.Properties.Resources._4_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button4_MouseUp");
                button4.BackgroundImage = SoftKeyBoard.Properties.Resources._4_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button5_MouseDown");
                button5.BackgroundImage = SoftKeyBoard.Properties.Resources._5_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button5_MouseUp");
                button5.BackgroundImage = SoftKeyBoard.Properties.Resources._5_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button6_MouseDown");
                button6.BackgroundImage = SoftKeyBoard.Properties.Resources._6_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button6_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button6_MouseUp");
                button6.BackgroundImage = SoftKeyBoard.Properties.Resources._6_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button7_MouseDown");
                button7.BackgroundImage = SoftKeyBoard.Properties.Resources._7_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button7_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button7_MouseUp");
                button7.BackgroundImage = SoftKeyBoard.Properties.Resources._7_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button8_MouseDown");
                button8.BackgroundImage = SoftKeyBoard.Properties.Resources._8_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button8_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button8_MouseUp");
                button8.BackgroundImage = SoftKeyBoard.Properties.Resources._8_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button9_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button9_MouseDown");
                button9.BackgroundImage = SoftKeyBoard.Properties.Resources._9_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button9_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button9_MouseUp");
                button9.BackgroundImage = SoftKeyBoard.Properties.Resources._9_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button0_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button0_MouseDown");
                button0.BackgroundImage = SoftKeyBoard.Properties.Resources._0_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
             
        }

        private void button0_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button0_MouseUp");
                button0.BackgroundImage = SoftKeyBoard.Properties.Resources._0_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button00_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button00_MouseDown");
                button00.BackgroundImage = SoftKeyBoard.Properties.Resources._00_数字激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button00_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("button00_MouseUp");
                button00.BackgroundImage = SoftKeyBoard.Properties.Resources._00_数字标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDEL_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonDEL_MouseDown");
                buttonDEL.BackgroundImage = SoftKeyBoard.Properties.Resources.删_激活 ;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDEL_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonDEL_MouseUp");
                buttonDEL.BackgroundImage = SoftKeyBoard.Properties.Resources.删_标准 ;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonCOMPELET_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonCOMPELET_MouseDown");
                buttonCOMPELET.BackgroundImage = SoftKeyBoard.Properties.Resources.完成_激活 ;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonCOMPELET_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonCOMPELET_MouseUp");
                buttonCOMPELET.BackgroundImage = SoftKeyBoard.Properties.Resources.完成_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonLeft_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonLeft_MouseDown");
                buttonLeft.BackgroundImage = SoftKeyBoard.Properties.Resources.方向左_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonLeft_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonLeft_MouseUp");
                buttonLeft.BackgroundImage = SoftKeyBoard.Properties.Resources.方向左_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonRigth_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonRigth_MouseDown");
                buttonRigth.BackgroundImage = SoftKeyBoard.Properties.Resources.方向右_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonRigth_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonRigth_MouseUp");
                buttonRigth.BackgroundImage = SoftKeyBoard.Properties.Resources.方向右_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonLOCK_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonLOCK_MouseDown");
                buttonLOCK.BackgroundImage = SoftKeyBoard.Properties.Resources.大小写_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonLOCK_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonLOCK_MouseUP");
                buttonLOCK.BackgroundImage = SoftKeyBoard.Properties.Resources.大小写_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button30_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void button29_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void buttonSpace_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSpace_MouseDown");
                buttonSpace.BackgroundImage = SoftKeyBoard.Properties.Resources.空格_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSpace_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSpace_MouseUp");
                buttonSpace.BackgroundImage = SoftKeyBoard.Properties.Resources.空格_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonMultiply_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonMultiply_MouseDown");
                buttonMultiply.BackgroundImage = SoftKeyBoard.Properties.Resources.叉_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonMultiply_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonMultiply_MouseUp");
                buttonMultiply.BackgroundImage = SoftKeyBoard.Properties.Resources.叉_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonLeft1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonLeft1_MouseDown");
                buttonLeft1.BackgroundImage = SoftKeyBoard.Properties.Resources.方向左_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonLeft1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonLeft1_MouseUp");
                buttonLeft1.BackgroundImage = SoftKeyBoard.Properties.Resources.方向左_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonRight1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonRigth1_MouseDown");
                buttonRight1.BackgroundImage = SoftKeyBoard.Properties.Resources.方向右_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonRight1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonRigth1_MouseUp");
                buttonRight1.BackgroundImage = SoftKeyBoard.Properties.Resources.方向右_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT2_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSHIFT2_MouseDown");
                buttonSHIFT2.BackgroundImage = SoftKeyBoard.Properties.Resources.__激活1;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT2_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSHIFT2_MouseUp");
                buttonSHIFT2.BackgroundImage = SoftKeyBoard.Properties.Resources.__标准1;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT3_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSHIFT3_MouseDown");
                buttonSHIFT3.BackgroundImage = SoftKeyBoard.Properties.Resources.__激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT3_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSHIFT3_MouseUp");
                buttonSHIFT3.BackgroundImage = SoftKeyBoard.Properties.Resources.__标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSubtract_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSubtract_MouseDown");
                buttonSubtract.BackgroundImage = SoftKeyBoard.Properties.Resources.中横线_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSubtract_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSubtract_MouseUp");
                buttonSubtract.BackgroundImage = SoftKeyBoard.Properties.Resources.中横线_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT7_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSHIFT7_MouseDown");
                buttonSHIFT7.BackgroundImage = SoftKeyBoard.Properties.Resources.且_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT7_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSHIFT7_MouseUp");
                buttonSHIFT7.BackgroundImage = SoftKeyBoard.Properties.Resources.且_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDivide_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonDivide_MouseDown");
                buttonDivide.BackgroundImage = SoftKeyBoard.Properties.Resources.左撇_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDivide_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonDivide_MouseUp");
                buttonDivide.BackgroundImage = SoftKeyBoard.Properties.Resources.左撇_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttoncomma_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttoncomma_MouseDown");
                buttoncomma.BackgroundImage = SoftKeyBoard.Properties.Resources.逗号_激活 ;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttoncomma_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttoncomma_MouseUp");
                buttoncomma.BackgroundImage = SoftKeyBoard.Properties.Resources.逗号_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDecimal_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonDecimal_MouseDown");
                buttonDecimal.BackgroundImage = SoftKeyBoard.Properties.Resources.点_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDecimal_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonDecimal_MouseUp");
                buttonDecimal.BackgroundImage = SoftKeyBoard.Properties.Resources.点_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDEL1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonDEL1_MouseDown");
                buttonDEL1.BackgroundImage = SoftKeyBoard.Properties.Resources.删_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDEL1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonDEL1_MouseUp");
                buttonDEL1.BackgroundImage = SoftKeyBoard.Properties.Resources.删_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonCOMPELET_MouseClick(object sender, MouseEventArgs e)
        {

        }


        private void buttonCOMPELET1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonCOMPELET1_MouseDown");
                button41.BackgroundImage = SoftKeyBoard.Properties.Resources.完成_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonCOMPELET1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonCOMPELET1_MouseUp");
                button41.BackgroundImage = SoftKeyBoard.Properties.Resources.完成_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT9_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSHIFT9_MouseDown");
                buttonSHIFT9.BackgroundImage = SoftKeyBoard.Properties.Resources.左括号_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT9_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSHIFT9_MouseUp");
                buttonSHIFT9.BackgroundImage = SoftKeyBoard.Properties.Resources.左括号_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT0_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSHIFT0_MouseDown");
                buttonSHIFT0.BackgroundImage = SoftKeyBoard.Properties.Resources.右括号_激活;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT0_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                log.Info("buttonSHIFT0_MouseUp");
                buttonSHIFT0.BackgroundImage = SoftKeyBoard.Properties.Resources.右括号_标准;
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonRigth_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonRigth.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);

                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonRigth.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }


        internal static GraphicsPath CreateRoundedRectanglePath(Rectangle rect)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            try
            {
                int cornerRadius = 13;
                roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
                roundedRect.AddLine(rect.X + cornerRadius * 2, rect.Y+1, rect.Right - cornerRadius * 2, rect.Y+1);
                roundedRect.AddArc(rect.Right - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
                roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Bottom - cornerRadius * 2);
                roundedRect.AddArc(rect.Right - cornerRadius * 2, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
                roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                roundedRect.AddLine(rect.X+1, rect.Bottom - cornerRadius * 2, rect.X +1, rect.Y + cornerRadius * 2);
                roundedRect.CloseFigure();
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
            return roundedRect;
        }

        private void buttonLeft_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonLeft.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonLeft.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonM_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonM.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonM.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonN_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonN.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonN.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonB_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonB.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonB.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonV_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonV.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonV.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonC_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonC.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonC.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonX_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonX.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonX.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonZ_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonZ.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonZ.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSpace_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonSpace.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonSpace.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button28_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button28.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button28.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button29_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button29.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button29.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button30_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button30.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button30.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonLOCK_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonLOCK.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonLOCK.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonA_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonA.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                buttonA.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonS_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonS.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonS.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonD_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonD.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonD.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonF_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonF.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonF.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonG_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonG.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonG.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonH_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonH.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonH.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonJ_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonJ.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonJ.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonK_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonK.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonK.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonL_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonL.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonL.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonCOMPELET_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonCOMPELET.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonCOMPELET.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDEL_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonDEL.ClientRectangle;
                newRectangle.Inflate(0, 0);

                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonDEL.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonP_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonP.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonP.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonO_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonO.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonO.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonI_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonI.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonI.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonU_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonU.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonU.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonY_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonY.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonY.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonT_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonT.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                buttonT.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonR_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonR.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonR.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonE_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonE.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonE.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonW_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonW.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonW.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonQ_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonQ.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonQ.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button1.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button1.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button2.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button2.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button3.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button3.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button4_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button4.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button4.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button5.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button5.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button6_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button6.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button6.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button7_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button7.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button7.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button8_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button8.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button8.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button9_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button9.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button9.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button0_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button0.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button0.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button00_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button00.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button00.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonMultiply_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonMultiply.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonMultiply.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT2_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonSHIFT2.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonSHIFT2.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT3_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonSHIFT3.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonSHIFT3.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSubtract_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonSubtract.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonSubtract.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT7_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonSHIFT7.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonSHIFT7.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDivide_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonDivide.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonDivide.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttoncomma_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttoncomma.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttoncomma.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDecimal_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonDecimal.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonDecimal.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonDEL1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonDEL1.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonDEL1.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button41_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button41.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button41.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT9_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonSHIFT9.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonSHIFT9.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonSHIFT0_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonSHIFT0.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonSHIFT0.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button39_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button39.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button39.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button38_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button38.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button38.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button37_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button37.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button37.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button36_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button36.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button36.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button26_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button26.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button26.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button25_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button25.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button25.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void button19_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = button19.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                button19.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonLeft1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonLeft1.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonLeft1.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }

        private void buttonRight1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Rectangle newRectangle = buttonRight1.ClientRectangle;
                newRectangle.Inflate(0, 0);
                buttonPath = CreateRoundedRectanglePath(newRectangle);
                Pen pen = new Pen(Brushes.Black);
                pen.Width = 2.0F;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                e.Graphics.DrawPath(pen, buttonPath);
                buttonRight1.Region = new System.Drawing.Region(buttonPath);
            }
            catch (Exception err)
            {
                log.Info(err.ToString());
            }
        }






    }
}
