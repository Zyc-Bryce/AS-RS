using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace StackerEfficiency
{
    public partial class TextBoxNum : TextBox
    {
        #region 构造方法
        public TextBoxNum()
        {
            RegEvent();
        }

        private void RegEvent()
        {
            this.KeyPress += TextBoxNum_KeyPress;
        }


        public TextBoxNum(IContainer container)
        {
            container.Add(this);
            RegEvent();
        }
        #endregion


        private void TextBoxNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
                {
                    e.Handled = true;
                    MessageBox.Show("只能输入数字、小数！最大8位");
                    return;
                }

                //小数点的处理。
                if ((int)e.KeyChar == 46) //小数点                          
                {
                    if (((TextBox)sender).Text.Length <= 0)
                        e.Handled = true;   //小数点不能在第一位
                    else
                    {
                        float f;
                        float oldf;
                        bool b1 = false, b2 = false;
                        b1 = float.TryParse(((TextBox)sender).Text, out oldf);
                        b2 = float.TryParse(((TextBox)sender).Text + e.KeyChar.ToString(), out f);
                        if (b2 == false)
                        {
                            if (b1 == true)
                                e.Handled = true;
                            else
                                e.Handled = false;
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        protected override void OnTextChanged(EventArgs e)
        {
            string inputText = this.Text;
            int len = ASCIIEncoding.Default.GetByteCount(inputText);
            int maxLen = 8;

            //获取输入字符串的二进制数组
            byte[] b = ASCIIEncoding.Default.GetBytes(inputText);
            if (len > maxLen)
            {
                //把截取的字节数组转成字符串
                this.Text = ASCIIEncoding.Default.GetString(b, 0, maxLen);
                this.SelectionStart = maxLen;//把光标定位到输入字符最后
            }
            base.OnTextChanged(e);
        }

    }
}


