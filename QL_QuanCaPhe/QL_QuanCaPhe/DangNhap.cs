using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;

namespace QL_QuanCaPhe
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }
        ConnectDB db = new ConnectDB();

        private void ckb_HienMK_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_HienMK.Checked)
                txt_MK.UseSystemPasswordChar = false;
            else
                txt_MK.UseSystemPasswordChar = true;
        }
        private bool KT_TenDangNhap()
        {
            string sql = "exec KT_TenTaiKhoan '"+txt_TenTK.Text+"'";
            if (db.get_ExcuteScalarQuery(sql) == 1)
                return true;
            return false;
        }
        private bool KT_MatKhau()
        {
            string sql = "exec KT_MatKhau '"+txt_TenTK.Text+"', '"+txt_MK.Text+"'";
            if (db.get_ExcuteScalarQuery(sql) == 1)
                return true;
            return false;
        }
        private void btn_DangNhap_Click(object sender, EventArgs e)
        {
            db.OpenDB();
            if(KT_TenDangNhap() == true && KT_MatKhau() == true)
            {
                this.Hide();
                Order frmOrder = new Order();
                frmOrder.TK = txt_TenTK.Text;
                MessageBox.Show("Chào mừng tài khoản " + txt_TenTK.Text + " !");
                frmOrder.ShowDialog();
                db.CloseDB();
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng kiểm tra tài khoản hay mật khẩu lại !");
            }
        }



    }
}
