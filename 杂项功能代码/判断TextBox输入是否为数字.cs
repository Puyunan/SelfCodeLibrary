        /// 判断是否为数字函数
        bool IsNumber(char kc, TextBox textBox)
        {

            if ((kc >= 48 && kc <= 57) || kc == 8)
                return true;
            else if (kc == 46)                       //小数点
            {
                if (textBox.Text.Length <= 0)
                    return false;     //小数点不能在第一位
                else
                {
                    float f;
                    float oldf;
                    bool b1 = false, b2 = false;
                    b1 = float.TryParse(textBox.Text, out oldf);
                    b2 = float.TryParse(textBox.Text + kc.ToString(), out f);
                    if (b2 == false)
                    {
                        if (b1 == true)
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        
        用法：在控件的KeyPress事件中添加判断事件即可
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (IsNumber(e.KeyChar, textBox))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
