using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace sapjor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;            
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            set_field_size(1);
            try // on first launch there is no file with records
            {
                label9.Text = File.ReadAllText("record.txt");
            }
            catch { }            
        }
        List<string> bomb_array_search = new List<string>();
        List<string> free_cells = new List<string>();
        List<string> all_cells = new List<string>(); // list for winning condition
        int all_cells_count = 0;
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString();
        }
        // array with mines
        int[,] bomb_array = new int[56, 2];        
        Random rnd = new Random();
        int[] cord = { 0, 0 }; //coordinates. comlumn, row
        int result_points = 0;
        //mines generation
        public void generate_mines()
        {
            bomb_array_search.Clear();
            for (int i = 0; i < trackBar1.Value; i++)
            {
                int x = 0;
                int y = 0;
                
                metka:
                x = rnd.Next(0, dataGridView1.Rows.Count);
                y = rnd.Next(0, dataGridView1.Rows.Count);
                string tmp = x.ToString() + "$" + y.ToString();
                int ass = bomb_array_search.IndexOf(tmp);
                if (ass == -1)
                {
                    bomb_array[i, 0] = x;
                    bomb_array[i, 1] = y;
                    bomb_array_search.Add(tmp);
                }
                else
                {
                    //recreate mine
                    goto metka;
                }

            }
            // generate list with empty
            string empty_cell = "";
            all_cells.Clear();
            for (int i2 = 0; i2 < dataGridView1.Rows.Count; i2++)
            {
                for (int i3 = 0; i3 < dataGridView1.Rows.Count; i3++)
                {
                    empty_cell = i2.ToString() + "$" + i3.ToString();
                    free_cells.Add(empty_cell);
                    all_cells.Add(empty_cell);
                }
            }
            all_cells_count = all_cells.Count() - bomb_array_search.Count();
            foreach (string bomb in bomb_array_search)
            {
                free_cells.Remove(bomb);
            }
        }
        public string check_for_loose(string coordinates)
        {
            int a = bomb_array_search.IndexOf(coordinates);
            if (a == -1)
            {
                return "alive";
            }
            else
            {
                return "dead";
            }
        }
        List<string> free_nearby_cells = new List<string>();
        public int mines_around(int x, int y)
        {
            int mines_count = 0;
            free_nearby_cells.Clear();
            foreach (string bomb in bomb_array_search)
            {
                string[] string_cord = bomb.Split('$');
                double x_b = Convert.ToDouble(string_cord[0]);
                double y_b = Convert.ToDouble(string_cord[1]);
                double distance = Math.Sqrt(Math.Pow(x_b - x, 2) + Math.Pow(y_b - y, 2));

                if (distance < 2)
                {
                    mines_count++;
                }
                else if (distance >= 2)
                {
                    free_nearby_cells.Add(bomb);
                }
                else
                {

                }
            }
            return mines_count;
        }
        // checks nearby cells and paints them gray if there is no mines. 
        public void nearby_zeros(int x, int y)
        {
            foreach (string cell in free_cells)
            {
                //cell without mines
                string[] string_cord = cell.Split('$');
                double x_b = Convert.ToDouble(string_cord[0]);
                double y_b = Convert.ToDouble(string_cord[1]);
                //distance to cell             
                double distance_to = Math.Sqrt(Math.Pow(x_b - x, 2) + Math.Pow(y_b - y, 2));
                if (distance_to < 2)
                {
                    mark_cell(Convert.ToInt32(x_b), Convert.ToInt32(y_b));
                }
                else { }
            }
        }
        public void set_field_size(int mode)
        {
            if (mode == 1) // 5х5, 10/10
            {
                // size
                this.Width = 410; this.Height = 313;
                dataGridView1.Width = 253; dataGridView1.Height = 253;
                //location
                dataGridView1.Location = new Point(12, 12);
                comboBox1.Location = new Point(268, 40);
                label1.Location = new Point(299, 25);
                label2.Location = new Point(283, 76);
                label3.Location = new Point(316, 124);
                label4.Location = new Point(265, 202);
                label5.Location = new Point(299, 202);
                label6.Location = new Point(348, 179);
                label7.Location = new Point(265, 179);
                trackBar1.Location = new Point(268, 98);
                button1.Location = new Point(268, 143);
                button2.Location = new Point(286, 238);
                label8.Location = new Point(265, 190);
                label9.Location = new Point(310, 190);                            
            }
            else if (mode == 2) // 20x20 
            {
                // size
                this.Width = 582; this.Height = 466;
                dataGridView1.Width = 403; dataGridView1.Height = 403;
                //location
                dataGridView1.Location = new Point(12, 12);
                comboBox1.Location = new Point(421, 33);
                label1.Location = new Point(452, 8);
                label2.Location = new Point(436, 69);
                label3.Location = new Point(481, 119);
                label4.Location = new Point(418, 195);
                label5.Location = new Point(452, 195);
                label6.Location = new Point(501, 172);
                label7.Location = new Point(418, 172);
                trackBar1.Location = new Point(421, 85);
                button1.Location = new Point(421, 136);
                button2.Location = new Point(450, 267);
                label8.Location = new Point(418, 216);
                label9.Location = new Point(469, 216);               
            }
            // space left for more modes in future
            else
            {
               
            }

        }
        public void game_win()
        {
            if( dataGridView1.Rows.Count > 0)
            {
                MessageBox.Show("Yay, win!");
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                this.Update();
            }
            else
            { }            
        }
        public void mark_cell(int x, int y)
        {
            try
            {
                if (dataGridView1.Rows[y].Cells[x].Style.BackColor == Color.Empty)
                {
                    all_cells_count = all_cells_count - 1;
                    try
                    {
                        if (mines_around(x, y) == 0)
                        {
                            dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.LightGray;
                            nearby_zeros(x, y);
                        }
                        else
                        {
                            dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.Green;
                            //display mines count and points
                            int mines_around_cell = mines_around(x, y);
                            result_points = result_points + mines_around_cell; // points = mines count nearby
                            dataGridView1.Rows[y].Cells[x].Value = mines_around_cell;
                            label5.Text = result_points.ToString();
                        }
                        if (all_cells_count == 0)
                        {
                            game_win();
                        }
                        else { }
                    }
                    catch { }
                }
                else
                {
                    //cell already was pressed                 
                }
            }
            catch
            {

            }
        }
        public void show_mines()
        {
            try
            {
                int x = 0;
                int y = 0;
                for (int i = 0; i < trackBar1.Value; i++)
                {
                    x = bomb_array[i, 0];
                    y = bomb_array[i, 1];
                    dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.Red;
                }
            }
            catch
            { }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // if game was restarted not after winning, points count will reset
            if (dataGridView1.Rows.Count>0)
            {
                result_points = 0;
            }
            else
            {

            }
            dataGridView1.Rows.Clear(); dataGridView1.Columns.Clear();
            label6.Text = trackBar1.Value.ToString();
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            int[] sizes = { 5, 10, 15 };
            // fields 5/10
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 1)
            {
                set_field_size(1);
            int cell_size = 250 / sizes[comboBox1.SelectedIndex];
            for (int i = 0; i < sizes[comboBox1.SelectedIndex]; i++)
            {
                dataGridView1.Columns.Add("", "");
                dataGridView1.Rows.Add();
                dataGridView1.Columns[i].Width = cell_size;
                dataGridView1.Rows[i].Height = cell_size;
                
                dataGridView1.DefaultCellStyle.Font = new Font("Arial", 15,FontStyle.Bold);
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            generate_mines();
            }
            //field 20x20
            else if (comboBox1.SelectedIndex == 2) 
            {
                set_field_size(2);
                int cell_size = 20;
                for (int i = 0; i < cell_size; i++)
                {
                    dataGridView1.Columns.Add("", "");
                    dataGridView1.Rows.Add();
                    dataGridView1.Columns[i].Width = cell_size;
                    dataGridView1.Rows[i].Height = cell_size;

                    dataGridView1.DefaultCellStyle.Font = new Font("Arial", 10,FontStyle.Bold);
                    dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                generate_mines();
            }
            else
            {}

        }
        // marking empty cells "mines". 
        public void mark_mine(int x,int y)
        {
            try
            {
                if (dataGridView1.Rows[y].Cells[x].Style.BackColor != Color.Blue)
                {

                    dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.Blue;
                    label6.Text = (Convert.ToInt32(label6.Text) - 1).ToString();
                }
                else
                {
                    dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.Empty;
                    label6.Text = (Convert.ToInt32(label6.Text) + 1).ToString();
                }
            }
            catch
            {

            }
        }
        
        // button for cheat/debug 
        private void button2_Click(object sender, EventArgs e)
        {
            show_mines();               
        }
        
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
           dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           
            if (e.Button == MouseButtons.Left)
            {
                cord[0] = dataGridView1.CurrentCellAddress.X;                       
                cord[1] = dataGridView1.CurrentCellAddress.Y;
                string live_status = check_for_loose(cord[0].ToString() + "$" + cord[1].ToString());
                if (live_status == "dead")
                {
                    show_mines();
                    dataGridView1.Rows[cord[1]].Cells[cord[0]].Style.BackColor = Color.Yellow;
                    MessageBox.Show("You are dead. Not big surprise");
                    if (Convert.ToInt32(label5.Text) > Convert.ToInt32(label9.Text))
                    {
                        try
                        {
                            File.WriteAllText("record.txt", label5.Text);
                        }
                        catch
                        {
                            MessageBox.Show("Unable to write record");
                        }
                    }
                    else
                    {
                        // record will not update
                    }
                    ///
                    try // there is no file on first launch
                    {
                        label9.Text = File.ReadAllText("record.txt");
                    }
                    catch { }
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    result_points = 0;
                    label5.Text = "0"; // reset points
                }
                else { }
                // game continues
                mark_cell(cord[0], cord[1]);
            }
            else // RMB, mark mine
            {
             
                cord[0] = e.ColumnIndex;
                cord[1] = e.RowIndex;
                if (dataGridView1.Rows[cord[1]].Cells[cord[0]].Style.BackColor != Color.Green && dataGridView1.Rows[cord[1]].Cells[cord[0]].Style.BackColor != Color.LightGray)
                {
                    mark_mine(cord[0], cord[1]);
                }
                else
                {
                    MessageBox.Show("all cels" + all_cells.Count().ToString());
                    // cell already green
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                trackBar1.Maximum = 10;
                label3.Text = trackBar1.Value.ToString();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                trackBar1.Maximum = 20;
                label3.Text = trackBar1.Value.ToString();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                trackBar1.Maximum = 55;
                label3.Text = trackBar1.Value.ToString();
            }
            else { }
        }
    }
}
