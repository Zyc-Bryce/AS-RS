using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StackerEfficiency
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //清空窗体文本框
            foreach (Control control in this.Controls)
            {
                if (control is TextBox || control is ComboBox)
                {
                    control.Text = "";
                }
            }
            ////清空容器内文本框
            //foreach (Control control in gb1.Controls)
            //{
            //    if (control is TextBox || control is ComboBox)
            //    {
            //        control.Text = "";
            //    }
            //}
        }
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string caseNum = comboBox1.Text;//1 单出单入 2 出入循环 
                if (caseNum == "")
                {
                    MessageBox.Show("请选择运行模式");
                }
                double lenght = double.Parse(textBoxNum1.Text);//仓库长度
                double height = double.Parse(textBoxNum2.Text);//仓库高度
                double Inheight = double.Parse(textBoxNum3.Text);//入口高度
                double OutHeiht = double.Parse(textBoxNum4.Text);//出口高度
                double Inlenght = double.Parse(textBoxNum5.Text);//入口水平距离
                double Outlenght = double.Parse(textBoxNum6.Text);//出口水平距离

                double runspe = double.Parse(textBoxNum10.Text);//行走速度
                double runspea = double.Parse(textBoxNum11.Text);//行走加速度
                double risispe = double.Parse(textBoxNum12.Text);//提升速度
                double risispea = double.Parse(textBoxNum13.Text);//提升加速度
                double loadForkSpe = double.Parse(textBoxNum14.Text);//负载伸叉速度
                double loadForkSpea = double.Parse(textBoxNum15.Text);//负载伸叉加速度
                double unloadForkSpe = double.Parse(textBoxNum16.Text);//空载伸叉速度
                double unloadForkSpea = double.Parse(textBoxNum17.Text);//空载伸叉加速度
                double forkLenght = double.Parse(textBoxNum18.Text);//单叉长度
                double lifttime = double.Parse(textBoxNum7.Text);//取放货微升降时间
                ///double settime;//堆垛机定位时间--备用
                double softwaretime = double.Parse(textBoxNum8.Text);//软件调度损耗
                double altertime = double.Parse(textBoxNum9.Text);//硬件调度损耗（一套完整动作内设备运行外的损耗时间）

                double AtoIPtime; //入口到入库库位时间
                double IPtoOPtime;//入库库位到出库库位时间
                double OPtoEtime;//出库库位到出口时间
                double EtoAtime;//出口到入口时间
                double OPtoAtime;//出库库位到入口时间
                double loadForkTime;//负载伸叉时间
                double unloadForkTime;//空载伸叉时间

                double allTime;//流程总时间
                double allFockTime;//货叉总时间
                double allRunTime=0;//行走总时间
                double allOtherTime;//其他损耗时间

                //lenght = 90; height = 24; Inheight = 13.5; OutHeiht = 1.5; Inlenght = 0; Outlenght = 0;//仓库部分数据(长度、高度、入口高度、出口高度、入口水平距离、出口水平距离)
                //runspe = 2.66667; runspea = 0.5; risispe = 0.66667; risispea = 0.5;//堆垛机部分参数（行走V、行走a、提升V、提升a）
                //forkLenght = 1.1; loadForkSpe = 0.33334; loadForkSpea = 0.4; unloadForkSpe = 0.66667; unloadForkSpea = 0.8;//货叉参数(伸叉长度、负载V、负载a、空载V、空载a)
                //lifttime = 3; softwaretime = 3; altertime = 3;//无关动作时间(微提升、软件、硬件)



                allOtherTime = 4 * lifttime + 2 * softwaretime + 2 * altertime;

                ///货叉运行的时间是固定的 单独计算出即可  待计算
                unloadForkTime = getRunTime(forkLenght, unloadForkSpe, unloadForkSpea);
                loadForkTime = getRunTime(forkLenght, loadForkSpe, loadForkSpea);
                allFockTime = 4 * (loadForkTime + unloadForkTime);

                ///定义四个坐标  A 入口 E 出口 IP 入库货位  OP 出库货位
                double[,] axis = new double[,] {
                { Inlenght,Inheight},//入口
                { Outlenght,OutHeiht},//出口
                { lenght/5,height*2/3},//入库坐标
                { lenght*2/3,height/5}//出库坐标
                };
                double[] AtoIP = new double[2] { Math.Abs(axis[2, 0] - axis[0, 0]), Math.Abs(axis[2, 1] - axis[0, 1]) };//入口到入库库位需要行走的横纵距离，进而得到时间
                double[] IPtoOP = new double[2] { Math.Abs(axis[3, 0] - axis[2, 0]), Math.Abs(axis[3, 1] - axis[2, 1]) };//入库库位到出库库位需要行走的横纵距离，进而得到时间
                double[] OPtoE = new double[2] { Math.Abs(axis[3, 0] - axis[1, 0]), Math.Abs(axis[3, 1] - axis[1, 1]) };//出库库位到出口需要行走的横纵距离，进而得到时间
                double[] EtoA = new double[2] { Math.Abs(axis[1, 0] - axis[0, 0]), Math.Abs(axis[1, 1] - axis[0, 1]) };//出口到入口需要行走的横纵距离，进而得到时间
                double[] OPtoA = new double[2]{ Math.Abs(axis[3, 0] - axis[0, 0]), Math.Abs(axis[3, 1] - axis[0, 1]) };//出库库位到入口需要行走的横纵距离，进而得到时间

                ///计算过程
                double[] AtoIPtimes = new double[2] { getRunTime(AtoIP[0], runspe, runspea), getRunTime(AtoIP[1], risispe, risispea) };
                double[] IPtoOPtimes = new double[2] { getRunTime(IPtoOP[0], runspe, runspea), getRunTime(IPtoOP[1], risispe, risispea) };
                double[] OPtoEtimes = new double[2] { getRunTime(OPtoE[0], runspe, runspea), getRunTime(OPtoE[1], risispe, risispea) };
                double[] EtoAtimes = new double[2] { getRunTime(EtoA[0], runspe, runspea), getRunTime(EtoA[1], risispe, risispea) };
                double[] OPtoAtimes = new double[2] { getRunTime(OPtoA[0], runspe, runspea), getRunTime(OPtoA[1], risispe, risispea) };


                ///比较提升与行走时间，取最大值，得到时间
                AtoIPtime = AtoIPtimes.Max();//入口到入库库位时间
                IPtoOPtime = IPtoOPtimes.Max();//入库库位到出库库位时间
                OPtoEtime = OPtoEtimes.Max();//出库库位到出口时间
                EtoAtime = EtoAtimes.Max();//出口到入口时间
                OPtoAtime = OPtoAtimes.Max();//出库库位到入口时间

                switch (caseNum)
                {
                    case "复合循环":
                        allRunTime = AtoIPtime + IPtoOPtime + OPtoEtime + EtoAtime;//复合运行时间
                        break;
                    case "单循环":
                        allRunTime = 2*(AtoIPtime + OPtoAtime);
                        break;
                    case "3": break;

                    default:
                        break;
                }
                allTime = allRunTime + allFockTime + allOtherTime;//总时间


                ///效率
                double ideal = 7200 / allTime;//100%
                double eplan = ideal * 0.8;//80%
                double efplan = ideal * 0.85;//80%
                textBox20.Text = string.Format("100%效率为{0}，80%效率为{1}，85%效率为{2}", (int)ideal, (int)eplan, (int)efplan);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        /// <summary>
        /// 根据路程、最大速度、加速度获得运行时间
        /// </summary>
        /// <param name="trip"></param>总路程
        /// <param name="V"></param>最大速度
        /// <param name="a"></param>加速度
        /// <returns></returns>
        public static double getRunTime(double trip, double V, double a)
        {
            double maxtime;//到到最大速度需要的时间
            double maxspa;//达到最大速度时走过的路程的2倍
            double mintime;//总路程小于maxspa时消耗时间
            double avgtime;//总路程大于maxsap时匀速行使时长
            double alltime;//总时间
            maxtime = V / a;
            maxspa = a * maxtime * maxtime;
            if (trip == 0)
            {
                return 0;
            }
            if (trip > maxspa)
            {
                avgtime = (trip - maxspa) / V;
                alltime = 2 * maxtime + avgtime;
            }
            else
            {
                mintime = Math.Sqrt(trip / a);
                alltime = mintime;
            }
            return alltime;
        }

    }
}
