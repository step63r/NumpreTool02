using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumpreTool02
{
    public struct Numpre
    {
        public int Number;
        public bool IsError, IsValid, IsOverlap;
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private TextBox[] txtbox;
        private CheckBox[] chkbox;
        private Button[] execbutton;
        private Label[] label;
        Numpre[,] Num = new Numpre[9, 9];

        private void Form1_Load(object sender, EventArgs e)
        {
            txtbox = new TextBox[81];
            chkbox = new CheckBox[9];
            execbutton = new Button[5];
            label = new Label[18];
            int i, j, k, m;
            j = 0;
            k = 0;
            m = 0;

            this.SuspendLayout();

            // Create TextBox
            for (i = 0; i <= txtbox.Length - 1; i++)
            {
                txtbox[i] = new TextBox();
                txtbox[i].Name = "Textbox" + i.ToString();
                txtbox[i].Text = "";
                txtbox[i].Font = new Font("メイリオ", 14);
                txtbox[i].BackColor = Color.White;
                txtbox[i].BorderStyle = BorderStyle.FixedSingle;
                txtbox[i].TextAlign = HorizontalAlignment.Center;
                txtbox[i].Size = new Size(36, 36);
                if (i == 0)
                {
                    k = -30;
                }
                if (i % 3 == 0)
                {
                    if (i % 9 == 0)
                    {
                        if (i % 27 == 0)
                        {
                            k += 50;
                            j = 10;
                        }
                        else
                        {
                            k += 40;
                            j = 10;
                        }
                    }
                    else
                    {
                        j = j + 10;
                    }
                }
                txtbox[i].Location = new Point(j, k);
                j += 40;
            }
                
            // Create Button
            execbutton[0] = new Button();
            execbutton[0].Name = "buttonExecute";
            execbutton[0].Text = "実 行";
            execbutton[0].Font = new Font("MS UI Gothic", 12);
            execbutton[0].Size = new Size(90, 35);
            execbutton[0].Location = new Point(550, 30);
            execbutton[0].Click += new EventHandler(buttonExecute_Click);

            execbutton[1] = new Button();
            execbutton[1].Name = "buttonNext";
            execbutton[1].Text = "次 へ";
            execbutton[1].Font = new Font("MS UI Gothic", 12);
            execbutton[1].Size = new Size(90, 35);
            execbutton[1].Location = new Point(550, 70);
            execbutton[1].Click += new EventHandler(buttonNext_Click);

            execbutton[2] = new Button();
            execbutton[2].Name = "buttonConfirm";
            execbutton[2].Text = "確 認";
            execbutton[2].Font = new Font("MS UI Gothic", 12);
            execbutton[2].Size = new Size(90, 35);
            execbutton[2].Location = new Point(550, 110);
            execbutton[2].Click += new EventHandler(buttonConfirm_Click);

            execbutton[3] = new Button();
            execbutton[3].Name = "buttonClear";
            execbutton[3].Text = "クリア";
            execbutton[3].Font = new Font("MS UI Gothic", 12);
            execbutton[3].Size = new Size(90, 35);
            execbutton[3].Location = new Point(550, 150);
            execbutton[3].Click += new EventHandler(buttonClear_Click);

            execbutton[4] = new Button();
            execbutton[4].Name = "buttonAnswer";
            execbutton[4].Text = "自動解答";
            execbutton[4].Font = new Font("MS UI Gothic", 12);
            execbutton[4].Size = new Size(90, 35);
            execbutton[4].Location = new Point(550, 190);
            execbutton[4].Click += new EventHandler(buttonAnswer_Click);

            // Create Label
            for (i = 0; i <= 8; i++)
            {
                label[i] = new Label();
                label[i].Name = "LabelVT" + i.ToString();
                label[i].Text = "9";
                label[i].Font = new Font("MS UI Gothic", 12);
                label[i].Size = new Size(20, 20);
                if (i == 0)
                {
                    m = 0;
                }
                else if (i % 3 == 0)
                {
                    m += 50;
                }
                else
                {
                    m += 40;
                }
                label[i].Location = new Point(m + 20, 400);
            }

            for (i = 0; i <= 8; i++)
            {
                label[i + 9] = new Label();
                label[i + 9].Name = "LabelHT" + i.ToString();
                label[i + 9].Text = "9";
                label[i + 9].Font = new Font("MS UI Gothic", 12);
                label[i + 9].Size = new Size(20, 20);
                if (i == 0)
                {
                    m = 35;
                }
                else if (i % 3 == 0)
                {
                    m += 50;
                }
                else
                {
                    m += 40;
                }
                label[i + 9].Location = new Point(390, m);
            }

            // Create CheckBox
            for (i = 0; i <= chkbox.Length - 1; i++)
            {
                chkbox[i] = new CheckBox();
                chkbox[i].Name = "CheckBox" + i.ToString();
                chkbox[i].Text = (i + 1).ToString() + " ---------";
                chkbox[i].Font = new Font("MS UI Gothic", 12);
                chkbox[i].Size = new Size(120, 20);
                chkbox[i].Location = new Point(420, (i * 25) + 30);
            }

            // Add Control
            Controls.AddRange(txtbox);
            Controls.AddRange(execbutton);
            Controls.AddRange(label);
            Controls.AddRange(chkbox);

            this.ResumeLayout();

            labelNotify.Text = "準備完了";
        }

        // チェックした数字についてハイライト
        public void buttonExecute_Click(object sender, EventArgs e)
        {
            int i, errorNum;
            int[] VError = new int[9];
            int[] HError = new int[9];
            int[] Exist = new int[9];
            string[] Ins = new string[9];
            
            // まず不正な値が入力されていた場合、全ての処理を中止する
            if (CheckAllValidity(txtbox) == false)
            {
                labelNotify.Text = "エラー: 不正なテキストが入力されていたため処理を中止しました";
                return;
            }

            labelNotify.Text = "お待ちください...";
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            errorNum = 0;
            NumpreInitialization(ref Num, ref VError, ref HError, ref Exist, ref Ins);

            for (i = 0; i <= 80; i++)
            {
                txtbox[i].BackColor = Color.White;
            }

            CheckNull(ref Num);
            CheckValid(ref Num);
            ChangeColor(ref Num);
            CountNull(ref Num, VError, HError, ref errorNum);
            CountExist(ref Num, Exist, Ins);

            if (errorNum == 0)
            {
                labelNotify.Text = "完了";
            }
            else
            {
                labelNotify.Text = errorNum + " 個の空白が存在しています";
            }
            Cursor.Current = Cursors.Default;

        }

        // 重複確認
        public void buttonConfirm_Click(object sender, EventArgs e)
        {
            int overlap, errorNum;
            int[] VError = new int[9];
            int[] HError = new int[9];
            int[] Exist = new int[9];
            string[] Ins = new string[9];

            // まず不正な値が入力されていた場合、全ての処理を中止する
            if (CheckAllValidity(txtbox) == false)
            {
                labelNotify.Text = "エラー: 不正なテキストが入力されていたため処理を中止しました";
                return;
            }

            labelNotify.Text = "確認しています...";
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            overlap = 0;
            errorNum = 0;
            NumpreInitialization(ref Num, ref VError, ref HError, ref Exist, ref Ins);
            CheckNull(ref Num);
            CheckOverlap(ref Num, ref overlap);
            OverlapChangeColor(ref Num);
            CountNull(ref Num, VError, HError, ref errorNum);
            CountExist(ref Num, Exist, Ins);

            if (overlap == 0)
            {
                labelNotify.Text = "重複はありません";
            }
            else
            {
                labelNotify.Text = overlap + " 個の重複が存在しています。";
            }
            Cursor.Current = Cursors.Default;
        }

        public void buttonClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 80; i++)
            {
                txtbox[i].Text = "";
                txtbox[i].BackColor = Color.White;
            }
            for (int i = 0; i <= 8; i++)
            {
                label[i].Text = "9";
                label[i + 9].Text = "9";
                chkbox[i].Text = (i + 1).ToString() + " ---------";
                chkbox[i].Checked = false;

                for (int j = 0; j <= 8; j++)
                {
                    Num[i, j].Number = 0;
                }
            }
            ConsoleList.Items.Clear();
            labelNotify.Text = "全てのテキストをクリアしました";
        }

        public void buttonNext_Click(object sender, EventArgs e)
        {
            int chkstate, checkedNum;
            chkstate = 0;
            checkedNum = 100;

            for (int i = 0; i <= chkbox.Length - 1; i++)
            {
                if (chkbox[i].Checked == true)
                {
                    chkstate += 1;
                    checkedNum = i;
                }
            }
            if (chkstate == 1 && checkedNum != 100)
            {
                if (chkbox[8].Checked == true)
                {
                    chkbox[8].Checked = false;
                    chkbox[0].Checked = true;
                    execbutton[0].PerformClick();
                }
                else
                {
                    chkbox[checkedNum].Checked = false;
                    chkbox[checkedNum + 1].Checked = true;
                    execbutton[0].PerformClick();
                }
            }
        }

        // 自動解答のアルゴリズム
        // 【注意】
        //   複数の数値候補があり、他のマスの候補から当てはまらない数値を消去していく
        //   解答方法には対応していません。
        public void buttonAnswer_Click(object sender, EventArgs e)
        {
            int inputNum, checkNum, TimeOver;
            bool IsComplete = false;
            inputNum = 0;
            checkNum = 1;
            TimeOver = 0;

            labelNotify.Text = "自動解答が進行中。何もせずお待ちください...";
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    try
                    {
                        Num[i, j].Number = int.Parse(txtbox[inputNum].Text);
                        inputNum += 1;
                    }
                    catch (Exception ex)
                    {
                    }
                    
                }
                chkbox[i].Checked = false;
            }

            chkbox[checkNum - 1].Checked = true;

            while (TimeOver <= 27)
            {
                AnswerHrizonCheck(ref Num, ref checkNum, ref TimeOver);
                AnswerVerticalCheck(ref Num, ref checkNum, ref TimeOver);
                AnswerBoxCheck(ref Num, ref checkNum, ref TimeOver);
                
                // 全てのマスに答えが入っていれば終了
                if (AnswerBreakCheck(Num) == 81)
                {
                    IsComplete = true;
                    break;
                }

                // 次に調べる数値を変更
                if (checkNum == 9)
                {
                    chkbox[8].Checked = false;
                    chkbox[0].Checked = true;
                    checkNum = 1;
                }
                else
                {
                    chkbox[checkNum - 1].Checked = false;
                    chkbox[checkNum].Checked = true;
                    checkNum += 1;
                }
            }

            if (IsComplete == true)
            {
                labelNotify.Text = "解答完了";
            }
            else
            {
                labelNotify.Text = "規定のアルゴリズムでは解答できません";
            }
            Cursor.Current = Cursors.Default;
        }

        // 構造体の初期化
        public void NumpreInitialization(ref Numpre[,] numpre, ref int[] VErr, ref int[] HErr, ref int[] exist, ref string[] ins)
        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    Num[i, j].IsError = false;
                    Num[i, j].IsValid = false;
                }
                VErr[i] = 0;
                HErr[i] = 0;
                exist[i] = 0;
                ins[i] = "";
            }
        }

        public bool CheckAllValidity(TextBox[] _txt)
        {
            bool _chk = true;
            for (int i = 0; i <= 80; i++)
            {
                try
                {
                    if (!(_txt[i].Text == "" || (int.Parse(_txt[i].Text) >= 1 && int.Parse(_txt[i].Text) <= 9)))
                    {
                        _chk = false;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _chk = false;
                    break;
                }
                
            }
            return _chk;
        }

        public void CheckNull(ref Numpre[,] numpre)
        {
            int i, j, k;
            i = 0;
            j = 0;
            for (k = 0; k <= 80; k++)
            {
                try
                {
                    numpre[i, j].Number = int.Parse(txtbox[k].Text);
                }
                catch (Exception ex)
                {
                    numpre[i, j].IsError = true;
                }

                if (j == 8)
                {
                    j = 0;
                    i += 1;
                }
                else
                {
                    j += 1;
                }
            }
        }

        public void CheckValid(ref Numpre[,] numpre)
        {
            int x, y;
            for (int k = 0; k <= 8; k++)
            {
                if (chkbox[k].Checked == true)
                {
                    for (int i = 0; i <= 8; i++)
                    {
                        for (int j = 0; j <= 8; j++)
                        {
                            if (numpre[i, j].Number == k + 1)
                            {
                                x = i;
                                y = j;
                                numpre[i, j].IsValid = true;

                                CheckValidOfVerticalAndHorizon(ref numpre, x, y);
                                if (x % 3 == 0 && y % 3 == 0)
                                {
                                    CheckValidOfBox(ref numpre, x, y, 1, 1, 2, 1, 1, 2, 2, 2);
                                }
                                else if (x % 3 == 1 && y % 3 == 0)
                                {
                                    CheckValidOfBox(ref numpre, x, y, -1, 1, -1, 2, 1, 1, 1, 2);
                                }
                                else if (x % 3 == 2 && y % 3 == 0)
                                {
                                    CheckValidOfBox(ref numpre, x, y, -1, 1, -1, 2, -2, 1, -2, 2);
                                }
                                else if (x % 3 == 0 && y % 3 == 1)
                                {
                                    CheckValidOfBox(ref numpre, x, y, 1, -1, 2, -1, 1, 1, 2, 1);
                                }
                                else if (x % 3 == 1 && y % 3 == 1)
                                {
                                    CheckValidOfBox(ref numpre, x, y, -1, -1, -1, 1, 1, 1, 1, -1);
                                }
                                else if (x % 3 == 2 && y % 3 == 1)
                                {
                                    CheckValidOfBox(ref numpre, x, y, -1, 1, -1, -1, -2, 1, -2, -1);
                                }
                                else if (x % 3 == 0 && y % 3 == 2)
                                {
                                    CheckValidOfBox(ref numpre, x, y, 1, -1, +2, -1, 1, -2, +2, -2);
                                }
                                else if (x % 3 == 1 && y % 3 == 2)
                                {
                                    CheckValidOfBox(ref numpre, x, y, 1, -1, 1, -2, -1, -1, -1, -2);
                                }
                                else if (x % 3 == 2 && y % 3 == 2)
                                {
                                    CheckValidOfBox(ref numpre, x, y, -1, -1, -1, -2, -2, -1, -2, -2);
                                }
                            }
                        }
                    }
                }
            }
        }

        // CheckValid()で使用します
        public void CheckValidOfVerticalAndHorizon(ref Numpre[,] numpre, int x, int y)
        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    if (i == x || j == y)
                    {
                        numpre[i, j].IsValid = true;
                    }
                }
            }
        }

        // CheckValid()で使用します
        public void CheckValidOfBox(ref Numpre[,] numpre, int x, int y, int a, int b, int c, int d, int e, int f, int g, int h)
        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    if ((i == x + a && j == y + b) || (i == x + c && j == y + d) || (i == x + e && j == y + f) || (i == x + g && j == y + h))
                    {
                        numpre[i, j].IsValid = true;
                    }
                }
            }
        }

        public void ChangeColor(ref Numpre[,] numpre)
        {
            int x, y;
            x = 0;
            y = 0;
            for (int i = 0; i <= 80; i++)
            {
                if (numpre[x, y].IsValid == true)
                {
                    txtbox[i].BackColor = Color.Yellow;
                }
                if (y == 8)
                {
                    y = 0;
                    x += 1;
                }
                else
                {
                    y += 1;
                }
            }
        }

        public void CountNull(ref Numpre[,] numpre, int[] VErr, int[] HErr, ref int ErrNum)
        {
            for (int j = 0; j <= 8; j++)
            {
                for (int i = 0; i <= 8; i++)
                {
                    if (numpre[i, j].IsError == true)
                    {
                        VErr[j] += 1;
                    }
                    if (numpre[j, i].IsError == true)
                    {
                        HErr[j] += 1;
                    }
                }
                label[j].Text = VErr[j].ToString();
                label[j + 9].Text = HErr[j].ToString();

                ErrNum += VErr[j];
            }
        }

        public void CountExist(ref Numpre[,] numpre, int[] exist, string[] ins)
        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    for (int num = 1; num <= 9; num++)
                    {
                        if (numpre[i, j].Number == num)
                        {
                            exist[num - 1] += 1;
                        }
                    }
                }
            }

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8 - exist[i]; j++)
                {
                    ins[i] = ins[i] + "-";
                }
                chkbox[i].Text = (i + 1).ToString() + " " + ins[i];
            }
        }

        public void CheckOverlap(ref Numpre[,] numpre, ref int ovl)
        {
            int x, y;
            CheckOverlapOfVerticalAndHorizon(ref numpre, ref ovl);

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    x = i;
                    y = j;
                    if (x % 3 == 0 && y % 3 == 0)
                    {
                        CheckOverlapOfBox(ref numpre, x, y, 1, 1, 2, 1, 1, 2, 2, 2, ref ovl);
                    }
                    else if (x % 3 == 1 && y % 3 == 0)
                    {
                        CheckOverlapOfBox(ref numpre, x, y, -1, 1, -1, 2, 1, 1, 1, 2, ref ovl);
                    }
                    else if (x % 3 == 2 && y % 3 == 0)
                    {
                        CheckOverlapOfBox(ref numpre, x, y, -1, 1, -1, 2, -2, 1, -2, 2, ref ovl);
                    }
                    else if (x % 3 == 0 && y % 3 == 1)
                    {
                        CheckOverlapOfBox(ref numpre, x, y, 1, -1, 2, -1, 1, 1, 2, 1, ref ovl);
                    }
                    else if (x % 3 == 1 && y % 3 == 1)
                    {
                        CheckOverlapOfBox(ref numpre, x, y, -1, -1, -1, 1, 1, 1, 1, -1, ref ovl);
                    }
                    else if (x % 3 == 2 && y % 3 == 1)
                    {
                        CheckOverlapOfBox(ref numpre, x, y, -1, 1, -1, -1, -2, 1, -2, -1, ref ovl);
                    }
                    else if (x % 3 == 0 && y % 3 == 2)
                    {
                        CheckOverlapOfBox(ref numpre, x, y, 1, -1, +2, -1, 1, -2, +2, -2, ref ovl);
                    }
                    else if (x % 3 == 1 && y % 3 == 2)
                    {
                        CheckOverlapOfBox(ref numpre, x, y, 1, -1, 1, -2, -1, -1, -1, -2, ref ovl);
                    }
                    else if (x % 3 == 2 && y % 3 == 2)
                    {
                        CheckOverlapOfBox(ref numpre, x, y, -1, -1, -1, -2, -2, -1, -2, -2, ref ovl);
                    }
                }
            }
        }

        // CheckOverlapで使用します
        public void CheckOverlapOfVerticalAndHorizon(ref Numpre[,] numpre, ref int ovl)
        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    for (int k = j + 1; k <= 8; k++)
                    {
                        if (numpre[i, j].IsError == false)
                        {
                            if (numpre[i, j].Number == numpre[i, k].Number)
                            {
                                numpre[i, j].IsOverlap = true;
                                numpre[i, k].IsOverlap = true;
                                ovl += 1;
                            }
                        }
                        if (numpre[j, i].IsError == false)
                        {
                            if (numpre[j, i].Number == numpre[k, i].Number)
                            {
                                numpre[j, i].IsOverlap = true;
                                numpre[k, i].IsOverlap = true;
                                ovl += 1;
                            }
                        }
                    }
                }
            }
        }

        // CheckOverlapで使用します
        public void CheckOverlapOfBox(ref Numpre[,] numpre, int x, int y, int a, int b, int c, int d, int e, int f, int g, int h, ref int ovl)
        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    if (numpre[i, j].IsError == false)
                    {
                        if (((i == x + a && j == y + b) && numpre[x, y].Number == numpre[i, j].Number) || ((i == x + c && j == y + d) && numpre[x, y].Number == numpre[i, j].Number) || ((i == x + e && j == y + f) && numpre[x, y].Number == numpre[i, j].Number) || ((i == x + g && j == y + h) && numpre[x, y].Number == numpre[i, j].Number))
                        {
                            numpre[i, j].IsOverlap = true;
                            ovl += 1;
                        }
                    }
                }
            }
        }

        public void OverlapChangeColor(ref Numpre[,] numpre)
        {
            int x, y;
            x = 0;
            y = 0;
            for (int i = 0; i <= 80; i++)
            {
                if (numpre[x, y].IsOverlap == true)
                {
                    txtbox[i].BackColor = Color.LightCoral;
                }
                if (y == 8)
                {
                    y = 0;
                    x += 1;
                }
                else
                {
                    y += 1;
                }
            }
        }

        public void AnswerHrizonCheck(ref Numpre[,] numpre, ref int checknum, ref int timeover)
        {
            int countblank, i_out, j_out;
            bool IsSameNum = false;
            countblank = 0;
            i_out = 0;
            j_out = 0;

            execbutton[0].PerformClick();

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    // checknumの効いていない空白があったら
                    if (numpre[i, j].IsError == true && numpre[i, j].IsValid == false)
                    {
                        // とりあえず覚えておく
                        countblank += 1;
                        i_out = i;
                        j_out = j;
                    }
                    // その行に既にchecknumがあったら
                    if (numpre[i, j].Number == checknum)
                    {
                        // その行はもう終了
                        IsSameNum = true;
                        continue;
                    }
                }
                // その行で該当する空白がぴったり1個だった場合のみ
                if (countblank == 1 && IsSameNum == false)
                {
                    // そのマスにchecknumを入れる
                    numpre[i_out, j_out].Number = checknum;
                    int txtbxi = FindTextBoxIndexFromNumpreIndex(i_out, j_out);
                    if (txtbxi == -1)
                    {
                        ConsoleList.Items.Add("エラー: Numpre[,]配列のインデックスが不正です");
                        ConsoleList.Update();
                        return;
                    }
                    else
                    {
                        txtbox[txtbxi].Text = checknum.ToString();
                        txtbox[txtbxi].Update();
                        ConsoleList.Items.Add("自動解答: ヨコの消去法により (" + (i_out + 1) + ", " + (j_out + 1) + ") は " + checknum);
                        ConsoleList.Update();
                        timeover = 0;

                        // で、もう一度最初から試行する
                        AnswerHrizonCheck(ref numpre, ref checknum, ref timeover);
                    }
                }
                IsSameNum = false;
                countblank = 0;

            }

            // 何もせずにループを抜けたら
            // タイムアウトカウンタを増やす
            timeover += 1;
        }

        public void AnswerVerticalCheck(ref Numpre[,] numpre, ref int checknum, ref int timeover)
        {
            int countblank, i_out, j_out;
            bool IsSameNum = false;
            countblank = 0;
            i_out = 0;
            j_out = 0;

            execbutton[0].PerformClick();

            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    // checknumの効いていない空白があったら
                    if (numpre[j, i].IsError == true && numpre[j, i].IsValid == false)
                    {
                        // とりあえず覚えておく
                        countblank += 1;
                        i_out = i;
                        j_out = j;
                    }
                    // その列に既にchecknumがあったら
                    if (numpre[j, i].Number == checknum)
                    {
                        // その列はもう終了
                        IsSameNum = true;
                        continue;
                    }
                }
                // その行で該当する空白がぴったり1個だった場合のみ
                if (countblank == 1 && IsSameNum == false)
                {
                    // そのマスにchecknumを入れる
                    numpre[j_out, i_out].Number = checknum;
                    int txtbxi = FindTextBoxIndexFromNumpreIndex(j_out, i_out);
                    if (txtbxi == -1)
                    {
                        ConsoleList.Items.Add("エラー: Numpre[,]配列のインデックスが不正です");
                        ConsoleList.Update();
                        return;
                    }
                    else
                    {
                        txtbox[txtbxi].Text = checknum.ToString();
                        txtbox[txtbxi].Update();
                        ConsoleList.Items.Add("自動解答: タテの消去法により (" + (j_out + 1) + ", " + (i_out + 1) + ") は " + checknum);
                        ConsoleList.Update();
                        timeover = 0;

                        // で、もう一度最初から試行する
                        AnswerHrizonCheck(ref numpre, ref checknum, ref timeover);
                    }
                    
                }
                IsSameNum = false;
                countblank = 0;
            }

            // 何もせずにループを抜けたら
            // タイムアウトカウンタを増やす
            timeover += 1;
        }

        public void AnswerBoxCheck(ref Numpre[,] numpre, ref int checknum, ref int timeover)
        {
            int countblank, i_out, j_out;
            bool IsSameNum = false;
            countblank = 0;
            i_out = 0;
            j_out = 0;

            execbutton[0].PerformClick();

            // それぞれのグループの左上だけを指定する
            for (int i = 0; i <= 6; i += 3)
            {
                for (int j = 0; j <= 6; j += 3)
                {
                    // 指定した位置から順に1行目、2行目、3行目と数値判定を行う
                    for (int k = i; k <= i + 2; k++)
                    {
                        for (int l = j; l <= j + 2; l++)
                        {
                            // checknumの効いていない空白があったら
                            if (numpre[k, l].IsError == true && numpre[k, l].IsValid == false)
                            {
                                // とりあえず覚えておく
                                countblank += 1;
                                i_out = k;
                                j_out = l;
                            }
                            // そのグループに既にchecknumがあったら
                            if (numpre[k, l].Number == checknum)
                            {
                                // そのグループはもう終了
                                IsSameNum = true;
                                continue;
                            }
                        }
                    }
                    // そのグループにchecknumがまだなく
                    // 空白が一つ、かつその空白のIsValidがfalseであった場合
                    if (countblank == 1 && IsSameNum == false)
                    {
                        // そのマスにchecknumを入れる
                        numpre[i_out, j_out].Number = checknum;
                        int txtbxi = FindTextBoxIndexFromNumpreIndex(i_out, j_out);
                        if (txtbxi == -1)
                        {
                            ConsoleList.Items.Add("エラー: Numpre[,]配列のインデックスが不正です");
                            ConsoleList.Update();
                            return;
                        }
                        else
                        {
                            txtbox[txtbxi].Text = checknum.ToString();
                            txtbox[txtbxi].Update();
                            ConsoleList.Items.Add("自動解答: ボックスの消去法により (" + (i_out + 1) + ", " + (j_out + 1) + ") は " + checknum);
                            ConsoleList.Update();
                            timeover = 0;

                            // で、もう一度最初から試行する
                            AnswerHrizonCheck(ref numpre, ref checknum, ref timeover);
                        }
                    }
                    IsSameNum = false;
                    countblank = 0;
                }
            }

            // 何もせずにループを抜けたら
            // タイムアウトカウンタを増やす
            timeover += 1;
        }

        // 0以外の数値(答え)が入っているマスの数を数える
        public int AnswerBreakCheck(Numpre[,] numpre)
        {
            int compnum = 0;
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    if (numpre[i, j].Number != 0)
                    {
                        compnum += 1;
                    }
                }
            }

            return compnum;
        }

        // Numpre配列のインデックス -> TextBoxのコントロール配列のインデックスを逆算
        public int FindTextBoxIndexFromNumpreIndex(int x, int y)
        {
            int txtbxi = 0;

            for (int _x = 0; _x <= 8; _x++)
            {
                if (_x != x)
                {
                    txtbxi += 9;
                    continue;
                }

                for (int _y = 0; _y <= 8; _y++)
                {
                    if (_y == y)
                    {
                        return txtbxi;
                    }
                    else
                    {
                        txtbxi += 1;
                    } 
                }
            }
            return -1;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
