using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Standard_UI.UI
{
    public partial class UserLogin : Form
    {
        string userName= "初级用户";
        string userPassword="";
        public UserLogin()
        {
            InitializeComponent();
        }

        private void dudUser_SelectedItemChanged(object sender, EventArgs e)
        {
            userName = dudUser.SelectedItem.ToString();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            userPassword = txtPassword.Text;
            if (userName == "初级用户")
            {
                if (userPassword == "123456")
                {
                    MessageBox.Show("权限：初级，登录成功！");
                }
                else
                {
                    MessageBox.Show("权限：初级，登录失败，密码错误！");
                }
            }
            else if (userName == "高级用户")
            {
                if (userPassword == "12345678")
                {
                    MessageBox.Show("权限：高级，登录成功！");
                }
                else
                {
                    MessageBox.Show("权限：高级，登录失败，密码错误！");
                }
            }
            this.Hide();
        }
    }
}
