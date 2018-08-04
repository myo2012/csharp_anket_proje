using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
namespace anketproje
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        OleDbConnection conn = new OleDbConnection("provider=Microsoft.Jet.Oledb.4.0;data source=anket.mdb");
        OleDbDataAdapter da = new OleDbDataAdapter();
        DataSet ds = new DataSet();

        private void button3_Click(object sender, EventArgs e)
        {

            da.SelectCommand = new OleDbCommand("Select * From bilgi", conn);
            ds.Clear();
            da.Fill(ds);
            dg.DataSource = ds.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
       
            OleDbCommand cmd = new OleDbCommand("Select MAX(anket_id) as anket_id From bilgi", conn); //anket son id burdan
            
            conn.Open();

            OleDbDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                txtid.Text = dr["anket_id"].ToString();

            }

            conn.Close();

            da.InsertCommand = new OleDbCommand("INSERT INTO sorular(anket_idfk,soru) VALUES(@anket_idfk,@soru)", conn);
            da.InsertCommand.Parameters.Add("@anket_idfk", OleDbType.Char).Value = txtid.Text;
            da.InsertCommand.Parameters.Add("@soru", OleDbType.Char).Value = txtsoru.Text;

            conn.Open();
            da.InsertCommand.ExecuteNonQuery();
            conn.Close();



            OleDbCommand cmdsoru = new OleDbCommand("Select MAX(soru_id) as soru_id From sorular", conn); //soruların son id burdan

            conn.Open();

            OleDbDataReader drsoru = cmdsoru.ExecuteReader();

            while (drsoru.Read())
            {

                txtsoruid.Text = drsoru["soru_id"].ToString();

            }

            conn.Close();


            da.InsertCommand = new OleDbCommand("INSERT INTO secenekler(soru_idfk,s1,s2,s3,s4) VALUES(@soru_idfk,@s1,@s2,@s3,@s4)", conn);
            da.InsertCommand.Parameters.Add("@soru_idfk", OleDbType.Char).Value = txtsoruid.Text;
            da.InsertCommand.Parameters.Add("@s1", OleDbType.Char).Value = s1.Text;
            da.InsertCommand.Parameters.Add("@s2", OleDbType.Char).Value = s2.Text;
            da.InsertCommand.Parameters.Add("@s3", OleDbType.Char).Value = s3.Text;
            da.InsertCommand.Parameters.Add("@s4", OleDbType.Char).Value = s4.Text;

            conn.Open();
            da.InsertCommand.ExecuteNonQuery();
            conn.Close();




            MessageBox.Show("EKLENDİ");

            txtsoru.Clear();
            s1.Clear();
            s2.Clear();
            s3.Clear();
            s4.Clear();
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dia;

            dia = MessageBox.Show("Veri tabanından veri siliyorsunuz bu işlemi geri alamassınız eminmisiniz ?", "DİKKAT !", MessageBoxButtons.YesNo);

            if (dia == DialogResult.Yes)
            {
                da.DeleteCommand = new OleDbCommand("DELETE FROM bilgi WHERE anket_id=@anket_id", conn);
                da.DeleteCommand.Parameters.Add("@anket_id", OleDbType.Integer).Value = txtsil.Text;
                conn.Open();

                da.DeleteCommand.ExecuteNonQuery();

                conn.Close();
                MessageBox.Show("Silindi");
            }

            else
            {
                MessageBox.Show("Silme işlemi iptal edildi");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            da.SelectCommand = new OleDbCommand("select * From bilgi where anket_adi=@anket_adi", conn);
            da.SelectCommand.Parameters.Add("@anket_adi", OleDbType.Char).Value = txtadara.Text;
            conn.Open();

            ds.Clear();
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds);
            dg.DataSource = ds.Tables[0];

            conn.Close();
            
       

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ds.Clear();
            da.SelectCommand = new OleDbCommand("Select * From kullanıcılar", conn);
            ds.Clear();
            da.Fill(ds);
            dg.DataSource = ds.Tables[0];



        }

        private void button6_Click(object sender, EventArgs e)
        {
            int kayittarihi;
            kayittarihi = DateTime.Today.Year;


            String saat = "";
            saat = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;


            da.InsertCommand = new OleDbCommand("INSERT INTO bilgi(anket_adi,kayıt_tarihi,saat) VALUES(@anket_adi,@kayıt_tarihi,@saat)", conn); //ANKET ADI EKLİYORUZ
            da.InsertCommand.Parameters.Add("@anket_adi", OleDbType.Char).Value = anketad.Text;
            da.InsertCommand.Parameters.Add("@kayıt_tarihi", OleDbType.Char).Value = kayittarihi;
            da.InsertCommand.Parameters.Add("@saat", OleDbType.Char).Value = saat;
            conn.Open();
            da.InsertCommand.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Anket Oluşturuldu Aşağıdan Soruları girebilirsiniz...");

            button1.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            da.SelectCommand = new OleDbCommand("select * From kullanıcılar where kadi=@kadi AND ksoyadi=@ksoyadi", conn);
            da.SelectCommand.Parameters.Add("@kadi", OleDbType.Char).Value = txtkulara.Text;
            da.SelectCommand.Parameters.Add("@ksoyadi", OleDbType.Char).Value = txtkulsoyad.Text;
            conn.Open();

            ds.Clear();
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "kullanıcılar");
            dg.DataSource = ds.Tables["kullanıcılar"];

            conn.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ds.Clear();
            da.SelectCommand = new OleDbCommand("Select katılımanket,cevaplanansoru,sA,sB,sC,sD,s1yzde,s2yzde,s3yzde,s4yzde From kullanıcılar", conn);
            ds.Clear();
            da.Fill(ds);
            dg.DataSource = ds.Tables[0];
        }


        private void button9_Click(object sender, EventArgs e)
        {
            da.SelectCommand = new OleDbCommand("Select kadi,AdSoyad From admin", conn);
            ds.Clear();
            da.Fill(ds);
            dg.DataSource = ds.Tables[0];
        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            da.SelectCommand = new OleDbCommand("Select soru_id,soru From sorular", conn);
            ds.Clear();
            da.Fill(ds);
            dg.DataSource = ds.Tables[0];
        }

        private void button11_Click(object sender, EventArgs e)
        {
        
            da.SelectCommand = new OleDbCommand("Select secenek_id,s1,s2,s3,s4 From secenekler", conn);
            ds.Clear();
            da.Fill(ds);
            dg.DataSource = ds.Tables[0];
        }

        private void button12_Click(object sender, EventArgs e)
        {
            da.SelectCommand = new OleDbCommand("Select sorular.soru as soru , secenekler.s1 as s1,secenekler.s2 as s2,secenekler.s3 as s3,secenekler.s4 as s4  From sorular,secenekler WHERE sorular.soru_id=secenekler.soru_idfk AND sorular.soru_id=@soru_id", conn);
            da.SelectCommand.Parameters.Add("@soru_id", OleDbType.Integer).Value = textsoruid.Text;
            ds.Clear();
            da.Fill(ds);
            dg.DataSource = ds.Tables[0];


            OleDbCommand cmd = new OleDbCommand("Select sorular.soru , secenekler.s1 as s1,secenekler.s2 as s2,secenekler.s3 as s3,secenekler.s4 as s4  From sorular,secenekler WHERE sorular.soru_id=secenekler.soru_idfk AND sorular.soru_id=@soru_id",conn);
           //as soru gibi yeni isim tanımalamada geliyormuş denedim şimdi sorular.soruda as soru yok :)
            cmd.Parameters.Add("@soru_id", OleDbType.Integer).Value = textsoruid.Text;
          
            conn.Open();

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                gncsoru.Text = dr["soru"].ToString();
                gncs1.Text = dr["s1"].ToString();
                gncs2.Text = dr["s2"].ToString();
                gncs3.Text = dr["s3"].ToString();
                gncs4.Text = dr["s4"].ToString();

            }

            conn.Close();





        }

        private void button13_Click(object sender, EventArgs e)
        {
            int x;


            da.UpdateCommand = new OleDbCommand("UPDATE sorular INNER JOIN secenekler ON sorular.soru_id=secenekler.soru_idfk SET sorular.soru=@soru  WHERE sorular.soru_id=@soru_id", conn);
            da.UpdateCommand.Parameters.Add("@soru", OleDbType.Char).Value = gncsoru.Text;
            da.UpdateCommand.Parameters.Add("@soru_id", OleDbType.Integer).Value = textsoruid.Text;
            conn.Open();
            da.UpdateCommand.ExecuteNonQuery();


            da.UpdateCommand = new OleDbCommand("UPDATE secenekler INNER JOIN sorular ON sorular.soru_id=secenekler.soru_idfk  SET secenekler.s1=@s1,secenekler.s2=@s2,secenekler.s3=@s3,secenekler.s4=@s4 WHERE sorular.soru_id=@soru_id ", conn);
            da.UpdateCommand.Parameters.Add("@s1", OleDbType.Char).Value = gncs1.Text;
            da.UpdateCommand.Parameters.Add("@s2", OleDbType.Char).Value = gncs2.Text;
            da.UpdateCommand.Parameters.Add("@s3", OleDbType.Char).Value = gncs3.Text;
            da.UpdateCommand.Parameters.Add("@s4", OleDbType.Char).Value = gncs4.Text;
            da.UpdateCommand.Parameters.Add("@soru_id", OleDbType.Integer).Value = textsoruid.Text;
            x=da.UpdateCommand.ExecuteNonQuery();
            

            conn.Close();
            if (x >= 1)
                MessageBox.Show("Düzenleme  işlemi tamamlanmıştır");
            else
                MessageBox.Show("Bir hata meydana geldi");

        }

      

    
    }
}
