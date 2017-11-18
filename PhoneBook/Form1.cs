using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhoneBook.Model;
using System.Data.Entity;
using System.Threading;

namespace PhoneBook
{
    interface IRepository<T>:IDisposable
        where T : class
    {
        IEnumerable<T> GetList();
        T GetObject(int Id);
        void Create(T Item);
        void Update(T Item);
        void Delete(int Id);
        void Save();
    }
    public partial class Form1 : Form
    {
        
        FioRepository dbfio;
        PhoneRepository dbphone;
        //variables for changing mode(create or update) on groupbox3 and groupbox4
        int editFio;
        int editPhone;
        
        public Form1()
        {
            Database.SetInitializer<CatalogContext>(new CatalogInitializer());
            InitializeComponent();
            dbfio = new FioRepository();
            dbphone = new PhoneRepository();
        }
       
        async private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<Fio> fios = await Task.Run(() => dbfio.GetList());
                foreach (var t in fios)
                    listBox1.Items.Add(t.Name + " " + t.Surname);
            }
            catch (Exception)
            { }
            
            groupBox2.Enabled = false;
            groupBox3.Visible = false;
            groupBox4.Visible = false;
            comboBox1.Items.Add(PhoneType.Мобильный);
            comboBox1.Items.Add(PhoneType.Рабочий);
            comboBox1.Items.Add(PhoneType.Домашний);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            groupBox2.Enabled = false;
            groupBox3.Visible = true;
            editFio = 0;
        }

         async private void button4_Click(object sender, EventArgs e)
        {
             //if editFio is 0 then add object in database, if 1 then update object 
            if (editFio == 0)
            {            
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    Fio fio = new Fio() { Name = textBox1.Text, Surname = textBox2.Text };
                    await Task.Run(() => 
                    {
                        dbfio.Create(fio);
                        dbfio.Save();
                        });
                    
                    listBox1.Items.Add(fio.Name + " " + fio.Surname);
                    
                }
                else
                {
                    MessageBox.Show("Введите имя и фамилию");
                }
            }
            else
            {               
                string selectstr = listBox1.SelectedItem.ToString();
                //get id of updating object
                int index = dbfio.GetList().Where(x => (x.Name + " " + x.Surname) == selectstr).FirstOrDefault().Id;
                //create new object with the same ID
                Fio fio = new Fio() { Id=index ,Name = textBox1.Text, Surname = textBox2.Text };
                await Task.Run(() => 
                {
                    dbfio.Update(fio);
                    dbfio.Save();
                });
                
                listBox1.Items[listBox1.SelectedIndex] = fio.Name + " " + fio.Surname;
                
            }
            groupBox3.Visible = false;
        }

        async private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                groupBox2.Enabled = false;
                groupBox3.Visible = true;
                string selectstr = listBox1.SelectedItem.ToString();
                int index = dbfio.GetList().Where(x => (x.Name + " " + x.Surname) == selectstr).FirstOrDefault().Id;
                Fio fio = await Task.Run(() => dbfio.GetObject(index));
                textBox1.Text = fio.Name;
                textBox2.Text = fio.Surname;
                editFio = 1;
            }
            else
                MessageBox.Show("Выберите человека из списка.");
        }

        async private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectstr = listBox1.SelectedItem.ToString();
                int index = dbfio.GetList().Where(x => (x.Name + " " + x.Surname) == selectstr).FirstOrDefault().Id;
                await Task.Run(() => 
                {
                    dbfio.Delete(index);
                    dbfio.Save();
                });
                
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        async private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbfio = new FioRepository();
            groupBox4.Visible = false;
            if (listBox1.SelectedIndex != -1)
            {
                
                listBox2.Items.Clear();
                groupBox2.Enabled = true;
                string selectstr = listBox1.SelectedItem.ToString() ;
                Fio fio =  dbfio.GetList().FirstOrDefault();
                try
                {
                    await Task.Run(() => fio = (Fio)dbfio.GetList().Where(x => (x.Name + " " + x.Surname) == selectstr).FirstOrDefault());
                    //Thread.Sleep(10);
                    if (fio.Phones != null)
                    {
                        var phones = fio.Phones.Select(x => x.pType + " " + x.Number);

                        foreach (var item in phones)
                        {
                            listBox2.Items.Add(item);
                        }

                    }                                         
                }
                catch (NullReferenceException )
                {
                    listBox2.Items.Clear();
                }
                              
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            groupBox4.Visible = true;
            editPhone = 0;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = false;
        }

        async private void button9_Click(object sender, EventArgs e)
        {
            int id;       
            string selectstr = listBox1.SelectedItem.ToString();
            if (editPhone == 0)
            {
                //add a new phone
                if (comboBox1.SelectedIndex != -1 && textBox3.Text != "")
                {

                    try
                    {
                        id = dbfio.GetList().Where(x => x.Name + " " + x.Surname == selectstr).FirstOrDefault().Id;
                    }
                    catch (NullReferenceException)
                    {
                        id = dbfio.GetList().FirstOrDefault().Id;
                    }
                    Phone phone = new Phone() { pType = (PhoneType)comboBox1.SelectedIndex, Number = textBox3.Text, FioId = id };

                    await Task.Run(() =>
                    {
                        dbphone.Create(phone);
                        dbphone.Save();
                    });
                    listBox2.Items.Add(phone.pType + " " + phone.Number);
                }
                else
                {
                    MessageBox.Show("Введите данные");
                }
            }
            else
            {
                //update selected phone
                string selectstr2 = listBox2.SelectedItem.ToString();
                try
                {
                    id = dbfio.GetList().Where(x => x.Name + " " + x.Surname == selectstr).FirstOrDefault().Id;
                }
                catch (NullReferenceException)
                {
                    id = dbfio.GetList().FirstOrDefault().Id;
                }
                int index = dbphone.GetList().Where(x => x.pType + " " + x.Number == selectstr2).FirstOrDefault().Id;
                Phone phone = new Phone() { Id = index, pType = (PhoneType)comboBox1.SelectedIndex, Number = textBox3.Text, FioId = id };
                await Task.Run(() =>
                {
                    dbphone.Update(phone);
                    dbphone.Save();
                });
                listBox2.Items[listBox2.SelectedIndex] = phone.pType + " " + phone.Number;

            }
            groupBox4.Visible = false;
        }
                                                         

        async private void button7_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                string phone = listBox2.SelectedItem.ToString();
                string[] arr = phone.Split(' ');
                await Task.Run(() =>
                {
                    dbphone.Delete(dbphone.GetList().Where(x => x.Number == arr[1]).Select(x => x.Id).FirstOrDefault());
                    dbphone.Save();
                });
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
            }
        }

        async private void button8_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                groupBox4.Visible = true;
                editPhone = 1;
                int index = listBox1.SelectedIndex + 1;
                string selectstr = listBox2.SelectedItem.ToString();
                Fio fio = await Task.Run(() => dbfio.GetList().Where(x => x.Id == index).FirstOrDefault());
                try
                {
                    textBox3.Text = fio.Phones.Where(x => (x.pType + " " + x.Number) == selectstr).FirstOrDefault().Number;
                }
                catch (NullReferenceException)
                {
                    textBox3.Text = "";
                }
            }
        }

    }
}
