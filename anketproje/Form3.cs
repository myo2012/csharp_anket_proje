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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public static string scn1="";
        public static string scn2="";
        public static string scn3="";
        public static string scn4="";
        public static string cvpsoru="";
        public static int A,B,C,D,toplam;
        public static double SA, SB, SC, SD;
        

        OleDbConnection conn = new OleDbConnection("provider=Microsoft.Jet.Oledb.4.0;data source=anket.mdb");
        OleDbDataAdapter da = new OleDbDataAdapter();
        private void Form3_Load(object sender, EventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT anket_adi FROM bilgi", conn);

            conn.Open();

            OleDbDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                listBox1.Items.Add(dr["anket_adi"].ToString());
            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            string list = "";
            list = listBox1.SelectedItem.ToString();

            OleDbCommand cmd = new OleDbCommand("SELECT sorular.soru_id as soru_id,sorular.soru as soru , secenekler.s1 as s1,secenekler.s2 as s2,secenekler.s3 as s3,secenekler.s4 as s4,secenekler.secenek_id as secenek_id FROM bilgi,sorular,secenekler WHERE bilgi.anket_adi=@anket_adi AND bilgi.anket_id=sorular.anket_idfk AND sorular.soru_id=secenekler.soru_idfk", conn);
            cmd.Parameters.Add("@anket_adi", OleDbType.Char).Value = list;
            conn.Open();

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtsoru.Text = dr["soru"].ToString();
                s1.Text = dr["s1"].ToString();
                s2.Text = dr["s2"].ToString();
                s3.Text = dr["s3"].ToString();
                s4.Text = dr["s4"].ToString();
                txtsoruid.Text = dr["soru_id"].ToString();  //diger sorular için sorunun id'si aldım
                txtsecenekid.Text = dr["secenek_id"].ToString(); //diger secenekler için secenek id sini aldım

            }

            conn.Close();

            txtnextsoru.Text = txtsoruid.Text;
            txtnextsecenek.Text = txtsecenekid.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string list = "";
            list = listBox1.SelectedItem.ToString();

   
                int sorunext = int.Parse(txtnextsoru.Text);
                sorunext += 1;
                txtnextsoru.Text = sorunext.ToString();
                int seceneknext = int.Parse(txtnextsecenek.Text);
                seceneknext += 1;
                txtnextsecenek.Text = seceneknext.ToString();

                cvpsoru += txtsoru.Text +"\n";

                if (s1.Checked)
                    scn1 += s1.Text + "\n";
                if (s2.Checked)
                    scn2 += s2.Text + "\n";
                if (s3.Checked)
                    scn3 += s3.Text + "\n";
                if (s4.Checked)
                    scn4 += s4.Text + "\n";

                if (s1.Checked == true)
                {
                    A++;
                    SA++;
                    toplam++;
                }
                if (s2.Checked == true)
                {
                    B++;
                    SB++;
                    toplam++;
                }
                if (s3.Checked == true)
                {
                    C++;
                    SC++;
                    toplam++;
                }
                if (s4.Checked == true)
                {
                    D++;
                    SD++;
                    toplam++;
                }
              

                OleDbCommand cmd = new OleDbCommand("SELECT sorular.soru as soru , secenekler.s1 as s1,secenekler.s2 as s2,secenekler.s3 as s3,secenekler.s4 as s4 FROM bilgi,sorular,secenekler WHERE bilgi.anket_adi=@anket_adi AND bilgi.anket_id=sorular.anket_idfk AND sorular.soru_id=secenekler.soru_idfk AND sorular.soru_id=@soru_id AND secenekler.secenek_id=@secenek_id", conn);
                cmd.Parameters.Add("@anket_adi", OleDbType.Char).Value = list;
                cmd.Parameters.Add("@soru_id", OleDbType.Integer).Value = sorunext;
                cmd.Parameters.Add("@secenek_id", OleDbType.Integer).Value = seceneknext;
                conn.Open();

                OleDbDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtsoru.Text = dr["soru"].ToString();
                    s1.Text = dr["s1"].ToString();
                    s2.Text = dr["s2"].ToString();
                    s3.Text = dr["s3"].ToString();
                    s4.Text = dr["s4"].ToString();

                }

                conn.Close();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            label11.Text ="A  "+ A.ToString() + " Seçim";
            label12.Text ="B  "+ B.ToString() + " Seçim";
            label13.Text ="C  "+ C.ToString() + " Seçim";
            label14.Text ="D  "+ D.ToString() + " Seçim";

            label15.Text = "% " + Math.Round(((100 * SA) / toplam), 1);
            label16.Text = "% " + Math.Round(((100 * SB) / toplam), 1);
            label17.Text = "% " + Math.Round(((100 * SC) / toplam), 1);
            label18.Text = "% " + Math.Round(((100 * SD) / toplam), 1);

            progressBar1.Value = ((100 * A) / toplam);
            progressBar2.Value = ((100 * B) / toplam);
            progressBar3.Value = ((100 * C) / toplam);
            progressBar4.Value = ((100 * D) / toplam);

            int kayittarihi;
            kayittarihi = DateTime.Today.Year;


            String saat = "";
            saat = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

            String cinsiyet = "";
            if (rbbay.Checked)
                cinsiyet = "Erkek";
            else if (rbbayan.Checked)
                cinsiyet = "Kadın";


            da.InsertCommand = new OleDbCommand("INSERT INTO kullanıcılar(kadi,ksoyadi,cinsiyet,yas,kayittarihi,saat,katılımanket,cevaplanansoru,sA,sB,sC,sD,s1yzde,s2yzde,s3yzde,s4yzde) VALUES(@kadi,@ksoyadi,@cinsiyet,@yas,@kayittarihi,@saat,@katılımanket,@cevaplanansoru,@sA,@sB,@sC,@sD,@s1yzde,@s2yzde,@s3yzde,@s4yzde)", conn);
            da.InsertCommand.Parameters.Add("@kadi", OleDbType.Char).Value = txtad.Text;
            da.InsertCommand.Parameters.Add("@ksoyadi", OleDbType.Char).Value = txtsoyad.Text;
            da.InsertCommand.Parameters.Add("@cinsiyet", OleDbType.Char).Value = cinsiyet;
            da.InsertCommand.Parameters.Add("@yas", OleDbType.Char).Value = txtyas.Text;
            da.InsertCommand.Parameters.Add("@kayittarihi", OleDbType.Char).Value = kayittarihi;
            da.InsertCommand.Parameters.Add("@saat", OleDbType.Char).Value = saat;
            da.InsertCommand.Parameters.Add("@katılımanket", OleDbType.Char).Value = listBox1.SelectedItem.ToString();
            da.InsertCommand.Parameters.Add("@cevaplanansoru", OleDbType.Char).Value = cvpsoru;
            da.InsertCommand.Parameters.Add("@sA", OleDbType.Char).Value = scn1;
            da.InsertCommand.Parameters.Add("@sB", OleDbType.Char).Value = scn2;
            da.InsertCommand.Parameters.Add("@sC", OleDbType.Char).Value = scn3;
            da.InsertCommand.Parameters.Add("@sD", OleDbType.Char).Value = scn4;
            da.InsertCommand.Parameters.Add("@s1yzde", OleDbType.Char).Value = label15.Text;
            da.InsertCommand.Parameters.Add("@s2yzde", OleDbType.Char).Value = label16.Text;
            da.InsertCommand.Parameters.Add("@s3yzde", OleDbType.Char).Value = label17.Text;
            da.InsertCommand.Parameters.Add("@s4yzde", OleDbType.Char).Value = label18.Text;
            conn.Open();

            da.InsertCommand.ExecuteNonQuery();

            conn.Close();

            label19.Visible = true;
            button2.Visible = false;
            button1.Visible = false;
            button3.Visible = false;

                
        }
    }
}