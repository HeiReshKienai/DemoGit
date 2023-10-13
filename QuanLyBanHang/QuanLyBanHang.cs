using QuanLyBanHang.QLBHModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang {
    public partial class QuanLyBanHang : Form {
        static QLBHDBcontext context = new QLBHDBcontext();
        List<Invoice> invoiceList = context.Invoices.ToList();
        List<Order> orderList = context.Orders.ToList();
        List<Product> productList = context.Products.ToList();
        //ham tinh tien dựa vào mã hóa đơn
        public double tinhTienDuaVaoMaHoaDon(string maHD) {

            double tong = 0;
            //truy vấn tìm tất cả các đơn hàng có cùng mã với maHD truyền vào
            var kq = context.Orders.Where(s=>s.InvoiceNo.Equals(maHD)).ToList();
            foreach (var item in kq) { 
            
                tong = tong+item.Quantity * (double)item.Price;
            
            }
            return tong;

        
        }
        //đỗ dữ liệu
        public void BindingInvoiceToGridView(List<Invoice> invoices) {
            //xóa dữ liệu trong gridview
            dgvDanhSach.Rows.Clear();
            foreach (var item in invoices) {
                int index = dgvDanhSach.Rows.Add();
                dgvDanhSach.Rows[index].Cells[0].Value = index + 1;//stt
                dgvDanhSach.Rows[index].Cells[1].Value = item.InvoiceNo;
                dgvDanhSach.Rows[index].Cells[2].Value = item.OrderDate;
                dgvDanhSach.Rows[index].Cells[3].Value = item.DeliveryDate;
                dgvDanhSach.Rows[index].Cells[4].Value =tinhTienDuaVaoMaHoaDon(item.InvoiceNo) ; 
            }

        }
        public void tongTien() { 
            double tong = 0;
            foreach (DataGridViewRow item in dgvDanhSach.Rows) {

                tong += (double)item.Cells[4].Value;
            }
            lblTongTien.Text = tong.ToString();
        }
        public QuanLyBanHang() {
            InitializeComponent();
            BindingInvoiceToGridView(invoiceList);
            tongTien();
        }

        private void btnTim_Click(object sender, EventArgs e) {
            try {
                if (ckbXemDonHangThang.Checked) {
                    var kq = context.Invoices.Where(s => s.DeliveryDate.Month == DateTime.Now.Month && s.DeliveryDate.Year == DateTime.Now.Year).ToList();
                    BindingInvoiceToGridView(kq);
                    tongTien();

                } else {

                    DateTime startDate = dtpNgayBatDau.Value;
                    DateTime endDate = dtpNgayKetThuc.Value;
                    if (startDate > endDate) {
                        MessageBox.Show("Ngày bắt đầu không đươc lớn hơn ngày kết thúc");
                        return;
                    } else {
                        var kq = context.Invoices.Where(s => s.DeliveryDate >= startDate && s.DeliveryDate <= endDate).ToList();
                        BindingInvoiceToGridView(kq);
                        tongTien();
                    }
                }
            } catch (Exception ex) { 
                MessageBox.Show(ex.ToString());
                return;
            }
            
            
            
        }
    }
}
