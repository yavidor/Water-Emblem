using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static System.Timers.Timer aTimer;

        // Notes: Add a few text boxes but disable editing. Then label them with damage AS etc. Then at the end of "button" have it store all the values in those text boxes as strings.

        string Name1;
        string Name2;
        decimal Mgt1;
        decimal Wgt1;
        decimal Crit1;
        decimal Hit1;
        decimal HP1;
        decimal Str1;
        decimal Skl1;
        decimal Spd1;
        decimal Lck1;
        decimal Def1;
        decimal Con1;
        decimal Mgt2;
        decimal Wgt2;
        decimal Crit2;
        decimal Hit2;
        decimal HP2;
        decimal Str2;
        decimal Skl2;
        decimal Spd2;
        decimal Lck2;
        decimal Def2;
        decimal Con2;
        // decimal answer;
        decimal TerrainBonus1;
        decimal TerrainBonus2;
        decimal Attack1;
        decimal Attack2;
        decimal Defense1;
        decimal Defense2;
        decimal Damage1;
        decimal Damage2;
        decimal AS1;
        decimal AS2;
        decimal Accuracy1;
        decimal Accuracy2;
        decimal Avoid1;
        decimal Avoid2;
        decimal HitChance1;
        decimal HitChance2;
        decimal CritRate1;
        decimal CritRate2;
        decimal CritEvade1;
        decimal CritEvade2;
        decimal CritHit1;
        decimal CritHit2;
        decimal WTB1;
        decimal WTB2;
        decimal Effective1;
        decimal Effective2;
        decimal AccuracyCheck1;
        decimal AccuracyCheck2;
        decimal CritProc1;
        decimal CritProc2;
        decimal CritAccuracy1;
        decimal CritAccuracy2;
        decimal HitConfirm1;
        decimal HitConfirm2;
        decimal CritConfirm1;
        decimal CritConfirm2;
        decimal DoubleConfirm1;
        decimal DoubleConfirm2;
        string words1;
        string words2;
        string words3;
        string words4;
        string words5;
        string words6;
        string words7;
        string words8;
        // decimal r2;




        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxName1_TextChanged(object sender, EventArgs e)
        {
            Name1 = textBoxName1.Text;
        }

        private void textBoxName2_TextChanged(object sender, EventArgs e)
        {
            Name2 = textBoxName2.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {

            Attack1 = Str1 + Mgt1 * Effective1;
            Attack2 = Str2 + Mgt2 * Effective2;
            Defense1 = Def1 + TerrainBonus1;
            Defense2 = Def2 + TerrainBonus2;
            Damage1 = (Attack1 - Defense2);
            Damage2 = (Attack2 - Defense1);
            Accuracy1 = Hit1 + (Skl1 * 2) + (Lck1 / 2);
            Accuracy2 = Hit2 + (Skl2 * 2) + (Lck2 / 2);
            AS1 = Spd1 - (Wgt1 - Con1);
            AS2 = Spd2 - (Wgt2 - Con2);
            if (AS1 < 0)
            {
                AS1 = 0;
            }

            if (AS2 < 0)
            {
                AS2 = 0;
            }

            CritRate1 = Crit1 + (Skl1 / 2);
            CritRate2 = Crit2 + (Skl2 / 2);
            CritEvade1 = Lck1;
            CritEvade2 = Lck2;
            CritHit1 = CritRate1 - CritEvade2;
            CritHit2 = CritRate2 - CritEvade1;
            Avoid1 = (AS1 * 2) + Lck1 + TerrainBonus1;
            Avoid2 = (AS2 * 2) + Lck2 + TerrainBonus2;
            HitChance2 = Accuracy2 - Avoid1;
            HitChance1 = Accuracy1 - Avoid2;
            //set time delay here
            // Random Random1 = new Random(DateTime.Now.Millisecond);
            // Random Random2 = new Random(DateTime.Now.Millisecond);
            // Random Random3 = new Random(DateTime.Now.Millisecond);
            // Random Random4 = new Random(DateTime.Now.Millisecond);
            Random r = new Random();
            int Random1 = r.Next(0, 100);
            int range = 100;
            aTimer = new System.Timers.Timer(10000);
            Random ra = new Random();
            int Random2 = r.Next(0, 100);
            int range2 = 100;
            aTimer = new System.Timers.Timer(10000);
            Random r3 = new Random();
            int Random3 = r.Next(0, 100);
            int range3 = 100;
            aTimer = new System.Timers.Timer(10000);
            Random r4 = new Random();
            int Random4 = r.Next(0, 100);
            int range4 = 100;
            decimal AccuracyCheck1 = Convert.ToDecimal(Random1);
            decimal AccuracyCheck2 = Convert.ToDecimal(Random2);
            decimal CritProc1 = Convert.ToDecimal(Random3);
            decimal CritProc2 = Convert.ToDecimal(Random4);
            if (AccuracyCheck1 > HitChance1)
            {
                HitConfirm1 = 1;
            }
            else
            {
                HitConfirm1 = 0;
            }

            if (AccuracyCheck2 > HitChance2)
            {
                HitConfirm2 = 1;
            }

            else
            {
                HitConfirm2 = 0;
            }
            if (CritHit1 > CritProc1)
            {
                CritConfirm1 = 1;
            }
            else
            {
                CritConfirm1 = 0;
            }
            if (CritHit2 > CritProc2)
            {
                CritConfirm2 = 1;
            }
            else
            {
                CritConfirm2 = 0;
            }
            if ((AS1 - AS2) >= 4)
            {
                DoubleConfirm1 = 1;
            }
            else
            {
                DoubleConfirm1 = 0;
            }
            if ((AS2 - AS1) >= 4)
            {
                DoubleConfirm2 = 1;
            }
            else
            {
                DoubleConfirm2 = 0;
            }
            if (Accuracy1 > 100)
            {
                HitConfirm1 = 1;
            }
            if (Accuracy2 > 100)
            {
                HitConfirm2 = 1;
            }
            words1 = "Results. Damage to Defender is:";
            words2 = "Damage to Attacker is:";
            words3 = "Player hit is:";
            words4 = "Enemy hit is:";
            words5 = "Player crit is:";
            words6 = "Enemy crit is:";
            words7 = "Player Double?";
            words8 = "Enemy double?";
            //Console.WriteLine("{0}.{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}.{9}.{10}.{11}.{12}.{13}.{14}.{15}.{16}", words1, Damage1, words2, Damage2, words3, HitConfirm1, words4, HitConfirm2, words5, CritConfirm1, words6, CritConfirm2, words7, DoubleConfirm1, words8, DoubleConfirm2);  
            MessageBox.Show(string.Format(words1 + Damage1 + Environment.NewLine + words2 + Damage2 + Environment.NewLine + words3 + HitConfirm1 + Environment.NewLine + words4 + HitConfirm2 + Environment.NewLine + words5 + CritConfirm1 + Environment.NewLine + words6 + CritConfirm2 + Environment.NewLine + words7 + DoubleConfirm1 + Environment.NewLine + words8 + DoubleConfirm2));
            //MessageBox.Show();
            //MessageBox.Show();
            // MessageBox.Show(Damage1.ToString());
            // MessageBox.Show("Results, Damage2:");
            // MessageBox.Show(Damage2.ToString());
            // MessageBox.Show("Results, Hit1:");
            // MessageBox.Show(HitConfirm1.ToString());
            // MessageBox.Show("Results, Hit2:");
            // MessageBox.Show(HitConfirm2.ToString());
            // MessageBox.Show("Results, Crit1:");
            // MessageBox.Show(CritConfirm1.ToString());
            // MessageBox.Show("Results, Crit2:");
            // MessageBox.Show(CritConfirm2.ToString());
            // MessageBox.Show("Results, Double1:");
            // MessageBox.Show(DoubleConfirm1.ToString());
            // MessageBox.Show("Results, Double2:");
            // MessageBox.Show(DoubleConfirm2.ToString());
            // MessageBox.Show( answer.ToString()  );
        }

        private void numericUpDownMgt1_ValueChanged(object sender, EventArgs e)
        {
            Mgt1 = numericUpDownMgt1.Value;
        }

        private void numericUpDownDef2_ValueChanged(object sender, EventArgs e)
        {
            Def2 = numericUpDownDef2.Value;
        }

        private void numericUpDownWgt1_ValueChanged(object sender, EventArgs e)
        {
            Wgt1 = numericUpDownWgt1.Value;
        }

        private void numericUpDownHit1_ValueChanged(object sender, EventArgs e)
        {
            Hit1 = numericUpDownHit1.Value;
        }

        private void numericUpDownCrit1_ValueChanged(object sender, EventArgs e)
        {
            Crit1 = numericUpDownCrit1.Value;
        }

        private void numericUpDownHP1_ValueChanged(object sender, EventArgs e)
        {
            HP1 = numericUpDownHP1.Value;
        }

        private void numericUpDownStr1_ValueChanged(object sender, EventArgs e)
        {
            Str1 = numericUpDownStr1.Value;
        }

        private void numericUpDownSkl1_ValueChanged(object sender, EventArgs e)
        {
            Skl1 = numericUpDownSkl1.Value;
        }

        private void numericUpDownSpd1_ValueChanged(object sender, EventArgs e)
        {
            Spd1 = numericUpDownSpd1.Value;
        }

        private void numericUpDownLck1_ValueChanged(object sender, EventArgs e)
        {
            Lck1 = numericUpDownLck1.Value;
        }

        private void numericUpDownDef1_ValueChanged(object sender, EventArgs e)
        {
            Def1 = numericUpDownDef1.Value;
        }


        private void numericUpDownCon1_ValueChanged(object sender, EventArgs e)
        {
            Con1 = numericUpDownCon1.Value;
        }

        private void numericUpDownMgt2_ValueChanged(object sender, EventArgs e)
        {
            Mgt2 = numericUpDownMgt2.Value;
        }

        private void numericUpDownWgt2_ValueChanged(object sender, EventArgs e)
        {
            Wgt2 = numericUpDownWgt2.Value;
        }

        private void numericUpDownHit2_ValueChanged(object sender, EventArgs e)
        {
            Hit2 = numericUpDownHit2.Value;
        }

        private void numericUpDownCrit2_ValueChanged(object sender, EventArgs e)
        {
            Crit2 = numericUpDownCrit2.Value;
        }

        private void numericUpDownHP2_ValueChanged(object sender, EventArgs e)
        {
            HP2 = numericUpDownHP2.Value;
        }

        private void numericUpDownStr2_ValueChanged(object sender, EventArgs e)
        {
            Str2 = numericUpDownStr2.Value;
        }

        private void numericUpDownSkl2_ValueChanged(object sender, EventArgs e)
        {
            Skl2 = numericUpDownSkl2.Value;
        }

        private void numericUpDownSpd2_ValueChanged(object sender, EventArgs e)
        {
            Spd2 = numericUpDownSpd2.Value;
        }

        private void numericUpDownLck2_ValueChanged(object sender, EventArgs e)
        {
            Lck2 = numericUpDownLck2.Value;
        }


        private void numericUpDownCon2_ValueChanged(object sender, EventArgs e)
        {
            Con2 = numericUpDownCon2.Value;
        }

        private void numericUpDownTerrainBonus1_ValueChanged(object sender, EventArgs e)
        {
            TerrainBonus1 = numericUpDownTerrainBonus1.Value;
        }

        private void numericUpDownTerrainBonus2_ValueChanged(object sender, EventArgs e)
        {
            TerrainBonus2 = numericUpDownTerrainBonus2.Value;
        }

        private void numericUpDownEffective1_ValueChanged(object sender, EventArgs e)
        {
            Effective1 = numericUpDownEffective1.Value;
        }

        private void numericUpDownEffective2_ValueChanged(object sender, EventArgs e)
        {
            Effective2 = numericUpDownEffective2.Value;
        }


    }
}