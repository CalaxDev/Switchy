namespace Switchy
{
    partial class Switchy_Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            availableProcessList = new ListView();
            availableListViewNameColumn = new ColumnHeader();
            availableListViewPIDColumn = new ColumnHeader();
            textBox1 = new TextBox();
            selectBtn = new Button();
            selectedProcessList = new ListView();
            selectedListViewNameColumn = new ColumnHeader();
            selectedListViewPIDColumn = new ColumnHeader();
            removeFromSelectionBtn = new Button();
            startBtn = new Button();
            stopBtn = new Button();
            label1 = new Label();
            refreshBtn = new Button();
            timerInSeconds = new NumericUpDown();
            label2 = new Label();
            progressBar1 = new ProgressBar();
            progressTimer = new System.Windows.Forms.Timer(components);
            label3 = new Label();
            moveSelectedItemDownBtn = new Button();
            moveSelectedItemUpBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)timerInSeconds).BeginInit();
            SuspendLayout();
            // 
            // availableProcessList
            // 
            availableProcessList.Columns.AddRange(new ColumnHeader[] { availableListViewNameColumn, availableListViewPIDColumn });
            availableProcessList.FullRowSelect = true;
            availableProcessList.Location = new Point(12, 101);
            availableProcessList.Margin = new Padding(3, 0, 3, 0);
            availableProcessList.Name = "availableProcessList";
            availableProcessList.Size = new Size(379, 259);
            availableProcessList.TabIndex = 0;
            availableProcessList.UseCompatibleStateImageBehavior = false;
            availableProcessList.View = View.Details;
            availableProcessList.DoubleClick += AvailableProcessList_DoubleClick;
            // 
            // availableListViewNameColumn
            // 
            availableListViewNameColumn.Text = "Name";
            availableListViewNameColumn.Width = 250;
            // 
            // availableListViewPIDColumn
            // 
            availableListViewPIDColumn.Text = "PID";
            availableListViewPIDColumn.Width = 100;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 50);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(379, 27);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += TextBox1_TextChanged;
            // 
            // selectBtn
            // 
            selectBtn.Location = new Point(456, 101);
            selectBtn.Name = "selectBtn";
            selectBtn.Size = new Size(44, 43);
            selectBtn.TabIndex = 2;
            selectBtn.Text = ">>";
            selectBtn.UseVisualStyleBackColor = true;
            selectBtn.Click += SelectBtn_Click;
            // 
            // selectedProcessList
            // 
            selectedProcessList.Columns.AddRange(new ColumnHeader[] { selectedListViewNameColumn, selectedListViewPIDColumn });
            selectedProcessList.FullRowSelect = true;
            selectedProcessList.Location = new Point(559, 101);
            selectedProcessList.Name = "selectedProcessList";
            selectedProcessList.Size = new Size(381, 259);
            selectedProcessList.TabIndex = 3;
            selectedProcessList.UseCompatibleStateImageBehavior = false;
            selectedProcessList.View = View.Details;
            selectedProcessList.DoubleClick += SelectedProcessList_DoubleClick;
            // 
            // selectedListViewNameColumn
            // 
            selectedListViewNameColumn.Text = "Name";
            selectedListViewNameColumn.Width = 250;
            // 
            // selectedListViewPIDColumn
            // 
            selectedListViewPIDColumn.Text = "PID";
            selectedListViewPIDColumn.Width = 100;
            // 
            // removeFromSelectionBtn
            // 
            removeFromSelectionBtn.Location = new Point(456, 317);
            removeFromSelectionBtn.Name = "removeFromSelectionBtn";
            removeFromSelectionBtn.Size = new Size(44, 43);
            removeFromSelectionBtn.TabIndex = 4;
            removeFromSelectionBtn.Text = "<<";
            removeFromSelectionBtn.UseVisualStyleBackColor = true;
            removeFromSelectionBtn.Click += RemoveFromSelectionBtn_Click;
            // 
            // startBtn
            // 
            startBtn.Location = new Point(559, 366);
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(96, 29);
            startBtn.TabIndex = 5;
            startBtn.Text = "Start";
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += StartBtn_Click;
            // 
            // stopBtn
            // 
            stopBtn.Location = new Point(844, 366);
            stopBtn.Name = "stopBtn";
            stopBtn.Size = new Size(96, 29);
            stopBtn.TabIndex = 6;
            stopBtn.Text = "Stop";
            stopBtn.UseVisualStyleBackColor = true;
            stopBtn.Click += StopBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 18);
            label1.Name = "label1";
            label1.Size = new Size(165, 20);
            label1.TabIndex = 7;
            label1.Text = "Search for Process here:";
            // 
            // refreshBtn
            // 
            refreshBtn.Location = new Point(436, 50);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(94, 29);
            refreshBtn.TabIndex = 8;
            refreshBtn.Text = "Refresh Lists";
            refreshBtn.UseVisualStyleBackColor = true;
            refreshBtn.Click += RefreshBtn_Click;
            // 
            // timerInSeconds
            // 
            timerInSeconds.Location = new Point(717, 45);
            timerInSeconds.Name = "timerInSeconds";
            timerInSeconds.Size = new Size(152, 27);
            timerInSeconds.TabIndex = 9;
            timerInSeconds.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(717, 18);
            label2.Name = "label2";
            label2.Size = new Size(173, 20);
            label2.TabIndex = 10;
            label2.Text = "Switching windows after:";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(15, 423);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(926, 29);
            progressBar1.TabIndex = 11;
            // 
            // progressTimer
            // 
            progressTimer.Interval = 1000;
            progressTimer.Tick += ProgressTimer_Tick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(873, 47);
            label3.Name = "label3";
            label3.Size = new Size(64, 20);
            label3.TabIndex = 12;
            label3.Text = "Seconds";
            // 
            // moveSelectedItemDownBtn
            // 
            moveSelectedItemDownBtn.Location = new Point(456, 246);
            moveSelectedItemDownBtn.Name = "moveSelectedItemDownBtn";
            moveSelectedItemDownBtn.Size = new Size(44, 43);
            moveSelectedItemDownBtn.TabIndex = 13;
            moveSelectedItemDownBtn.Text = "↓";
            moveSelectedItemDownBtn.UseVisualStyleBackColor = true;
            moveSelectedItemDownBtn.Click += MoveSelectedItemDownBtn_Click;
            // 
            // moveSelectedItemUpBtn
            // 
            moveSelectedItemUpBtn.Location = new Point(456, 174);
            moveSelectedItemUpBtn.Name = "moveSelectedItemUpBtn";
            moveSelectedItemUpBtn.Size = new Size(44, 43);
            moveSelectedItemUpBtn.TabIndex = 14;
            moveSelectedItemUpBtn.Text = "↑";
            moveSelectedItemUpBtn.UseVisualStyleBackColor = true;
            moveSelectedItemUpBtn.Click += MoveSelectedItemUpBtn_Click;
            // 
            // Switchy_Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(955, 467);
            Controls.Add(moveSelectedItemUpBtn);
            Controls.Add(moveSelectedItemDownBtn);
            Controls.Add(label3);
            Controls.Add(progressBar1);
            Controls.Add(label2);
            Controls.Add(timerInSeconds);
            Controls.Add(refreshBtn);
            Controls.Add(label1);
            Controls.Add(stopBtn);
            Controls.Add(startBtn);
            Controls.Add(removeFromSelectionBtn);
            Controls.Add(selectedProcessList);
            Controls.Add(selectBtn);
            Controls.Add(textBox1);
            Controls.Add(availableProcessList);
            Name = "Switchy_Main";
            Text = "Switchy";
            ((System.ComponentModel.ISupportInitialize)timerInSeconds).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView availableProcessList;
        private TextBox textBox1;
        private Button selectBtn;
        private ListView selectedProcessList;
        private Button removeFromSelectionBtn;
        private Button startBtn;
        private Button stopBtn;
        private Label label1;
        private Button refreshBtn;
        private NumericUpDown timerInSeconds;
        private Label label2;
        private ProgressBar progressBar1;
        private ColumnHeader availableListViewNameColumn;
        private ColumnHeader availableListViewPIDColumn;
        private ColumnHeader selectedListViewNameColumn;
        private ColumnHeader selectedListViewPIDColumn;
        private System.Windows.Forms.Timer progressTimer;
        private Label label3;
        private Button moveSelectedItemDownBtn;
        private Button moveSelectedItemUpBtn;
    }
}
