using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;

namespace QL_QuanCaPhe
{
    public partial class DoiMatKhau : Form
    {
        public DoiMatKhau()
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
        string TaiKhoan;
        public string TK
        {
            get { return TaiKhoan; }
            set { TaiKhoan = value; }
        }
        private void btn_Doi_Click(object sender, EventArgs e)
        {
            string MK = txt_MK.Text.Trim();
            if(MK == string.Empty)
            {
                MessageBox.Show("Chưa nhập mật khẩu cần đổi");
                return;
            }
            DialogResult change;
            change = MessageBox.Show("Bạn có chắc muốn đổi ?", "Đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (change == DialogResult.Yes)
            {
                db.OpenDB();
                string sql = "update TaiKhoan set MatKhau = '" + MK + "' where TenTK = '" + TK + "'";
                if (db.get_ExcuteNoneQuery(sql) == 1)
                    MessageBox.Show("Đổi thành công");
                else
                    MessageBox.Show("Đổi không thành công");
                db.CloseDB();
            }
        }

        private void DoiMatKhau_Load(object sender, EventArgs e)
        {
            lbl_TK.Text = TK;
        }
    }
}
