using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace StackerEfficiency
{
    public partial class Impose
    {

        /// <summary>
        /// 限制只可输入数字
        /// </summary>
        /// <param name="e"></param>
        public static void setNumber(KeyPressEventArgs e)
        {
            //IsDigit 判断的是十进制数字，就是 '0 '..'9 '。 
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))//\b是退格键
            {
                e.Handled = true;
                MessageBox.Show("请输入数字", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        /// <summary>
        /// 限制只能输入汉字或字母
        /// </summary>
        /// <param name="c"></param>
        public static void setChinese(KeyPressEventArgs e)
        {
            Regex rg = new Regex(@"^[\u4e00-\u9fa5a-zA-Z\b]+$");
            if (!rg.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
                MessageBox.Show("只能输入汉字或字母!", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        /// <summary>
        /// 只能输入字母或数字
        /// </summary>
        /// <param name="e"></param>
        public static void setEnglishOrNum(KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z')
   || (e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == 8))
            {
                e.Handled = false;

            }
            else
            {
                e.Handled = true;
                MessageBox.Show("密码只能是字母或者数字！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

    }
}
