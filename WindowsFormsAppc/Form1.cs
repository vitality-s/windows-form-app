using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsAppc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "customerDataSet.Order". При необходимости она может быть перемещена или удалена.
            this.orderTableAdapter1.Fill(this.customerDataSet.Order);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "customerDataSet.Customer". При необходимости она может быть перемещена или удалена.
            this.customerTableAdapter1.Fill(this.customerDataSet.Customer);

        }

        private void dataGridView3_Click(object sender, EventArgs e)
        {
            // створити фільтр, який виведе в dataGridView2 те що потрібно
            int id;
            int index;

            index = dataGridView3.CurrentRow.Index;

            // взяти значення id = Worker.ID_Worker
            id = (int)dataGridView3.Rows[index].Cells[0].Value;
            bindingSource2.Filter = "ID_Customer = " + id.ToString();
        }

        private void dataGridView3_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // створити фільтр, який виведе в dataGridView2 те що потрібно
            int id;
            int index;

            index = dataGridView3.CurrentRow.Index;

            // взяти значення id = Worker.ID_Worker
            id = (int)dataGridView3.Rows[index].Cells[0].Value;
            bindingSource2.Filter = "ID_Customer = " + id.ToString();
        }

        private void додатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormAddCustomer f = new FormAddCustomer(); // створити форму

                if (f.ShowDialog() == DialogResult.OK) // відобразити форму
                {
                    // якщо OK, то додати працівника
                    string CName, CFullname, CAdress, CStan;
                    int CTelefon;

                    CName = f.textBox1.Text;
                    CFullname = f.textBox2.Text;
                    CAdress = f.textBox3.Text;
                    CTelefon = Convert.ToInt32(f.textBox4.Text);
                    CStan = f.comboBox1.Items[f.comboBox1.SelectedIndex].ToString();

                    // працює
                    this.customerTableAdapter1.Insert(CName, CFullname, CAdress, CTelefon, CStan); // вставка
                    this.customerTableAdapter1.Fill(this.customerDataSet.Customer); // відображення
                }
            }
            catch (Exception ex)
            {
                FormError f = new FormError();
                f.ShowDialog();
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            // Customer => Delete
            try
            {
                FormDeleteCustomer f = new FormDeleteCustomer(); // створити форму
                int id_customer;
                string CName, CFullname, CAdress, CStan;
                int CTelefon;

                // взяти номер поточного (виділеного) рядка в dataGridView1
                int index = dataGridView3.CurrentRow.Index;

                // заповнити внутрішні змінні з поточного рядка dataGridView1
                id_customer = Convert.ToInt32(dataGridView3[0, index].Value);
                CName = Convert.ToString(dataGridView3[1, index].Value);
                CFullname = Convert.ToString(dataGridView3[2, index].Value);
                CAdress = Convert.ToString(dataGridView3[3, index].Value);
                CTelefon = Convert.ToInt32(dataGridView3[4, index].Value);
                CStan = Convert.ToString(dataGridView3[5, index].Value);

                // сформувати інформаційний рядок
                f.label2.Text = CName + " " + CFullname;

                if (f.ShowDialog() == DialogResult.OK)
                {
                    customerTableAdapter1.Delete(id_customer, CName, CFullname, CAdress, CTelefon, CStan); // метод Delete
                    this.customerTableAdapter1.Fill(this.customerDataSet.Customer);
                }
            }
            catch (Exception ex)
            {
                FormError f = new FormError();
                f.ShowDialog();
            }
        } 

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                // Worker => Edit...
                FormEditCustomer f = new FormEditCustomer(); // створити форму
                int index;
                string CName, CFullname, CAdress, CStan;
                int id_customer;
                int CTelefon;

                if (dataGridView3.RowCount <= 1) return;

                // отримати позицію виділеного рядка в dataGridView1
                index = dataGridView3.CurrentRow.Index;

                if (index == dataGridView3.RowCount - 1) return; //

                // отримати дані рядка
                id_customer = (int)dataGridView3.Rows[index].Cells[0].Value;
                CName = (string)dataGridView3.Rows[index].Cells[1].Value;
                CFullname = (string)dataGridView3.Rows[index].Cells[2].Value;
                CAdress = (string)dataGridView3.Rows[index].Cells[3].Value;
                CTelefon = Convert.ToInt32(dataGridView3.Rows[index].Cells[4].Value);
                CStan = (string)dataGridView3.Rows[index].Cells[5].Value;

                // заповнити поля форми f
                f.textBox1.Text = CName;
                f.textBox2.Text = CFullname;
                f.textBox3.Text = CAdress;
                f.textBox4.Text = Convert.ToString(CTelefon);
                f.comboBox1.Text = CStan;

                if (CStan == "виконано") f.comboBox1.SelectedIndex = 0;
                if (CStan == "у черзі") f.comboBox1.SelectedIndex = 1;
                if (CStan == "не виконано") f.comboBox1.SelectedIndex = 2;

                if (f.ShowDialog() == DialogResult.OK) // викликати форму FormEditWorker
                {
                    string nCName, nCFullname, nCAdress, nCStan;
                    int nCTelefon;
                    // отримати нові (змінені) значення з форми
                    nCName = f.textBox1.Text;
                    nCFullname = f.textBox2.Text;
                    nCAdress = f.textBox3.Text;
                    nCTelefon = Convert.ToInt32(f.textBox4.Text);
                    nCStan = Convert.ToString(f.comboBox1.Items[f.comboBox1.SelectedIndex].ToString());

                    // змінити в адаптері
                    /*this.customerTableAdapter1.Update(id_customer, nCName, nCFullname, nCAdress, nCTelefon, nCStan, id_customer, CName, CFullname, CAdress, CTelefon, CStan);
                    this.customerTableAdapter1.Fill(this.customerDataSet.Customer);*/

                }
            }
            catch (Exception ex)
            {
                FormError f = new FormError();
                f.ShowDialog();
            }
            
        }

        private void додатиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                // Inventory => Add...
                FormAddOrder f = new FormAddOrder(); // вікно

                if (f.ShowDialog() == DialogResult.OK)
                {
                    int Id_Customer;
                    int index;
                    string Otype, OYear, OMPrice;
                    int OPrice;
                    DateTime OData1, OData2;

                    // взяти значення ID_Worker з таблиці Worker
                    index = dataGridView3.CurrentRow.Index; // позиція в dataGridView1
                    Id_Customer = (int)dataGridView3.Rows[index].Cells[0].Value;

                    // взяти значення інших полів з форми FormAddAccount
                    Otype = f.comboBox1.Items[f.comboBox1.SelectedIndex].ToString();
                    OYear = f.textBox2.Text;
                    OData1 = f.dateTimePicker1.Value;
                    OData2 = f.dateTimePicker2.Value;
                    OMPrice = f.comboBox1.Items[f.comboBox2.SelectedIndex].ToString();
                    OPrice = Convert.ToInt32(f.textBox1.Text);

                    this.orderTableAdapter1.Insert(Id_Customer, Otype, OYear, OData1, OData2, OPrice, OMPrice);
                    this.orderTableAdapter1.Fill(this.customerDataSet.Order);
                }
            }
            catch(Exception ex)
            {
                FormError f = new FormError();
                f.ShowDialog();
            }
            
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            try
            {
                // Inventory => Delete
                FormDeleteOrder f = new FormDeleteOrder();
                int id_order, id_customer;
                string Otype, OYear, OMPrice;
                int OPrice;
                DateTime OData1, OData2;
                int index;

                // взяти індекс виділеного (поточного) рядка в dataGridView2
                index = dataGridView4.CurrentRow.Index;

                // взяти значення полів рядка з номером index
                id_order = Convert.ToInt32(dataGridView4[0, index].Value);
                id_customer = Convert.ToInt32(dataGridView4[1, index].Value);
                Otype = Convert.ToString(dataGridView4[2, index].Value);
                OYear = Convert.ToString(dataGridView4[3, index].Value);
                OData1 = Convert.ToDateTime(dataGridView4[4, index].Value);
                OData2 = Convert.ToDateTime(dataGridView4[5, index].Value);
                OPrice = Convert.ToInt32(dataGridView4[6, index].Value);
                OMPrice = Convert.ToString(dataGridView4[7, index].Value);

                // сформувати інформаційний рядок у вікні FormDelAccount
                f.label2.Text = Otype + " / " + OYear + " / " + OData1 + " / " +
                OData2 + " / " + OPrice + " / " + OMPrice;

                if (f.ShowDialog() == DialogResult.OK) // вивести вікно
                {
                    this.orderTableAdapter1.Delete(id_order, id_customer,
                    Otype, OYear, OData1, OData2, OPrice, OMPrice); // видалити рядок
                    this.orderTableAdapter1.Fill(this.customerDataSet.Order); // зафіксувати зміни
                }
            }
            catch(Exception ex)
            {
                FormError f = new FormError();
                f.ShowDialog();
            }
            
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            try
            {
                // Inventory => Edit...
                FormEditOrder f = new FormEditOrder();
                int index;
                int id_order;
                int id_customer;
                string Otype, OYear, OMPrice;
                int OPrice;
                DateTime OData1, OData2;

                if (dataGridView4.RowCount <= 1) return;

                // взяти номер поточного рядка в dataGridView2
                index = dataGridView4.CurrentRow.Index;

                if (index == dataGridView4.RowCount - 1) return; //

                // отримати дані рядка
                id_order = (int)dataGridView4.Rows[index].Cells[0].Value;
                id_customer = (int)dataGridView4.Rows[index].Cells[1].Value;
                Otype = (string)dataGridView4.Rows[index].Cells[2].Value;
                OYear = (string)dataGridView4.Rows[index].Cells[3].Value;
                OData1 = (DateTime)dataGridView4.Rows[index].Cells[4].Value;
                OData2 = (DateTime)dataGridView4.Rows[index].Cells[5].Value;
                OPrice = (int)dataGridView4.Rows[index].Cells[6].Value;
                OMPrice = (string)dataGridView4.Rows[index].Cells[7].Value;

                // заповнити даними рядка поля у FormEditAccount
                f.comboBox1.Text = Otype;
                f.textBox2.Text = OYear;
                f.dateTimePicker1.Value = OData1;
                f.dateTimePicker2.Value = OData2;
                f.textBox1.Text = Convert.ToString(OPrice);
                f.comboBox2.Text = Convert.ToString(OMPrice);

                // заповнити поля форми FormEditAccount
                if (f.ShowDialog() == DialogResult.OK) // викликати форму
                {
                    // нові значення рядка
                    string nOtype, nOYear, nOMPrice;
                    int nOPrice;
                    DateTime nOData1, nOData2;

                    // взяти нові значення
                    nOtype = f.comboBox1.Text;
                    nOYear = f.textBox2.Text;
                    nOData1 = f.dateTimePicker1.Value;
                    nOData2 = f.dateTimePicker2.Value;
                    nOPrice = Convert.ToInt32(f.textBox1.Text);
                    nOMPrice = Convert.ToString(f.comboBox2.Text);

                    // оновити дані в рядку
                    this.orderTableAdapter1.Update(id_customer, nOtype, nOYear, nOData1, nOData2, nOPrice, nOMPrice,
                          id_order, id_customer, Otype, OYear, OData1, OData2, OPrice, OMPrice);
                    this.orderTableAdapter1.Fill(this.customerDataSet.Order);
                }
            }
            catch(Exception ex)
            {
                FormError f = new FormError();
                f.ShowDialog();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bindingSource1.Filter = "Імя LIKE '" + textBox2.Text + "%'";
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

